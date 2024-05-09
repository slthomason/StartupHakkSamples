using System.Data.SQLite;

public class SampleDatabase
{
    private readonly string _connectionString;
    
    public SampleDatabase(string databaseFilePath)
    {
        _connectionString = $"Data Source={databaseFilePath};Version=3;";
    }

    public void CreateSampleTable()
    {
        using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS SampleTable (
                    ID INTEGER PRIMARY KEY,
                    Name TEXT,
                    Age INTEGER
                )";
            using (SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, connection))
            {
                createTableCommand.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    // You can add more methods for other database operations like inserting, updating, deleting, querying data, etc.
}
public class Program
{
  public void Main(string[] args)
  {
    SampleDatabase sampleDB = new SampleDatabase("myDatabase.db");
    sampleDB.CreateSampleTable();
  }
}