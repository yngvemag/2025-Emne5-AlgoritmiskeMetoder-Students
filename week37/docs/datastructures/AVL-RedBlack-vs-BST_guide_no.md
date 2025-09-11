# AVL- og Red–Black-trær vs. ubalansert binært søketre (BST)

En praktisk guide for utviklere (med fokus på C#/.NET)

> **Kort oppsummert:** Alle tre er binære søketrær (BST). Ubalanserte trær er enkle og raske å kode, men kan bli trege på “uheldig” input. **AVL** og **Red–Black** er **selvbalanserende** og gir garantert `O(log n)`-ytelse for søk, innsetting og sletting.

---

## 1) Hva er et binært søketre (BST)?

Et BST er en trestruktur der hver node har inntil to barn og oppfyller **BST-invarianten**:

- Alle verdier i **venstre** subtre er **mindre** enn nodeverdien.
- Alle verdier i **høyre** subtre er **større eller lik** nodeverdien (duplikat-policy kan variere).

**Kompleksitet (BST generelt):**

- Søk/Innsetting/Sletting: `O(h)`, der `h` er trehøyden.

---

## 2) Ubalansert BST

Et “vanlig” BST uten ekstra balanse-mekanikk.

**Fordeler**

- Enkelt å implementere og forstå.
- Rask i praksis på små datasett eller når input er relativt tilfeldig.

**Ulemper**

- Høyden kan degenerere til `O(n)` (f.eks. hvis du setter inn sorterte verdier).
- I verste fall blir Søk/Innsetting/Sletting `O(n)`.

**Når passer det?**

- Små datasett.
- Enkle oppgaver/undervisning.
- Når input antas å være godt blandet og du ikke trenger garanti.

**Mini-eksempel på degenerering:**
Setter du inn `1, 2, 3, 4, 5` i et tomt tre med “duplikater til høyre”-policy, kan treet ende slik:

```
1
 \
  2
   \
    3
     \
      4
       \
        5
```

Høyden ≈ `n-1` → dårlig ytelse.

---

## 3) AVL-tre (Adelson-Velsky & Landis)

**Idé:** Hold treet **strengt balansert** ved å kontrollere høydeforskjellen mellom venstre og høyre subtre for hver node.

**Metadata per node**

- **Height** (høyde, heltall).

**Balansefaktor**

- `balance = height(left) - height(right)` må være i `{ -1, 0, +1 }` for **alle** noder.

**Rebalansering**

- Etter hver `Insert`/`Delete` går man oppover mot roten, **oppdaterer høyder** og sjekker balanse.
- Hvis `|balance| > 1`, utføres **rotasjoner**:
  - **LL** (venstre-venstre): én **høyre-rotasjon**.
  - **RR** (høyre-høyre): én **venstre-rotasjon**.
  - **LR** (venstre-høyre): **venstre**-rotasjon på venstre barn, deretter **høyre**-rotasjon.
  - **RL** (høyre-venstre): **høyre**-rotasjon på høyre barn, deretter **venstre**-rotasjon.

**Kompleksitet**

- Søk/Innsetting/Sletting: `O(log n)` garantert.
- Som oftest 1–2 rotasjoner ved innsetting; sletting kan trigge flere, men fortsatt `O(log n)`.

**Fordeler**

- Meget lav høyde i praksis → **raskt søk**.
- Sterk balansegaranti.

**Ulemper**

- Litt mer arbeid ved oppdateringer (oppdatere høyder + evt. flere rotasjoner).

**ASCII-illustrasjon av rotasjoner**

_Høyre-rotasjon (LL-case):_

```
    y                 x
   / \               / \
  x   T3   -->      T1  y
 / \                   / \
T1 T2                 T2 T3
```

_Venstre-rotasjon (RR-case):_

```
  x                   y
 / \                 / \
T1  y     -->       x  T3
   / \             / \
  T2 T3           T1 T2
```

---

## 4) Red–Black-tre (RB-tre)

**Idé:** Tillat litt “slark”, men håndhev **fargeregler** som begrenser høyden.

**Regler (invarianter)**

1. Hver node er **rød** eller **svart**.
2. **Roten** er svart.
3. Alle null-barn (tomme blader) regnes som svarte.
4. Ingen **rød** node har rød forelder (ingen to røde på rad).
5. Alle enkle stier fra en node til null-blad har samme antall **svarte** noder (lik **black-height**).

**Metadata per node**

- Én **fargebit** (rød/svart).

**Rebalansering (intuisjon)**

- **Insert:** Hvis forelder er rød → se på “onkel”:
  - Onkel **rød** → recolor (forelder+onkel svart, bestefar rød) og fortsett oppover.
  - Onkel **svart** → én eller to **rotasjoner** + **recolor**.
- **Delete:** Flere tilfeller (mer intrikat), men fortsatt `O(log n)`.

**Kompleksitet**

- Søk/Innsetting/Sletting: `O(log n)` garantert.
- Høyden ≤ `2 * log2(n+1)`.

**Fordeler**

- Få rotasjoner i snitt, spesielt ved sletting.
- Veldig vanlig i standardbiblioteker (f.eks. `SortedDictionary<TKey,TValue>` og `SortedSet<T>` i .NET).

**Ulemper**

- Litt høyere enn AVL i teorien → søk kan være ørlite tregere enn AVL.
- Reglene gjør implementasjon av sletting mer detaljert.

---

## 5) AVL vs. Red–Black vs. Ubalansert – sammenligning

| Egenskap | Ubalansert BST | AVL | Red–Black |
|---|---|---|---|
| Garanti på høyde | Nei | Ja (stram) | Ja (litt løsere) |
| Søk (worst case) | `O(n)` | `O(log n)` | `O(log n)` |
| Innsetting (worst) | `O(n)` | `O(log n)` | `O(log n)` |
| Sletting (worst) | `O(n)` | `O(log n)` | `O(log n)` |
| Rotasjoner – innsetting | 0 | 1–2 (typisk) | 0–2 (typisk) |
| Rotasjoner – sletting | 0 | 0–O(log n) | 0–2 (typisk, men flere tilfeller) |
| Overhead per node | Ingen ekstra | `height` (int) | `color` (bit/enum) |
| Best for | Små datasett, undervisning | Lesetunge workloads | Skrivetunge workloads, generelt bruk |

**Tommelregel**

- **Maks søkeytelse:** velg **AVL**.
- **Jevn totalytelse + enklere oppdateringer:** velg **Red–Black**.
- **Enkelt og greit, liten kodebase:** ubalansert BST (men uten garantier).

---

## 6) Praktiske tips i C#/.NET

- **Standardcontainere:** `SortedDictionary<TKey,TValue>` og `SortedSet<T>` er Red–Black-trær.
- **Comparer:** Definér alltid en **konsistent** `IComparer<T>` (antisymmetri, transitivitet) for forutsigbar orden.
- **Duplikater:** Bestem policy (typisk “duplikater til høyre”). For mengder (`Set`) er duplikater **ikke** tillatt.
- **API-design:** Eksponer `Add`, `Contains`, `Remove`, `Min/Max`, `Count`, `Height` og traverseringer (`InOrder`, `PreOrder`, `PostOrder`, evt. `IEnumerable<T>`).

---

## 7) Hvilket tre bør du velge?

- **Datamengden kan bli stor** og/eller **inputrekkefølgen kan være uheldig** → velg et **selvbalanserende tre**.
- **Lesetungt** (mange søk): **AVL**.
- **Mye innsetting/sletting**: **Red–Black** (vanlig standardvalg).
- **Rask prototyping/læring**: ubalansert BST.

---

## 8) Hurtigreferanse

- **BST-invariant:** venstre < node ≤ høyre (eller en annen konsekvent duplikat-policy).
- **AVL:** balansefaktor i {−1,0,+1}; roter ved LL, RR, LR, RL.
- **RB:** rød/svart-regler; opprettholder lik black-height; få rotasjoner.
- **Mål:** hold høyden i `O(log n)` for forutsigbar ytelse.

---

### Videre lesning (stikkord)

- “AVL Tree rotations”, “Red–Black Tree insertion/deletion cases”
- .NET Referansedokumentasjon for `SortedDictionary` og `SortedSet`
