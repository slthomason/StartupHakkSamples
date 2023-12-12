using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ArticleDevOpsEfCore.Database;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .Build();

        var options = new DbContextOptionsBuilder<DatabaseContext>().UseSqlServer(
            configuration.GetConnectionString("ArticleDevOpsEfCore"),
            o => o.MigrationsAssembly(typeof(Program).Assembly.FullName)
        ).Options;

        return new DatabaseContext(options);
    }
}