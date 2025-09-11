# Heap vs. Binært tre – en praktisk guide

Dette dokumentet forklarer hva et **heap** er, hva et **binært tre** er (inkl. binært søketre/BST), reglene for disse, likheter og forskjeller, og hva dette betyr for **søk**. Det inkluderer ASCII‑illustrasjoner, array‑mapping og kompleksiteter.

---

## 🧱 Hva er et binært tre?

- Et **binært tre** er en hierarkisk datastruktur der hver node har maks **to barn** (venstre/høyre).
- Det finnes mange varianter: *generelt binært tre*, *binært søketre (BST)*, *heap*, *AVL*, *rød‑svart tre*, osv.
- Et **generelt binært tre** har **ingen** ordensregel mellom noder.

### Binært søketre (BST)

En BST innfører rekkefølgen **venstre < node < høyre** rekursivt for alle deltrær.

```
        [8]
       /   \
     [3]   [10]
     / \      \
   [1] [6]    [14]
```

Egenskaper:

- Venstre barn < forelder < høyre barn (gjelder **rekursivt**).
- **Inorden-traversering** (venstre, node, høyre) gir sortert rekkefølge: `1, 3, 6, 8, 10, 14`.
- **Søk**: sammenlign og gå venstre/høyre. I et **balansert** tre er søk `O(log n)`; i verste fall (degenerert) `O(n)`.

---

## ⛰️ Hva er en heap?

En **heap** er et (nesten alltid) **komplett binært tre** som oppfyller *heap‑egenskapen*:

- **Min‑heap**: forelder ≤ sine barn. Roten er minimum.
- **Maks‑heap**: forelder ≥ sine barn. Roten er maksimum.
- **Komplett** betyr: alle nivåer er fulle unntatt kanskje det siste, som fylles **fra venstre mot høyre**.

**Min‑heap (eksempel):**

```
           [2]
         /     \
       [5]     [8]
      /  \     /
   [10] [15] [20]
```

Regler: `2 ≤ 5,8`, `5 ≤ 10,15`, `8 ≤ 20`. Det finnes **ingen** garanti for at alle verdier i venstre deltre er ≤ alle verdier i høyre deltre – kun lokale forelder↔barn‑relasjoner.

**Maks‑heap (eksempel):**

```
           [9]
         /     \
       [8]     [5]
      /  \     /
    [3]  [2] [1]
```

### Array‑representasjon (0‑indeksert)

Heaps lagres typisk kompakt i en **array** (takket være at de er komplette). Da slipper vi pekere.

```
Index:   0   1   2   3   4   5
Value:  [2] [5] [8] [10][15][20]
```

Foreldre/barn‑mapping (0‑indeksert):

```
left(i)  = 2*i + 1
right(i) = 2*i + 2
parent(i)= (i - 1) // 2
```

**Samme struktur som tre:**  

```
           [2](0)
          /      \
      [5](1)    [8](2)
       /  \       /
   [10](3)[15](4)[20](5)
```

---
<div style="page-break-after:always;"></div>

## 🔁 Likheter mellom heap og binært tre

- Begge er **binære trær** (maks to barn).
- Begge kan representeres med **pekere** (nodeklasser) eller som **array**.
- Begge uttrykker **hierarki** og kan traverseres (pre/in/post‑order, BFS).

---

## 🔍 Forskjeller og regler

| Egenskap                 | Binært søketre (BST)                              | Heap (min/maks)                                                |
|---|---|---|
| Ordensregel              | `venstre < node < høyre` (rekursivt)              | `forelder ≤ barn` (min) / `forelder ≥ barn` (maks)             |
| Global vs lokal orden    | **Global**: venstre deltre ≤ node ≤ høyre deltre  | **Lokal**: kun forelder ↔ barn                                 |
| Struktur                 | Kan være ubalansert                               | Nesten alltid **komplett** (venstrefylt)                       |
| Traversering sortert     | **Ja** (inorden)                                   | **Nei**                                                        |
| Søk etter vilkårlig verdi| `O(log n)` (balansert) / `O(n)` (ubalansert)      | `O(n)` (ingen vei å eliminere halvdeler)                       |
| Topp‑element             | Ikke nødvendigvis min/maks                        | Rot er alltid **min** (min‑heap) eller **maks** (maks‑heap)    |
| Typiske bruksområder     | Oppslag, sorterte datastrukturer, range‑spørringer | Prioritetskø, korteste‑vei (Dijkstra), **heapsort**, streaming |
| Vanlige operasjoner      | Søk/innsett/slett i `O(log n)` (balansert tre)     | `peek` `O(1)`, `insert`/`extract` `O(log n)`, `heapify` `O(log n)` |

---
<div style="page-break-after:always;"></div>

## 🧭 Konsekvenser for søk (ASCII‑eksempler)

**Heap – hvorfor ikke effektivt oppslag:**  

```
           2
         /   \
        5     8
       / \   /
     10 15 20   ← hvor ligger 20? venstre/høyre gir ingen garanti
```

- Heap‑regelen sier ingenting om **venstre vs høyre** utover forelder↔barn.  
- For å finne `20` kan du i verste fall måtte sjekke **alle** noder → `O(n)`.

**BST – målrettet søk:**  

```
            8
          /   \
         5     10
        / \      \
       2   6      20
```

Søk etter `20`:

```
8 → (20 > 8) gå høyre
10 → (20 > 10) gå høyre
20 → funnet
```

- Vi kaster halvparten av treet på hvert steg → `O(log n)` i balansert tre.

---
<div style="page-break-after:always;"></div>

## ⚙️ Heap‑operasjoner og kompleksitet

**Peek (topp):** Roten – `O(1)`  
**Insert (push):** Legg bakerst i arrayen og *sift‑up* til heap‑regelen holder – `O(log n)`  
**Extract (pop min/maks):** Bytt rot ↔ siste element, fjern siste, *sift‑down* fra roten – `O(log n)`  
**Heapify (sift‑down i):** Fiks deltre ved indeks `i` når undertrær er heaps – `O(log n)`  
**Build‑heap:** Kall `heapify` fra siste indre node ned til 0 – **`O(n)`**

**ASCII – extract fra maks‑heap:**

```
Start (max‑heap):        Etter bytte rot↔siste:   Sift‑down (heapify):
    9                           1                       8
   / \                         / \                     / \
  8   5        swap →         8   5       →          1   5
 / \   \                     / \   \                 / \
3   2   1                   3   2                   3   2
```

Roten (`9`) er tatt ut, og resten gjen‑heapifiseres.

---
<div style="page-break-after:always;"></div>

## 🧮 Heapsort – kjapp oversikt

1. **Build‑max‑heap** over hele arrayen (`O(n)`).
2. Gjenta for `end = n-1 … 1`:
   - **Swap** `A[0]` ↔ `A[end]` (største til slutten).
   - **Heapify(0, heapSize=end)** (`O(log n)`).
3. Resultatet er sortert i stigende rekkefølge.  
**Total:** `O(n log n)` tid, `O(1)` ekstra minne, **ikke stabil**.

**ASCII – ett pass i Heapsort (max‑heap):**

```
Heap: [9, 8, 5, 3, 2, 1]
Swap A[0]↔A[5] → [1, 8, 5, 3, 2, 9]
Heapify(0,5)   → [8, 3, 5, 1, 2, 9]
```

---

## 📌 Oppsummeringstabell

| Feature                    | Binært søketre (BST)           | Heap                         |
|---|---|---|
| Ordensregel                | `venstre < node < høyre`       | `forelder ≤/≥ barn` (lokal)  |
| Struktur                   | Potensielt ubalansert          | **Komplett**                 |
| Søk vilkårlig verdi        | `O(log n)` balansert / `O(n)` ubalansert | `O(n)`                |
| Topp‑element               | Ingen garanti                   | Alltid min/maks i roten      |
| Traversering sortert       | **Ja** (inorden)                | **Nei**                      |
| Typiske bruksområder       | Oppslag, sorterte sett/map      | Prioritetskø, heapsort       |
| Nøkkeloperasjoner          | Søk/sett/slett `O(log n)`       | `peek O(1)`, `push/pop O(log n)` |

---

## 💡 Husk

- Alle **heaps** er **binære trær**, men ikke alle binære trær er heaps.
- **BST** gir effektiv søking og rekkefølge; **heap** gir effektiv tilgang til **min/maks** og er perfekt for **prioritetskøer** og **heapsort**.
