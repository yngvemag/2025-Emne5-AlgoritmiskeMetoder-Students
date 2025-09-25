namespace SpanDemo;

public static class Numbers
{
    public static void NumbersArrayOnHeapExample()
    {
        int[] numbers = new int[3]{ 1, 2, 3 };
        
        foreach (var number in numbers)
        {
            Console.WriteLine(number);
        }
    }
    
    public static void NumbersWithoutSpanExample()
    {
        int[] numbers = Enumerable.Range(1, 10)
            .Select(n => n)
            .ToArray();

        foreach (var number in numbers)
        {
            Console.WriteLine(number);
        }

        Console.WriteLine("----");

        var n2 = numbers[2..6]; // fra og med indeks 2, til og med indeks 5]
        n2[0] = 42;
        foreach (var number in n2)
        {
            Console.WriteLine(number);
        }   
    }
    
    public static void NumbersSpanExample()
    {
        int[] numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9];

        foreach (var number in numbers)
        {
            Console.WriteLine(number);
        }

        Console.WriteLine("----");

        Span<int> n2 = numbers.AsSpan(2, 5);
        n2[0] = 42;
        foreach (var number in n2)
        {
            Console.WriteLine(number);
        }
    }

    public static void NumbersArrayOnStackExample()
    {
        Span<int> numbers = stackalloc int[] { 1, 2, 3, 4 , 5, 6, 7, 8, 9 };
        foreach (var number in numbers)
        {
            Console.WriteLine(number);
        }
    }
    
}