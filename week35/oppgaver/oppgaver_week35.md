# Oppgave: Benchmark av `List<T>` og Kapasitet

I denne oppgaven skal du undersøke hvordan kapasiteten til en `List<T>` påvirker ytelse og minnebruk.

---

## Del 1 – Kode

**Oppgave:**  
Skriv en BenchmarkDotNet-klasse som:

- Har en `[Params]`-property for startkapasitet (for eksempel `0, 100, 1_000, 10_000`).
- Har en konstant som angir hvor mange elementer som skal legges til (for eksempel 10 000).
- Inneholder to `[Benchmark]`-metoder:
  - Én som oppretter en liste **med** startkapasitet (`new List<int>(Capacity)`) og fyller den med elementer.
  - Én som oppretter en liste **uten** å sette startkapasitet (`new List<int>()`) og fyller den med elementer.

Eksempelstruktur (du må selv implementere):
```csharp
[MemoryDiagnoser, RankColumn]
public class ListWithCapacityBenchmark
{
    [Params(0, 100, 1_000, 10_000)]
    public int Capacity { get; set; }

    private const int _listSize = 10_000;

    [Benchmark]
    public void RunListCapacityBenchmark()
    {
        // Lag ny liste med gitt kapasitet og fyll med _listSize elementer
    }

    [Benchmark]
    public void RunListWithoutCapacityBenchmark()
    {
        // Lag ny liste uten startkapasitet og fyll med _listSize elementer
    }
}
```
Kjør benchmarken med BenchmarkDotNet (`BenchmarkRunner.Run<...>();`) og sørg for at `[MemoryDiagnoser]` er aktivert for å se minnebruk.

---
<div style="page-break-after: always;"></div>

## Del 2 – Refleksjonsspørsmål

Når du har kjørt benchmarken og fått resultater, reflekter over følgende spørsmål:

1. Hvordan endrer **Mean (tid)** og **Allocated (minne)** seg når du varierer kapasiteten?
2. Hva skjer under panseret når en `List<T>` går tom for plass? Hvordan håndterer .NET dette?
3. Hvorfor kan det være lurt å sette kapasiteten hvis du vet hvor mange elementer du trenger?
4. Hva er ulempene ved å sette en altfor høy kapasitet i forhold til faktisk bruk?
5. Hvilke scenarier i praksis kan dra nytte av at du setter kapasitet eksplisitt?

---
