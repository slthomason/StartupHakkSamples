//Middleware 1: Request Logger

public class RequestLoggerMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");

        await _next(context);
    }
}

//Middleware 2: Request Modifier

public class RequestModifierMiddleware
{
    private readonly RequestDelegate _next;

    public RequestModifierMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalQueryString = context.Request.QueryString.Value;

        context.Request.QueryString = new QueryString($"{originalQueryString}&custom-param=middleware-added");

        Console.WriteLine($"Modified Request Query: {context.Request.QueryString}");

        await _next(context);
    }
}

//Middleware 3: Response Modifier

public class ResponseModifierMiddleware
{
    private readonly RequestDelegate _next;

    public ResponseModifierMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        context.Response.Headers.Add("Custom-Header", "Middleware-Modified");
    }
}


//Middleware Registration

    builder.Services.UseMiddleware<RequestLoggerMiddleware>();

    builder.Services.UseMiddleware<RequestModifierMiddleware>();

    builder.Services.UseMiddleware<ResponseModifierMiddleware>();
