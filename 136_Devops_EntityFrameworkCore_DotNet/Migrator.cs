using ArticleDevOpsEfCore.Database.DataSeeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
// ReSharper disable InconsistentNaming
// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace ArticleDevOpsEfCore.Database;

public class Migrator(
    ILogger<Migrator> _logger,
    DatabaseContext _context,
    IEnumerable<IDataSeed> _dataSeeds
)
{
    public async ValueTask RunAsync(CancellationToken ct)
    {
        _logger.LogDebug("Migrating database to the latest version");
        await _context.Database.MigrateAsync(ct);
        _logger.LogInformation("Database migrated to latest version");

        var seededOn = DateTimeOffset.UtcNow;
        await using var tx = await _context.Database.BeginTransactionAsync(ct);

        foreach (var dataSeed in _dataSeeds)
        {
            using var _ = _logger.BeginScope("DataSeed:{DataSeed}", dataSeed.GetType().Name);

            _logger.LogDebug("Seeding data");

            await dataSeed.SeedAsync(
                "Migrator",
                seededOn,
                ct
            );
            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Data was seeded");
        }

        await tx.CommitAsync(ct);
    }
}