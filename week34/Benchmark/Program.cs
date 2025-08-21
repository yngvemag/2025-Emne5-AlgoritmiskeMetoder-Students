
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;


// Top-level statement for BenchmarkDotNet

BenchmarkRunner.Run<LoopBenchmark>();

//BenchmarkRunner.Run<ArrayListVsListBenchmark>();
//BenBenchmarkRunner.Run<StringBenchmark>();