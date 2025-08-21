# Guide til BenchmarkDotNet i C# 

BenchmarkDotNet (BDN) er et rammeverk for **presise, reproduserbare mikro-benchmarks** i .NET. Det håndterer alt det vanskelige: riktig kompilering (Release), JIT-oppvarming, flere iterasjoner, statistikk (median, gjennomsnitt, standardavvik), GC-styring, CPU-affinitet m.m. Resultatene presenteres i pene tabeller og kan eksporteres til flere formater.

---

## Hvorfor bruke BenchmarkDotNet?

- **Pålitelighet:** Unngår vanlige feil (måle i Debug, for få iterasjoner, cold start, JIT-støy).
- **Reproduserbarhet:** Lagrer miljøinfo (OS, CPU, .NET-versjon, GC-modus).
- **Statistikk:** Viser median, P95, standardavvik, allokeringer, outliers.
- **Automatisering:** Lett å kjøre lokalt og i CI.

---

## Oppsett (fra tom .NET-konsollapp)

1. Opprett et nytt prosjekt:

   ```bash
   dotnet new console -n BenchDemo
   cd BenchDemo
   ```

2. Legg til NuGet-pakken:

   ```bash
   dotnet add package BenchmarkDotNet
   ```

3. **Viktig:** Kjør alltid i **Release** og helst **x64**:

   ```bash
   dotnet run -c Release
   ```

   (BDN håndterer selv bygging av egne artefakter, men startkommandoen må være `-c Release`.)

---
<div style="page-break-after:always;"></div>

## Første benchmark

### Program.cs

```csharp
using BenchmarkDotNet.Running;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<StringBenchmarks>();
    }
}
```

### StringBenchmarks.cs

```csharp
using System.Text;
using BenchmarkDotNet.Attributes;

[MemoryDiagnoser] // adds Gen0/1/2 and Allocated (B) columns
[RankColumn]      // adds a ranking column
public class StringBenchmarks
{
    private const int N = 10_000;

    [Benchmark(Baseline = true)]
    public string PlusOperator()
    {
        string s = string.Empty;
        for (int i = 0; i < N; i++)
            s += "x";
        return s;
    }

    [Benchmark]
    public string StringBuilder_Append()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < N; i++)
            sb.Append('x');
        return sb.ToString();
    }
}
```
<div style="page-break-after:always;"></div>

Kjør:

```bash
dotnet run -c Release
```

**Hva du får:** En tabell med `Method`, `Mean`, `Error`, `StdDev`, `Allocated`, `Rank`, og miljøinformasjon nederst. Du vil normalt se at `StringBuilder_Append` er raskere og allokerer mindre minne enn `PlusOperator`.

---

## Hva måler vi – og hvordan tolke tallene?

- **Mean / Median:** Gjennomsnittlig (og median) tid per operasjon. Median er robust mot outliers.
- **Error / StdDev:** Usikkerhet og spredning. Høy `StdDev` kan tyde på støy (GC, turbo boost, bakgrunnsprosesser).
- **Allocated (B):** Allokert minne per operasjon (viktig for GC-trykk).
- **Gen0/1/2:** Hvor ofte GC skjer per 1000 operasjoner.
- **Rank:** Relativ rangering (1 = best) innenfor samme benchmark-tabell.

God praksis:

- Sammenlign **varianter av samme oppgave**.
- Se etter **konsistent forbedring** (lavere Mean/Median *og* lavere allokeringer).
- Unngå å konkludere basert på én kjøring; kjør gjerne flere ganger.

---
<div style="page-break-after:always;"></div>

## Konfigurasjon av jobber og runtime

```csharp
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Configs;

public class Program
{
    public static void Main(string[] args)
    {
        var config = DefaultConfig.Instance
            .WithOptions(ConfigOptions.DisableOptimizationsValidator) // if you must allow attributes that look like no-opt
            .AddJob(Job.Default
                .WithRuntime(CoreRuntime.Core80)  // target .NET 8
                .WithPlatform(Platform.X64)
                .WithJit(Jit.RyuJit)
                .WithGcServer(true)
                .WithGcConcurrent(true)
                .WithId(".NET 8 ServerGC"));

        BenchmarkRunner.Run<ArrayVsList>(config);
    }
}

[MemoryDiagnoser]
public class ArrayVsList
{
    private int[] _arr = new int[10_000];
    private List<int> _list = Enumerable.Range(0, 10_000).ToList();

    [Benchmark(Baseline = true)]
    public int Sum_Array() => _arr.Sum();

    [Benchmark]
    public int Sum_List() => _list.Sum();
}
```

**Tips:** Du kan bruke `BenchmarkSwitcher` for å velge benchmarks via kommandolinje, og `RuntimeMoniker` for å teste på flere .NET-versjoner i samme kjøring.

---
<div style="page-break-after:always;"></div>

## Parametrisering (test flere størrelser/inputs)

```csharp
using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class ParseBench
{
    [Params(10, 100, 1_000, 10_000)]
    public int N;

    private string[] _nums = Array.Empty<string>();

    [GlobalSetup]
    public void Setup()
    {
        _nums = Enumerable.Range(0, N).Select(i => i.ToString()).ToArray();
    }

    [Benchmark(Baseline = true)]
    public int IntParse()
    {
        int sum = 0;
        foreach (var s in _nums)
            sum += int.Parse(s);
        return sum;
    }

    [Benchmark]
    public int TryParse()
    {
        int sum = 0;
        foreach (var s in _nums)
        {
            if (int.TryParse(s, out var v))
                sum += v;
        }
        return sum;
    }
}
```

Dette viser **skalering**: hvordan ytelse endrer seg med inputstørrelse.

---
<div style="page-break-after:always;"></div>

## Baseline og prosentvis forbedring

```csharp
using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class SearchBench
{
    private int[] data = Enumerable.Range(0, 100_000).ToArray();

    [Benchmark(Baseline = true)]
    public bool Linear() => data.Contains(99_999);

    [Benchmark]
    public bool Binary() => Array.BinarySearch(data, 99_999) >= 0;
}
```

Med `Baseline = true` viser BDN en `%`-kolonne for relativ endring.

---

## Eksport og logging

Legg til eksportører i config:

```csharp
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;

var config = DefaultConfig.Instance
    .AddLogger(ConsoleLogger.Default)
    .AddExporter(MarkdownExporter.GitHub)
    .AddExporter(CsvExporter.Default)
    .AddExporter(HtmlExporter.Default);
```

Etter kjøring finner du resultater i `./BenchmarkDotNet.Artifacts/` (tabeller, grafer, miljøinfo).

---
<div style="page-break-after:always;"></div>

## Vanlige fallgruver og hva du bør se etter

- **Debug i stedet for Release:** Alltid `-c Release`.
- **For liten arbeidsmengde:** Øk `N` for å unngå at måling domineres av overhead.
- **Dead-code elimination:** Returner resultater eller bruk `[Arguments]`/`[Params]` så JIT ikke fjerner arbeidet.
- **Warmup:** BDN gjør warmup automatisk, men korte benchmarks kan trenge flere iterasjoner.
- **Støy:** Lukk tunge apper, koble fra strømstyring som endrer CPU-frekvens, kjør flere ganger.
- **Allokeringer:** Se på `Allocated (B)`; høye allokeringer kan være dyrere over tid (GC-trykk).
- **Outliers:** BDN markerer utliggere; jevn drift er bedre enn spiky ytelse.

**Bra resultat:** Lav `Mean/Median`, lav `StdDev`, lave allokeringer, konsistente forbedringer.  
**Dårlig resultat:** Høy spredning, mye allokering, inkonsistent vinner mellom kjøringer.

---

## Måle async og I/O (vær forsiktig)

I/O styres av eksterne faktorer. For mikrobenchmarks: isoler CPU-arbeid. For `async`:

```csharp
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

public class AsyncBench
{
    [Benchmark]
    public async Task<int> DelayAndCompute()
    {
        await Task.Delay(1); // avoid real I/O in microbenchmarks
        int s = 0;
        for (int i = 0; i < 10_000; i++) s += i;
        return s;
    }
}
```

Bruk dette mer som **eksempel** enn presis måling av I/O.

---

## Flere nyttige attributter (utvalg)

- `[MemoryDiagnoser]` – viser allokeringer og GC.
- `[RankColumn]`, `[MinColumn]`, `[MaxColumn]`, `[MedianColumn]` – ekstra statistikk.
- `[SimpleJob]`, `[MediumRun]`, `[LongRun]` – kontroll over antall iterasjoner.
- `[Params]`, `[Arguments]`, `[GlobalSetup]`, `[GlobalCleanup]` – data og livssyklus.
- `[DisassemblyDiagnoser]` – se JIT-asm for dyptgående analyse.
- `[HardwareCounters]` – les CPU-tellere (plattformavhengig).

---
<div style="page-break-after:always;"></div>

## Eksempel: Komplett mini-prosjekt

**Program.cs**

```csharp
using BenchmarkDotNet.Running;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run(typeof(Program).Assembly);
    }
}
```

**Benchmarks/CollectionsBench.cs**

```csharp
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser, RankColumn]
public class CollectionsBench
{
    [Params(100, 1_000, 10_000)]
    public int N;

    private List<int> _list = default!;
    private HashSet<int> _set = default!;

    [GlobalSetup]
    public void Setup()
    {
        _list = Enumerable.Range(0, N).ToList();
        _set = new HashSet<int>(_list);
    }

    [Benchmark(Baseline = true)]
    public bool ListContains() => _list.Contains(N - 1);

    [Benchmark]
    public bool SetContains() => _set.Contains(N - 1);
}
```
<div style="page-break-after:always;"></div>

Kjør:

```bash
dotnet run -c Release
```

Forvent at `HashSet.Contains` slår `List.Contains` for større `N`, med lavere `Mean` og færre allokeringer.

---

## Sjekkliste før du konkluderer

- [ ] Kjørt i Release (x64), flere ganger?
- [ ] Input representativt (bruk `[Params]`)?
- [ ] Ser du **både** tidsgevinst og færre allokeringer?
- [ ] Lav `StdDev` og få outliers?
- [ ] Sammenligner epler med epler (samme arbeid)?
- [ ] Har du validert funn i en faktisk ende-til-ende test?

---

Lykke til! Med BenchmarkDotNet får du **troverdige tall** og kan ta bedre, data-drevne beslutninger om ytelse.
