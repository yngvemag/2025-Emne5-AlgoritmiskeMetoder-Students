```

BenchmarkDotNet v0.15.2, Windows 11 (10.0.26100.4946/24H2/2024Update/HudsonValley)
Intel Core Ultra 7 155H 1.40GHz, 1 CPU, 22 logical and 16 physical cores
.NET SDK 9.0.302
  [Host]     : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2
  DefaultJob : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2


```
| Method    | Mean     | Error    | StdDev   | Ratio | RatioSD | Rank | Allocated | Alloc Ratio |
|---------- |---------:|---------:|---------:|------:|--------:|-----:|----------:|------------:|
| Sum_Array | 661.2 ns | 12.80 ns | 15.24 ns |  1.00 |    0.03 |    1 |         - |          NA |
| Sum_List  | 635.3 ns |  4.75 ns |  4.44 ns |  0.96 |    0.02 |    1 |         - |          NA |
