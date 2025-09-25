
# C# Range & Index – kort oppsummering

Denne oversikten viser hvordan du bruker **range** (`a[b..c]`) og **index-fra-slutt** (`^n`) i C# (8+).

---

## Grunnidé

- `a[b..c]` er et **halvåpent intervall**: tar elementene fra **b** (inkludert) til **c** (ekskludert).
- Fungerer på **arrays** (`T[]`), **string**, og **Span/ReadOnlySpan/Memory**.
- **Arrays/strings** → returnerer **ny kopi**.  
  **Span/Memory** → returnerer **slice/visning** (ingen allokering).

```csharp
var numbers = new[] { 10, 11, 12, 13, 14, 15, 16, 17 };

var a = numbers[2..6];   // {12, 13, 14, 15}
var b = numbers[..3];    // {10, 11, 12} (første tre)
var c = numbers[4..];    // {14, 15, 16, 17} (hale fra indeks 4)
var d = numbers[..];     // kopi av hele arrayen
```

---

## Indeksering fra slutten (`^`)

- `^1` er **siste** element, `^2` nest siste, osv.
- Kan kombineres med range.

```csharp
var last = numbers[^1];     // 17
var allButLast = numbers[..^1]; // alt unntatt siste
var middle = numbers[2..^2];    // fra indeks 2 til (lengde-2) ekskludert
```

---

## Med `Span<T>` (ingen allokering)

```csharp
int[] data = Enumerable.Range(0, 10).ToArray();
Span<int> slice = data.AsSpan()[2..6];  // "visning" (0 allokering)
int[] copy = slice.ToArray();            // allokerer først nå
```

---
<div style="page-break-after:always;"></div>

## Hva støtter range?

| Type               | `a[b..c]` | Effekt                          |
|--------------------|-----------|----------------------------------|
| `T[]` (array)      | Ja        | **Ny array** (kopi)              |
| `string`           | Ja        | **Ny string** (substring)        |
| `Span<T>`/`Memory` | Ja        | **Slice** (referanse/visning)    |
| `List<T>`          | Nei       | Bruk `GetRange(start, count)` eller `list.AsSpan()[range]` |

---

## Vanlige mønstre

```csharp
var firstN = numbers[..N];
var lastN  = numbers[^N..];      // de siste N
var skipFirst = numbers[1..];    // hopp over første
var skipEnds  = numbers[1..^1];  // hopp over første og siste
```

---

## Fallgruver

- **Slutt er ekskludert** → `2..6` gir 4 elementer (2,3,4,5).
- **Start må være ≤ slutt**, og begge må ligge innenfor lengden → ellers `ArgumentOutOfRangeException`.
- På **array/string** er dette **ikke in-place**; du får en **kopi**.

---

## Under panseret (kort)

C# oversetter `b..c` til `System.Range` (og `^n` til `System.Index`). For arrays/strings blir det grensesjekk + `Array.Copy`/substring; for `Span<T>` beregnes et nytt slice mot samme buffer.

---

## Kjapp sjekkliste

- Trenger du **ytelse uten allokering**? → bruk `Span<T>/ReadOnlySpan<T>`.
- Trenger du **ny samling** å endre på? → array-subrange (kopi) er fint.
- Husk at `^` teller **fra slutten**.

```csharp
// Eksempler å huske
var x = arr[2..^1];   // midten
var y = arr[^3..];    // tre siste
var z = s[5..];       // substring fra posisjon 5
```
