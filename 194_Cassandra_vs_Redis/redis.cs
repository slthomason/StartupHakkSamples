using StackExchange.Redis;

public class RedisManager
{
    private readonly ConnectionMultiplexer _connection;

    public RedisManager(string connectionString)
    {
        _connection = ConnectionMultiplexer.Connect(connectionString);
    }

    public void SetHash(string key, HashEntry[] hashEntries)
    {
        var db = _connection.GetDatabase();
        db.HashSet(key, hashEntries);
    }

    // Add other methods for performing different database operations

    public void CloseConnection()
    {
        _connection.Close();
    }
}
class Program
{
    static void Main(string[] args)
    {
        string connectionString = "localhost";
        using (var redisManager = new RedisManager(connectionString))
        {
            // Sample Hash Set
            redisManager.SetHash("hash_key", new HashEntry[] { new HashEntry("Name", "John"), new HashEntry("Age", 30) });
            
            // Perform other database operations
            
            // Don't forget to close the connection when done
            redisManager.CloseConnection();
        }
    }
}
