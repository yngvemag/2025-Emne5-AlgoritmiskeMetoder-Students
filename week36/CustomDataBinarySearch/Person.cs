namespace CustomDataBinarySearch;

public class Person : IComparable<Person>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }


    // -1 -> this < other
    // 0  -> this == other
    // 1  -> this > other
    public int CompareTo(Person? other)
    {
        if (other == null) return 1;
        
        int byLastName = 
            string.Compare(LastName, other.LastName, StringComparison.OrdinalIgnoreCase);
        
        // If last names are different, return that comparison result
        if (byLastName != 0)
            return byLastName;
        
        // Last names are the same, compare by first name
        int byFirstName = 
            string.Compare(FirstName, other.FirstName, StringComparison.OrdinalIgnoreCase);
        
        // If first names are different, return that comparison result
        if (byFirstName != 0)
            return byFirstName;
        
        // Last and first names are the same, compare by age
        return Age.CompareTo(other.Age);
    }

    public override string ToString() => $"FirstName: {FirstName}, LastName: {LastName}, Age: {Age}";
}
public class LastNameComparer : IComparer<Person>
{
    public int Compare(Person? x, Person? y)
    {
        if (ReferenceEquals(x, y)) 
            return 0;
        
        if (x == null) 
            return -1;
        
        if (y == null) 
            return 1;
        
        return string.Compare(x.LastName, y.LastName, StringComparison.InvariantCultureIgnoreCase);
    }
}


public class Person2
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }

    public override string ToString() => $"FirstName: {FirstName}, LastName: {LastName}, Age: {Age}";
}


public class LastNameComparer2 : IComparer<Person2>
{
    public int Compare(Person2? x, Person2? y)
    {
        if (ReferenceEquals(x, y)) 
            return 0;
        
        if (x == null) 
            return -1;
        
        if (y == null) 
            return 1;
        
        return string.Compare(x.LastName, y.LastName, StringComparison.InvariantCultureIgnoreCase);
    }
}


public class FirstNameComparer : IComparer<Person2>
{
    public int Compare(Person2? x, Person2? y)
    {
        if (ReferenceEquals(x, y)) 
            return 0;
        
        if (x == null) 
            return -1;
        
        if (y == null) 
            return 1;
        
        return string.Compare(x.FirstName, y.FirstName, StringComparison.InvariantCultureIgnoreCase);
    }
}
