
# Tråder (threading) og delt minne – nybegynnerguide for .NET

Denne guiden er laget for studenter som er **helt nye** til tråder og delt minne i C#/.NET. Du får:

- Enkle forklaringer på begrepene
- Små, kjørbare kodeeksempler
- Beste praksis og typiske feil
- Øvelser du kan gjøre selv

> Koden er skrevet for .NET 8 og kan kjøres i en konsollapp.

---

## 1) Hvorfor tråder?

- **Responsivitet**: Unngå at brukergrensesnittet fryser (f.eks. WPF/WinForms) ved tung jobb.
- **Utnytte flere CPU-kjerner**: Kjør arbeid parallelt på flere kjerner (CPU-bound).
- **Samtidige oppgaver**: Utfør flere ting "på samme tid" (f.eks. server håndterer mange forespørsler).

**Viktig skille**  

- **Konkurrens (concurrency)**: Flere oppgaver *starter/overlapper* i tid.  
- **Parallellisme**: Flere oppgaver *fysisk* kjører samtidig på flere kjerner.

---

## 2) Prosess vs tråd

- **Prosess**: Programmet ditt + eget minneområde.
- **Tråd**: Utførelseskontekst *inne i prosessen*. En prosess kan ha mange tråder og **deler minne** mellom trådene.

Fordel: rask deling av data mellom tråder. Ulempe: **race conditions** hvis du ikke synkroniserer.

---

## 3) Task vs Thread (og ThreadPool)

- `Thread`: Lavnivå representasjon av en tråd. Du oppretter og styrer selv.

- `Task`: Høynivå jobbabstraksjon som **vanligvis** kjører på **ThreadPool** (gjenbrukte tråder, administrert av .NET). Opprettes ofte med `Task.Run(...)`.

**Anbefaling**: Bruk **`Task`** og `async/await` for det meste. Bruk `Thread` kun når du **må** (spesielle scenarier).

---
<div style="page-break-after:always;"></div>

## 4) Første eksempel – opprette tråder

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        // 1) Rå tråd
        var t = new Thread(() => Console.WriteLine($"Hello fra Thread {Thread.CurrentThread.ManagedThreadId}"));
        t.Start();
        t.Join();

        // 2) Task (ThreadPool)
        Task.Run(() => Console.WriteLine($"Hello fra Task på Thread {Thread.CurrentThread.ManagedThreadId}")).Wait();
    }
}
```

**Observasjon**: Begge skriver fra en annen tråd enn main, men `Task` bruker poolen.

---
<div style="page-break-after:always;"></div>

## 5) Delt minne og race conditions

Når flere tråder **leser/skriver samme data** uten koordinering kan du få:

- **Feil/inkonsistent data**
- **Exceptions** (f.eks. når du enumérerer en `List<T>` mens en annen tråd endrer den)
- **Heisenbug** – feil som kommer og går

### 5.1 Feil eksempel – ingen synkronisering

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        int counter = 0;
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));

        var inc = Task.Run(() =>
        {
            while (!cts.IsCancellationRequested)
            {
                // Ikke atomisk: counter++ = les, +1, skriv
                counter++;
            }
        });
        var dec = Task.Run(() =>
        {
            while (!cts.IsCancellationRequested)
            {
                counter--;
            }
        });

        await Task.WhenAll(inc, dec);
        Console.WriteLine($"Counter endte som {counter} (uansett, dette er datakorrupsjon-demo)");
    }
}
```

**Problemet**: `counter++` er **ikke** atomisk. Les/skriv kolliderer.

<div style="page-break-after:always;"></div>

### 5.2 Riktig med `Interlocked` (atomiske operasjoner)

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        int counter = 0;
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));

        var inc = Task.Run(() =>
        {
            while (!cts.IsCancellationRequested)
                Interlocked.Increment(ref counter); // atomisk
        });
        var dec = Task.Run(() =>
        {
            while (!cts.IsCancellationRequested)
                Interlocked.Decrement(ref counter); // atomisk
        });

        await Task.WhenAll(inc, dec);
        Console.WriteLine($"Counter (atomisk) = {counter}");
    }
}
```

---
<div style="page-break-after:always;"></div>

## 6) Enumerering + modifisering = typisk exception

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
        var list = new List<int>(Enumerable.Range(0, 5000));
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));

        var writer = Task.Run(() =>
        {
            var rnd = new Random();
            while (!cts.IsCancellationRequested)
            {
                if (rnd.Next(2) == 0) list.Add(rnd.Next());
                else if (list.Count > 0) list.RemoveAt(list.Count - 1);
            }
        });

        var reader = Task.Run(() =>
        {
            try
            {
                while (!cts.IsCancellationRequested)
                {
                    foreach (var x in list) { /* gjør lite */ }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Reader feilet: {ex.GetType().Name} - {ex.Message}");
                cts.Cancel();
            }
        });

        await Task.WhenAll(writer, reader);
    }
}
```

**Vanlig resultat**: `InvalidOperationException: Collection was modified; enumeration operation may not execute.`

---
<div style="page-break-after:always;"></div>

## 7) Synkronisering – verktøy du må kunne

- **`lock` / `Monitor`**: Gjør kodeblokk atomisk; kun én tråd om gangen innenfor blokken.
- **`Interlocked`**: Atomiske operasjoner på primitive typer (int, long, referanser).
- **`SemaphoreSlim`**: Tillater *N* samtidige tilgang (rate limiting / begrense parallellitet).
- **`Mutex`**: Kryss-prosess-lås. Tyngre; sjelden nødvendig i ren .NET app.
- **`AutoResetEvent` / `ManualResetEventSlim`**: Signalering mellom tråder.
- **`Concurrent`-samlinger**: `ConcurrentQueue<T>`, `ConcurrentDictionary<TKey,TValue>` osv.
- **`Immutable`-samlinger**: Les-mest / skriv-sjelden scenarier.

### 7.1 `lock`-eksempel for `List<T>`

```csharp
var gate = new object();
var list = new List<int>();

// writer
lock (gate)
{
    list.Add(42);
}

// reader (sikker enumerering)
List<int> snapshot;
lock (gate)
{
    snapshot = new List<int>(list); // kopi
}
foreach (var x in snapshot) { /* prosessér */ }
```

### 7.2 Trådsikker samling – `ConcurrentQueue<T>`

```csharp
using System.Collections.Concurrent;

var q = new ConcurrentQueue<int>();

// Produsent
q.Enqueue(1);

// Forbruker
if (q.TryDequeue(out var item))
{
    // bruk item
}
```
<div style="page-break-after:always;"></div>

### 7.3 Begrense samtidig kjøring – `SemaphoreSlim`

```csharp
var sem = new SemaphoreSlim(8); // maks 8 samtidig
await sem.WaitAsync();
try
{
    await DoWorkAsync();
}
finally
{
    sem.Release();
}
```

---

## 8) Deadlock, livelock, starvation (kort)

- **Deadlock**: To tråder låser ressurser i motsatt rekkefølge og venter på hverandre → evig venting.
  - **Regel**: Lås ressursene i **samme rekkefølge** over alt, eller bruk færre låser.

- **Livelock**: Tråder er "aktive", men kommer ikke fremover (gir etter for hverandre konstant).

- **Starvation**: En tråd får aldri CPU eller lås fordi andre dominerer.

**Tips**: Hold låste seksjoner **små** og velg *enkle* låsestrategier.

---

## 9) Async/await vs flere tråder

- **I/O-bound arbeid** (nettverk/disk): Bruk **`async/await`** → frigjør tråden mens du venter.
- **CPU-bound arbeid**: Bruk **`Task.Run`** (konsoll/desktop) for å utnytte flere kjerner.
- **ASP.NET Core**: Ikke pakk I/O i `Task.Run`. For bakgrunnsarbeid, bruk `BackgroundService` + `Channel<T>` eller køsystem.

---

## 10) Verktøy for måling og feilsøking

- **Visual Studio Profiler** / **dotnet-trace** / **dotnet-counters**: CPU, allokeringer, locks.
- **PerfView**: Flammer, GC, CPU, locks (avansert).
- **BenchmarkDotNet**: Mikrobenchmarks (ikke for hele apper, men for små biter).

---
<div style="page-break-after:always;"></div>

## 11) Vanlige feil og beste praksis

**Feil**

- Dele `List<T>` uten synkronisering → exceptions/datakorrupsjon.
- Lange operasjoner **inni** `lock` → dårlig skalerbarhet.
- Glemme å avbryte (`CancellationToken`) og å rydde opp.

**Beste praksis**

- Velg **enkleste** synkroniseringsmekanisme som funker (ofte `lock`).
- Bruk **immutable** eller **concurrent** samlinger når mønsteret passer.
- Skill **CPU-bound** vs **I/O-bound** og velg `Task.Run`/`async` deretter.
- Hold delte mutable data **små** og godt kapslet.

---

## 12) Mini-oppgaver (for studenter)

1. **Race med counter**: Fjern `Interlocked` og observer feil, legg det tilbake og verifiser stabilt resultat.
2. **List + lock**: Lag writer/reader som bruker `lock` + snapshot; mål forskjell når du tar snapshot med `ToArray()` vs `new List<int>(list)`.
3. **ConcurrentQueue**: Lag produsent/forbruker med throughput-måling (items/sek).
4. **SemaphoreSlim**: Last ned 50 URL-er, men maks 5 samtidige.
5. **Deadlock-demo**: Lag to låser og to tråder som låser i ulik rekkefølge → deadlock; fiks ved å standardisere rekkefølgen.

---

## 13) Kjapp jukselapp

- **Delte mutable data** → krever koordinering (`lock`, `Interlocked`, `Concurrent*`, `Immutable*`).
- **`Task`** = jobb, ofte på **ThreadPool**. **`Thread`** = rå tråd (sjeldnere behov).
- **I/O** → `async/await`. **CPU** → `Task.Run` (utenfor web).
- **Hold låser korte**, unngå nested låser med forskjellig rekkefølge.
- **Mål og logg** for å forstå hva som faktisk skjer.

---
<div style="page-break-after:always;"></div>

## 14) Ekstra: Produsent/forbruker med `Channel<T>` (moderne)

```csharp
using System;
using System.Threading.Channels;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var ch = Channel.CreateUnbounded<int>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false
        });

        // Consumer
        var consumer = Task.Run(async () =>
        {
            await foreach (var item in ch.Reader.ReadAllAsync())
            {
                // prosessér
                await Task.Yield();
            }
        });

        // Producers
        var p1 = Task.Run(async () => { for (int i = 0; i < 1000; i++) await ch.Writer.WriteAsync(i); });
        var p2 = Task.Run(async () => { for (int i = 1000; i < 2000; i++) await ch.Writer.WriteAsync(i); });

        await Task.WhenAll(p1, p2);
        ch.Writer.Complete();
        await consumer;
    }
}
```

---

## Oppsummering

- Tråder lar deg kjøre arbeid samtidig og utnytte flere kjerner, men **delt minne** krever **synkronisering**.
- Start enkelt: forstå `Task`, `lock`, `Interlocked`, og `Concurrent*`-samlinger.
- Velg riktig verktøy for **I/O vs CPU** og **målt behov**.
