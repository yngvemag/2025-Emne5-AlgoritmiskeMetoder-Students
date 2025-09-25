

using SortedListDemo;

// Sortedlist<key, value>
SortedList<Person, Person> sortedList = new SortedList<Person, Person>(new PersonAgeComparer());

Tuple<string, int>[] people =
{
    Tuple.Create("John", 39),
    Tuple.Create("Jane", 29),
    Tuple.Create("Jack", 49),
    Tuple.Create("Jill", 19),
    Tuple.Create("Joe", 59)
};

// vi kan bruke en foreach-løkke for å fylle listen
foreach (var (name, age) in people)
{
    var person = new Person(name, age);
    sortedList.Add(person, person);
}

// vi kan bruke en for-løkke for å skrive ut listen
foreach (var person in sortedList.Values)
{
    Console.WriteLine(person);
}

// unngå duplikater keys
// Key=string, Value=List<Person>
var peopleByName = new SortedList<string, List<Person>>();

AddPerson("Yngve", 39);
AddPerson("Yngve", 66);
AddPerson("Yngve", 49);

// lage en funksjon for å legge til personer i listen
void AddPerson(string name, int age)
{
    var person = new Person(name, age);
    if (!peopleByName.TryGetValue(name, out var list))
    {
        // hvis listen ikke finnes, opprett en ny liste
        list = new List<Person>();
        peopleByName[name] = list;
    }
    
    // legg til personen i listen
    list.Add(person);
}

// skriv ut listen
foreach (var person in peopleByName["Yngve"])
{
    Console.WriteLine(person);
}