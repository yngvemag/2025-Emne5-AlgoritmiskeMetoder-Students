namespace SortedListDemo;

public sealed class Person() // Merk den parameterløse konstruktøren
{
    public Person(string name, int age) : this() // Kall den parameterløse konstruktøren for å initialisere init-only properties
    {
        Name = name;
        Age = age;
    }
    
    public string Name { get; init; } = string.Empty;
    public int Age { get; init; }    
    
    public override string ToString() => $"{Name} {Age}";
}

public sealed class PersonAgeComparer : IComparer<Person>
{
    public int Compare(Person? x, Person? y)
    {
        if (ReferenceEquals(x, y)) return 0;

        if (x is null) return -1;
        if (y is null) return 1;

        return x.Age.CompareTo(y.Age);        
    }
}