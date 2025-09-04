# Grunnleggende innføring i lineært søk (sekvensielt søk)

Lineært søk er den enkleste søkealgoritmen: vi går **element for element** gjennom en samling (f.eks. en tabell/array eller liste) til vi finner det vi leter etter — eller vi når slutten uten treff.

---

## Når bruker vi lineært søk?

- Når **datastrukturen er liten** eller søk skjer sjelden.
- Når **data ikke er sortert** (binærsøk krever sortert data).
- Når **innsettingsrekkefølge** eller **stabilitet** er viktig (første treff).
- Når datastrukturen ikke har raskere oppslag tilgjengelig (f.eks. `List<T>` i stedet for `Dictionary<K,V>`).

---

## Kompleksitet

- **Tidskompleksitet:**  
  - Beste fall: `O(1)` (første element er treff)  
  - Gjennomsnitt: `O(n)`  
  - Verste fall: `O(n)` (elementet er sist eller finnes ikke)
- **Plasskompleksitet:** `O(1)` (iterativ) — trenger bare noen få variabler.

---

## Stack og heap (kort repetisjon)

- **Stack:** Funksjonskall og lokale variabler. Et *iterativt* lineært søk bruker normalt **ett** stackframe. Et *rekursivt* lineært søk vil bruke **ett stackframe per kall** (kan bli opptil `n`) og er derfor lite hensiktsmessig.
- **Heap:** Inneholder objekter (f.eks. selve arrayet/listen). Søk **oppretter normalt ikke nye objekter** på heapen.

---

## Grunnidé (ASCII)

Vi skanner fra venstre mot høyre og sammenligner:

```
Index:  0   1   2   3   4
Data:  [7] [3] [9] [2] [5]
Target: 9

Sammenlign i rekkefølge:
i=0: 7 == 9? nei
i=1: 3 == 9? nei
i=2: 9 == 9? JA -> returner 2
```

---
<div style="page-break-after:always;"></div>

## C# – Iterativ implementasjon (array)

```csharp
public static int LinearSearch(int[] arr, int target)
{
    if (arr == null) return -1; // defensiv sjekk

    for (int i = 0; i < arr.Length; i++)
    {
        if (arr[i] == target)
            return i; // første indeks der vi finner target
    }

    return -1; // ikke funnet
}
```

**Bruk:**

```csharp
int[] data = { 7, 3, 9, 2, 5 };
int index = LinearSearch(data, 9); // -> 2
```

---
<div style="page-break-after:always;"></div>

## C# – Rekursiv variant (kun for læringsformål)

Rekursjon er sjelden praktisk for lineært søk, men kan demonstrere kallstakken.

```csharp
public static int LinearSearchRecursive(int[] arr, int target, int index = 0)
{
    if (arr == null) return -1;
    if (index >= arr.Length) return -1;        // basis 1: slutt uten treff
    if (arr[index] == target) return index;    // basis 2: fant elementet
    return LinearSearchRecursive(arr, target, index + 1); // rekursivt trinn
}
```

**Stack (for søk etter 9):**

```
LSR(arr, 9, 0) -> LSR(arr, 9, 1) -> LSR(arr, 9, 2) -> return 2
```

---
## Sammenligning med andre metoder

| Problem | Lineært søk | Binærsøk | Hash-baserte oppslag (`Dictionary`, `HashSet`) |
|---|---|---|---|
| Krever sortert data | Nei | Ja | Nei |
| Forventet tid | O(n) | O(log n) | O(1) gj.snitt |
| Verste tid | O(n) | O(log n) | O(n) |
| Minnetillegg | O(1) | O(1) | O(n) |
| Stabilitet (første treff) | Ja | Ja | Ikke relevant |

---

## Edge cases & testidéer

- Tom samling (`Length == 0`) → return `-1`/`false`.
- `null`-referanser → defensiv sjekk.
- Duplikater → forvent **første indeks**.
- Ulike likhetsregler (case-insensitiv tekst, kulturspesifikke sammenligninger).
- Store datasett → vurder alternativ datastruktur.

---

## Kort oppsummering

- Lineært søk går gjennom elementene **ett for ett** og er enkelt å implementere.
- Beste bruk: **små eller usorterte datasett**, eller når søk skjer sjelden.
- Kompleksitet: `O(n)` tid, `O(1)` plass (iterativ).  
- Rekursiv variant finnes, men er mest **pedagogisk** – ikke praktisk i produksjonskode.
