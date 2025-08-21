```

BenchmarkDotNet v0.15.2, Windows 11 (10.0.26100.4946/24H2/2024Update/HudsonValley)
Intel Core Ultra 7 155H 1.40GHz, 1 CPU, 22 logical and 16 physical cores
.NET SDK 9.0.302
  [Host]     : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2
  DefaultJob : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2


```
| Method      | Mean     | Error   | StdDev   | Rank | Allocated |
|------------ |---------:|--------:|---------:|-----:|----------:|
| ForLoop     | 312.1 ns | 3.06 ns |  2.86 ns |    2 |         - |
| ForeachLoop | 225.1 ns | 2.48 ns |  2.32 ns |    1 |         - |
| WhileLoop   | 323.3 ns | 6.48 ns | 12.64 ns |    2 |         - |
