# 📊 Sammenligning: `T[]` (Array) vs `List<T>` i C #

`List<T>` og `T[]` (array) har mye overlapp, men det er også viktige forskjeller. Denne oversikten viser likheter, forskjeller, og eksempler.

---

## 🔹 Likheter

- Begge er **sekvenser av elementer** med indekstilgang (`list[i]`, `array[i]`).  
- Begge kan sorteres (`Array.Sort(array)` / `list.Sort()`).  
- Begge støtter **binærsøk** (`Array.BinarySearch` / `list.BinarySearch`).  
- Begge kan itereres med `foreach`.  
- Kompleksitet for oppslag (`[i]`) er O(1).  

---

## 🔹 Viktige forskjeller

| Egenskap | `T[]` (Array) | `List<T>` |
|----------|---------------|-----------|
| **Størrelse** | Fast – kan ikke endres | Dynamisk – vokser automatisk |
| **Resize** | Må opprette ny array og kopiere | Skjules bak `Add`, `Insert`, osv. |
| **API** | Mange statiske metoder i `Array`-klassen (`Sort`, `BinarySearch`, `Find`, osv.) | Metodene ligger direkte på `List<T>` (`Sort`, `BinarySearch`, `Find`, `Add`, `Remove`, osv.) |
| **Ytelse** | Litt raskere (mindre overhead) | Litt tregere pga. ekstra logikk (ofte ubetydelig) |
| **Bruk** | Når du vet størrelsen på forhånd og trenger rå ytelse | Når du trenger fleksibilitet (legge til/fjerne elementer) |
| **Minne** | Mer kompakt (bare elementene) | Holder en intern array, ofte med ekstra kapasitet (slack space) |

---

## 🔹 Eksempel side ved side

```csharp
// Array
int[] arr = { 1, 3, 5, 7 };
Array.Sort(arr);
int idx1 = Array.BinarySearch(arr, 5);

// List
var list = new List<int> { 1, 3, 5, 7 };
list.Sort();
int idx2 = list.BinarySearch(5);

Console.WriteLine($"{arr[idx1]} == {list[idx2]}"); // 5 == 5
```

Resultatet blir det samme – men `List<T>` er mer fleksibel i bruk.

---

## ✅ Oppsummering

- Bruk **array (`T[]`)** når:  
  - størrelsen er kjent og fast,  
  - du vil ha maks ytelse og minst minnebruk.  

- Bruk **`List<T>`** når:  
  - du trenger å legge til/fjerne elementer,  
  - du vil ha et mer moderne og praktisk API.  
