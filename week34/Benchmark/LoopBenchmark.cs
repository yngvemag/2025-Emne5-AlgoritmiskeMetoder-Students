using BenchmarkDotNet.Attributes;

// Benchmark-klasse med ulike loop-varianter
[MemoryDiagnoser, RankColumn]
public class LoopBenchmark
{
    private int[] data = [1000];

    [Benchmark]
    public int SumWithForLoop()
    {
        int sum = 0;
        for (int i = 0; i < data.Length; i++)
        {
            sum += data[i];
        }
        return sum;
    }

    [Benchmark]
    public int SumWithForeachLoop()
    {
        int sum = 0;
        foreach (var item in data)
        {
            sum += item;
        }
        return sum;
    }

    [Benchmark]
    public int SumWithWhileLoop()
    {
        int sum = 0;
        int i = 0;
        while (i < data.Length)
        {
            sum += data[i];
            i++;
        }
        return sum;
    }

    [Benchmark]
    public int SumWithLinq()
    {
        return data.Sum();
    }

}