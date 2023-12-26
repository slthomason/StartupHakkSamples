public class ApiKeyAuthentication
{
    private const string ApiKeySectionName = "Authentication:ApiKey";
    private const string ApiKeyHeaderName = "X-Api-Key";
    
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ApiKeyAuthentication(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var receivedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key is required.");
            return;
        }

        var apiKey = _configuration.GetValue<string>(ApiKeySectionName);
        if (!apiKey.Equals(receivedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid API Key.");
            return;
        }

        await _next(context);
    }
}