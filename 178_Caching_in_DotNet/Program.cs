// Example caching data in MemoryCache
var cacheKey = "myCachedData";
var cachedData = MemoryCache.Default.Get(cacheKey) as string;

if (cachedData == null)
{
    // Data not in cache, fetch and cache it
    cachedData = FetchDataFromSource();
    MemoryCache.Default.Add(cacheKey, cachedData, DateTimeOffset.Now.AddMinutes(30));
}


// Example of distributed caching with Redis using StackExchange.Redis
var cacheKey = "myCachedData";
var cachedData = await distributedCache.GetStringAsync(cacheKey);

if (cachedData == null)
{
    // Data not in cache, fetch and cache it
    cachedData = FetchDataFromSource();
    await distributedCache.SetStringAsync(cacheKey, cachedData, new DistributedCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
    });
}

// Example of output caching in an MVC controller
[OutputCache(Duration = 3600, VaryByParam = "none")]
public ActionResult Index()
{
    // Code for generating view...
}

// Example of cache expiration in MemoryCache
MemoryCache.Default.Add(cacheKey, cachedData, DateTimeOffset.Now.AddMinutes(30));