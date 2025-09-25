
# Objektpooling i .NET – en praktisk guide

Denne guiden forklarer **hva objektpooling er**, **når du bør bruke det**, og gir **konkrete C#-eksempler** – inkludert en egen `Person`-klasse med tilpasset policy.

---

## Hva er objektpooling?

**Objektpooling** betyr at du **gjenbruker** objekter i stedet for å opprette og garbage-collekte dem hver gang. Poenget er å:

- **Redusere allokeringer** og **GC-trykk** (mindre pauser / jevnere latency).
- **Spare CPU** som ellers går til initialisering/teardown av "dyre" objekter.
- **Øke throughput** ved hyppig, kortvarig bruk av samme objekttype.

I .NET finnes to hovedspor:

1. **`ArrayPool<T>`** (for buffere) – høy avkastning når du ellers ville opprettet mange store arrayer.
2. **`ObjectPool<T>`** (for egne objekttyper) – via `Microsoft.Extensions.ObjectPool`.

---

## Når bør du (og ikke) bruke pooling?

**Bruk pooling når…**

- Objektet er **dyrt å opprette** (store graf-strukturer, komplekse init, store lister/buffere).
- Objektet brukes **hyppig og kortvarig** (midlertidig i en operasjon).
- Du kan **resette** objektet lett mellom brukere (til en "ren" tilstand).
- Du er i **hot path** (f.eks. serialisering, parsing, protokollhåndtering).

**Unngå pooling når…**

- Objektet er **billig og lite** (gevinstene spises ofte opp av kompleksitet).
- Objektet **holder eksterne ressurser** (sockets, file handles, unmanaged) – bruk heller de etablerte poolene/handlerne som rammeverket tilbyr.
- Objektet **ikke kan resettes trygt** (risiko for lekkasje av tilstand/PII).
- Objektet forventes **langlivet** (da er pooling unødvendig).
- Du allerede bruker typer som **selv** pooler internt (f.eks. HttpClient benytter handler som har egne pooler).

**Tommelregel:** Profilér først. Pooling skal **løse et målt problem**, ikke introdusere kompleksitet uten effekt.

---
<div style="page-break-after:always;"></div>

## Alternativer og komplementer

- **`ArrayPool<T>`** for rå buffere (`byte[]`, `char[]`, …). Returnér med `clearArray:true` dersom data er sensitivt.
- **`StringBuilder`-pooling** via `StringBuilderPooledObjectPolicy`.
- **`ConcurrentQueue<T>`, `Channel<T>`** for arbeidskøer (ikke det samme som pooling, men ofte brukt sammen).

---

## Rask intro: `ArrayPool<T>`

```csharp
using System;
using System.Buffers;
using System.IO;

int ReadSum(Stream s)
{
    var pool = ArrayPool<byte>.Shared;
    byte[] buf = pool.Rent(64 * 1024);
    try
    {
        int read, sum = 0;
        while ((read = s.Read(buf, 0, buf.Length)) > 0)
            for (int i = 0; i < read; i++) sum += buf[i];
        return sum;
    }
    finally
    {
        // Sett clearArray:true dersom buffer kan inneholde sensitivt innhold
        pool.Return(buf, clearArray: false);
    }
}
```

---
<div style="page-break-after:always;"></div>

## `ObjectPool<T>` for dine egne typer

Pakk inn din egen type i en **policy** som forteller poolen hvordan objekter opprettes og klargjøres ved retur.

### 1) Referanser og pakker

Legg til NuGet-pakken **`Microsoft.Extensions.ObjectPool`**.

### 2) En enkel `Person`-type + policy

```csharp
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;

// Din domene-type
public class Person
{
    public string? FirstName { get; set; }
    public string? LastName  { get; set; }
    public DateTime BirthDate { get; set; }
    public List<string> Tags { get; } = new();

    // Tilbakestill state slik at neste bruker får "rent" objekt
    public void Reset()
    {
        FirstName = null;
        LastName  = null;
        BirthDate = default;
        Tags.Clear();
    }
}

// Policy: hvordan lage og resette Person
public sealed class PersonPooledObjectPolicy : PooledObjectPolicy<Person>
{
    // Opprettes når poolen trenger flere
    public override Person Create() => new Person();

    // Return true => behold i pool. False => kast (ikke egnet for gjenbruk).
    public override bool Return(Person obj)
    {
        // RESETT ALLTID!
        obj.Reset();
        return true;
    }
}
```
<div style="page-break-after:always;"></div>

### 3) Opprette pool og bruke den

```csharp
using Microsoft.Extensions.ObjectPool;
using System;

var provider = new DefaultObjectPoolProvider();
// Valgfritt: provider.MaximumRetained <- (konfigurer poolstørrelse om ønskelig)
var pool = provider.Create(new PersonPooledObjectPolicy());

// Leie -> bruke -> levere tilbake
Person p = pool.Get();
try
{
    p.FirstName = "Ada";
    p.LastName  = "Lovelace";
    p.BirthDate = new DateTime(1815, 12, 10);
    p.Tags.Add("math");
    p.Tags.Add("computing");

    Console.WriteLine($"{p.FirstName} {p.LastName} ({p.BirthDate:yyyy}) [{string.Join(',', p.Tags)}]");
}
finally
{
    pool.Return(p);  // VIKTIG: alltid returnér!
}
```
<div style="page-break-after:always;"></div>

### 4) Praktisk «using»-mønster (sikrer `Return` automatisk)

`ObjectPool<T>` returnerer ikke et `IDisposable`. Du kan lage en **liten helper** som sørger for at Return alltid kalles:

```csharp
using Microsoft.Extensions.ObjectPool;
using System;

public readonly struct Pooled<T> : IDisposable where T : class
{
    public T Instance { get; }
    private readonly ObjectPool<T> _pool;

    public Pooled(ObjectPool<T> pool, T instance)
    {
        _pool = pool;
        Instance = instance;
    }

    public void Dispose() => _pool.Return(Instance);
}

public static class ObjectPoolExtensions
{
    public static Pooled<T> GetPooled<T>(this ObjectPool<T> pool) where T : class
        => new(pool, pool.Get());
}

// Bruk:
using var leased = pool.GetPooled();     // leased.Instance er Person
var person = leased.Instance;
person.FirstName = "Grace";
person.LastName = "Hopper";
```

Dette reduserer risiko for «glemt Return», spesielt i metoder med flere return paths.

---
<div style="page-break-after:always;"></div>

## Integrasjon i ASP.NET Core (DI)

Registrér poolen i `IServiceCollection` og injiser `ObjectPool<Person>` der du trenger den.

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;

var services = new ServiceCollection();

services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
services.AddSingleton<PersonPooledObjectPolicy>();
services.AddSingleton<ObjectPool<Person>>(sp =>
{
    var provider = sp.GetRequiredService<ObjectPoolProvider>();
    var policy   = sp.GetRequiredService<PersonPooledObjectPolicy>();
    return provider.Create(policy);
});

var sp = services.BuildServiceProvider();
var pool = sp.GetRequiredService<ObjectPool<Person>>();

// Eksempelbruk (f.eks. i en controller/handler)
var person = pool.Get();
try
{
    person.FirstName = "Marie";
    person.LastName = "Curie";
    // ... bruk person ...
}
finally
{
    pool.Return(person);
}
```

> I web-apper bør du vurdere **trådsikkerhet** i bruken (selve `ObjectPool<T>` er trådsikker). Ikke del ett og samme pooled **objekt** mellom samtidige forespørsler – **hver** forespørsel må hente sitt eget objekt fra poolen.

---
<div style="page-break-after:always;"></div>

## Eksempel: Pooled `StringBuilder` (innebygd policy)

```csharp
using Microsoft.Extensions.ObjectPool;
using System.Text;

var provider = new DefaultObjectPoolProvider();
var sbPool = provider.Create(new StringBuilderPooledObjectPolicy());

var sb = sbPool.Get();
try
{
    sb.Append("Hello ");
    sb.Append("pooled ");
    sb.Append("StringBuilder");
    var text = sb.ToString();
    // bruk text...
}
finally
{
    sb.Clear();      // viktig
    sbPool.Return(sb);
}
```

---
<div style="page-break-after:always;"></div>

## Feilmønstre og fallgruver

- **Use-after-return**: Ikke bruk objektet **etter** at det er returnert til poolen.
- **Double return**: Ikke returnér samme instans to ganger.
- **Manglende reset**: All tidligere tilstand/PII må nullstilles før retur.
- **Pooling av feil type**: Ikke pool objekter som er billige, immutable eller med utrygge eksterne ressurser.
- **Deling på tvers av tråder**: Et objekt hentet fra poolen skal brukes eksklusivt av én logisk operasjon om gangen.
- **Overdreven poolstørrelse**: For stor pool kan øke minnefotavtrykk uten ytelsesgevinst. Justér konservativt.

---

## Når ser du effekt i praksis?

- «Tight loops»/hot paths som tidligere genererte mange allokeringer.
- Serialisering/deserialisering (buffers, mellomobjekter).
- Parsere, protokollhåndtering, meldingsbygging (`StringBuilder`).
- Midlertidige grafer/DOM-lignende modeller som bygges/destrueres ofte.

**Mål alltid** med profiler/benchmark (f.eks. BenchmarkDotNet). Du ser ofte:

- **Lavere Gen0/Gen1-frekvens**, færre allokeringer (bytes og count).
- **Jevnere latency** i applikasjoner med høy last.

---

## Sjekkliste før du innfører pooling

- [ ] Har du målt et faktisk allokerings-/GC-problem?
- [ ] Kan objektet resettes **fullstendig og trygt**?
- [ ] Har du tester som fanger bruk etter `Return()`?
- [ ] Har du valgt riktig pool-type (`ArrayPool<T>` vs `ObjectPool<T>`)?
- [ ] Er poolstørrelsen fornuftig?
- [ ] Har du vurdert sikkerhet/PII (f.eks. `clearArray:true`)?

---
<div style="page-break-after:always;"></div>

## Bonus: Mini-benchmark-mal

```csharp
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.ObjectPool;

[MemoryDiagnoser]
public class PoolVsNew
{
    private readonly ObjectPool<Person> _pool =
        new DefaultObjectPoolProvider().Create(new PersonPooledObjectPolicy());

    [Params(1000, 10000)]
    public int N;

    [Benchmark(Baseline = true)]
    public int NewObjects()
    {
        int checksum = 0;
        for (int i = 0; i < N; i++)
        {
            var p = new Person { FirstName = "A", LastName = "B" };
            checksum += p.GetHashCode();
        }
        return checksum;
    }

    [Benchmark]
    public int PooledObjects()
    {
        int checksum = 0;
        for (int i = 0; i < N; i++)
        {
            var p = _pool.Get();
            try
            {
                p.FirstName = "A";
                p.LastName = "B";
                checksum += p.GetHashCode();
            }
            finally
            {
                _pool.Return(p);
            }
        }
        return checksum;
    }
}

public class Program
{
    public static void Main() => BenchmarkRunner.Run<PoolVsNew>();
}
```

> Husk: Resultater varierer med workload. Effekten er størst når objektene er reelt "dyrere" enn trivielle POCO-er.

---

## Oppsummering

- **Objektpooling** reduserer allokeringer/GC i hot paths og kan gi jevnere latency.  
- Bruk **`ArrayPool<T>`** for buffere og **`ObjectPool<T>`** for egne typer.  
- Skriv en **policy** som **reseter** objektet trygt ved retur.  
- Unngå pooling når gevinsten er liten eller risikoen høy.  
- **Mål før og etter** — la data styre valgene.
