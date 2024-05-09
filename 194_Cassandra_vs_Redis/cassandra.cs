using Cassandra;

public class CassandraConnector
{
    private Cluster _cluster;
    private ISession _session;

    public CassandraConnector(string contactPoint)
    {
        Connect(contactPoint);
    }

    private void Connect(string contactPoint)
    {
        _cluster = Cluster.Builder().AddContactPoint(contactPoint).Build();
        _session = _cluster.Connect();
    }

    public void CreateKeyspace(string keyspaceName)
    {
        string createKeyspaceQuery = $"CREATE KEYSPACE IF NOT EXISTS {keyspaceName} WITH REPLICATION = {{ 'class' : 'SimpleStrategy', 'replication_factor' : 1 }}";
        _session.Execute(createKeyspaceQuery);
    }

    public void CreateSampleTable(string keyspaceName)
    {
        string createTableQuery = $"CREATE TABLE IF NOT EXISTS {keyspaceName}.SampleTable (ID UUID PRIMARY KEY, Name TEXT, Age INT)";
        _session.Execute(createTableQuery);
    }

    public void InsertData(string keyspaceName, Guid id, string name, int age)
    {
        string insertQuery = $"INSERT INTO {keyspaceName}.SampleTable (ID, Name, Age) VALUES (?, ?, ?)";
        _session.Execute(insertQuery, id, name, age);
    }

    public void Shutdown()
    {
        _cluster.Shutdown();
    }
}
class Program
{
    static void Main(string[] args)
    {
        var connector = new CassandraConnector("127.0.0.1");

        // Create keyspace if not exists
        connector.CreateKeyspace("keyspace_name");

        // Create sample table if not exists
        connector.CreateSampleTable("keyspace_name");

        // Insert sample data
        connector.InsertData("keyspace_name", Guid.NewGuid(), "John Doe", 30);

        // Shutdown the cluster
        connector.Shutdown();
    }
}
