using System.Runtime.CompilerServices;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// See the notes on this middleware below.
app.UseMiddleware<OperationCanceledMiddleware>();

app.UseCors(policy => policy
  .AllowAnyHeader()
  .AllowAnyMethod()
  .WithOrigins(
    "http://localhost:5122",
    "http://localhost:5500"
  ));

var SSE_CONTENT_TYPE = "text/event-stream";
var LINE_END = $"{Environment.NewLine}{Environment.NewLine}";

/// <summary>
/// Simple get route to return the stream.  POST works the same way, but you'll
/// need to use something like this on the front-end: https://github.com/Azure/fetch-event-source
/// </summary>
app.MapGet("/sse", async (
  IHttpContextAccessor accessor,
  CancellationToken cancellation
) => {
    var response = accessor.HttpContext!.Response;

    response.Headers.Add(HeaderNames.ContentType, SSE_CONTENT_TYPE);

    while (!cancellation.IsCancellationRequested)
    {
        await foreach (var segment in GenerateTextStreamAsync(cancellation))
        {
            await response.WriteAsync($"data: {segment}{LINE_END}", cancellation);
            await response.Body.FlushAsync(cancellation);
        }

        await response.WriteAsync($"data: [END]{LINE_END}", cancellation);
        await response.Body.FlushAsync(cancellation);
    }
});

app.Run();

/// <summary>
/// Helper function to simulate receiving a stream of data with a random delay of 5-50ms.
/// </summary>
async IAsyncEnumerable<string> GenerateTextStreamAsync(
  [EnumeratorCancellation] CancellationToken cancellation
)
{
    var loremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus a finibus dolor. Cras felis quam, interdum et turpis vitae, tempor sagittis justo. Morbi non commodo quam. Pellentesque ut elit varius sem mollis sollicitudin. Vestibulum dictum mauris non malesuada mollis. Mauris lacinia ante at bibendum hendrerit. Aliquam quis augue ligula. Nullam arcu tellus, ultrices vitae libero ac, mollis cursus nisi. Integer interdum lacinia lectus et scelerisque. Mauris sollicitudin pulvinar lacus, vitae semper magna dictum nec. Sed nunc orci, pretium nec nisi id, facilisis euismod lorem. Sed tempor, ante at varius tincidunt, nulla metus malesuada justo, sit amet eleifend erat magna a turpis. In hac habitasse platea dictumst. Sed sapien velit, accumsan non sodales et, sagittis eget tortor. ";

    var segments = loremIpsum.Split(' ');

    foreach (var segment in segments)
    {
        if (cancellation.IsCancellationRequested)
        {
            break;
        }

        await Task.Delay(TimeSpan.FromMilliseconds(Random.Shared.Next(5, 150)));

        yield return segment;
    }
}

/// <summary>
/// Without this middleware, you'll see a lot of exceptions in the execution from
/// clients disconnecting.
/// </summary>
public class OperationCanceledMiddleware
{
    private readonly RequestDelegate _next;

    public OperationCanceledMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Client closed connection.");
        }
    }
}