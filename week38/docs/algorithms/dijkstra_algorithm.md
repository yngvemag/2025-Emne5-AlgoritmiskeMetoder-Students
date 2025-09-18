
# Dijkstra's Algoritme

## Hva er Dijkstra's algoritme?

**Dijkstra's algoritme** er en algoritme som brukes for å finne den **korteste veien** fra en **startnode** til alle andre noder i en **vektet graf**.  
I motsetning til **Breadth-First Search (BFS)**, som fungerer kun på **uvektede grafer**, tar Dijkstra hensyn til **vektene på kantene**.

Algoritmen ble utviklet av **Edsger W. Dijkstra** i 1956 og er mye brukt i moderne teknologi, blant annet i:

- GPS og navigasjonssystemer
- Nettverksruting (f.eks. internett-trafikk)
- Spillutvikling og AI
- Planlegging og logistikk

---

## Begrepsforklaringer

- **Startnode:** Noden vi starter beregningen fra.
- **Vektet graf:** En graf der hver kant har en kostnad eller vekt.
- **Avstand (distance):** Den totale kostnaden for å komme fra startnoden til en bestemt node.
- **Prioritetskø (min-heap):** Datastruktur som alltid velger noden med **lavest nåværende avstand** først.
- **Besøkt (visited):** En liste eller et sett som markerer hvilke noder som er ferdig prosessert.

---

## Hvorfor Dijkstra?

BFS finner den korteste veien i **uvektede grafer**, men i virkelige problemer har kanter ofte forskjellige kostnader.  
Eksempler:
- Kjappeste vei i et veinett med ulik kjøretid mellom byer.
- Laveste kostnad i et datanettverk.
- Korteste rute i et spillkart.

Dijkstra løser dette ved å **gradvis bygge den korteste veien** til hver node.

---
<div style="page-break-after:always;"></div>

## Hvordan fungerer Dijkstra's algoritme?

### Prinsipp

1. Start med å sette avstanden til startnoden = 0, og alle andre noder = ∞ (uendelig).
2. Bruk en **prioritetskø** for å velge noden med **lavest avstand** som ikke er prosessert ennå.
3. Oppdater avstanden til alle naboene ved å sjekke om en **kortere vei** er funnet via den nåværende noden.
4. Marker noden som besøkt når alle naboer er sjekket.
5. Gjenta til alle noder er besøkt eller køen er tom.

---

## Steg-for-steg eksempel

Vi har følgende graf:

```
      (A)
     /   \
   4/     \2
   /       \
 (B)---1---(C)
   \       /
   5\     /3
     \   /
      (D)
```

### Startnode: A

| Node | Avstand (initialt) |
|------|---------------------|
| A    | 0                   |
| B    | ∞                   |
| C    | ∞                   |
| D    | ∞                   |

#### Stegene

1. Start ved **A**. Avstand[A] = 0.  
   Oppdater naboer:  
   - B = 4  
   - C = 2

2. Velg neste node med lavest avstand → **C**.  
   Oppdater naboer:  
   - D = 5 (C → D gir 2 + 3)

3. Velg neste node med lavest avstand → **B**.  
   Oppdater naboer:  
   - D = 5 (via B ville vært 4 + 5 = 9 → ikke bedre)

4. Velg neste node → **D**. Ingen oppdateringer.

**Resultat:** Korteste avstand fra A → {A: 0, B: 4, C: 2, D: 5}

---

## Pseudokode

```
Dijkstra(G, startnode):
    for hver node i G:
        avstand[node] = ∞
    avstand[startnode] = 0

    lag en prioritetskø
    legg (0, startnode) i køen

    mens køen ikke er tom:
        nåværende = noden med lavest avstand fra køen

        hvis nåværende er besøkt:
            fortsett

        marker nåværende som besøkt

        for hver nabo av nåværende:
            ny_avstand = avstand[nåværende] + vekt(nåværende, nabo)
            hvis ny_avstand < avstand[nabo]:
                avstand[nabo] = ny_avstand
                legg (ny_avstand, nabo) i køen
```

---
<div style="page-break-after:always;"></div>

## Viktige egenskaper

- **Tidskompleksitet:** O((V + E) log V)  
  - V = antall noder (vertices)
  - E = antall kanter (edges)
  - Bruker prioritetskø for effektivitet.

- **Plasskompleksitet:** O(V) for lagring av avstander og besøkte noder.

| Algoritme  | Vektet graf? | Finner korteste vei? |
|------------|--------------|----------------------|
| BFS        | Nei          | Ja (kun uvektet)    |
| DFS        | Nei          | Ikke garantert      |
| Dijkstra   | Ja           | Ja                  |

---

## Bruksområder

| Bruksområde       | Beskrivelse |
|-------------------|-------------|
| **GPS og navigasjon** | Finne raskeste rute mellom steder. |
| **Internett-ruting** | Velge vei med minst latency eller kostnad. |
| **Spill og AI** | Pathfinding for NPC-er og enheter. |
| **Transport og logistikk** | Optimalisere ruter og kostnader. |

---

## Oppsummering

- Dijkstra finner **korteste vei i vektede grafer**.
- Starter fra én node og bygger gradvis opp den korteste veien til alle andre.
- Bruker en **prioritetskø** for effektivitet.
- Har mange praktiske bruksområder, inkludert GPS, nettverk, spill og planlegging.
