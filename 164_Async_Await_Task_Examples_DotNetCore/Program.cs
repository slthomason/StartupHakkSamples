//Example 1️ Simple Async/Await Task

using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        await PrintMessageAsync();
    }

    static async Task PrintMessageAsync()
    {
        Console.WriteLine("Start of async method.");
        await Task.Delay(2000); // Simulating an asynchronous delay
        Console.WriteLine("End of async method.");
    }
}

//Example 2️ Async/Await with Task.Run

using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        await PerformTaskAsync();
    }

    static async Task PerformTaskAsync()
    {
        Console.WriteLine("Start of async method.");

        // Using Task.Run to run a synchronous method asynchronously
        await Task.Run(() => PerformSynchronousTask());

        Console.WriteLine("End of async method.");
    }

    static void PerformSynchronousTask()
    {
        Console.WriteLine("Executing synchronous task.");
    }
}

//Example 3️ Async/Await with Exception Handling

using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        try
        {
            await PerformAsyncOperationWithErrorAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception caught: {ex.Message}");
        }
    }

    static async Task PerformAsyncOperationWithErrorAsync()
    {
        Console.WriteLine("Start of async method with exception.");

        // Simulating an asynchronous operation that throws an exception
        await Task.Run(() => throw new InvalidOperationException("Async operation failed."));

        Console.WriteLine("End of async method with exception.");
    }
}

//Example 4️ Async/Await with Multiple Tasks

using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        await Task.WhenAll(DownloadDataAsync(), ProcessDataAsync());
    }

    static async Task DownloadDataAsync()
    {
        Console.WriteLine("Downloading data asynchronously.");
        await Task.Delay(3000); // Simulating download operation
        Console.WriteLine("Download complete.");
    }

    static async Task ProcessDataAsync()
    {
        Console.WriteLine("Processing data asynchronously.");
        await Task.Delay(2000); // Simulating data processing
        Console.WriteLine("Data processing complete.");
    }
}

//Example 5️ Async/Await with CancellationToken

using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        using (var cts = new CancellationTokenSource())
        {
            var cancellationToken = cts.Token;

            // Start the asynchronous operation with cancellation support
            var task = PerformAsyncOperationWithCancellationAsync(cancellationToken);

            // Simulate user input to cancel the operation after 2000 milliseconds
            await Task.Delay(2000);
            cts.Cancel();

            try
            {
                await task;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Async operation canceled by user.");
            }
        }
    }

    static async Task PerformAsyncOperationWithCancellationAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Start of async method with cancellation support.");

        // Simulating a long-running asynchronous operation
        await Task.Delay(5000, cancellationToken);

        Console.WriteLine("End of async method with cancellation support.");
    }
}