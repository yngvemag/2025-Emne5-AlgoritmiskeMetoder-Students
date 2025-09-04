```

BenchmarkDotNet v0.15.2, Windows 11 (10.0.26100.4946/24H2/2024Update/HudsonValley)
Intel Core Ultra 7 155H 1.40GHz, 1 CPU, 22 logical and 16 physical cores
.NET SDK 9.0.302
  [Host]     : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2
  DefaultJob : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2


```
| Method                        | N    | Mean          | Error         | StdDev        | Median        | Allocated |
|------------------------------ |----- |--------------:|--------------:|--------------:|--------------:|----------:|
| **SumPositiveNumbersInLoop**      | **10**   |      **3.357 ns** |     **0.0117 ns** |     **0.0104 ns** |      **3.357 ns** |         **-** |
| SumPositiveNumbersInRecursive | 10   |      6.840 ns |     0.0181 ns |     0.0151 ns |      6.845 ns |         - |
| **SumPositiveNumbersInLoop**      | **100**  |     **41.709 ns** |     **0.2675 ns** |     **0.2503 ns** |     **41.792 ns** |         **-** |
| SumPositiveNumbersInRecursive | 100  |    209.654 ns |    22.1999 ns |    65.4570 ns |    235.982 ns |         - |
| **SumPositiveNumbersInLoop**      | **1000** |    **349.910 ns** |     **6.8276 ns** |    **14.5501 ns** |    **350.511 ns** |         **-** |
| SumPositiveNumbersInRecursive | 1000 |  2,992.708 ns |   334.5838 ns |   986.5281 ns |  3,324.095 ns |         - |
| **SumPositiveNumbersInLoop**      | **5000** |  **1,589.884 ns** |    **12.4825 ns** |    **10.4235 ns** |  **1,586.600 ns** |         **-** |
| SumPositiveNumbersInRecursive | 5000 | 17,772.222 ns | 2,527.9930 ns | 7,374.2747 ns | 21,553.267 ns |         - |
