
# Depth-First Search (DFS)

## Hva er Depth-First Search?

**Depth-First Search (DFS)** er en fundamental søkealgoritme som brukes for å utforske **grafer** eller **trær**.
Algoritmen utforsker **så dypt som mulig langs hver gren før den backtracker** (går tilbake) og prøver en ny vei.

DFS er en av de mest grunnleggende grafalgoritmene og danner grunnlaget for mange andre viktige algoritmer, som:

- Syklusdeteksjon
- Toppologisk sortering
- Løsing av labyrinter
- Oppdage sammenhengende komponenter

---

## Begrepsforklaringer

Før vi går inn på selve algoritmen, er det viktig å forstå noen sentrale begreper:

- **Startnode (root/start vertex):** Den noden vi starter søket fra.
- **Besøkt (visited):** En node som allerede er besøkt, merkes slik at vi ikke besøker den igjen.
- **Backtracking:** Prosessen med å gå tilbake til forrige node når man har nådd bunnen av en gren.
- **Sti (path):** En sekvens av kanter som forbinder to noder.

---
<div style="page-break-after:always;"></div>

## Hvilke datastrukturer kan DFS brukes på?

DFS er primært laget for **grafer**, men den kan også brukes på **trær**, siden et tre er en spesiell type graf.
Likevel er det noen viktige forskjeller i hvordan algoritmen brukes:

### 1. Grafer

- Grafer kan ha **sykluser** (sirkler).
- En node kan ha **flere forbindelser**, og man må holde styr på hvilke noder som er besøkt for å unngå uendelige løkker.
- DFS i grafer brukes ofte til å løse problemer som:
  - Syklusdeteksjon
  - Korteste sti (modifisert DFS)
  - Toppologisk sortering
  - Oppdage sammenhengende komponenter

> **Viktig:** I grafer **må** man ha en `visited`-liste eller et sett for å holde styr på hvilke noder som er besøkt.

<div style="page-break-after:always;"></div>

### 2. Trær

- Et **tre** er en **acyklisk, sammenhengende graf**, der det finnes nøyaktig én vei mellom to noder.
- Det betyr at man **ikke trenger en visited-liste** for å unngå sykluser, fordi treet aldri har en sirkulær struktur.
- DFS i trær kalles ofte **traversering**, og det finnes spesifikke varianter som:
  - **Preorder**: Besøk roten før barna.
  - **Inorder**: Besøk venstre barn → roten → høyre barn (vanlig for binære søketrær).
  - **Postorder**: Besøk barna før roten.

Eksempel på DFS i et binært tre:

```
      A
     / \
    B   C
   / \
  D   E
```

- Preorder (A, B, D, E, C)
- Inorder (D, B, E, A, C)
- Postorder (D, E, B, C, A)

### Sammenligning

| Egenskap        | Graf                      | Tre                         |
|-----------------|--------------------------|----------------------------|
| Sykluser        | Kan ha sykluser           | Ingen sykluser              |
| `visited` nødvendig | Ja                       | Nei                         |
| Vanlige problemer | Syklusdeteksjon, sti-søk | Traversering, sortering     |
| Koblinger       | Kan være komplekst nettverk | En hierarkisk struktur       |

**Konklusjon:** Selve DFS-algoritmen er den samme i bunn og grunn, men i et tre er implementasjonen enklere fordi vi ikke trenger å håndtere sykluser.

---
<div style="page-break-after:always;"></div>

## Hva brukes DFS til?

DFS har mange praktiske bruksområder:

| Bruksområde | Beskrivelse |
|-------------|-------------|
| **Syklusdeteksjon** | Finne om en graf inneholder en syklus (f.eks. deadlock i systemer). |
| **Toppologisk sortering** | Planlegging av oppgaver med avhengigheter, f.eks. kompilering av kode. |
| **Finn sammenhengende komponenter** | Brukes i nettverk for å se hvilke noder som er koblet sammen. |
| **Løse labyrinter og puslespill** | DFS går så dypt som mulig for å finne en løsning. |
| **Generere eller traversere trær** | Vanlig innen AI, spill, og søkeproblemer. |

---

## Hvorfor er DFS viktig?

DFS er en grunnleggende algoritme som danner **byggeklossene for mange komplekse algoritmer**.
Ved å forstå DFS, blir det lettere å lære algoritmer som Dijkstra, A*, og nettverksflyt-algoritmer.

DFS er også effektiv for grafer med mange noder og forbindelser, siden den ikke trenger å utforske alle naboer samtidig slik BFS gjør.

---

## Hvordan fungerer DFS?

### Prinsipp

DFS følger en **rekursiv strategi**:

1. Start ved en node og marker den som besøkt.
2. Utforsk **en nabo om gangen**, og gå så dypt som mulig.
3. Når du ikke finner flere uoppdagede naboer, **backtrack** til forrige node.
4. Fortsett til alle noder er besøkt.

Dette kan implementeres både **rekursivt** og **iterativt** (med en stack).

---
<div style="page-break-after:always;"></div>

### Steg-for-steg eksempel

Vi tar utgangspunkt i denne grafen:

```
A --- B --- D
|     |
C     E
```

**Startnode:** A

#### Stegene

1. Start ved `A`. Marker `A` som besøkt. (Visited = {A})
2. Gå til en nabo av `A`. Velg `B`. (Visited = {A, B})
3. Fra `B`, gå til en nabo som ikke er besøkt. Velg `D`. (Visited = {A, B, D})
4. `D` har ingen uoppdagede naboer → **backtrack** til `B`.
5. Fra `B`, velg neste uoppdagede nabo → `E`. (Visited = {A, B, D, E})
6. `E` har ingen uoppdagede naboer → **backtrack** til `B` → deretter til `A`.
7. Fra `A`, velg neste uoppdagede nabo → `C`. (Visited = {A, B, D, E, C})
8. Alle noder er nå besøkt → algoritmen avsluttes.

**Rekkefølge på besøk:** A → B → D → E → C

---

## Pseudokode for DFS (rekursiv versjon)

```
DFS(G, startnode):
    marker startnode som besøkt
    for hver nabo i G[startnode]:
        hvis nabo ikke er besøkt:
            DFS(G, nabo)
```

---

## Iterativ implementasjon (med stack)

```
DFS_iterativ(G, startnode):
    lag en tom stack
    push startnode på stack
    mens stack ikke er tom:
        node = pop fra stack
        hvis node ikke er besøkt:
            marker node som besøkt
            push alle naboer av node på stack
```

---

## Viktige egenskaper ved DFS

- **Tidskompleksitet:** O(V + E)
  - V = antall noder (vertices)
  - E = antall kanter (edges)
- **Plasskompleksitet:** O(V)
  - Rekursjonsdybden eller størrelsen på stacken kan være opptil antall noder.

DFS er veldig effektiv når man ønsker å utforske en hel graf og spesielt når man trenger å gå **dypt** i grafen før man sjekker bredere alternativer.

---

## Oppsummering

- DFS er en søkealgoritme som utforsker en graf **dypt før bredt**.
- Kan brukes på både **grafer** og **trær**, men i grafer må man ha en `visited`-struktur for å unngå sykluser.
- I trær brukes DFS til **traversering**, som Preorder, Inorder, og Postorder.
- Har mange bruksområder som syklusdeteksjon, toppologisk sortering, og nettverksanalyse.
- Kan implementeres rekursivt eller iterativt med en stack.
- Effektiv med en tidskompleksitet på O(V + E).
