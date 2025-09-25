
using BenchmarkDotNet.Running;
using SpanDemo;

BenchmarkRunner.Run<Benchy>();

/*
var result = DateParser.ParseDateWithStringAndSubstring();
Console.WriteLine(result);
result = DateParser.ParseDateWithSpan();
Console.WriteLine(result);*/