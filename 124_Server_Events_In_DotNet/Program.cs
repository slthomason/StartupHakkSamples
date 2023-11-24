/*
Headers

HTTP/1.1 200 OK
Content-Type: text/event-stream
*/

/*
Body

event: itemCreated
data: {"name": "bucket", "Price": "114.50"}

data: Here's an example of a multi-line message 
data: returned piece by piece by a 
data: chatbot application. 
*/

//Implementing Server-Sent Events in .NET


app.MapGet("/", async (HttpContext ctx, ItemService service, CancellationToken ct) =>
{
    ctx.Response.Headers.Add("Content-Type", "text/event-stream");
    
    while (!ct.IsCancellationRequested)
    {
        var item = await service.WaitForNewItem();
        
        await ctx.Response.WriteAsync($"data: ");
        await JsonSerializer.SerializeAsync(ctx.Response.Body, item);
        await ctx.Response.WriteAsync($"\n\n");
        await ctx.Response.Body.FlushAsync();
            
        service.Reset();
    }
});


public record Item(string Name, double Price);

public class ItemService
{
    private TaskCompletionSource<Item?> _tcs = new();
    private long _id = 0;
    
    public void Reset()
    {
        _tcs = new TaskCompletionSource<Item?>();
    }

    public void NotifyNewItemAvailable()
    {
        _tcs.TrySetResult(new Item($"New Item {_id}", Random.Shared.Next(0, 500)));
    }

    public Task<Item?> WaitForNewItem()
    {
        // Simulate some delay in Item arrival
        Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(Random.Shared.Next(0, 29)));
            NotifyNewItemAvailable();
        });
        
        return _tcs.Task;
    }
}

//Implementing an SSE Client

const eventSource = new EventSource('http://localhost:5006/sse');

eventSource.onmessage = function (event) {
    const item = JSON.parse(event.data);
    console.log("New item received:", item);
};