using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTipsApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Async Tips Demo");

            // 1. Proper use of async and await
            var data = await FetchDataAsync();
            Console.WriteLine(data);

            // 2. Avoid async void
            await ProperAsyncMethod();

            // 3. Use ConfigureAwait(false)
            var configAwaitResult = await ConfigureAwaitExample();

            // 4. Use Task.WhenAll and Task.WhenAny
            await RunMultipleTasksAsync();

            // 5. Use SemaphoreSlim for throttling
            await ThrottledTasksAsync();

            // 6. Avoid blocking calls
            var nonBlockingResult = await NonBlockingCallAsync();

            // 7. Use IAsyncEnumerable<T> for asynchronous streams
            await foreach (var item in GetNumbersAsync())
            {
                Console.WriteLine(item);
            }
        }

        static async Task<string> FetchDataAsync()
        {
            using var client = new HttpClient();
            return await client.GetStringAsync("https://jsonplaceholder.typicode.com/todos/1");
        }

        static async Task ProperAsyncMethod()
        {
            await Task.Delay(1000); // Simulating async work
            Console.WriteLine("Async work done!");
        }

        static async Task<string> ConfigureAwaitExample()
        {
            await Task.Delay(1000).ConfigureAwait(false);
            return "Task completed";
        }

        static async Task RunMultipleTasksAsync()
        {
            var tasks = new List<Task>
            {
                Task.Delay(1000),
                Task.Delay(2000),
                Task.Delay(3000)
            };
            await Task.WhenAll(tasks);
            Console.WriteLine("All tasks completed");
        }

        static async Task ThrottledTasksAsync()
        {
            var semaphore = new SemaphoreSlim(2);
            var tasks = Enumerable.Range(0, 5).Select(async i =>
            {
                await semaphore.WaitAsync();
                try
                {
                    await Task.Delay(1000);
                    Console.WriteLine($"Task {i} completed");
                }
                finally
                {
                    semaphore.Release();
                }
            });
            await Task.WhenAll(tasks);
        }

        static async Task<string> NonBlockingCallAsync()
        {
            await Task.Delay(1000);
            return "Non-blocking call completed";
        }

        static async IAsyncEnumerable<int> GetNumbersAsync()
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(500);
                yield return i;
            }
        }
    }
}
