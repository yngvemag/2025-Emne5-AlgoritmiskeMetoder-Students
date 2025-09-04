# Forklaring av IComparer<T> i .NET C #

`IComparer<T>` er et **generisk interface** i .NET som lar deg definere **egendefinert sorteringslogikk** for objekter.  
Det brukes blant annet av metoder som `List<T>.Sort()` og `Array.Sort()`.

---

## Grunnidé

Interface'et har én metode som **må implementeres**:

```csharp
int Compare(T x, T y);
```

Denne metoden skal returnere:

- `< 0` hvis `x` er **mindre enn** `y`
- `0` hvis `x` er **lik** `y`
- `> 0` hvis `x` er **større enn** `y`

Dette gjør at du kan **bestemme selv hvordan objekter sammenlignes og sorteres**.

---

## Eksempel 1: Sortere tall i synkende rekkefølge

```csharp
using System;
using System.Collections.Generic;

class DescendingComparer : IComparer<int>
{
    public int Compare(int x, int y)
    {
        return y.CompareTo(x); // Reverserer standard sortering
    }
}

class Program
{
    static void Main()
    {
        var numbers = new List<int> { 5, 1, 8, 3 };

        numbers.Sort(new DescendingComparer());

        Console.WriteLine(string.Join(", ", numbers));
        // Output: 8, 5, 3, 1
    }
}
```

---

## Eksempel 2: Sortere en liste med objekter

```csharp
using System;
using System.Collections.Generic;

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

public class AgeComparer : IComparer<Person>
{
    public int Compare(Person x, Person y)
    {
        return x.Age.CompareTo(y.Age);
    }
}

class Program
{
    static void Main()
    {
        var people = new List<Person>
        {
            new Person { Name = "Anna", Age = 30 },
            new Person { Name = "Bjørn", Age = 25 },
            new Person { Name = "Clara", Age = 35 }
        };

        people.Sort(new AgeComparer());

        foreach (var p in people)
        {
            Console.WriteLine($"{p.Name}, {p.Age}");
        }
        // Output:
        // Bjørn, 25
        // Anna, 30
        // Clara, 35
    }
}
```

---
<div style="page-break-after:always;"></div>

## Hvorfor bruke `IComparer<T>`?

- Gir **full kontroll** over hvordan objekter sammenlignes.
- Lar deg gjenbruke samme sammenligningslogikk flere steder.
- Kan brukes sammen med standard .NET-metoder for sortering.

---

## Oppsummering

| Verdi returnert av Compare | Betydning |
|----------------------------|-----------|
| `< 0`                       | `x` er mindre enn `y` |
| `0`                         | `x` er lik `y` |
| `> 0`                       | `x` er større enn `y` |

`IComparer<T>` er nyttig når du vil **styre sorteringsrekkefølgen** på dine egne objekter på en enkel og fleksibel måte.
