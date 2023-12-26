var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseMiddleware<ApiKeyAuthentication>();
app.MapGet("/api", () => "Secured API Endpoint!");

app.Run();