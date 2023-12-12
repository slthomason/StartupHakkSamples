namespace ArticleDevOpsEfCore.Database.DataSeeds;

public interface IDataSeed
{
    ValueTask SeedAsync(
        string seededBy,
        DateTimeOffset seededOn,
        CancellationToken ct
    );
}