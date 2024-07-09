using Microsoft.Extensions.Caching.Hybrid;

public class CacheService
{
    private readonly HybridCache _hybridCache;

    public CacheService(HybridCache hybridCache)
    {
        _hybridCache = hybridCache;
    }

    public async Task<string> GetDataAsync(string key)
    {
        return await _hybridCache.GetOrCreateAsync(key, async entry =>
        {
            // Simulate data fetching and set cache entry options if needed
            await Task.Delay(500); // Simulate async operation
            return "Cached Data for key: " + key;
        });
    }
}
