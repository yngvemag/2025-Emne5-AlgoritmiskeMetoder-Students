# Heap vs Binært Tre

Dette dokumentet forklarer hva et **heap** er, hva et **binært tre** er, reglene for disse, likheter og forskjeller, og hvilke konsekvenser dette har for søking. Illustrasjoner er inkludert.

---

## Hva er et Binært Tre?
- Et **binært tre** er en hierarkisk datastruktur der hver node har maks **to barn** (venstre og høyre).
- Det finnes mange typer binære trær (generelt binært tre, binært søketre, heap osv.).
- **Generelt binært tre** har ingen spesifikke regler for rekkefølge mellom noder.

Eksempel på et **binært søketre (BST)** med sorteringsregel (venstre < node < høyre):

```
       [8]
      /   \
    [3]   [10]
    / \      \
  [1] [6]    [14]
```

- Venstre barn < forelder < høyre barn
- Traversering inorden gir sortert rekkefølge: 1,3,6,8,10,14
- Effektivt søk: gå venstre/høyre basert på verdi (O(log n) i balansert tre).

---

## Hva er en Heap?
En **heap** er en spesialisert type binært tre som oppfyller *heap-egenskapen*:

- **Min-heap:** Forelder ≤ sine barn
- **Max-heap:** Forelder ≥ sine barn
- Heap er nesten alltid et **komplett binært tre**:
  - Alle nivåer er fylt unntatt kanskje det siste, som fylles fra venstre mot høyre.

Eksempel på **min-heap**:

```
        [2]
      /     \
    [5]     [8]
    / \    /
 [10] [15][20]
```

Regler som holder:
- 2 ≤ 5,8
- 5 ≤ 10,15
- 8 ≤ 20

**Men:** Ingen garanti for venstre/høyre rekkefølge utover dette.

---

## Likheter mellom Heap og Binært Tre
- Begge er binære trær (hver node har maks 2 barn).
- Begge kan representeres i minne som pekere (noder) eller som array.
- Begge brukes for hierarkisk lagring.

---

## Forskjeller og regler

| Egenskap         | Binært søketre (BST)                 | Heap (min/max)                         |
|------------------|--------------------------------------|----------------------------------------|
| Ordensregel      | venstre < node < høyre (rekursivt)   | forelder ≤ barn (min) eller ≥ (max)    |
| Struktur         | Kan være ubalansert                  | Nesten alltid komplett (fylles fra venstre) |
| Traversering     | Inorden gir sortert rekkefølge       | Traversering gir ikke sortert rekkefølge |
| Bruksområde      | Effektivt søk, sorterte data         | Prioritetskø, heapsort, rask tilgang til topp |
| Søketid vilkårlig| O(log n) balansert (bruker søkeregelen)| O(n) (må sjekke flere noder)          |
| Topp-element     | Ikke nødvendigvis min/maks           | Rot er alltid min (min-heap) eller maks (max-heap) |

---

## Konsekvenser for søk

La oss se på heapen:

```
        2
      /   \
     5     8
    / \   /
  10  15 20
```

Hvis vi vil søke etter 20:
- **Heap-regelen** gir ingen garanti for om 20 ligger til venstre eller høyre for roten.  
- Må i verste fall sjekke alle noder (lineært søk).
- Heap er designet for raskt å finne **min/maks** (toppen), ikke for effektivt oppslag.

<div style="page-break-after:always;"></div>

I et BST med samme elementer:

```
        8
      /   \
     5     10
    / \      \
  2   6      20
```

- Kan navigere: 20 > 8, gå høyre; 20 > 10, gå høyre; funnet 20.  
- Søketid O(log n) hvis balansert.

---

## Oppsummeringstabell

| Feature                | Binært Søketre (BST)           | Heap                    |
|------------------------|--------------------------------|------------------------|
| Ordensregel            | venstre < node < høyre         | forelder ≤/≥ barn      |
| Struktur               | Kan være ubalansert            | Komplett binært tre    |
| Søk etter vilkårlig verdi | O(log n) (balansert)        | O(n)                   |
| Topp-element           | Ikke nødvendigvis min/maks     | Rot = min/maks         |
| Traversering sortert   | Ja (inorden)                   | Nei                    |
| Typiske bruksområder   | Søk, sortering                 | Prioritetskø, heapsort |

---

**Oppsummering:**  
- Alle heaps er binære trær, men ikke alle binære trær er heaps.
- BST brukes for effektiv søking/sortering, heap for prioritetskøer og rask min/maks-tilgang.
