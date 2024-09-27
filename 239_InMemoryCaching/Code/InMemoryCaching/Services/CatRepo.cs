using System;
using InMemoryCaching.Database;
using InMemoryCaching.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCaching.Services;

public class CatRepo : ICatRepo
{
    private readonly DatabaseContext _db;
    private readonly IMemoryCache _cache;

    public CatRepo(DatabaseContext _db,IMemoryCache _cache)
    {
        this._db = _db;
        this._cache = _cache;
    }

    public async Task<List<Categories>> getCategories()
    {
        List<Categories> categories = new List<Categories>();
        var cacheKey = $"Category-List";
        if (_cache.TryGetValue(cacheKey, out List<Categories> cachedData))
        {
            return cachedData;
        }

        // Simulate processing
        categories = await _db.categories
                .ToListAsync()
                .ConfigureAwait(false);

        _cache.Set(cacheKey, categories, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });

        return categories;
    }
}
