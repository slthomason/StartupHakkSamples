using System;

namespace Prallelism;

public static class AsyncParallelism
{
    public static async Task Main()
    {
        var urls = new[] 
        {
            "https://example.com",
            "https://example.org",
            "https://example.net"
        };

        Task[] tasks = new Task[urls.Length];

        for (int i = 0; i < urls.Length; i++)
        {
            tasks[i] = FetchDataAsync(urls[i]);
        }

        // Wait for all tasks to complete
        await Task.WhenAll(tasks);
    }

    static async Task FetchDataAsync(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            string data = await client.GetStringAsync(url);
            Console.WriteLine($"Fetched {data.Length} characters from {url}");
        }
    }
}
