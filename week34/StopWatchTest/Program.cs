using System.Diagnostics;

int loopCount = 10_000_000;
int count = 100000; 

Console.WriteLine($"Sum of numbers from 1 to {count}: {SumNumbers(count)}");

Stopwatch stopWatch = new Stopwatch();
stopWatch.Start();
for (int i = 0; i < loopCount; i++)
{
    SumNumbers(count);
}
stopWatch.Stop();
Console.WriteLine($"Total time for {loopCount} iterations: {stopWatch.ElapsedMilliseconds} ms");


int SumNumbers(int n)
{
    int sum = 0;
    for (int i = 1; i <= n; i++)
    {
        sum += i;
    }
    return sum;
}
