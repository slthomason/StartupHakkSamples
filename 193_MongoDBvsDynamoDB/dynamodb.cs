using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Threading.Tasks;

public class DynamoDBManager
{
    private AmazonDynamoDBClient _client;
    private Table _table;

    public DynamoDBManager(string tableName, AmazonDynamoDBClient client)
    {
        _client = client;
        _table = Table.LoadTable(_client, tableName);
    }

    public async Task InsertItemAsync(int id, string name, int age)
    {
        var document = new Document();
        document["ID"] = id;
        document["Name"] = name;
        document["Age"] = age;
        await _table.PutItemAsync(document);
    }

    // You can add more methods for other database operations here
}
public class Program
{
    public void Main(string[] args)
    {
        var client = new AmazonDynamoDBClient();
        var dynamoDBManager = new DynamoDBManager("table_name", client);

        await dynamoDBManager.InsertItemAsync(1, "John", 30);
        
    }
}