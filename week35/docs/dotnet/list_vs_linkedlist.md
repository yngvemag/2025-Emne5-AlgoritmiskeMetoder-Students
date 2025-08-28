# Forskjellen på List<T> og LinkedList<T> i C #

Denne guiden forklarer forskjellen på `List<T>` og `LinkedList<T>` i C#, hvordan de fungerer under panseret, og hvordan de lagres på heapen.

---

## List<T>

- **Array-basert implementasjon**  
  Internt bruker `List<T>` et array på heapen.
- **Kontinuerlig minne** → god cache-lokalitet (rask iterasjon).
- Når listen vokser og det ikke er plass → nytt array opprettes på heapen og gamle elementer kopieres dit.

### Visualisering

```
Heap (kontinuerlig blokk):

[ 10 ][ 20 ][ 30 ][ 40 ] . . .
   ↑
   List<T> peker på dette arrayet
```

### Eksempel

```csharp
var list = new List<int>();
list.Add(1);
list.Add(2);
list.Add(3);

Console.WriteLine(list[1]); // O(1) access -> 2
list.Insert(1, 99); // dyrt for store lister
```

### Egenskaper

- Rask tilfeldig tilgang `O(1)`
- Innsetting/sletting midt i listen er dyrt `O(n)`
- Iterasjon er svært rask (sekvensiell i minnet)

---
<div style="page-break-after:always;"></div>

## LinkedList<T>

- **Noder koblet med pekere**  
  Hver node lagres separat på heapen, og har pekere til neste og forrige node.
- **Ingen kontinuerlig blokk** → dårlig cache-lokalitet (tregere iterasjon).

### Visualisering

```
Heap (separate noder):

Node1(Value=10, Next=ptr2, Prev=null)
Node2(Value=20, Next=ptr3, Prev=ptr1)
Node3(Value=30, Next=null, Prev=ptr2)

LinkedList<T> holder referanse til head/tail
```

### Eksempel

```csharp
var linked = new LinkedList<int>();
linked.AddLast(1);
linked.AddLast(2);
linked.AddLast(3);

var node = linked.Find(2);
linked.AddAfter(node, 99); // O(1) given node reference

foreach (var val in linked)
    Console.WriteLine(val);
```

### Egenskaper

- Innsetting/sletting er `O(1)` **hvis du har node-referanse**
- Tilfeldig tilgang er treg (`O(n)`)
- Høyere minnebruk per element (pekere til neste/forrige)

---
<div style="page-break-after:always;"></div>

## Heap og minneforskjeller

- **List<T>**:  
  Ett stort array på heapen (kontinuerlig minne). Effektivt og cache-vennlig.

- **LinkedList<T>**:  
  Mange små objekter på heapen (noder). Mer minneoverhead og dårlig cache-lokalitet.

---

## Sammenligningstabell

| Egenskap                 | List<T>                     | LinkedList<T>               |
|---------------------------|-----------------------------|------------------------------|
| Intern struktur          | Array (kontinuerlig)        | Dobbeltlenket liste (noder) |
| Tilfeldig tilgang        | `O(1)`                      | `O(n)`                       |
| Iterasjon                | Rask (cache-vennlig)        | Tregere (pekere)             |
| Innsetting bak           | Amortisert `O(1)`           | `O(1)`                       |
| Innsetting midt          | `O(n)`                      | `O(1)` gitt node             |
| Sletting midt            | `O(n)`                      | `O(1)` gitt node             |
| Minnebruk                | Kompakt                     | Mer overhead (pekere)        |

---

## Oppsummering

- Bruk **List<T>** som standardvalg: raskt, effektivt og lett å bruke.
- Bruk **LinkedList<T>** når du må gjøre mange innsettinger/slettinger gitt node-referanser.
- Begge ligger på **heapen**, men strukturen og ytelsen er svært forskjellig.
