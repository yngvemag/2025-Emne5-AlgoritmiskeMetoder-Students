```

BenchmarkDotNet v0.15.4, Windows 11 (10.0.26100.6584/24H2/2024Update/HudsonValley)
Intel Core Ultra 7 155H 1.40GHz, 1 CPU, 22 logical and 16 physical cores
.NET SDK 10.0.100-rc.1.25451.107
  [Host]     : .NET 9.0.9 (9.0.9, 9.0.925.41916), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 9.0.9 (9.0.9, 9.0.925.41916), X64 RyuJIT x86-64-v3


```
| Method                      | Mean     | Error    | StdDev   | Gen0   | Allocated |
|---------------------------- |---------:|---------:|---------:|-------:|----------:|
| BenchmarkStringAndSubstring | 22.61 ns | 0.461 ns | 0.566 ns | 0.0076 |      96 B |
| BenchmarkSpanAndSlice       | 14.04 ns | 0.133 ns | 0.118 ns |      - |         - |
