using System.Data;
using Microsoft.Data.Sqlite;

namespace HealthCheck.Database;

public interface IDbConnectionService
{
    Task<IDbConnection> GetConnectionAsync();
}

public class DbConnectionService : IDbConnectionService
{
    private readonly string _connectionString;

    public DbConnectionService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IDbConnection> GetConnectionAsync()
    {
        var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}