# 📚 Binærsøk på `Person` med etternavn som nøkkel

Hvis du ønsker å søke på personer basert på etternavn kan du bruke **`SortedList<string, Person>`**, men det har noen begrensninger.

---

## 🔹 `SortedList<TKey, TValue>` recap

- Holder nøkkel–verdi-par i **sortert rekkefølge** basert på nøkkelen.  
- Oppslag gjøres med **binærsøk internt** (O(log n)).  
- **Nøkler må være unike.**  

Eksempel:

```csharp
var peopleByLastName = new SortedList<string, Person>();
peopleByLastName.Add("Hopper", new Person("Grace", "Hopper", 85));

var person = peopleByLastName["Hopper"]; // Direkte oppslag
```

---

## 🔹 Problemet med duplikate etternavn

Hvis du legger til flere personer med samme etternavn:

```csharp
peopleByLastName.Add("Hopper", new Person("Grace", "Hopper", 85));
peopleByLastName.Add("Hopper", new Person("Dennis", "Hopper", 50)); // 💥 Exception
```

→ Dette kaster en `ArgumentException` fordi **duplikate keys ikke er tillatt**.

---
<div style="page-break-after: always;"></div>

## 🔹 Løsninger

### 1. `SortedList<string, List<Person>>`

Lag nøkkelen = etternavn, verdien = liste med personer.

```csharp
var peopleByLastName = new SortedList<string, List<Person>>();

void AddPerson(Person p) {
    if (!peopleByLastName.TryGetValue(p.LastName, out var list))
    {
        list = new List<Person>();
        peopleByLastName[p.LastName] = list;
    }
    list.Add(p);
}

// Bruk
AddPerson(new Person("Grace", "Hopper", 85));
AddPerson(new Person("Dennis", "Hopper", 50));

foreach (var p in peopleByLastName["Hopper"])
    Console.WriteLine(p);
```

✅ Tillater flere personer per nøkkel.  
✅ Beholder rekkefølge sortert på nøkkel.  
❌ Litt mer kode.

---
<div style="page-break-after: always;"></div>

### 2. `SortedDictionary<string, List<Person>>`

Samme idé, men basert på en **rød-svart-tre** i stedet for intern array.

```csharp
var peopleByLastName = new SortedDictionary<string, List<Person>>();

// Samme AddPerson-metode som over
```

---

## 🔹 `SortedList` vs `SortedDictionary`

Begge gir **sorterte nøkkel–verdi-par**, men de er implementert forskjellig:

| Egenskap | `SortedList<TKey,TValue>` | `SortedDictionary<TKey,TValue>` |
|----------|----------------------------|----------------------------------|
| **Intern struktur** | To interne arrays (én for keys, én for values) | Rød-svart-tre |
| **Oppslag (ContainsKey / indexer)** | O(log n) (binærsøk i array) | O(log n) (treoppslag) |
| **Innsetting / fjerning** | O(n) i verste fall (må flytte elementer i arrayet) | O(log n) |
| **Minnebruk** | Mer kompakt (mindre overhead) | Mer overhead pga. tre-struktur |
| **Tilfeldig tilgang via indeks** | Ja, kan slå opp via `Keys[index]` og `Values[index]` | Nei, kun via nøkkel |
| **Ytelse best når** | Mange oppslag, få innsettinger/slettinger | Hyppige innsettinger/slettinger |

**Oppsummert:**  

- `SortedList` er raskere og mer minneeffektiv når du har mange oppslag og relativt sjelden legger til/fjerner.  
- `SortedDictionary` er bedre hvis du ofte legger til eller fjerner elementer.  

---
<div style="page-break-after: always;"></div>

### 3. `ILookup<string, Person>` med LINQ

`ILookup` er en **read-only oppslagsstruktur** som lages via **LINQ**.  
Den bygger underliggende på en **`Dictionary<TKey, List<TElement>>`**, altså en hash-baserte struktur, ikke sortert.

```csharp
var lookup = people.ToLookup(p => p.LastName);

foreach (var p in lookup["Hopper"])
    Console.WriteLine(p);
```

✅ Enkelt å bruke for gruppering.  
✅ Støtter flere elementer per nøkkel direkte.  
❌ Ikke sortert på nøkkel som standard.  
❌ Read-only (kan ikke legge til etter opprettelse).  

---
<div style="page-break-after: always;"></div>

## 🔹 Rød-svart-tre (bak `SortedDictionary`)

Et **rød-svart-tre** er en type **selvbalanserende binært søketre**.  

Egenskaper:  

- Hver node er merket **rød** eller **svart**.  
- Røtter og null-noder (`null leaves`) er alltid svarte.  
- Ingen vei fra rot til blad har mer enn **dobbelt så mange noder som en annen vei** → sikrer balanse.  
- Dette gir at alle operasjoner (`Add`, `Remove`, `ContainsKey`) har **O(log n)** tid.  

Fordeler:  

- Holder treet balansert **automatisk**.  
- Gir jevn ytelse selv med mange innsettinger og slettinger.  

---

## 🔹 ASCII Flytskjema – valg av struktur

```
                 Vil du ha sorterte nøkler?
                           |
              +------------+------------+
              |                         |
             Nei                       Ja
              |                         |
         Bruk ILookup          Skal du ofte sette inn/fjerne?
       (LINQ ToLookup)               |
                                     +-------------+
                                     |             |
                                   Ofte           Sjelden
                                     |             |
                           Bruk SortedDictionary   Bruk SortedList
```

---
<div style="page-break-after: always;"></div>

## 🔹 Best practice

- **Unike etternavn?** → Bruk `SortedList<string, Person>`.  
- **Flere personer per etternavn?** → Bruk `SortedList<string, List<Person>>` eller `SortedDictionary<string, List<Person>>`.  
- **Gruppering uten sortering, read-only** → Bruk `ILookup` (LINQ `ToLookup`).  

---

## ✅ Oppsummering

- `SortedList<string, Person>` fungerer kun hvis etternavn er **unike**.  
- For flere personer per nøkkel, pakk verdiene inn i en **liste**.  
- `SortedList` og `SortedDictionary` gir begge **sorterte oppslag**, men med ulike styrker:  
  - `SortedList` → kompakt, rask på oppslag, tregere på innsetting/sletting.  
  - `SortedDictionary` → litt tyngre, men bedre når innsetting/sletting skjer ofte.  
- `ILookup` gir enkel **gruppering** basert på en hash-basert dictionary (`Dictionary<TKey, List<T>>`), men er **ikke sortert** og **read-only**.  
