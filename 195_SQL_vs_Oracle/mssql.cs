using System;
using System.Data.SqlClient;

public class DatabaseManager : IDisposable
{
    private readonly string _dbConnectionString;
    private SqlConnection _connection;

    public DatabaseManager(string dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
        _connection = new SqlConnection(_dbConnectionString);
    }

    public void OpenConnection()
    {
        if (_connection.State != System.Data.ConnectionState.Open)
        {
            _connection.Open();
        }
    }

    public void CloseConnection()
    {
        if (_connection.State != System.Data.ConnectionState.Closed)
        {
            _connection.Close();
        }
    }

    public void ExecuteNonQuery(string query)
    {
        using (SqlCommand command = new SqlCommand(query, _connection))
        {
            command.ExecuteNonQuery();
        }
    }

    public void Dispose()
    {
        if (_connection != null)
        {
            _connection.Dispose();
            _connection = null;
        }
    }
}
public class Program
{
    public void Main(string args[])
    {
        string dbConnectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";

        using (DatabaseManager dbManager = new DatabaseManager(dbConnectionString))
        {
            dbManager.OpenConnection();

            // Sample Table Creation
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS SampleTable (
                    ID INT PRIMARY KEY,
                    Name NVARCHAR(50),
                    Age INT
                )";

            dbManager.ExecuteNonQuery(createTableQuery);

            // Perform other database operations

            dbManager.CloseConnection();
        }
    }
}