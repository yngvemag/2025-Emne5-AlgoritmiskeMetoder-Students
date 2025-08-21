```

BenchmarkDotNet v0.15.2, Windows 11 (10.0.26100.4946/24H2/2024Update/HudsonValley)
Intel Core Ultra 7 155H 1.40GHz, 1 CPU, 22 logical and 16 physical cores
.NET SDK 9.0.302
  [Host]     : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2
  DefaultJob : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2


```
| Method               | Mean         | Error      | StdDev     | Ratio | RatioSD | Rank | Gen0      | Gen1     | Allocated   | Alloc Ratio |
|--------------------- |-------------:|-----------:|-----------:|------:|--------:|-----:|----------:|---------:|------------:|------------:|
| PlusOperator         | 3,121.434 μs | 42.0613 μs | 39.3442 μs | 1.000 |    0.02 |    2 | 7976.5625 | 390.6250 | 97910.13 KB |       1.000 |
| StringBuilder_Append |     6.429 μs |  0.0840 μs |  0.0786 μs | 0.002 |    0.00 |    1 |    4.2343 |   0.5264 |    51.95 KB |       0.001 |
