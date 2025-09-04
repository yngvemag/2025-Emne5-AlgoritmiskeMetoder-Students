
using BenchmarkDotNet.Running;
using Recursion;

// Console.WriteLine($"Factorial of 4 is: {RecursiveMethods.Factorial(4)}");

BenchmarkRunner.Run<SumBenchmark>();

