
# Forskjeller og Likheter: `SortedList<TKey, TValue>` vs `Dictionary<TKey, TValue>`

## Likheter

| Egenskap | Beskrivelse |
|-----------|-------------|
| **Generiske** | Begge er `Dictionary<TKey, TValue>`-lignende og støtter type-sikkerhet via `TKey` og `TValue`. |
| **Key-Value par** | Begge lagrer data som nøkkel/verdi-par (`KeyValuePair<TKey, TValue>`). |
| **Unike nøkler** | Ingen duplikatnøkler er tillatt. |
| **Indexer** | Du kan hente og sette verdier med `[]`: `myDict["key"] = value;` |
| **Oppslag med nøkkel** | `ContainsKey(key)` fungerer likt. |
| **Bruker hashing av nøkkel** | Begge bruker `GetHashCode()` på nøkler. *(Men SortedList bruker også sortering.)* |

---

## Forskjeller

| Funksjon / Egenskap | **Dictionary<TKey, TValue>** | **SortedList<TKey, TValue>** |
|----------------------|------------------------------|------------------------------|
| **Rekkefølge** | **Ingen garanti** for rekkefølge på elementene. | Alltid **sortert etter nøkkel** (stigende standard, eller basert på custom `IComparer<TKey>`). |
| **Intern implementasjon** | Bruker **hash-baserte buckets** → veldig rask oppslagstid. | Bruker **to interne arrayer** (en for keys og en for values). Holder alltid sortert rekkefølge. |
| **Oppslagstid (`get`)** | **O(1)** i gjennomsnitt. | **O(log n)** – binærsøk. |
| **Sett inn (`add`)** | **O(1)** i gjennomsnitt (men O(n) i verste fall ved rehashing). | **O(n)** – fordi elementer må flyttes for å holde array sortert. |
| **Fjerne (`remove`)** | O(1) i gjennomsnitt. | O(n) – fordi elementer må flyttes. |
| **Minnesforbruk** | Bruker litt ekstra minne til hash buckets. | Bruker to kompakte arrayer → mer **minneeffektiv** ved små datamengder. |
| **Iterasjon** | Iterasjon er **ikke sortert**. | Iterasjon skjer alltid i **sortert rekkefølge**. |
| **DefaultComparer** | Basert på nøkkelens `GetHashCode()`. | Basert på nøkkelens `IComparer<TKey>`. |
| **Beste bruksområde** | Når du trenger **raskt oppslag** og ikke bryr deg om rekkefølge. | Når du ofte må **iterere i sortert rekkefølge**, men med få innsettinger/slettinger. |

---

## Ytelsesanalyse

| Operasjon            | Dictionary | SortedList |
|----------------------|------------|------------|
| **Add**              | O(1)       | O(n) |
| **Remove**           | O(1)       | O(n) |
| **Lookup (ContainsKey, get)** | O(1) | O(log n) |
| **Iterasjon**        | Ingen spesiell rekkefølge | Sortert rekkefølge (O(n)) |

---

## Når bruke hva

### **Bruk Dictionary når:**

- Du **ofte legger til/fjerner elementer**.  
- Du **kun bryr deg om rask oppslagstid**, ikke rekkefølgen.  
- Store datasett der ytelse er kritisk.

**Eksempel:**

```csharp
var dict = new Dictionary<int, string>();
dict[42] = "Hello";
Console.WriteLine(dict[42]); // Rask O(1) tilgang
```

---

### **Bruk SortedList når:**

- Du **ofte trenger sortert iterasjon**.  
- Samlingen er **relativt liten** og det er få innsettinger/slettinger.  
- Du ønsker **minneeffektivitet**.

**Eksempel:**

```csharp
var sortedList = new SortedList<int, string>();
sortedList[5] = "Five";
sortedList[1] = "One";
sortedList[3] = "Three";

// Itererer alltid i sortert rekkefølge: 1, 3, 5
foreach (var kvp in sortedList)
{
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}
```

---
<div style="page-break-after:always;"></div>

## Alternativ: `SortedDictionary<TKey, TValue>`

| Egenskap                | SortedList | SortedDictionary |
|--------------------------|------------|------------------|
| **Intern struktur**      | Arrays | Balanced binary tree (rød-svart-tre) |
| **Innsetting/fjerning**  | O(n) | O(log n) |
| **Lookup**               | O(log n) | O(log n) |
| **Minnesforbruk**        | Lavt | Litt høyere |

`SortedDictionary` passer når du vil ha **sortert data** **og** mange hyppige innsettinger/slettinger.

---

## Oppsummering

- **Dictionary** → raskest for oppslag, mest brukt, ingen sortert rekkefølge.  
- **SortedList** → alltid sortert, men treg for hyppige innsettinger/fjerninger.  
- **SortedDictionary** → alltid sortert og balansert mellom ytelse og fleksibilitet.  

---

## Visualisering

Anta at vi legger inn nøkler: `4, 2, 5, 1`

**Dictionary (hash buckets):**

```
[?, 1, ?, 4, ?, 2, ?, 5, ?]  // Ingen naturlig sortering
Iterasjon → tilfeldig rekkefølge
```

**SortedList (arrays):**

```
Keys:   [1, 2, 4, 5]
Values: [A, B, C, D]
Iterasjon → alltid 1, 2, 4, 5
```

**SortedDictionary (rød-svart-tre):**

```
      4
     / \
    2   5
   /
  1
Iterasjon → 1, 2, 4, 5
```
