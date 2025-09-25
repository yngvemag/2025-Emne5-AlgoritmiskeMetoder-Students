namespace TaskAndMemoryDemo;

public static class WithoutSynchronize
{
    public static async Task Run()
    {
        List<int> numbers = new(capacity: 10_000);
        using var cts = new CancellationTokenSource();
        
        var writer = Task.Run(() =>
        {
            while (!cts.IsCancellationRequested)
            {
                Task.Delay(10).Wait();
                if (Random.Shared.Next(2) == 0)
                {
                    numbers.Add(Random.Shared.Next(10_000));
                    Console.WriteLine($"Wrote, count={numbers.Count}");
                }
                else
                {
                    numbers.RemoveAt(numbers.Count - 1);
                    Console.WriteLine($"Wrote, count={numbers.Count}");
                }

            }
            // for (int i = 0; i < 10; i++)
            // {
            //     Task.Delay(2).Wait();
            //     Console.WriteLine(i);
            // }
        }, cts.Token);

        var reader = Task.Run(() =>
        {
            try
            {
                while (!cts.IsCancellationRequested)
                {
                    Task.Delay(10).Wait();
                    foreach (var number in numbers)
                    {
                        _ = number + 1; // simulere noe arbeid
                        Console.WriteLine($"Read {number}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in reader: {ex.GetType().Namespace} {ex.Message}");
                cts.Cancel();
            }
            
            // for (int i = 100; i < 110; i++)
            // {
            //     Task.Delay(1).Wait();
            //     Console.WriteLine(i);
            // }
        }, cts.Token);

        await Task.WhenAll(reader, writer);
        Console.WriteLine("Ferdig");
    }

}