# Innføring i datastrukturer i C# 

Denne guiden gir en praktisk oversikt over sentrale datastrukturer i C#, når du bør bruke dem, typiske kompleksiteter og små kodeeksempler.

## Minnehåndtering i C#

- **Stack:** Hurtigminne som brukes for lokale variabler og verdityper. Livsløpet styres automatisk av kallestakken.
- **Heap:** Dynamisk minneområde styrt av .NET Garbage Collector. Referansetyper lagres her.

Merk: Et **objekt** ligger alltid på heap, men en **referanse** til objektet kan ligge på stack (f.eks. en lokal variabel som peker til et `List<T>`).

---
<div style="page-break-after:always;"></div>

## 1) Array (tabell)

**Kort:** Fast lengde, elementer tilgjengelig via indeks. God plass- og tidsbruk for sekvensiell lagring og tilfeldig tilgang.

**ASCII**

```
Index:  0    1    2    3
       [10] [20] [30] [40]
```

- Direkte indeks-tilgang (`arr[i]`).

**Når bruke:** Når antall elementer er kjent, og du trenger rask indeksbasert tilgang.

**Kompleksitet (typisk):**

- Tilgang via indeks: `O(1)`  
- Søk lineært: `O(n)`  
- Innsetting/sletting: `O(n)` (må ofte flytte elementer)

**Eksempel**

```csharp
int[] numbers = new int[4] { 10, 20, 30, 40 };
Console.WriteLine(numbers[2]); // 30
Array.Resize(ref numbers, 5);  // creates a new array
numbers[4] = 50;
```

**Fallgruver:** Fast lengde; `Array.Resize` lager nytt array. Unngå hyppige resizes.

- **Minne:** Kontiguøst område på heap (selv om elementene kan være verdityper eller referanser).
- **Hvis verdityper (int, double, struct):** Selve verdiene lagres i arrayet (kontiguøst).
- **Hvis referansetyper (objekter, string):** Arrayet inneholder referanser, mens objektene ligger spredt på heap.
---
<div style="page-break-after:always;"></div>

## 2) List<T> (liste)

**Kort:** Dynamisk liste som utvider kapasitet automatisk. Praktisk standard-beholder.

**ASCII**

```
[alpha] -> [beta] -> [gamma]  (logical order)
```

**Når bruke:** Generell samling med mye innsetting bakerst og rask indeks-tilgang.

**Kompleksitet (typisk):**

- Tilgang via indeks: `O(1)`
- Legge til bakerst (amortisert): `O(1)`
- Innsetting/sletting midt i: `O(n)`
- Søk lineært: `O(n)`

**Eksempel**

```csharp
var list = new List<string> { "alpha", "beta" };
list.Add("gamma");
list.Insert(1, "middle");   // shifts elements
list.Remove("alpha");
Console.WriteLine(list[0]);  // index access
```

**Fallgruver:** Innsetting/sletting i midten er dyrt; vurder andre strukturer hvis dette skjer ofte.

- **Minne:** Intern implementasjon basert på et underliggende array på heap.
- **Kontiguøst?** Ja – elementene lagres i et array, så verdityper er lagret tett. Ved referansetyper er det pekere som peker rundt i heap.
- **Resizing:** Når kapasiteten overskrides, lages et nytt større array, og elementene kopieres.
- **Fordel:** Rask indeksaksess, bra cache-lokalitet (for verdityper).
- **Ulempe:** Innsetting midt i lista er dyrt pga. flytting.
  
---
<div style="page-break-after:always;"></div>

## 3) Stack<T> (stakk) – LIFO

**Kort:** Last-In, First-Out. Brukes til funksjonskall, historikk, backtracking.

**ASCII**

```
Top
 ┌─────┐
 │  3  │ <- Peek/Pop
 │  2  │
 │  1  │
 └─────┘
Bottom
```

**Operasjoner:** `Push`, `Pop`, `Peek`

**Kompleksitet (typisk):**

- `Push/Pop/Peek`: `O(1)`

**Eksempel**

```csharp
var stack = new Stack<int>();
stack.Push(1);
stack.Push(2);
stack.Push(3);
Console.WriteLine(stack.Peek()); // 3
Console.WriteLine(stack.Pop());  // 3
```

**Fallgruver:** `Pop/Peek` kaster unntak hvis tom; bruk `TryPop/TryPeek` ved behov.

- **Minne:** Bygger på `List<T>` internt (dvs. array på heap).
- **Kontiguøst:** Ja, verdityper lagres tett i arrayet.
- **Bruk:** LIFO-operasjoner (`Push`, `Pop`).
  
---
<div style="page-break-after:always;"></div>

## 4) Queue<T> (kø) – FIFO

**Kort:** First-In, First-Out. Brukes til køsystemer, bredde-først-søk (BFS).

**ASCII**

```
Front -> [first] -> [second] -> [third] -> Back
```

**Operasjoner:** `Enqueue`, `Dequeue`, `Peek`

**Kompleksitet (typisk):**

- `Enqueue/Dequeue/Peek`: `O(1)`

**Eksempel**

```csharp
var queue = new Queue<string>();
queue.Enqueue("first");
queue.Enqueue("second");
Console.WriteLine(queue.Peek());    // first
Console.WriteLine(queue.Dequeue()); // first
```

**Fallgruver:** `Dequeue/Peek` kaster unntak hvis tom; bruk `TryDequeue` i nyere .NET.

- **Minne:** Implementert som sirkulært array på heap.
- **Kontiguøst:** Ja, array på heap med "wrap-around"-indeksering.
- **Bruk:** FIFO-operasjoner (`Enqueue`, `Dequeue`).
  
---
<div style="page-break-after:always;"></div>

## 5) LinkedList<T> (lenket liste)

**Kort:** Noder koblet med pekere. Rask innsetting/sletting *når du allerede har en node-referanse*. Dårlig tilfeldig tilgang.

**ASCII**

```
Head -> [A] <-> [B] <-> [C] <- Tail
```

**Når bruke:** Når du ofte setter inn/fjerner elementer på kjente steder (foran/bak/ved node), og du ikke trenger indeks-tilgang.

- Dobbeltlenket: kan traversere begge veier.
- Bedre enn `List<T>` for hyppige innsettinger i midten **hvis** du har node-referanser.

**Kompleksitet (typisk):**

- Innsetting/sletting *gitt node*: `O(1)`
- Finne element eller posisjon: `O(n)` (sekvensiell traversering)
- Tilgang via indeks: `O(n)`

**Eksempel**

```csharp
var linked = new LinkedList<string>();
var a = linked.AddLast("A");
var b = linked.AddLast("B");
linked.AddFirst("Start");         // O(1)
linked.AddAfter(a, "A2");         // O(1) given node
linked.Remove(b);                 // O(1) given node

foreach (var item in linked)
    Console.WriteLine(item);
```

**Fallgruver:** Høyere minneoverhead per element, dårlig cache-lokalitet, treg `Contains` og indeks-tilgang.

- **Minne:** Hver node er et eget objekt på heap, med pekere til neste/forrige node.
- **Kontiguøst?** Nei – elementene kan være spredt på heap.
- **Fordel:** O(1) innsetting/sletting hvis du har en node.
- **Ulempe:** Dårlig cache-lokalitet og høyere overhead (ekstra pekere per node).

---
<div style="page-break-after:always;"></div>

## 6) Tree (tre) – hierarkisk struktur

**Kort:** Noder koblet i forelder–barn-forhold. Vanlig variant: binært søketre (BST) for sortert data.

**ASCII (binært tre)**

```
      [8]
     /   \
   [3]   [10]
   / \     \
 [1] [6]    [14]
```

**Når bruke:** Hierarkisk data (filsystem, UI-hierarki), søk/innsetting i sortert struktur, heap/priority queue.

**BST-kompleksitet (balansert):**

- Søk/innsetting/sletting: `O(log n)`; i verste fall (skjevt): `O(n)`  
→ Bruk selvbalanserende trær via `SortedSet<T>` eller `SortedDictionary<TKey,TValue>` når mulig.

**Eksempel (enkel inorder):**

```csharp
public class Node
{
    public int Value;
    public Node? Left;
    public Node? Right;
    public Node(int value) => Value = value;
}

public static void InOrder(Node? root)
{
    if (root == null) return;
    InOrder(root.Left);
    Console.Write($"{root.Value} ");
    InOrder(root.Right);
}
```

**.NET-alternativer:** `SortedSet<T>`, `SortedDictionary<TKey,TValue>` (selvbalanserende).

- **Minne:** Hver node er et objekt på heap med referanser til barn.
- **Kontiguøst?** Nei – noder kan ligge hvor som helst på heap.
- **BST/AVL/Red-Black:** Selvhåndhevet balanse sikrer `O(log n)`-operasjoner.
- **Fordel:** God for søk, sortert data, hierarki.
- **Ulempe:** Mindre cache-vennlig enn arrays/lister.

---
<div style="page-break-after:always;"></div>


## 7) Heap (prioritetskø / binærheap)

**Kort:** Heap er en spesialisert trestruktur (ofte binærheap) som brukes til å holde orden på elementer etter prioritet. Rask tilgang til minste/største element.

**ASCII (min-heap):**

```
      [1]
     /   \
   [3]   [5]
   / \   /
 [8] [6] [9]
```

**Når bruke:** Når du trenger å hente ut det minste/største elementet raskt, f.eks. i prioriterte køer, sortering (heapsort), eller algoritmer som Dijkstra.

**Kompleksitet (typisk):**
- Hente ut min/max: `O(1)`
- Legge til/fjerne: `O(log n)`

**Eksempel (bruk av PriorityQueue i .NET):**

```csharp
var pq = new PriorityQueue<string, int>();
pq.Enqueue("lavest", 1);
pq.Enqueue("middels", 5);
pq.Enqueue("høyest", 10);
Console.WriteLine(pq.Dequeue()); // "lavest"
```

**Fallgruver:** Ikke rask tilfeldig tilgang. Brukes kun for prioritert inn/ut.

- **Implementasjon i .NET:** `PriorityQueue<TElement,TPriority>`.
- **Minne:** Basert på et array som representerer en binærheap på heap.
- **Kontiguøst?** Ja – arraybasert representasjon.
- **Fordel:** Rask `Enqueue/Dequeue` av minste/største element (`O(log n)`).
- **Ulempe:** Ikke rask indeksaksess som `List<T>`.

---
<div style="page-break-after:always;"></div>

## 8) Graph (graf) – noder og kanter

**Kort:** Modell av forbindelser. Kan være rettet/urettet, med/uten vekter.

**ASCII (urettet graf)**

```
  (0)----- (1)
   | \      \
   |  \      (3)
   |   \
  (2)---
```

**Representasjon:**

- **Adjacency list**: `Dictionary<T, List<T>>` – plasseffektiv for spredte grafer.
- **Adjacency matrix**: `bool[,]` – rask kantspørring `O(1)`, men `O(n^2)` plass.

**BFS (adjacency list):**

```csharp
var graph = new Dictionary<int, List<int>>
{
    [0] = new() { 1, 2 },
    [1] = new() { 0, 3 },
    [2] = new() { 0, 3 },
    [3] = new() { 1, 2 }
};

IEnumerable<int> Bfs(int start)
{
    var visited = new HashSet<int>();
    var q = new Queue<int>();
    visited.Add(start);
    q.Enqueue(start);

    while (q.Count > 0)
    {
        int u = q.Dequeue();
        yield return u;
        if (!graph.TryGetValue(u, out var neighbors)) continue;
        foreach (var v in neighbors)
            if (visited.Add(v)) q.Enqueue(v);
    }
}

foreach (var v in Bfs(0))
    Console.WriteLine(v);
```

- **Minne:** Avhenger av representasjon:
  - **Adjacency list:** Dictionary/HashSet/Lists – noder spredt på heap.
  - **Adjacency matrix:** 2D array på heap (kontiguøst).
- **Kontiguøst?** Kun for matriser. Adjacency list kan være fragmentert.
  
---
<div style="page-break-after:always;"></div>

## Når skal man velge hva? (tommelregler)

- **Array:** Fast størrelse, maksimal hastighet på indeksaksess, lavt overhead.
- **List<T>:** Standardvalg for variabel mengde data med mye append/iterasjon.
- **LinkedList<T>:** Hyppige innsettinger/slettinger ved kjente noder/kanter (foran/bak) – *ikke* for tilfeldig indeks-tilgang.
- **Stack<T>:** LIFO-oppgaver (parser, undo/redo, backtracking).
- **Queue<T>:** FIFO-oppgaver (planlegging, BFS).
- **Tree:** Sortert data med logaritmiske operasjoner; bruk innebygde sorterte strukturer for balanse.
- **Graph:** Nettverk, ruter, sammenkoblinger; velg adjacency list for spredte grafer.

---

## Big-O-oversikt (grovt)

| Struktur          | Tilgang | Søk     | Innsetting            | Sletting             |
|------------------|---------|---------|-----------------------|----------------------|
| Array            | O(1)    | O(n)    | O(n)                  | O(n)                 |
| List<T>          | O(1)    | O(n)    | O(1)\* (bak) / O(n)   | O(n)                 |
| Stack<T>         | –       | –       | O(1)                  | O(1)                 |
| Queue<T>         | –       | –       | O(1)                  | O(1)                 |
| LinkedList<T>    | O(n)    | O(n)    | O(1) (med node)       | O(1) (med node)      |
| BST (balansert)  | O(log n)| O(log n)| O(log n)              | O(log n)             |
| Graf (adj. list) | –       | –       | O(1) per kant         | O(1) per kant        |

\* Amortisert tid ved `Add` bakerst i `List<T>`.

