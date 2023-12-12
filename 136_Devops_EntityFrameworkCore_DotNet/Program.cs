using ArticleDevOpsEfCore.Database;
using ArticleDevOpsEfCore.Database.DataSeeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
// ReSharper disable once AccessToDisposedClosure

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, eventArgs) =>
{
    cts.Cancel();
    eventArgs.Cancel = true;
};

ILogger<Program> logger = null;
try
{
    var builder = Host.CreateApplicationBuilder(args);

    ConfigureLogging(
        builder.Logging
    );

    ConfigureServices(
        builder.Services,
        builder.Configuration
    );

    using var host = builder.Build();

    logger = host.Services.GetRequiredService<ILogger<Program>>();

    var migrator = host.Services.GetRequiredService<Migrator>();

    logger.LogDebug("Running migrator");
    await migrator.RunAsync(cts.Token);
    logger.LogInformation("Migrator run successfully");
}
catch (Exception e)
{
    if (logger is null)
    {
        await Console.Error.WriteLineAsync("Application failed with a fatal error");
        await Console.Error.WriteLineAsync(e.ToString());
    }
    else
        logger.LogCritical(e, "Application failed with a fatal error");

    throw;
}

return;

static void ConfigureLogging(
    ILoggingBuilder logging
)
{
    // configure other logging providers, like NLog, Serilog or even Application Insights
}

static void ConfigureServices(
    IServiceCollection services,
    IConfiguration configuration
)
{
    var connectionString = configuration.GetConnectionString("ArticleDevOpsEfCore");
    services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(
        connectionString,
        o => o.MigrationsAssembly(typeof(Program).Assembly.FullName)
    ), ServiceLifetime.Singleton);

    services.AddSingleton<Migrator>();

    services.AddSingleton<IDataSeed, TestProductsDataSeed>();
}