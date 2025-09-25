# 📚 `SortedList<TKey, TValue>` i C# – En praktisk guide

`SortedList<TKey, TValue>` (i `System.Collections.Generic`) er en nøkkel–verdi-kolleksjon som **alltid holder elementene sortert etter nøkkelen**. Den kombinerer egenskaper fra både en `List<T>` (indekstilgang) og et sortert oppslagsverk.

---

## 🧠 Hvorfor bruke `SortedList<TKey, TValue>`?

- **Alltid sortert** etter nøkkel (stigende som standard, eller styrt av en `IComparer<TKey>`).
- **Raskt oppslag** via binærsøk (≈ O(log n)).
- **Indekstilgang**: du kan slå opp **både på nøkkel og på posisjon** (index).
- **Lavt minneforbruk** sammenlignet med `SortedDictionary<TKey,TValue>` for samme antall elementer.

**Ulemper:**

- Innsetting/sletting kan være dyrt (O(n)) fordi elementer flyttes i underliggende arrays.
- Nøkler må være **unike** og bør være **immuterbare** mht. sorteringsordenen.

---

## 🔩 Hvordan fungerer den internt?

`SortedList<TKey, TValue>` er implementert over **to parallelle arrays**: én for nøkler (`TKey[] keys`) og én for verdier (`TValue[] values`).

- **Ved oppslag**: bruker **binærsøk** i `keys` → O(log n).
- **Ved innsetting**:
  1. Finn korrekt indeks med binærsøk.
  2. **Skift** elementer til høyre for å gjøre plass (O(n)).
  3. Sett inn nøkkel og verdi.
- **Ved sletting**:
  1. Finn indeks med binærsøk.
  2. **Skift** elementer til venstre for å fylle hullet (O(n)).

> Kort sagt: `SortedList` er best når du **setter inn mesteparten av dataene først** og deretter **gjør mange oppslag/iterasjoner**, eller når du **trenger indekstilgang**.

---
<div style="page-break-after:always;"></div>

## 📋 Funksjoner/medlemmer og ytelse (inkl. beste valg for søk)

> **Kort svar for søk:** Bruk **`TryGetValue(key, out value)`** for enkeltoppslag (unngår unødige unntak og ekstra arbeid). Bruk **`IndexOfKey(key)`** når du både vil vite **om** nøkkelen finnes *og* hvilken **posisjon** den ligger på.

| Medlem | Type | Formål | Asymptotisk kompleksitet | Notater / Beste bruk |
|---|---|---|---|---|
| `this[TKey key]` (indekserer) | Getter/Setter | Oppslag/oppdatering via nøkkel | Get: **O(log n)**, Set (eksisterende): **O(log n)**, Set (ny): **O(n)** | Kaster `KeyNotFoundException` ved manglende nøkkel på `get`. For ny nøkkel via `set` må elementer flyttes. |
| `Add(key, value)` | Metode | Sett inn nytt par | **O(n)** | Raskt ved små n eller når de fleste innsatsene skjer på forhånd. |
| `TryAdd(key, value)` | Metode | Forsøk å legge til uten unntak | **O(n)** | Returnerer `false` i stedet for å kaste unntak ved duplikat. |
| `ContainsKey(key)` | Metode | Finnes nøkkelen? | **O(log n)** | Binærsøk i nøkkel-array. Brukes hvis du bare trenger en bool. |
| `TryGetValue(key, out value)` | Metode | Hent verdi hvis mulig | **O(log n)** | **Anbefalt for oppslag**: raskt og uten unntak. |
| `ContainsValue(value)` | Metode | Finnes verdien? | **O(n)** | Lineært søk i verdier; unngå hyppig bruk. |
| `IndexOfKey(key)` | Metode | Finn posisjon til nøkkel | **O(log n)** | Bruk når du trenger **indeksen** (kombinert med `Keys[i]`/`Values[i]`). |
| `IndexOfValue(value)` | Metode | Finn posisjon til verdi | **O(n)** | Lineært; dyre operasjoner på store lister. |
| `Remove(key)` | Metode | Fjern element ved nøkkel | **O(n)** | Fjerner og skifter elementer til venstre. |
| `RemoveAt(index)` | Metode | Fjern element ved posisjon | **O(n)** | Nyttig sammen med posisjonsbasert logikk (topp-N, “min-heap”-aktige mønstre). |
| `Clear()` | Metode | Tøm hele listen | **O(n)** | Setter referanser til default og nullstiller teller. |
| `Keys` | Egenskap (`IList<TKey>`) | Indekstilgang til nøkler | Indeks: **O(1)**, iterasjon: **O(n)** | Sortert rekkefølge; støtter `Keys[i]`. |
| `Values` | Egenskap (`IList<TValue>`) | Indekstilgang til verdier | Indeks: **O(1)**, iterasjon: **O(n)** | Samme rekkefølge som `Keys`. |
| `Count` | Egenskap | Antall elementer | **O(1)** | – |
| `Capacity` | Egenskap | Allokert kapasitet | **O(1)** | Kan være > `Count`. |
| `EnsureCapacity(int)` | Metode | Øk kapasitet | Amortisert **O(n)** | Forhåndsallokér for å redusere reallokering ved masseinnsetting. |
| `TrimExcess()` | Metode | Krymp kapasitet | **O(n)** | Reduser minne etter store slettinger. |
| `CopyTo(KeyValuePair<TKey,TValue>[] array, int index)` | Metode | Kopiér til array | **O(n)** | Nyttig for bulk-operasjoner. |
| `GetEnumerator()` / `foreach` | Enumerator | Iterasjon i sortert rekkefølge | **O(n)** | Rekkefølgen følger sortering etter nøkkel. |

**Ytelsestips for søk:**  
- Foretrekk `TryGetValue` for enkeltoppslag.  
- Trenger du *posisjonen*: bruk `IndexOfKey` etterfulgt av `Keys[i]`/`Values[i]`.  
- Skal du gjøre mange oppslag på *sekvensielt økende nøkler*, kan det lønne seg å iterere med indeks i stedet for gjentatte binærsøk.

---

## ⚖️ `SortedList<TKey,TValue>` vs. `SortedDictionary<TKey,TValue>`

| Egenskap | SortedList | SortedDictionary |
|---|---|---|
| Intern struktur | **To arrays (keys/values)** | **Selvbalanserende tre (Red-Black)** |
| Oppslag (`ContainsKey`, indexer) | O(log n) | O(log n) |
| Innsetting/sletting | **O(n)** (skifting i array) | **O(log n)** |
| Indekstilgang (via posisjon) | ✅ Ja (`Keys[i]`, `Values[i]`) | ❌ Nei |
| Minnetoverhead | **Lavere** | Høyere |
| Best for | Mange oppslag, lite mutasjoner, behov for indeks | Hyppige inn-/ut- operasjoner |

---
<div style="page-break-after:always;"></div>

## 🧩 Grunnleggende bruk

```csharp
using System;
using System.Collections.Generic;

var sl = new SortedList<int, string>();

sl.Add(10, "ti");
sl.Add(2, "to");
sl.Add(5, "fem");
// Ligger nå sortert etter nøkkel: 2, 5, 10

Console.WriteLine(sl[5]);            // "fem" (oppslag ved nøkkel)
Console.WriteLine(sl.Values[1]);     // "fem" (oppslag ved indeks 1)
Console.WriteLine(sl.Keys[0]);       // 2

// Trygt oppslag:
if (sl.TryGetValue(10, out var value))
    Console.WriteLine(value);        // "ti"

// Iterasjon går i sortert rekkefølge:
foreach (var kv in sl)
    Console.WriteLine($"{kv.Key} -> {kv.Value}");
```

**Viktige medlemmer:**

- `Add(key, value)`, `Remove(key)`, `RemoveAt(index)`
- `ContainsKey(key)`, `ContainsValue(value)`
- `IndexOfKey(key)`, `IndexOfValue(value)`
- `Keys` (`IList<TKey>`), `Values` (`IList<TValue>`)
- Indekstilgang: `sl[key]` **og** posisjonsindekser: `sl.Keys[i]`, `sl.Values[i]`

> `RemoveAt(index)` er nyttig når du håndterer basert på posisjoner (f.eks. topp-N).

---
<div style="page-break-after:always;"></div>

## 🧮 Kompleksitet (Big-O)

| Operasjon | Kompleksitet |
|---|---|
| Oppslag (`ContainsKey`, `sl[key]`, `TryGetValue`) | O(log n) |
| Innsetting (`Add`, `TryAdd`) | O(n) |
| Sletting (`Remove`, `RemoveAt`) | O(n) |
| Indekstilgang (`Keys[i]`, `Values[i]`) | O(1) |
| Iterasjon | O(n) |

---

## 🔎 `IComparer<TKey>` – hvordan styre sorteringen?

`SortedList` bruker en **komparator** til å bestemme rekkefølgen. Du kan:

1. Bruke **standard** sammenligning (`Comparer<TKey>.Default`). Da må `TKey` støtte `IComparable<TKey>` eller det finnes en standard comparer.
2. Gi inn en egen **`IComparer<TKey>`** i konstruktøren.

### Eksempel: case-insensitiv sortering av strenger

```csharp
var sl = new SortedList<string, int>(StringComparer.OrdinalIgnoreCase);
sl.Add("apple", 1);
sl.Add("Banana", 2);
// "apple" og "APPLE" regnes som samme nøkkel med denne compareren
```

---
<div style="page-break-after:always;"></div>

## 🧑‍💻 Egendefinert type + `IComparer<T>`

La oss si at vi har en `Person` som skal sorteres etter **Alder**, og ved lik alder etter **Navn**.

### 1) Implementer en `IComparer<Person>`

```csharp
using System;
using System.Collections.Generic;

public sealed class Person
{
    public string Navn { get; }
    public int Alder { get; }
    public Person(string navn, int alder) { Navn = navn; Alder = alder; }
    public override string ToString() => $"{Navn} ({Alder})";
}

public sealed class PersonComparer : IComparer<Person>
{
    public int Compare(Person? x, Person? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x is null) return -1;
        if (y is null) return 1;

        int byAlder = x.Alder.CompareTo(y.Alder);
        if (byAlder != 0) return byAlder;

        return StringComparer.OrdinalIgnoreCase.Compare(x.Navn, y.Navn);
    }
}
```

**Bruk i `SortedList<Person, string>`:**

```csharp
var personer = new SortedList<Person, string>(new PersonComparer())
{
    [new Person("Ada", 30)] = "Utvikler",
    [new Person("bert", 30)] = "Analytiker",
    [new Person("Chris", 25)] = "Designer",
};

foreach (var kv in personer)
    Console.WriteLine($"{kv.Key} -> {kv.Value}");
// Sortert: Chris (25), Ada (30), bert (30)
```

### 2) Alternativ: La `Person` implementere `IComparable<Person>`

```csharp
public sealed class Person : IComparable<Person>
{
    public string Navn { get; }
    public int Alder { get; }
    public Person(string navn, int alder) { Navn = navn; Alder = alder; }

    public int CompareTo(Person? other)
    {
        if (other is null) return 1;
        int byAlder = Alder.CompareTo(other.Alder);
        if (byAlder != 0) return byAlder;
        return StringComparer.OrdinalIgnoreCase.Compare(Navn, other.Navn);
    }

    public override string ToString() => $"{Navn} ({Alder})";
}

// Da kan du bare gjøre:
var personer = new SortedList<Person, string>  // bruker Comparer<Person>.Default
{
    [new Person("Ada", 30)] = "Utvikler",
    [new Person("bert", 30)] = "Analytiker",
    [new Person("Chris", 25)] = "Designer",
};
```

> **Tips:** Bruk `IComparer<T>`-varianten når du vil ha **flere sorteringsstrategier** uten å bake dem inn i klassen.

---
<div style="page-break-after:always;"></div>

## 🧷 Vanlige fallgruver og tips

- **Endre ikke nøkkelen** etter at den er lagt inn i `SortedList` hvis endringen påvirker sorteringsrekkefølgen. Det **bryter** invariantene (samlingen re-sorterer ikke automatisk det elementet).
- `ContainsValue` er **O(n)** – unngå ofte kall; vurder en omvendt indeks hvis du trenger raskt oppslag på verdi.
- Ved **mange innsettinger** underveis, vurder `SortedDictionary<TKey,TValue>`.
- Trenger du **posisjons-basert** tilgang (topp-k, binærsøk-indekser), er `SortedList` som regel bedre.
- **Trådsikkerhet:** Samlingen er **ikke** trådsikker. Synkroniser ved parallell bruk.
- **Duplikatnøkler** er ikke tillatt. Bruk `TryAdd` i nyere .NET hvis du vil unngå `ArgumentException` ved kollisjon.

```csharp
if (!sl.TryAdd(5, "fem"))
{
    // Håndter duplikat
}
```

---

## 🔧 Praktiske oppskrifter

### Finn nærmeste nøkkel (binærsøk + indekser)

```csharp
int FinnNærmesteNedre(SortedList<int, string> sl, int key)
{
    int idx = sl.IndexOfKey(key);
    if (idx >= 0) return sl.Keys[idx];       // eksakt treff
    idx = ~idx; // bitkomplement: innsettingsposisjon
    return (idx > 0) ? sl.Keys[idx - 1] : sl.Keys[0];
}
```

### Hent “topp N” etter nøkkel (minste/største)

```csharp
// Minste N nøkler
IEnumerable<KeyValuePair<int,string>> MinsteN(SortedList<int,string> sl, int n) =>
    sl.Take(Math.Min(n, sl.Count));

// Største N nøkler
IEnumerable<KeyValuePair<int,string>> StørsteN(SortedList<int,string> sl, int n)
{
    for (int i = sl.Count - 1; i >= Math.Max(0, sl.Count - n); i--)
        yield return new KeyValuePair<int, string>(sl.Keys[i], sl.Values[i]);
}
```

---
<div style="page-break-after:always;"></div>

## ✅ Oppsummering

- `SortedList<TKey,TValue>` holder data **sortert etter nøkkel**, med **binærsøk** for oppslag og **arrays** under panseret.
- **Innsetting/sletting er O(n)** pga. skifting i array – flott for **lesetunge** scenarier.
- Bruk **`IComparer<TKey>`** for tilpasset sortering (eller `IComparable<T>` på nøkkelen).
- Velg `SortedDictionary` hvis du **muterer ofte**, men `SortedList` hvis du **trenger indekstilgang** og/eller vil spare minne.

---

## ✍️ Fullt eksempel: personer sortert etter etternavn, deretter fornavn

```csharp
using System;
using System.Collections.Generic;

public sealed class Person
{
    public string Fornavn { get; }
    public string Etternavn { get; }
    public Person(string fornavn, string etternavn) =>
        (Fornavn, Etternavn) = (fornavn, etternavn);
    public override string ToString() => $"{Fornavn} {Etternavn}";
}

public sealed class PersonNavnComparer : IComparer<Person>
{
    public int Compare(Person? x, Person? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x is null) return -1;
        if (y is null) return 1;

        int byEtternavn = StringComparer.OrdinalIgnoreCase.Compare(x.Etternavn, y.Etternavn);
        if (byEtternavn != 0) return byEtternavn;

        return StringComparer.OrdinalIgnoreCase.Compare(x.Fornavn, y.Fornavn);
    }
}

public static class Program
{
    public static void Main()
    {
        var personer = new SortedList<Person, string>(new PersonNavnComparer())
        {
            [new Person("Ada", "Lovelace")] = "Matematiker",
            [new Person("Alan", "Turing")]  = "Informatiker",
            [new Person("Grace", "Hopper")] = "Pioner",
            [new Person("Barbara", "Liskov")] = "Professor",
        };

        // Indekstilgang
        Console.WriteLine(personer.Keys[0]);   // Barbara Liskov
        Console.WriteLine(personer.Values[0]); // Professor

        // Iterasjon i sortert rekkefølge
        foreach (var kv in personer)
            Console.WriteLine($"{kv.Key} -> {kv.Value}");
    }
}
```

---

### Videre lesning

- `System.Collections.Generic.SortedList<TKey,TValue>` i .NET-dokumentasjonen
- `IComparer<T>` og `IComparable<T>`
