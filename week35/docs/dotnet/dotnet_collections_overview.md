# .NET Collections for Common Data Structures (C# / ASP.NET Core)

Dette dokumentet gir en praktisk oversikt over vanlige **.NET collections** som støtter grunnleggende datastrukturer du bruker i ASP.NET Core-prosjekter. For hver struktur får du: **beskrivelse**, **når bruke**, og **korte C#-eksempler**.

> Navnerom som ofte brukes:  
> `using System.Collections.Generic;`  
> `using System.Collections.Concurrent;`  
> `using System.Collections.ObjectModel;`  
> `using System.Collections.Immutable;` (krever NuGet-pakken **System.Collections.Immutable**)

---

## 1) Sekvenser: Array & List

### `T[]` (Array)

- **Beskrivelse:** Sammenhengende minne, fast lengde, O(1) indeks-tilgang.
- **Når bruke:** Når antall elementer er kjent på forhånd eller sjelden endres; ytelseskritiske scenarier (minne/CPU).
- **Eksempel:**

```csharp
int[] a = new[] { 10, 20, 30 };
Console.WriteLine(a[1]); // 20
```

### `List<T>`

- **Beskrivelse:** Dynamisk array under panseret; god generell standardcontainer.
- **Når bruke:** Standardvalg for variabel størrelse, god O(1) tilgang og rask append.
- **Eksempel:**

```csharp
var list = new List<string> { "Ola", "Kari" };
list.Add("Nils");
Console.WriteLine(list[2]); // Nils
```
<div style="page-break-after:always;"></div>

### `ReadOnlyCollection<T>`

- **Beskrivelse:** Wrapper som eksponerer en liste som **read-only** (beskytter mot mutasjoner fra kallere).
- **Når bruke:** API-grenser der du vil gi tilgang uten å tillate endringer.
- **Eksempel:**

```csharp
var numbers = new List<int> { 1, 2, 3 };
var ro = new System.Collections.ObjectModel.ReadOnlyCollection<int>(numbers);
// ro.Add(4); // finnes ikke
```

### `ImmutableArray<T>` / `ImmutableList<T>` *(Immutable Collections)*

- **Beskrivelse:** Uforanderlige varianter; mutasjoner returnerer nye forekomster.
- **Når bruke:** Tråd-sikkerhet, funksjonell stil, deling mellom tråder uten låsing.
- **Eksempel:**

```csharp
using System.Collections.Immutable;

var immList = ImmutableList<int>.Empty.Add(1).Add(2);
var newList = immList.Add(3); // immList uendret
```

---

## 2) Lenket liste

### `LinkedList<T>`

- **Beskrivelse:** Dobbeltlenket liste; rask innsetting/fjerning gitt en node; O(n) for indeksert tilgang.
- **Når bruke:** Hyppige innsettinger/flyttinger i midten; når du manipulerer noder direkte.
- **Eksempel:**

```csharp
var ll = new LinkedList<int>();
ll.AddLast(1);
ll.AddLast(2);
ll.AddFirst(0);
var node = ll.Find(1);
ll.AddAfter(node!, 5); // 0,1,5,2
```

---
<div style="page-break-after:always;"></div>

## 3) Stakk og Kø

### `Stack<T>` (LIFO)

- **Beskrivelse:** Sist inn, først ut.
- **Når bruke:** Parsing, backtracking, kallstakk-lignende oppgaver.
- **Eksempel:**

```csharp
var st = new Stack<char>();
st.Push('A'); st.Push('B');
Console.WriteLine(st.Pop()); // B
```

### `Queue<T>` (FIFO)

- **Beskrivelse:** Først inn, først ut.
- **Når bruke:** Meldingskøer, breadth-first-prosesser.
- **Eksempel:**

```csharp
var q = new Queue<string>();
q.Enqueue("første"); q.Enqueue("andre");
Console.WriteLine(q.Dequeue()); // første
```

### `ConcurrentQueue<T>` / `ConcurrentStack<T>` *(trådsikre)*

- **Beskrivelse:** Låsefrie, skalerer godt ved parallell tilgang.
- **Når bruke:** Produsent/konsument-mønstre i ASP.NET Core-bakgrunnstjenester.
- **Eksempel:**

```csharp
var cq = new System.Collections.Concurrent.ConcurrentQueue<int>();
cq.Enqueue(42);
if (cq.TryDequeue(out var x)) { /* bruk x */ }
```

### `System.Threading.Channels` *(bonus)*

- **Beskrivelse:** Høyytelses prod/cons-primitive for asynk strømmer.
- **Når bruke:** Når du trenger backpressure og asynkron behandling.

---
<div style="page-break-after:always;"></div>

## 4) Mengder og Maps (Set/Dictionary)

### `Dictionary<TKey,TValue>`

- **Beskrivelse:** Hash-basert key→value, O(1) oppslag i snitt.
- **Når bruke:** Generelt oppslagsbehov, caching i runtime, konfig/parametre.
- **Eksempel:**

```csharp
var dict = new Dictionary<string,int> { ["A"]=1, ["B"]=2 };
Console.WriteLine(dict["A"]); // 1
dict.TryGetValue("C", out var val); // false, val=0
```

### `HashSet<T>`

- **Beskrivelse:** Mengde av unike elementer, O(1) medlemskap i snitt.
- **Når bruke:** Fjern duplikater, rask medlemskapstest.
- **Eksempel:**

```csharp
var set = new HashSet<string> { "A", "B" };
set.Add("B"); // ignorert, finnes fra før
Console.WriteLine(set.Contains("A")); // true
```

### `SortedDictionary<TKey,TValue>` (rød-svart-tre)

- **Beskrivelse:** Sortert map basert på balansert tre.
- **Når bruke:** Når du trenger **sortert iterasjon**/range-forespørsler og O(log n) operasjoner.
- **Eksempel:**

```csharp
var sd = new SortedDictionary<int,string>();
sd[10]="ti"; sd[1]="en"; sd[5]="fem";
// itererer i sortert rekkefølge: 1,en; 5,fem; 10,ti
```

### `SortedList<TKey,TValue>`

- **Beskrivelse:** Sorterte nøkler/verdier lagret i arrays; binærsøk under panseret.
- **Når bruke:** Statisk-ish datasett med hyppige oppslag, få innsettinger/slettinger.
- **Eksempel:**

```csharp
var sl = new SortedList<int,string> { {2,"to"},{1,"en"} };
Console.WriteLine(sl.Keys[0]); // 1
```
<div style="page-break-after:always;"></div>

### `SortedSet<T>`

- **Beskrivelse:** Sortert unik mengde (RB-tree), støtter range/ordered operasjoner.
- **Når bruke:** Når rekkefølge og unikhet er viktig, med O(log n) operasjoner.
- **Eksempel:**

```csharp
var ss = new SortedSet<int> { 5,1,3 };
// itererer: 1,3,5
```

### `ImmutableDictionary/ImmutableHashSet/ImmutableSortedSet` *(Immutable)*

- **Beskrivelse:** Uforanderlige varianter for tråd-sikker deling uten låser.
- **Når bruke:** Deling på tvers av request-scopes/bakgrunnsjobber uten låsing.

---

## 5) Prioritet og Heaps

### `PriorityQueue<TElement,TPriority>` (.NET 6+)

- **Beskrivelse:** Min-heap-basert prioritetskø.
- **Når bruke:** Ta alltid ut «minst»/«høyest prioritert» i O(log n).
- **Eksempel:**

```csharp
var pq = new PriorityQueue<string,int>();
pq.Enqueue("lav", 10);
pq.Enqueue("høy", 1);
Console.WriteLine(pq.Dequeue()); // "høy"
```

---

## 6) Observert/Bindbare Collections

### `ObservableCollection<T>`

- **Beskrivelse:** Reiser `CollectionChanged`-events på endringer.
- **Når bruke:** MVVM/GUI-binding (WPF/Blazor-interop); ikke primært for serverside-API-er.
- **Eksempel:**

```csharp
var oc = new System.Collections.ObjectModel.ObservableCollection<int>();
oc.CollectionChanged += (s,e) => Console.WriteLine($"Action: {e.Action}");
oc.Add(1);
```

---
<div style="page-break-after:always;"></div>

## 7) Trådsikre Collections (Concurrent)

- **`ConcurrentDictionary<TKey,TValue>`**
  - **Når:** Høy-konkurranse oppslag/inserts i bakgrunnstjenester.
  - **Eksempel:**

    ```csharp
    var cd = new System.Collections.Concurrent.ConcurrentDictionary<string,int>();
    cd.AddOrUpdate("A", 1, (_,old) => old + 1);
    ```

- **`ConcurrentBag<T>`**
  - **Når:** Uordnet pose/samling for work-stealing scenarier.
- **`BlockingCollection<T>`**
  - **Når:** Produsent/konsument med blokkering/timeout over en underliggende collection.

---

## 8) Grafer og Trær (ingen direkte standardtype)

> .NET har ikke en «Graph»/«Tree»-klasse i BCL. Vanlige representasjoner:

- **Graf (naboliste):**

```csharp
var graph = new Dictionary<string, List<string>>();
graph["A"] = new List<string> { "B", "C" };
graph["B"] = new List<string> { "A", "D" };
```

- **Tre (node-klasse):**

```csharp
class Node<T> { public T Value; public Node<T>? Left, Right; public Node(T v){ Value=v; } }
```

- **Sortert struktur som tre:** `SortedDictionary` / `SortedSet` (under panseret balansert tre).

---
<div style="page-break-after:always;"></div>

## 9) Valgveiledning (når bruke hva?)

| Behov | Anbefalt |
|---|---|
| Generell samling, rask indeks-tilgang, ofte append | `List<T>` |
| Fast størrelse, minne/ytelse-kritisk | `T[]` |
| FIFO-kø | `Queue<T>` / `ConcurrentQueue<T>` |
| LIFO-stakk | `Stack<T>` / `ConcurrentStack<T>` |
| Unike elementer (uordnet) | `HashSet<T>` |
| Oppslag key→value | `Dictionary<TKey,TValue>` / `ConcurrentDictionary<,>` |
| Sortert iterasjon / range-queries | `SortedDictionary<,>` / `SortedSet<T>` |
| Prioritetskø | `PriorityQueue<TElement,TPriority>` |
| Høy endringsfrekvens midt i lista | `LinkedList<T>` |
| Tråd-sikre og uten låser | `Immutable*`-collections |

---

## 10) Små ytelsesnotater

- `List<T>` er svært effektiv i praksis (kontiguous minne, god cache-lokalitet).
- `Dictionary`/`HashSet` er O(1) i snitt, men avhenger av god hashing (`GetHashCode`/`Equals`).
- `Sorted*`-collections gir O(log n)-operasjoner og opprettholder sortert rekkefølge.
- For parallelle scenarier i ASP.NET Core: foretrekk `Concurrent*` eller `Immutable*` fremfor egne låser.

---
<div style="page-break-after:always;"></div>

## 11) Bonus: Binærsøk og sortering i .NET

- `Array.BinarySearch(array, value)` og `list.BinarySearch(value)` (krever sortert samling).
- `Array.Sort` / `List<T>.Sort` / `Enumerable.OrderBy` for sortering.

```csharp
var data = new List<int> { 5, 1, 9, 2 };
data.Sort();                   // 1,2,5,9
int idx = data.BinarySearch(5); // indeks for 5
```

---

### Oppsummering

- Start med `List<T>` og `Dictionary<TKey,TValue>` som «go-to».
- Trenger du orden: `Sorted*`-variantene.  
- Trenger du prioritetskø: `PriorityQueue`.
- Trenger du tråd-sikkerhet: `Concurrent*` eller `Immutable*`.
- Tilpass valg etter **tilgangsmønster**, **mutasjonsfrekvens** og **tråd-modell** i appen din.
