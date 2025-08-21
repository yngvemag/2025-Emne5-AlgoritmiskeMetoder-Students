namespace Benchmark;
using BenchmarkDotNet.Attributes;


[MemoryDiagnoser, RankColumn]
public class LoopBenchmark
{
    private int[] _data = [1000];

    [Benchmark]
    public int SumWithForLoop()
    {
        int sum = 0;
        for (int i = 0; i < _data.Length; i++)
        {
            sum += _data[i];
        }
        return sum;
    }

    [Benchmark]
    public int SumWithWhileLoop()
    {
        int sum = 0;
        int i = 0;
        while (i < _data.Length)
        {
            sum += _data[i];
            i++;
        }
        return sum;
    }

    [Benchmark]
    public int SumWithForeachLoop()
    {
        int sum = 0;
        foreach (var item in _data)
        {
            sum += item;
        }
        return sum;
    }
    
    [Benchmark]
    public int SumWithLinq()
    {
        return _data.Sum();
    }
   
}