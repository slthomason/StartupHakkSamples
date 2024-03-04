//Use Async/Await for I/O Operations

public async Task<string> FetchDataAsync()
{
    using (HttpClient client = new HttpClient())
    {
        HttpResponseMessage response = await client.GetAsync("https://api.example.com/data");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}

//Minimize Object Allocations

StringBuilder stringBuilder = new StringBuilder();
for (int i = 0; i < 1000; i++)
{
    stringBuilder.Clear();
    stringBuilder.Append("Iteration: ").Append(i);
    Console.WriteLine(stringBuilder.ToString());
}

//Optimize LINQ Queries

var filteredUsers = dbContext.Users
    .Where(u => u.IsActive && u.Age > 18)
    .OrderBy(u => u.LastName)
    .Select(u => new { u.FirstName, u.LastName })
    .ToList();


//Implement Caching

public IActionResult GetData()
{
    var cachedData = _memoryCache.Get("CachedData");
    if (cachedData != null)
    {
        return Ok(cachedData);
    }

    var data = _dataService.GetData();
    _memoryCache.Set("CachedData", data, TimeSpan.FromMinutes(10));
    return Ok(data);
}

//Enable JIT Compilation Optimizations

/*
<PropertyGroup>
  <TieredCompilation>true</TieredCompilation>
</PropertyGroup>
*/

//Profile and Identify Performance Bottlenecks

//dotMemory.exe collect /Local

//Optimize Database Access

var activeUsers = dbContext.Users.Where(u => u.IsActive).ToList();

//Reduce Database Roundtrips

var orders = dbContext.Orders.Include(o => o.OrderItems).ToList();

//Use Structs Instead of Classes for Small Objects:

public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
}

//Optimize String Concatenation

var stringBuilder = new StringBuilder();
for (int i = 0; i < 1000; i++)
{
    stringBuilder.Append("Iteration: ").Append(i).AppendLine();
}
var result = stringBuilder.ToString();
