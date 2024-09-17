using System.Data;
using Microsoft.Data.SqlClient;

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
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}