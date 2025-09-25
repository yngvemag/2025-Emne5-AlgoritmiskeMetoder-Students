namespace TaskAndMemoryDemo;

public static class WithSynchronize
{
    public static async Task Run()
    {
        var list = new List<int>(capacity: 10_000);
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var listLock = new object();

        var writer = Task.Run(() =>
        {
            while (!cts.IsCancellationRequested)
            {
                Task.Delay(10).Wait(); // litt arbeid
                                       // Simuler tilfeldig oppførsel
                lock (listLock)
                {
                    if (Random.Shared.Next(2) == 0)
                    {
                        list.Add(Random.Shared.Next(10_000));
                        Console.WriteLine($"Wrote, count={list.Count}");
                    }
                    else if (list.Count > 0)
                    {
                        list.RemoveAt(list.Count - 1);
                        Console.WriteLine($"Removed, count={list.Count - 1}");
                    }
                }
            }
        }, cts.Token);

        var reader = Task.Run(() =>
        {
            try
            {
                while (!cts.IsCancellationRequested)
                {
                    Task.Delay(50).Wait(); // litt arbeid
                                           // Dette vil typisk kaste InvalidOperationException
                    lock (listLock)
                    {
                        foreach (var x in list)
                        {
                            // Lite arbeid
                            _ = x + 1;
                            Console.WriteLine($"Read {x}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Reader feilet: {ex.GetType().Name} - {ex.Message}");
                cts.Cancel(); // stopp writer også
            }
        }, cts.Token);

        await Task.WhenAll(writer, reader);
        Console.WriteLine("Ferdig.");
    }
    
}