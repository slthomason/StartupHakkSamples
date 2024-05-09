using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

public class DatabaseManager : IDisposable
{
    private OracleConnection _connection;
    private string _connectionString;

    public DatabaseManager(string connectionString)
    {
        _connectionString = connectionString;
        _connection = new OracleConnection(_connectionString);
    }

    public void OpenConnection()
    {
        if (_connection.State != ConnectionState.Open)
            _connection.Open();
    }

    public void CloseConnection()
    {
        if (_connection.State != ConnectionState.Closed)
            _connection.Close();
    }

    public void CreateTable(string createTableQuery)
    {
        OpenConnection();
        using (OracleCommand createTableCommand = new OracleCommand(createTableQuery, _connection))
        {
            createTableCommand.ExecuteNonQuery();
        }
        CloseConnection();
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}

public class Program
{
    public static void Main()
    {
        string dbConnectionString = "User Id=myUsername;Password=myPassword;Data Source=myDataSource;";

        using (DatabaseManager dbManager = new DatabaseManager(dbConnectionString))
        {
            // Sample Table Creation
            string createTableQuery = @"
                CREATE TABLE SampleTable (
                    ID NUMBER PRIMARY KEY,
                    Name VARCHAR2(50),
                    Age NUMBER
                )";

            dbManager.CreateTable(createTableQuery);

            // Perform other database operations
        }
    }
}
