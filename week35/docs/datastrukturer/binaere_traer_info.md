# Trær og Binære Trær i C# — Teori, begreper og representasjoner

## 1) Hvorfor bry seg om trær?
Trær er en grunnleggende datastruktur for hierarkiske data, raske søk og komprimering. De danner fundamentet for bl.a. binære søketrær, heaps (prioritetskøer), Huffman-trær, balanserte trær (AVL, rød-svarte) og B-trær.

---

## 2) Grunnbegreper i (binære) trær
**Definisjon:** Et binærtre er en samling noder (kan være tom) med kanter mellom foreldre og barn. Hver node har maks to barn; om den har to, kaller vi dem **venstre** og **høyre**. Roten har ingen forelder.

**Nivå (level):** Roten er på nivå 0; barn på nivå 1; barnebarn på nivå 2; etc.  
**Subtre:** Hver node kan ses som rot for sitt venstre/høyre subtre.  
**Bladnode:** Node uten barn. **Indre node:** Node med ett eller to barn.

**Høyde og dybde:** Høyden til treet er lengden på lengste vei fra rot til et blad. Høyden til en node = høyden til subtret med noden som rot. Dybden til en node = avstanden fra roten. Tomt tre kan defineres å ha høyde −1.

**Perfekt tre:** Alle nivåer er fulle. På nivå *k* er det plass til `2^k` noder.

### Mini-ASCII: begrepene
```
           (Rot)
             A               nivå 0
           /   \
          B     C            nivå 1
         / \   / \
        D   E F   G          nivå 2
              \
               H             blad = node uten barn
```

---

## 3) Binære trær i algoritmer (intuisjon)
- **Høyden styrer kostnad**: Mange operasjoner er proporsjonale med trehøyden. Et balansert tre gir ofte ~O(log n); et skjevt tre kan bli ~O(n).  
- **Strukturelle garantier** i bl.a. heaps og balanserte søketrær gir forutsigbar ytelse.

---
<div style="page-break-after:always;"></div>

## 4) Traverseringer (rekkefølgen vi «besøker» noder)
- **Bredde først / nivåorden**: nivå for nivå fra venstre mot høyre (bruker kø).  
- **Dybde først**: preorden (node–venstre–høyre), inorden (venstre–node–høyre), postorden (venstre–høyre–node).  
Nivåorden beskrives med kø; pre/in/post beskrives rekursivt eller med stakk.

**ASCII: nivåorden fra venstre mot høyre**
```
    E
   / \
  I   B
     / \
    G   A
...  => Nivåorden: E, I, B, ...
```

---

## 5) Nodeposisjoner (1-basert) og binære stier
Vi kan tilordne hver node et **posisjonstall** (1, 2, 3, …), der **rot = 1** og barna til node *k* har posisjoner **2k** (venstre) og **2k+1** (høyre). Dette er spesielt nyttig i «komplette»/tette trær (som heaps).

**ASCII: posisjoner i et perfekt binærtre**
```
             (1)
           /     \
         (2)     (3)
        /  \     /  \
      (4)  (5) (6)  (7)
```

**0/1 langs kanter:** Merker vi venstrekant med **0** og høyrekant med **1**, får vi en direkte kobling mellom binærrepresentasjon og posisjon. Går vi fra roten og leser 0/1 til en node, får vi posisjonstallet (uten den ledende 1-eren). Eksempel: Stien `1→0→1→1→0` gir posisjon **10110₂ = 22**.

**ASCII: 0/1-merkede kanter**
```
           (1)
          /   \
       0 /     \ 1
        /       \
     (2)        (3)
     / \        / \
  0 /   \ 1  0 /   \ 1
   /     \    /     \
 (4)    (5) (6)    (7)

```

---

## 6) Hvor mange forskjellige binære trær finnes for n noder?
Lar vi **C(n)** være antall isomorft forskjellige binære trær med *n* noder, får vi den klassiske **Catalan-rekurrensen**:
```
C(n) = C(0)·C(n-1) + C(1)·C(n-2) + ... + C(n-1)·C(0)
```
De første verdiene: 1, 1, 2, 5, 14, 42, 132, … (C(0) = 1).

---

## 7) Vanlige representasjoner i kode

### A) Node/referanse (pekere)
Den mest fleksible formen: hver node har en verdi og to referanser (Left/Right). Godt egnet for generelle trær, dynamiske oppdateringer og når treet er glissent eller endrer form.

### B) Tabell/heap-stil (1-basert indeks med posisjonstall)
Bruk en tabell `A` der `A[1]` er roten, `A[2k]` venstre barn og `A[2k+1]` høyre barn. Svært effektivt for komplette trær (klassisk for heaps), men kan bli plasssløsing for glisne trær.

**ASCII: layout i tabell**
```
Index:  1   2   3   4   5   6   7
Verdi:  A   B   C   D   E   F   G
```

### C) Posisjonskart (dictionary/map fra posisjon → verdi)
Representer bare **eksisterende** noder i et **map** (f.eks. `Dictionary<int,T>`), der nøkkelen er posisjonen.

### D) Parallelle tabeller
Bruk tre tabeller: `value[]`, `left[]`, `right[]`. Godt når antall noder er kjent og relativt statisk.

---
<div style="page-break-after:always;"></div>

## 8) Små «husketips» og sammenhenger
- **Perfekt nivåkapasitet:** nivå *k* har plass til `2^k` noder. Totalt opp til `2^(h+1) - 1` for høyde *h*.
- **Posisjoner:** barn av k er 2k og 2k+1; forelder er ⌊k/2⌋.
- **0/1-kanter:** stien fra roten koder posisjonen i binær (uten ledende 1).
- **Traversering:** nivåorden = kø; pre/in/post = rekursjon eller stakk.
- **Antall former:** C(n) følger Catalan-rekurrensen.

---

## 9) Litt større ASCII-eksempel (nivåer, blad/indre, posisjoner og 0/1-stier)

```
Nivå 0:                   (A)[1]
                         /       \
Nivå 1:              (B)[2]     (C)[3]
                     /    \      /    \
Nivå 2:          (D)[4]  (E)[5] (F)[6] (G)[7]
                          \
Nivå 3:                   (H)[11]

Merk:
- [k] er posisjonstallet.
- Barn: 2k (venstre), 2k+1 (høyre). Eks: [5]s høyre barn = [11].
- 0/1 langs kanter fra roten koder posisjonen: roten har "1".
  Sti til [11] er: 1 → 0 (til [2]) → 1 (til [5]) → 1 (til [11])  ⇒ 1011₂ = 11.
- D, H er blader (ingen barn). Andre er indre noder.
```


---
## 10) Binære Søketre (BST) og organisering av verdier

Ikke alle binære trær er binære søketrær (BST). Et binært tre sier kun at hver node kan ha maks to barn.
Et **binært søketre** har følgende regel:

- For hver node: 
  - Alle verdier i venstre subtre er **mindre** enn nodens verdi.
  - Alle verdier i høyre subtre er **større** enn nodens verdi (noen ganger >= avhengig av implementasjon).
- Denne regelen gjelder rekursivt for alle noder i treet.

### Må roten være midterste verdi?
Nei, **det er ikke et krav**. Midterste verdi som rot brukes når man ønsker å lage et **balansert BST** fra en sortert liste for å oppnå lavest mulig høyde. 
Men innsettingsrekkefølgen bestemmer faktisk strukturen dersom treet ikke balanseres.

### Eksempel: balansert BST fra sortert liste
Sorterte tall: 1, 2, 3, 4, 5, 6, 7  
Velg midterste tall som rot (4):

```
        4
       / \
      2   6
     / \ / \
    1  3 5  7
```

Dette gir et balansert tre med høyde 2.

### Eksempel: ubalansert BST ved innsetting i stigende rekkefølge
Setter vi inn 1, 2, 3, 4, 5, 6, 7 uten balansering:

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
         \
          6
           \
            7
```

Dette er også et gyldig BST (alle venstre < node < høyre), men svært ubalansert (høyde = n-1).

### Oppsummering:
- Binært søketre (BST) har ordensregelen venstre < node < høyre.
- Midterste verdi som rot er kun en teknikk for å balansere treet.
- Vanlige binære trær kan ha hvilken som helst organisering av verdier.
