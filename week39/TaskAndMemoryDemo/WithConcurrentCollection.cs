using System.Collections.Concurrent;

namespace TaskAndMemoryDemo;

public class WithConcurrentCollection
{
    public static async Task Run()
    {
        var queue = new ConcurrentQueue<int>();
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));

        var producer = Task.Run(() =>
        {
            while (!cts.IsCancellationRequested)
            {
                Task.Delay(10).Wait(); // litt arbeid
                queue.Enqueue(Random.Shared.Next(10_000));
                Console.WriteLine($"Enqueue ->, count={queue.Count}");
            }
        }, cts.Token);

        var consumer = Task.Run(() =>
        {
            while (!cts.IsCancellationRequested)
            {
                Task.Delay(10).Wait(); // litt arbeid
                if (queue.TryDequeue(out var item))
                {
                    // prosesser item
                    _ = item + 1;
                    Console.WriteLine($"Dequeue -> , count={queue.Count}");
                }
                else
                {
                    // ingen elementer akkurat nå
                    Thread.SpinWait(50);
                }
            }
        }, cts.Token);

        await Task.WhenAll(producer, consumer);
        Console.WriteLine("Ferdig med ConcurrentQueue – ingen exceptions.");
    }
}