using BenchmarkDotNet.Attributes;

namespace Recursion;


[MemoryDiagnoser]
public class SumBenchmark
{
    [Params(10, 100, 1000, 5_000)]
    public int N { get; set; }

    [Benchmark]
    public long SumPositiveNumbersInLoop() => SumPositiveNumbersLoop(N);
    
    [Benchmark]
    public long SumPositiveNumbersInRecursive() => SumPositiveNumbersRecursive(N);
    
    private static long SumPositiveNumbersLoop(int n)
    {
        if (n < 0)
            throw new ArgumentException("Argument n must be non-negative");

        long sum = 0;
        for (int i = 1; i <= n; i++)
            sum += i;

        return sum;
    }
    
    // SumPositiveNumbersRecursive(5) = 5 + 4 + 3 + 2 + 1
    private static long SumPositiveNumbersRecursive(int n)
    {
        if (n < 0)
            throw new ArgumentException("Argument n must be non-negative");
        
        // Base case - stopping condition
        if (n <= 1) return n;

        // recursive case
        return n + SumPositiveNumbersRecursive(n - 1);
    }
    
}


// | Method                      | N       | Mean           | Error         | StdDev         | Median         | Allocated |
// |---------------------------- |-------- |---------------:|--------------:|---------------:|---------------:|----------:|
// | SumPositiveNumbersRecursive | 10      |       5.724 ns |     0.0420 ns |      0.0393 ns |       5.721 ns |         - |
// | SumPositiveNumbersLoop      | 10      |       3.207 ns |     0.0120 ns |      0.0106 ns |       3.205 ns |         - |
// | SumPositiveNumbersRecursive | 100     |     210.324 ns |    20.1947 ns |     59.5447 ns |     233.195 ns |         - |
// | SumPositiveNumbersLoop      | 100     |      41.115 ns |     0.4500 ns |      0.4209 ns |      41.103 ns |         - |
// | SumPositiveNumbersRecursive | 1000    |   2,378.964 ns |   385.4031 ns |  1,136.3699 ns |   3,071.306 ns |         - |
// | SumPositiveNumbersLoop      | 1000    |     318.722 ns |     3.0293 ns |      2.8336 ns |     318.720 ns |         - |
// | SumPositiveNumbersRecursive | 10000   |  42,738.156 ns | 3,679.3170 ns | 10,848.5501 ns |  46,062.509 ns |         - |
// | SumPositiveNumbersLoop      | 10000   |   3,147.032 ns |    41.8950 ns |     39.1886 ns |   3,141.656 ns |         - |
// | SumPositiveNumbersRecursive | 100000  |             NA |            NA |             NA |             NA |        NA |
// | SumPositiveNumbersLoop      | 100000  |  31,100.784 ns |   440.6577 ns |    412.1915 ns |  30,894.449 ns |         - |
// | SumPositiveNumbersRecursive | 1000000 |             NA |            NA |             NA |             NA |        NA |
// | SumPositiveNumbersLoop      | 1000000 | 308,657.767 ns | 1,889.5210 ns |  1,675.0111 ns | 308,181.445 ns |         - |