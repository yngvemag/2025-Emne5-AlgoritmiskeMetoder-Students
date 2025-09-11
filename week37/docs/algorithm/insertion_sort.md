# Grunnleggende innføring i Insertion Sort

Insertion Sort er en enkel og intuitiv sorteringsalgoritme som fungerer på samme måte som når vi sorterer kort på hånden: vi tar ett og ett element og setter det inn på riktig plass i den allerede sorterte delen.

---

## Hvordan fungerer Insertion Sort?

1. Start med at første element regnes som sortert.  
2. Ta neste element og sammenlign med elementene i den sorterte delen. Sett det inn på rett plass.  
3. Gjenta for alle elementer til hele listen er sortert.

---

## ASCII-illustrasjon

Eksempel: Sorter `[5, 3, 8, 2]`

```
Start:   [5 | 3, 8, 2]

Steg 1:  [3, 5 | 8, 2]   (3 settes inn foran 5)
Steg 2:  [3, 5, 8 | 2]   (8 er større enn 5 → blir liggende)
Steg 3:  [2, 3, 5, 8 |]  (2 settes inn foran 3)
```

Resultat: `[2, 3, 5, 8]`

---

## Kompleksitet

- **Tidskompleksitet:**
  - Beste: `O(n)` (allerede sortert liste)  
  - Gjennomsnitt: `O(n²)`  
  - Verste: `O(n²)`  
- **Plasskompleksitet:** `O(1)` (in-place)  
- **Stabilitet:** Ja (beholder rekkefølgen for like elementer)

---

## Stack og Heap

- **Stack:** Insertion Sort er vanligvis *iterativ*, så den bruker kun **ett stackframe**.  
- **Heap:** Ingen ekstra objekter opprettes (kun datastrukturen selv).

---
<div style="page-break-after:always;"></div>

## C# – Implementasjon (array)

```csharp
public static void InsertionSort(int[] arr)
{
    if (arr == null) return;

    int n = arr.Length;
    for (int i = 1; i < n; i++)
    {
        int key = arr[i];
        int j = i - 1;

        // Flytt elementer som er større enn key ett steg til høyre
        while (j >= 0 && arr[j] > key)
        {
            arr[j + 1] = arr[j];
            j--;
        }
        arr[j + 1] = key;
    }
}
```

**Bruk:**

```csharp
int[] data = { 5, 3, 8, 2 };
InsertionSort(data);
Console.WriteLine(string.Join(", ", data)); // 2, 3, 5, 8
```

---

## Fordeler og ulemper

### Fordeler

- Enkel å implementere og forstå.
- Stabil algoritme.
- Effektiv for små eller nesten sorterte datasett (`O(n)` i beste tilfelle).
- Brukes ofte som delalgoritme i mer avanserte sorteringsalgoritmer (f.eks. QuickSort for små partisjoner).

### Ulemper

- Ineffektiv på store og usorterte datasett (`O(n²)`).
- Krever mange flyttinger av elementer.

---

## Når brukes Insertion Sort?

- Når datasettet er **lite**.  
- Når datasettet er **nesten sortert** (små endringer).  
- Som **delalgoritme** i mer komplekse sorteringsalgoritmer.  

---

## Oppsummering

- Insertion Sort bygger en sortert del av listen ved å **sette inn elementer på riktig plass**.  
- Kompleksitet: `O(n²)`, men `O(n)` i beste tilfelle.  
- Stabil og enkel å implementere.  
- God for **små** og **nesten sorterte** datasett, men ikke egnet for store.  
