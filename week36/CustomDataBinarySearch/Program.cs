
using CustomDataBinarySearch;

TestPeople();
//TestPeople2();

static void TestPeople()
{
    var people = new[]
    {
        new Person(){FirstName = "John", LastName = "Doe", Age = 30},
        new Person(){FirstName = "Mary", LastName = "Hopper", Age = 50},
        new Person(){FirstName = "Alan", LastName = "Turing", Age = 60},
        new Person(){FirstName = "Bjarne", LastName = "Hansen", Age = 35},
        new Person(){FirstName = "Gunn", LastName = "Olsen", Age = 24},
    };

    Console.WriteLine("Before sorting:");
    foreach( var person in people )
        Console.WriteLine(person);

    Array.Sort(people);

    Console.WriteLine("\nAfter sorting:");
    foreach( var person in people )
        Console.WriteLine(person);

    int idx = Array.BinarySearch(
        people,
        new Person()
        {
            FirstName = "", 
            LastName = "hansen", 
            Age = 0
        },
        new LastNameComparer());
    
    Console.WriteLine(
        idx >= 0
            ? $"\nFound at index {idx}: {people[idx]}"
            : "\nNot found"
        );
}

static void TestPeople2()
{
    var people = new[]
    {
        new Person2(){FirstName = "John", LastName = "Doe", Age = 30},
        new Person2(){FirstName = "Mary", LastName = "Hopper", Age = 50},
        new Person2(){FirstName = "Alan", LastName = "Turing", Age = 60},
        new Person2(){FirstName = "Bjarne", LastName = "Hansen", Age = 35},
        new Person2(){FirstName = "Gunn", LastName = "Olsen", Age = 24},
    };

    Console.WriteLine("Before sorting:");
    foreach( var person in people )
        Console.WriteLine(person);

    Array.Sort(people, new FirstNameComparer());

    Console.WriteLine("\nAfter sorting:");
    foreach( var person in people )
        Console.WriteLine(person);
    
    int idx = Array.BinarySearch(
        people,
        new Person2() { LastName = "Hansen", FirstName = "" , Age = 0},
        new LastNameComparer2());
    
    Console.WriteLine(
        idx >= 0
            ? $"\nFound at index {idx}: {people[idx]}"
            : "\nNot found"
    );
}
