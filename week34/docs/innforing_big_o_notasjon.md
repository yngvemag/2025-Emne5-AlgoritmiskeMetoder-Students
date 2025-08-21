# Innføring i Big-O Notasjon

Big-O notasjon er et verktøy vi bruker for å beskrive **effektiviteten til algoritmer**. Den hjelper oss å forstå hvor raskt en algoritme vokser i forhold til størrelsen på dataene den behandler.

## Hvorfor Big-O?

Når vi vurderer en algoritme, er vi spesielt interessert i: - **Kjøretid (tidseffektivitet)** -- hvor raskt algoritmen kjører. - **Minnebruk (plasskompleksitet)** -- hvor mye minne algoritmen bruker.

I stedet for å telle nøyaktige operasjoner, beskriver Big-O veksten **asymptotisk**, altså hvordan den oppfører seg når datasettet blir veldig stort.

## Grunnidé

- Vi ser på hvordan antall operasjoner øker når antall elementer `n` øker.
- Små detaljer (som konstanter og lavere ordens ledd) ignoreres.
- Vi fokuserer på det som betyr mest når `n` blir stort.

Eksempel:\
Hvis en algoritme bruker `3n + 5` operasjoner, beskriver vi den som **O(n)**, fordi den vokser proporsjonalt med `n`.

## Vanlige kompleksitetsklasser

| Notasjon | Navn                | Beskrivelse                                    | Eksempel                           |
|----------|---------------------|------------------------------------------------|------------------------------------|
| O(1)     | Konstant tid        | Tar alltid like lang tid, uavhengig av datasettets størrelse | Tilgang til et element i et array |
| O(log n) | Logaritmisk tid     | Blir litt tregere når `n` vokser, men vokser veldig sakte | Binærsøk |
| O(n)     | Lineær tid          | Vokser proporsjonalt med `n`                   | Lineært søk i en liste |
| O(n log n)| Lineær-logaritmisk tid | Vanlig i effektive sorteringsalgoritmer     | Quicksort, mergesort |
| O(n²)    | Kvadratisk tid      | Vokser raskt, lite effektiv for store data     | Bubble sort, nested loops |
| O(2^n)   | Eksponentiell tid   | Ekstremt treg for store `n`                    | Brute force-løsning av “reisende selger”-problemet |
| O(n!)    | Faktoriell tid      | Blir praktisk talt umulig for selv små `n`     | Generering av alle permutasjoner |

<div style="page-break-after:always;"></div>

## Eksempel på analyse

### Lineært søk (O(n))

``` python
def linear_search(lst, x):
    for item in lst:
        if item == x:
            return True
    return False
```

I verste fall må vi sjekke alle `n` elementene. Kompleksitet: **O(n)**.

### Binærsøk (O(log n))

``` python
def binary_search(lst, x):
    left, right = 0, len(lst) - 1
    while left <= right:
        mid = (left + right) // 2
        if lst[mid] == x:
            return True
        elif lst[mid] < x:
            left = mid + 1
        else:
            right = mid - 1
    return False
```

Hver gang deler vi listen i to. Antall steg vokser logaritmisk: **O(logn)**.

## Beste, verste og gjennomsnittlig tilfelle

Når vi analyserer algoritmer, ser vi ofte på:

- **Beste tilfelle**: raskest mulig (sjeldent nyttig).
- **Verste tilfelle**: det maksimale antall operasjoner. - **Gjennomsnittlig tilfelle**: forventet antall
operasjoner.

Eksempel:

- Lineært søk i en liste:
- Beste tilfelle: O(1) (elementet er først).
- Verste tilfelle: O(n).
- Gjennomsnittlig tilfelle: O(n).

## Big-O vs. andre notasjoner

- **Big-O**: øvre grense (worst case).\
- **Big-Ω (Omega)**: nedre grense (best case).\
- **Big-Θ (Theta)**: eksakt vekstrate (når både øvre og nedre grense
    er lik).

Eksempel: Lineært søk har:\

- O(n) (øverste grense),\
- Ω(1) (beste tilfelle),\
- Θ(n) (eksakt vekst i gjennomsnitt/verste fall).

## Oppsummering

- Big-O beskriver **hvor raskt en algoritme vokser** når data blir store.
- Det hjelper oss å sammenligne algoritmer uavhengig av maskinvare.
- Vi ignorerer konstanter og detaljer, og ser på hovedveksten.
- Kjennskap til vanlige kompleksitetsklasser (O(1), O(n), O(n log n), O(n²)) er avgjørende for å lage effektive løsninger.

------------------------------------------------------------------------

👉 Med Big-O kan vi raskt avgjøre om en algoritme er praktisk for store
problemer, eller bare fungerer for små datasett.
