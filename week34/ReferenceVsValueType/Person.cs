namespace ReferenceVsValueType;

public class Person
{
    public string Name { get; set; } = string.Empty;

    public static void ChangePerson(Person p)
    {
        p.Name = "Name Changed!";
    }
    
    public static void ReplacePerson(ref Person p)
    {
        p = new Person{Name = "Yngve"};
    }
}