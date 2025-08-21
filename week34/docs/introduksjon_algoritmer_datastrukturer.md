# Introduksjon til Algoritmer og Datastrukturer

Algoritmer og datastrukturer er grunnmuren i informatikk og programmering. For å forstå hvordan datamaskiner løser problemer, er det viktig å kunne begge deler. La oss starte med en oversikt som gir en lettfattelig inngang for studenter.

## Hva er en algoritme?

En algoritme er en **trinnvis oppskrift** for å løse et problem.

Den må være:

- **Presis**: Hvert steg må være klart definert.
- **Endelig**: Den må avslutte etter et begrenset antall steg.
- **Effektiv**: Den bør kunne utføres med en rimelig mengde ressurser (tid og minne).

Eksempel: Å finne det største tallet i en liste.

1. Start med første tall som det største.
2. Sammenlign det med neste tall.
3. Hvis det nye tallet er større, oppdater "størst".
4. Gjenta til slutten av listen.
5. Returner "størst".

Dette er en algoritme, uavhengig av programmeringsspråk.

## Algoritmer vs. programmer

- **Algoritme**: En idé skrevet som en oppskrift eller i pseudokode.
- **Program**: En konkret implementasjon av algoritmen i et språk som
    Python eller C#.

## Hva er en datastruktur?

En datastruktur er en **måte å organisere og lagre data på** slik at vi kan bruke den effektivt. Valg av datastruktur påvirker hvor raskt og enkelt algoritmer kan utføres.

Eksempler på datastrukturer:  

- **Array (tabell)**: Samling av elementer med indeks.
- **Liste**: Dynamisk samling av elementer.
- **Stakk stack)**: LIFO -- siste inn, første ut.
- **Kø (queue)**: FIFO -- første inn, første ut.
- **Tre (tree)**: Hierarkisk struktur.
- **Graf (graph)**: Noder og kanter som representerer forbindelser.

<div style="page-break-after:always;"></div>

## Hvorfor er dette viktig?

Riktige algoritmer og datastrukturer gjør at programmer: - Kjører raskere. - Bruker mindre minne. - Kan skalere til større datamengder.

Eksempel: Å søke i en liste på 1 million elementer kan ta sekunder med en dårlig algoritme, men millisekunder med en god algoritme og riktig datastruktur.

## Effektivitet og kompleksitet

For å vurdere algoritmer bruker vi ofte **Big-O notasjon**, som beskriver hvor raskt ressursbruk vokser når datamengden øker.

Eksempler: - **Lineært søk**: O(n) -- må sjekke alle elementer i verste fall. - **Binærsøk**: O(log n) -- deler søket i to for hvert steg.

## Oppsummering

- Algoritmer er oppskrifter på problemløsning.
- Datastrukturer er verktøykassen vi lagrer data i.
- Effektiv programmering handler om å kombinere de to på best mulig
    måte.
- Kunnskap om dette er grunnleggende for alle som vil bli gode
    utviklere og problemløsere.

