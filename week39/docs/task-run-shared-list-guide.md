
# Task.Run(), delt minne og trådsikkerhet – praktisk guide

Denne guiden viser **hvorfor en delt `List<T>` ikke er trådsikker**, hvordan det kan arte seg som exceptions i praksis, og **to løsninger**:
1) `lock` for å gjøre operasjoner atomiske, og  
2) Bytte til en **trådsikker** kolleksjon fra .NET.


---

## Bakgrunn: delt minne og `List<T>`

- **`List<T>` er ikke trådsikker.** Samtidige `Add`, `RemoveAt`, `Clear`, eller **enumerering** (`foreach`) mens en annen tråd endrer listen kan føre til **race conditions**, **datakorrupsjon** og exceptions.
- Typiske exceptions:
  - `InvalidOperationException: Collection was modified; enumeration operation may not execute.` (når du endrer under `foreach`).
  - `ArgumentOutOfRangeException` eller `IndexOutOfRangeException` (når elementer fjernes/legges til samtidig og indeksberegninger blir feil).
- **Task.Run** skaper arbeid på thread pool, men fjerner ikke behovet for synkronisering når minne/objekter deles.

---
<div style="page-break-after:always;"></div>

## Repro: to Tasks deler en `List<int>` og feiler

Eksempelet under har **én writer** (legger til/fjerner) og **én reader** (enumererer). Dette gir raskt `InvalidOperationException` uten synkronisering.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var list = new List<int>(Enumerable.Range(0, 10_000));
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        var writer = Task.Run(() =>
        {
            var rnd = new Random();
            while (!cts.IsCancellationRequested)
            {
                // Simuler tilfeldig oppførsel
                if (rnd.Next(2) == 0)
                    list.Add(rnd.Next());
                else if (list.Count > 0)
                    list.RemoveAt(list.Count - 1);
            }
        }, cts.Token);

        var reader = Task.Run(() =>
        {
            try
            {
                while (!cts.IsCancellationRequested)
                {
                    // Dette vil typisk kaste InvalidOperationException
                    foreach (var x in list)
                    {
                        // Lite arbeid
                        _ = x + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Reader feilet: {ex.GetType().Name} - {ex.Message}");
                cts.Cancel(); // stopp writer også
            }
        }, cts.Token);

        await Task.WhenAll(writer, reader);
        Console.WriteLine("Ferdig.");
    }
}
```

**Hvorfor feiler det?** `foreach` bruker en enumerator som **opdager modifikasjoner** (via en versjonsteller i `List<T>`). Når writer endrer listen, invalidere den enumeratoren – derav `InvalidOperationException`.

---
<div style="page-break-after:always;"></div>

## Løsning 1: `lock` – gjør kritiske seksjoner atomiske

Vi kan beskytte **all** tilgang (skriving og lesing/enumerering) bak samme lås (samme `lock`-objekt). Dette er enkelt og gir korrekte resultater, men kan redusere parallellitet.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var list = new List<int>(Enumerable.Range(0, 10_000));
        var gate = new object();
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));

        var writer = Task.Run(() =>
        {
            var rnd = new Random();
            while (!cts.IsCancellationRequested)
            {
                lock (gate)
                {
                    if (rnd.Next(2) == 0)
                        list.Add(rnd.Next());
                    else if (list.Count > 0)
                        list.RemoveAt(list.Count - 1);
                }
            }
        }, cts.Token);

        var reader = Task.Run(() =>
        {
            while (!cts.IsCancellationRequested)
            {
                // **Hel** enumerering må beskyttes for å garantere konsistens
                lock (gate)
                {
                    foreach (var x in list)
                    {
                        _ = x + 1;
                    }
                }
            }
        }, cts.Token);

        await Task.WhenAll(writer, reader);
        Console.WriteLine("Ferdig med lock – ingen exceptions.");
    }
}
```

### Optimalisering: Kopi under lås, prosessering utenfor
For å **minimere hvor lenge låsen holdes**, kan du lage en **snapshot** (kopi) under lås og prosessere utenfor:

```csharp
List<int> snapshot;
lock (gate)
{
    snapshot = new List<int>(list); // eller list.ToArray()
}
// prosesser snapshot uten lås
foreach (var x in snapshot) { /* ... */ }
```

**Fordeler:** Enkel å forstå, bevarer `List<T>`-semantikk.  
**Ulemper:** Mindre parallellisme; risiko for lås-kø (hot lock) ved tung bruk.

---
<div style="page-break-after:always;"></div>

## Løsning 2: Bruk en trådsikker kolleksjon

Hvis mønsteret i praksis er **produsent/forbruker** (en legger inn, en tar ut), er `ConcurrentQueue<T>` eller `BlockingCollection<T>` ofte riktige verktøy.

### 2A) `ConcurrentQueue<T>` (ikke-blokkerende)

```csharp
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var queue = new ConcurrentQueue<int>();
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));

        var producer = Task.Run(() =>
        {
            var rnd = new Random();
            while (!cts.IsCancellationRequested)
            {
                queue.Enqueue(rnd.Next());
            }
        }, cts.Token);

        var consumer = Task.Run(() =>
        {
            while (!cts.IsCancellationRequested)
            {
                if (queue.TryDequeue(out var item))
                {
                    // prosesser item
                    _ = item + 1;
                }
                else
                {
                    // ingen elementer akkurat nå
                    Thread.SpinWait(50);
                }
            }
        }, cts.Token);

        await Task.WhenAll(producer, consumer);
        Console.WriteLine("Ferdig med ConcurrentQueue – ingen exceptions.");
    }
}
```

### 2B) `BlockingCollection<T>` (blokkerende, enklere for køer)

`BlockingCollection<T>` bruker en trådsikker base (som `ConcurrentQueue<T>`) og gir en **blokkerende** `GetConsumingEnumerable()` for forbruker.

```csharp
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
        using var bc = new BlockingCollection<int>(new ConcurrentQueue<int>());

        var producer = Task.Run(() =>
        {
            var rnd = new Random();
            while (!cts.IsCancellationRequested)
            {
                bc.Add(rnd.Next(), cts.Token);
            }
        }, cts.Token);

        var consumer = Task.Run(() =>
        {
            try
            {
                foreach (var item in bc.GetConsumingEnumerable(cts.Token))
                {
                    // prosesser item
                    _ = item + 1;
                }
            }
            catch (OperationCanceledException) { /* normal shutdown */ }
        }, cts.Token);

        await Task.Delay(3000);
        cts.Cancel();
        bc.CompleteAdding(); // signaliser at ingen flere elementer kommer
        await Task.WhenAll(producer, consumer);

        Console.WriteLine("Ferdig med BlockingCollection – ingen exceptions.");
    }
}
```
<div style="page-break-after:always;"></div>

### 2C) Hva med tilfeldig tilgang og fjerning i midten?
Det finnes **ingen innebygd "ConcurrentList<T>"** med full liste-semantikk (indeksering, fjerning midt i). Alternativer:
- Fortsett å bruke `List<T>` + `lock` (ofte best).
- Bruk **immutable** samlinger (`ImmutableList<T>`) og oppdater atomisk når du skriver *sjelden* og leser *ofte*:
  ```csharp
  using System.Collections.Immutable;
  using System.Threading;

  ImmutableList<int> data = ImmutableList<int>.Empty;

  // Skriv (atomisk oppdatering)
  ImmutableInterlocked.Update(ref data, d => d.Add(42));

  // Les (trådsikkert uten lås)
  foreach (var x in data) { /* ... */ }
  ```

---

## Task.Run – korte retningslinjer

- **CPU-bound arbeid i desktop/konsoll**: `Task.Run` er greit for å utnytte flere kjerner.
- **ASP.NET Core (web)**: Ikke pakk I/O i `Task.Run` – bruk `async` I/O. For bakgrunnsarbeid, bruk `BackgroundService` og gjerne `Channel<T>`/kø.
- Uansett: **Delt minne må synkroniseres** (lock, concurrent collections, immutables).

---

## Oppsummering

- `List<T>` er **ikke** trådsikker. Samtidige endringer + enumerering gir lett exceptions.
- Løsning 1: **Beskytt all tilgang med `lock`**. Evt. snapshot for å korte ned låseperioder.
- Løsning 2: **Bruk trådsikre samlinger** som `ConcurrentQueue<T)` eller `BlockingCollection<T>` for prod/cons-mønstre.
- For lese-mest / skriv-sjelden: vurder **immutable** samlinger.

Mål, test og velg den enkleste løsningen som dekker behovet ditt.
