//Implementing Caching in .NET

using System.Runtime.Caching;

MemoryCache cache = MemoryCache.Default;
string cacheKey = "myCachedData";

if (cache.Contains(cacheKey))
{
    var cachedData = (string)cache.Get(cacheKey);
    DoSomethingWithCachedData(cachedData);
}
else
{
    var data = GetDataFromDatabase();
    cache.Add(cacheKey, data, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(10) });
    DoSomethingWithCachedData(data);
}

//Batch Processing

using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
    {
        bulkCopy.DestinationTableName = "MyTable";
        bulkCopy.WriteToServer(myDataTable);
    }
}

//Minimize network roundtrips
// Pagination
var page = 1;
var pageSize = 10;
var data = GetLargeDataSet();
var pageData = data.Skip((page - 1) * pageSize).Take(pageSize);

// Lazy Loading
var orders = db.Customers.SelectMany(c => c.Orders);

// Data Filtering
var filteredData = data.Where(d => d.Category == "Books");

//Asynchronous programming

//Use Async / Await
public async Task<int> GetResultAsync()
{
    var result = await SomeAsyncOperation();
    return result;
}

//Use Task Parallel Library (TPL)
var task1 = Task.Run(() => DoSomeWork());
var task2 = Task.Run(() => DoSomeOtherWork());
await Task.WhenAll(task1, task2);

List<int> numbers = new List<int>();
for (int i = 0; i < 100; i++)
{
    numbers.Add(i);
}

// Use Parallel.ForEach to perform an operation on each number in the list
Parallel.ForEach(numbers, (number) =>
{
    // Simulate a computationally expensive operation
    int result = 0;
    for (int i = 0; i < 100000; i++)
    {
        result += number;
    }

    Console.WriteLine($"Result for {number}: {result}");
});

//Use Async Streams

public async IAsyncEnumerable<Data> GetDataAsync()
{
    using (var connection = new SqlConnection(connectionString))
    {
        await connection.OpenAsync();
        using (var command = new SqlCommand("SELECT * FROM MyTable", connection))
        using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                yield return new Data
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                };
            }
        }
    }
}

//Avoid Boxing and Unboxing
int i = 42;
object o = i; // Boxing
int j = (int)o; // Unboxing

//Use StringBuilder for String Concatenation
StringBuilder sb = new StringBuilder();
sb.Append("Hello");
sb.Append(" ");
sb.Append("World");
string result = sb.ToString();

//Use Lazy Initialization
private Lazy<List<int>> _lazyNumbers = new Lazy<List<int>>(() => new List<int>() { 1, 2, 3, 4, 5 });
public List<int> Numbers
{
    get { return _lazyNumbers.Value; }
}

//Use Object Pooling
public class ObjectPool<T> where T : new()
{
    private readonly ConcurrentBag<T> _objects = new ConcurrentBag<T>();
    public T Get()
    {
        if (_objects.TryTake(out T item))
        {
            return item;
        }
        return new T();
    }
    public void Return(T item)
    {
        _objects.Add(item);
    }
}
