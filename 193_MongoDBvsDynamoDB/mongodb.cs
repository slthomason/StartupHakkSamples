using MongoDB.Driver;
using MongoDB.Bson;

public class MongoDBManager
{
    private readonly IMongoDatabase _database;
    private readonly string _collectionName;

    public MongoDBManager(string connectionString, string databaseName, string collectionName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
        _collectionName = collectionName;
    }

    public void InsertDocument(BsonDocument document)
    {
        var collection = _database.GetCollection<BsonDocument>(_collectionName);
        collection.InsertOne(document);
    }

    // Add other database operations here as needed
}
public class Program
{
    public void Main(string[] args)
    {
        // Create an instance of MongoDBManager
        var mongoDBManager = new MongoDBManager("mongodb://localhost:27017", "database_name", "collection_name");

        // Sample Document Insertion
        var document = new BsonDocument
        {
            { "Name", "John" },
            { "Age", 30 }
        };
        mongoDBManager.InsertDocument(document);

        // Perform other database operations using methods of MongoDBManager class
        
    }
}
