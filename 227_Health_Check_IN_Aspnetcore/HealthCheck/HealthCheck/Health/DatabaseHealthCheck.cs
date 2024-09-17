using System;
using HealthCheck.Database;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheck;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IDbConnectionService dbConnectionService;
    public DatabaseHealthCheck(IDbConnectionService dbConnectionService)
    {
        this.dbConnectionService = dbConnectionService;
    }
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = await dbConnectionService.GetConnectionAsync().ConfigureAwait(false);
            using var command = connection.CreateCommand();
            command.CommandText = "Select 1";
            command.ExecuteScalar();
            return HealthCheckResult.Healthy();
        }
        catch(Exception ex)
        {
            return HealthCheckResult.Unhealthy();
        }
       
    }
}