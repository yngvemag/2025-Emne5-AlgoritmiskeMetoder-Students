
# Traversering av binære trær: Preorder, Inorder, Postorder, og Level-order

Når vi jobber med **binære trær**, finnes det flere måter å besøke alle nodene på.
Hvilken rekkefølge vi velger avhenger av hva vi ønsker å gjøre med dataene.

Det finnes **to hovedkategorier av traverseringer**:

- **Depth-First Search (DFS)** – går **dypt først**, følger grener helt ned før den backtracker.
  - **Preorder**
  - **Inorder**
  - **Postorder**
- **Breadth-First Search (BFS)** – går **bredt først**, besøker treet **nivå for nivå**.
  - **Level-order**

---

## Eksempeltre

Vi bruker dette treet i alle eksemplene:

```
      A
     / \
    B   C
   / \
  D   E
```

- **Root (rot):** `A`
- **Venstre subtre:** `B`, `D`, `E`
- **Høyre subtre:** `C`

---
<div style="page-break-after:always;"></div>

## 1. Preorder Traversal (Root → Venstre → Høyre)

### Forklaring

- Besøk **roten først**, deretter venstre subtre, og til slutt høyre subtre.
- Brukes når du ønsker å prosessere roten **før barna**.
- Vanlig ved **serialisering** (lagring) av treet eller ved **kopiering** av strukturen.

**Algoritme:**

1. Prosesser **root**.
2. Traverser **venstre subtre** rekursivt.
3. Traverser **høyre subtre** rekursivt.

### Eksempel steg-for-steg

```
Steg 1: A (root)
Steg 2: B (venstre barn av A)
Steg 3: D (venstre barn av B)
Steg 4: E (høyre barn av B)
Steg 5: C (høyre barn av A)
```

**Resultat:** `A → B → D → E → C`

---

## 2. Inorder Traversal (Venstre → Root → Høyre)

### Forklaring

- Besøk **venstre subtre først**, deretter roten, og til slutt høyre subtre.
- Brukes i **binære søketrær (BST)** for å hente elementer i **sortert rekkefølge**.

**Algoritme:**

1. Traverser **venstre subtre** rekursivt.
2. Prosesser **root**.
3. Traverser **høyre subtre** rekursivt.

### Eksempel steg-for-steg

```
Steg 1: D (venstre barn av B)
Steg 2: B (root av venstre subtre)
Steg 3: E (høyre barn av B)
Steg 4: A (root)
Steg 5: C (høyre barn av A)
```

**Resultat:** `D → B → E → A → C`

> 💡 **Merk:** I et binært søketre gir Inorder **verdier i stigende rekkefølge**.

---

## 3. Postorder Traversal (Venstre → Høyre → Root)

### Forklaring

- Besøk barna **før** roten.
- Brukes når man skal **slette et tre** eller gjøre beregninger der barna må prosesseres før foreldrene.
- Vanlig i **uttrykks-trær** (Expression Trees) for å evaluere uttrykk.

**Algoritme:**

1. Traverser **venstre subtre** rekursivt.
2. Traverser **høyre subtre** rekursivt.
3. Prosesser **root**.

### Eksempel steg-for-steg

```
Steg 1: D (venstre barn av B)
Steg 2: E (høyre barn av B)
Steg 3: B (root av venstre subtre)
Steg 4: C (høyre barn av A)
Steg 5: A (root)
```

**Resultat:** `D → E → B → C → A`

---

## 4. Level-order Traversal (BFS – nivå for nivå)

### Forklaring

- **Breadth-First Search (BFS)** besøker nodene **nivå for nivå**, fra toppen og nedover.
- Bruker en **kø (FIFO)** for å holde orden på hvilke noder som skal besøkes.
- Veldig nyttig i situasjoner der du trenger å jobbe med elementer etter "avstand" fra roten, som i navigasjon eller spillutvikling.

**Algoritme:**

1. Legg **root** i køen.
2. Ta ut første element fra køen, prosesser det, og legg dets barn bakerst i køen.
3. Gjenta til køen er tom.

### Eksempel steg-for-steg

```
Steg 1: A (nivå 0)
Steg 2: B, C (nivå 1)
Steg 3: D, E (nivå 2)
```

**Resultat:** `A → B → C → D → E`

---

## Sammenligning av traverseringer

| Traversering | Type | Rekkefølge | Bruksområde |
|--------------|------|------------|-------------|
| **Preorder** | DFS | Root → Venstre → Høyre | Serialisering, kopiering av tre |
| **Inorder** | DFS | Venstre → Root → Høyre | Hente sortert rekkefølge fra BST |
| **Postorder** | DFS | Venstre → Høyre → Root | Slette tre, evaluering av uttrykk |
| **Level-order** | BFS | Nivå for nivå | Navigasjon, spill, breddeanalyse |

---

## Visuell oppsummering

For treet:

```
      A
     / \
    B   C
   / \
  D   E
```

| Traversering | Resultat |
|--------------|----------|
| **Preorder** | A → B → D → E → C |
| **Inorder**  | D → B → E → A → C |
| **Postorder**| D → E → B → C → A |
| **Level-order** | A → B → C → D → E |

---

## Viktig å huske

- **DFS** går **dypt først**: Preorder, Inorder, og Postorder er bare ulike måter å bestemme **når** du prosesserer roten i forhold til barna.
- **BFS (Level-order)** går **bredt først** og krever en **kø** i implementasjonen.
- I grafer må BFS og DFS håndtere **sykluser** ved å merke besøkte noder. I trær er dette unødvendig, siden trær aldri har sykluser.
