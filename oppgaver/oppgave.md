## **Oppgave 1 – Benchmarking**

### Beskrivelse

Lag en funksjon `SumNumbers(int n)` som returnerer summen av alle heltall fra 1 opp til `n`.  

Eksempel:  

```csharp
SumNumbers(6) => 21   // fordi 1 + 2 + 3 + 4 + 5 + 6 = 21
```
---

## Oppgave 1.1 – Implementasjoner

Lag minst **tre** ulike varianter:

1. **Iterativ med for-løkke**  
   Bruk en enkel `for`-løkke til å summere tallene.  

2. **While-løkke**  
   Bruk `while` i stedet for `for`.  

3. **LINQ-basert**  
   Bruk `Enumerable.Range` og `Sum()`.  

---

## Oppgave 1.2 – Benchmarking

Mål ytelsen for hver implementasjon ved hjelp av et benchmarking-verktøy (for eksempel **BenchmarkDotNet**).  
Dokumentér resultatene.

---

## Oppgave 1.3 – Kjør og analyser

Kjør benchmarkene i **Release**-modus og analyser resultatene:

- `Mean` (gjennomsnittlig kjøretid)  
- `StdDev` (variasjon i målingene)  
- `Allocated` (minnebruk)  

**Diskuter:**

- Hvilken implementasjon er raskest for liten `n`?  
- Hva skjer når `n` blir veldig stort?  
- Er det forskjell på de ulike tilnærmingene? Hvis ja, hvilke?  
- Finnes det en løsning som **slår alle** i ytelse og minnebruk?  

---

## Oppgave 1.4 – Manuell ytelsestest

Utfør en egen måling med et stoppeklokke-verktøy (for eksempel `Stopwatch`):

- Velg én av implementasjonene.  
- Kjør funksjonen mange ganger i en løkke (f.eks. 100 000 repetisjoner).  
- Mål hvor lang tid det tar totalt.  
- Gjenta for ulike verdier av `n` og ulike antall repetisjoner.  

**Reflekter:**

- Hvordan øker kjøretiden når `n` blir større?  
- Følger den et lineært mønster (`O(n)`), er den konstant (`O(1)`), logaritmisk (`O(log n)`) eller noe annet?  
- Begrunn svaret med målingene dine. 