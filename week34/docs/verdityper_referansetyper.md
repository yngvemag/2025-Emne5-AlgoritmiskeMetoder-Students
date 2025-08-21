# Verdityper og Referansetyper i C#

Når vi programmerer i C#, er det viktig å forstå forskjellen mellom **verdityper** og **referansetyper**. 
Dette påvirker hvordan data lagres i minnet, og hvordan variabler oppfører seg når vi kopierer eller sender dem til metoder.

---

## Verdityper (Value Types)

- Lagrer selve verdien direkte i minnet.  
- Når du kopierer en variabel med en verditype, får du en *kopi* av verdien.  
- Endringer i en kopi påvirker ikke den originale variabelen.  
- Verdityper ligger som regel på **stacken**.

### Eksempler på verdityper

- `int`
- `double`
- `bool`
- `struct`
- `enum`

### Eksempel

```csharp
int a = 10;
int b = a;   // b gets a copy of the value of a

b = 20;

Console.WriteLine(a); // 10
Console.WriteLine(b); // 20
```
Her ser vi at `a` beholder sin verdi selv om `b` endres.

---

## Referansetyper (Reference Types)

- Lagrer en *referanse* (peker) til hvor objektet er lagret i minnet (på heapen).  
- Når du kopierer en variabel med en referansetype, peker begge variablene på **samme objekt**.  
- Endringer gjennom én variabel påvirker også den andre.  

### Eksempler på referansetyper

- `class`
- `object`
- `string` (spesiell, men behandles som en referansetype)
- `array`

### Eksempel

```csharp
class Person
{
    public string Name { get; set; }
}

Person p1 = new Person();
p1.Name = "Anna";

Person p2 = p1;  // p2 points to the same object as p1

p2.Name = "John";

Console.WriteLine(p1.Name); // John
Console.WriteLine(p2.Name); // John
```
Her ser vi at både `p1` og `p2` peker på samme objekt i minnet.

---

## Viktig å merke seg

1. **String er spesiell**  
   Selv om `string` er en referansetype, oppfører den seg som en verditype fordi den er **immutabel** (kan ikke endres etter opprettelse). Når du endrer en `string`, opprettes en ny i stedet.

   ```csharp
   string s1 = "Hello";
   string s2 = s1;
   s2 = "Hi";

   Console.WriteLine(s1); // Hello
   Console.WriteLine(s2); // Hi
   ```

2. **Struct vs Class**  
   - `struct` er en verditype.  
   - `class` er en referansetype.  

3. **Metodekall**  
   Når vi sender en verditype til en metode, sendes en kopi.  
   Når vi sender en referansetype, sendes en peker til objektet.  

   ```csharp
   void ChangeNumber(int x)
   {
       x = 100;
   }

   void ChangePerson(Person p)
   {
       p.Name = "Mary";
   }

   int number = 5;
   ChangeNumber(number);
   Console.WriteLine(number); // 5 (ingen endring)

   Person p = new Person() { Name = "Peter" };
   ChangePerson(p);
   Console.WriteLine(p.Name); // Mary (endret)
   ```

---

## Oppsummering

| Type           | Lagring | Eksempel     | Oppførsel ved kopiering |
|----------------|---------|-------------|-------------------------|
| **Verditype**  | Stack   | int, double, bool, struct, enum | Kopieres som en ny verdi |
| **Referansetype** | Heap  | class, object, string, array | Kopieres som en peker til samme objekt |

---

Dette skillet mellom verdityper og referansetyper er grunnleggende for å forstå hvordan data håndteres i C#, 
og hvorfor endringer i objekter kan påvirke flere variabler samtidig.

---
<div style="page-break-after:always;"></div>

## out, ref og in i C#

I C# kan du bruke nøkkelordene `out`, `ref` og `in` for å styre hvordan argumenter sendes til metoder. Dette gjelder både verdityper og referansetyper.

### `ref`
Med `ref` kan du sende en variabel til en metode slik at endringer i metoden påvirker originalen. Gjelder både verdityper og referansetyper.

```csharp
void EndreMedRef(ref int x)
{
    x = 42;
}

int tall = 5;
EndreMedRef(ref tall);
Console.WriteLine(tall); // 42

void EndrePersonRef(ref Person p)
{
    p = new Person { Name = "Ny" };
}

Person person = new Person { Name = "Opprinnelig" };
EndrePersonRef(ref person);
Console.WriteLine(person.Name); // Ny
```

### `out`
`out` brukes når en metode skal returnere en verdi via argumentet. Variabelen må settes i metoden.

```csharp
void SettUt(out int x)
{
    x = 99;
}

int verdi;
SettUt(out verdi);
Console.WriteLine(verdi); // 99
```
<div style="page-break-after:always;"></div>

### `in`
`in` gir readonly-tilgang til argumentet i metoden. Gjelder hovedsakelig verdityper (f.eks. struct).

```csharp
void SkrivIn(in Point p)
{
    // p.X = 77; // Ikke lov, kun lesing
    Console.WriteLine(p.X);
}

Point pt = new Point { X = 7, Y = 8 };
SkrivIn(in pt); // Skriver ut 7
```

### Oppsummering

| Nøkkelord | Kan endre original? | Må settes i metoden? | Kun lesing? |
|-----------|---------------------|----------------------|-------------|
| `ref`     | Ja                  | Nei                 | Nei         |
| `out`     | Ja                  | Ja                  | Nei         |
| `in`      | Nei                 | Nei                 | Ja          |

Disse nøkkelordene gir deg kontroll over hvordan data sendes og endres i metoder, og er nyttige for både verdityper og referansetyper.
