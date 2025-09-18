
# Breadth-First Search (BFS)

## Hva er Breadth-First Search?

**Breadth-First Search (BFS)** er en søkealgoritme som brukes for å utforske grafer eller trær.
I motsetning til DFS, som går **dypt først**, utforsker BFS **grafen i bredde**, nivå for nivå.

BFS starter ved en valgt startnode og besøker **alle naboene** først, deretter naboenes naboer, og fortsetter til hele grafen er besøkt.

BFS brukes i mange praktiske situasjoner, blant annet for å finne **korteste sti** i uvektede grafer.

---

## Begrepsforklaringer

- **Startnode:** Noden BFS begynner utforskningen fra.
- **Nivå (level):** Alle noder som er like langt unna startnoden tilhører samme nivå.
- **Kø (queue):** En datastruktur hvor BFS lagrer hvilke noder som skal besøkes neste, basert på **FIFO** (First-In, First-Out).
- **Besøkt (visited):** En liste eller et sett som markerer hvilke noder som allerede er besøkt for å unngå uendelige løkker.

---
<div style="page-break-after:always;"></div>

## Hvilke datastrukturer kan BFS brukes på?

BFS fungerer på både **grafer** og **trær**, men det er noen viktige forskjeller:

### 1. Grafer

- Grafer kan ha **sykluser**, og BFS **må** derfor ha en `visited`-struktur for å unngå at algoritmen kjører uendelig.
- BFS brukes ofte til:
  - Finne korteste vei i uvektede grafer.
  - Oppdage sammenhengende komponenter i nettverk.
  - Navigering i kart og spill.
  - Nettverkstopologi (som ruting i datanettverk).

### 2. Trær

- Trær er **acykliske**, så det er **ikke nødvendig med en visited-liste**, fordi det ikke finnes sykluser.
- BFS i trær kalles ofte **level-order traversal**, hvor man besøker alle noder på samme nivå før man går dypere.

Eksempel på BFS i et binært tre:

```
      A
     / \
    B   C
   / \
  D   E
```

Rekkefølge på BFS: **A → B → C → D → E**

### Sammenligning

| Egenskap        | Graf                        | Tre                        |
|-----------------|----------------------------|----------------------------|
| Sykluser        | Kan ha sykluser             | Ingen sykluser              |
| `visited` nødvendig | Ja                         | Nei                         |
| Vanlig bruk     | Korteste vei, nettverk, spill | Traversering, nivå-besøk     |

**Konklusjon:** BFS-algoritmen er i bunn og grunn den samme for grafer og trær, men implementasjonen er enklere for trær.

---
<div style="page-break-after:always;"></div>

## Hva brukes BFS til?

BFS er en svært anvendelig algoritme, og brukes i mange viktige områder:

| Bruksområde | Beskrivelse |
|-------------|-------------|
| **Korteste vei** | BFS finner den korteste stien i en uvektet graf. |
| **Navigasjon** | Brukes i GPS-systemer og labyrintløsning. |
| **Nettverksanalyse** | Oppdager om alle noder i et nettverk er koblet sammen. |
| **Web Crawlers** | BFS brukes av søkemotorer for å traversere nettsider. |
| **Level-order traversering i trær** | Brukes i spillutvikling og AI-beslutningstrær. |

---

## Hvorfor er BFS viktig?

- BFS er **garantert å finne den korteste veien** i uvektede grafer, noe DFS ikke alltid gjør.
- Algoritmen er grunnlaget for mange avanserte algoritmer som **Dijkstra's algoritme** og **A\***.
- Brukes mye i nettverksruting, puslespill, og grafbaserte søkeproblemer.

---
<div style="page-break-after:always;"></div>

## Hvordan fungerer BFS?

### Prinsipp

1. Start ved en node og legg den i en **kø (queue)**.
2. Marker startnoden som besøkt.
3. Gjenta følgende til køen er tom:
   - Ta ut første element fra køen.
   - Besøk alle uoppdagede naboer til den noden og legg dem bakerst i køen.
4. Fortsett til alle noder er besøkt.

Dette sikrer at noder besøkes i rekkefølge basert på avstand fra startnoden.

---

### Steg-for-steg eksempel

Vi tar utgangspunkt i denne grafen:

```
A --- B --- D
|     |
C     E
```

**Startnode:** A

#### Stegene

1. Legg `A` i køen. (Queue = [A]) → Visited = {A}
2. Ta ut `A`. Besøk naboene `B` og `C`. (Queue = [B, C], Visited = {A, B, C})
3. Ta ut `B`. Besøk naboen `E` og `D`. (Queue = [C, E, D], Visited = {A, B, C, E, D})
4. Ta ut `C`. Ingen nye naboer.
5. Ta ut `E`. Ingen nye naboer.
6. Ta ut `D`. Ingen nye naboer.
7. Køen er tom → algoritmen avsluttes.

**Rekkefølge på besøk:** A → B → C → E → D

---
<div style="page-break-after:always;"></div>

## Pseudokode for BFS

```
BFS(G, startnode):
    lag en tom kø
    marker startnode som besøkt
    legg startnode i køen

    mens køen ikke er tom:
        node = fjern første element fra køen
        for hver nabo av node:
            hvis nabo ikke er besøkt:
                marker nabo som besøkt
                legg nabo i køen
```

---

## BFS i trær (Level-Order Traversal)

Når BFS brukes på trær, kalles det **level-order traversal**.
Dette besøker nodene nivå for nivå, fra venstre til høyre.

Eksempel:

```
      A
     / \
    B   C
   / \
  D   E
```

Rekkefølge: **A → B → C → D → E**

Pseudokode for BFS på trær:

```
LevelOrderTraversal(root):
    hvis root er null:
        return
    lag en tom kø
    legg root i køen

    mens køen ikke er tom:
        node = fjern første element fra køen
        besøk node
        hvis node har venstre barn:
            legg venstre barn i køen
        hvis node har høyre barn:
            legg høyre barn i køen
```

---

## Viktige egenskaper ved BFS

- **Tidskompleksitet:** O(V + E)
  - V = antall noder (vertices)
  - E = antall kanter (edges)
- **Plasskompleksitet:** O(V)
  - BFS lagrer noder i en kø, og i verste fall kan hele nivået måtte lagres samtidig.

**BFS vs DFS:**

| Egenskap | BFS | DFS |
|-----------|-----|-----|
| Søker dypt eller bredt? | Bredt | Dypt |
| Datastruktur brukt | Kø (queue) | Stack eller rekursjon |
| Korteste vei i uvektet graf | Ja | Ikke garantert |
| Minnebruk | Høyere | Lavere |
| Bruk på trær | Level-order traversal | Preorder, Inorder, Postorder |

---

## Oppsummering

- BFS utforsker grafen **nivå for nivå**, og bruker en **kø (FIFO)**.
- Algoritmen er ideell for å finne **korteste vei i uvektede grafer**.
- Krever en `visited`-struktur i grafer med sykluser, men ikke i trær.
- Har mange bruksområder, inkludert nettverk, spill, AI, og web crawling.
- Effektiv med en tidskompleksitet på O(V + E).
