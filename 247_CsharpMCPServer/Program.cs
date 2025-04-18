using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using MCPServer.CSharp;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole(consoleLogOptions =>
{
    // Configure all logs to go to stderr
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

// Configure LlmService with settings from appsettings.json
builder.Services.Configure<LlmServiceConfig>(
    builder.Configuration.GetSection("LlmServiceConfig"));

// Register all services
builder.Services.AddHttpClient();
builder.Services.AddSingleton<MonkeyService>();
builder.Services.AddSingleton<LlmService>();

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

await builder.Build().RunAsync();