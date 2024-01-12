interface ITask
{
    void Execute();
}

class NetFramework48 : ITask
{
    ...
}

class Net8 : ITask
{
    ...
}

//Migrate library project to .NET Standard 2.0

public class HttpContextAccessor : IHttpContextAccessor
{
    public HttpContext HttpContext 
    { 
        get => HttpContext.Current;
        set => throw new NotSupportedException("This is only for migration purposes");
    }
}

public class MyService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public MyService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public void DoSomething()
    {
        var context = _httpContextAccessor.HttpContext;
        // do something with the context
    }
}

//ASP.NET Web API — Migrate to ASP.NET Core 8

public class MyMiddleware
{
    private readonly RequestDelegate _next;
    public MyMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        // Begin_Request
        await _next(context);
        // End_Request
    }
}