# 🚀 Collection Expressions i C# 12

Collection expressions lar deg skrive kortere og mer lesbar kode for arrays, lister og spans.  
Bruk `[...]` i stedet for `new T[] { ... }`.

---

## 📊 Sammenligning: Gammel vs. Ny syntaks

| Hva | Gammel syntaks | Ny syntaks (C# 12) |
|-----|----------------|---------------------|
| **Tom array** | `int[] arr = new int[0];` | `int[] arr = [];` |
| **Array med verdier** | `int[] arr = new int[] { 1, 2, 3 };` | `int[] arr = [1, 2, 3];` |
| **Liste med verdier** | `List<string> names = new List<string> { "A", "B" };` | `List<string> names = ["A", "B"];` |
| **Kombinere arrays** | `int[] combined = arr1.Concat(arr2).ToArray();` | `int[] combined = [..arr1, ..arr2];` |
| **Blande verdier og samlinger** | `int[] mix = arr1.Concat(new[] { 99 }).Concat(arr2).ToArray();` | `int[] mix = [..arr1, 99, ..arr2];` |
| **Tom liste** | `List<int> list = new List<int>();` | `List<int> list = [];` |
| **Nested samling** | `int[][] matrix = new int[][] { new int[] {1,2}, new int[] {3,4} };` | `int[][] matrix = [[1,2], [3,4]];` |

---

## 🔹 Eksempler

### Arrays

```csharp
int[] numbers = [1, 2, 3, 4];
Console.WriteLine(string.Join(", ", numbers));
// Output: 1, 2, 3, 4
```

### Double med spread (`..`)

```csharp
double[] arr1 = [1.1, 2.2, 3.3];
double[] arr2 = [4.4, 5.5];

double[] merged = [..arr1, 99.99, ..arr2];

Console.WriteLine(string.Join(", ", merged));
// Output: 1.1, 2.2, 3.3, 99.99, 4.4, 5.5
```

### Blande typer i en liste

```csharp
List<object> stuff = ["Hello", 42, 3.14, true];
Console.WriteLine(string.Join(", ", stuff));
// Output: Hello, 42, 3.14, True
```

### Nested samlinger

```csharp
int[][] matrix = [[1, 2], [3, 4]];
Console.WriteLine(string.Join(", ", matrix[1]));
// Output: 3, 4
```

### Spans (avansert)

```csharp
ReadOnlySpan<int> span = [10, 20, 30];
Console.WriteLine(span[1]); 
// Output: 20
```

---

## ✅ Fordeler

- Kortere kode  
- Mindre støy (`new`, `ToArray`, `Concat`)  
- Mer fleksibelt – arrays, lister, spans  
- Moderne syntaks (lik JavaScript `[...arr]`, Python `[*list]`)  
