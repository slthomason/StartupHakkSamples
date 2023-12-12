using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
// ReSharper disable InconsistentNaming

namespace ArticleDevOpsEfCore.Database.DataSeeds;

public class TestProductsDataSeed(
    ILogger<TestProductsDataSeed> _logger,
    IHostEnvironment _env,
    DatabaseContext _context
) : IDataSeed
{
    public async ValueTask SeedAsync(string seededBy, DateTimeOffset seededOn, CancellationToken ct)
    {
        if (_env.IsProduction())
        {
            _logger.LogInformation("Running in production, no test data will be seeded");
            return;
        }

        var productsSet = _context.Set<ProductEntity>();

        var testCodes = Enumerable.Range(0, 150).Select(i => i.ToString("D8", NumberFormatInfo.InvariantInfo)).ToArray();

        var existingCodes = await (
            from p in productsSet
            where
                testCodes.Contains(p.Code)
            select
                p.Code
        ).ToArrayAsync(ct);

        var nonExistingProducts = testCodes.Where(c => !existingCodes.Contains(c)).Select(c => new ProductEntity
        {
            Code = c,
            Name = $"Test product '{c}'"
        });

        await productsSet.AddRangeAsync(nonExistingProducts, ct);
    }
}