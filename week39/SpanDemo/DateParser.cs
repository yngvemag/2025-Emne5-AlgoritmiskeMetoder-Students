using BenchmarkDotNet.Attributes;

namespace SpanDemo;

public static class DateParser
{
    private static string _dateAsString = "2025-09-25";

    public static (int year, int month, int day) ParseDateWithStringAndSubstring()
    {
        var yearText = _dateAsString.Substring(0, 4);
        var monthText = _dateAsString.Substring(5, 2);
        var dayText = _dateAsString.Substring(8, 2);

        var year = int.Parse(yearText);
        var month = int.Parse(monthText);
        var day = int.Parse(dayText);
        
        return (year, month, day);
    }
    
    public static (int year, int month, int day) ParseDateWithSpan()
    {
        ReadOnlySpan<char> span = _dateAsString.AsSpan();

        var yearText = span.Slice(0, 4); //span[..4];
        var monthText = span.Slice(5, 2); //span[5..7];
        var dayText = span.Slice(8, 2); //span[8..]
        
        var year = int.Parse(yearText);
        var month = int.Parse(monthText);
        var day = int.Parse(dayText);
        
        return (year, month, day);

    }
    
}


[MemoryDiagnoser]
public class Benchy
{
    [Benchmark]
    public void BenchmarkStringAndSubstring()
    {
        var result = DateParser.ParseDateWithStringAndSubstring();
    }

    [Benchmark]
    public void BenchmarkSpanAndSlice()
    {
        var result = DateParser.ParseDateWithSpan();
    }
    
}