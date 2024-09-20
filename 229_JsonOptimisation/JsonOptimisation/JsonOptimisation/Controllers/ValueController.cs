using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JsonOptimisation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan cacheExpiration = TimeSpan.FromMinutes(5);

        public ValueController(IMemoryCache cache)
        {
            _cache = cache;
        }
        [HttpGet("data")]
        public IActionResult inMemoryCache()
        {
            string cacheKey = "cachedData";
            if (!_cache.TryGetValue(cacheKey, out string cachedData))
            {
                // Data is not cached, so fetch it from the source (e.g., a database)
                cachedData = "This is the data to be cached.";

                // Set cache options (cache for 5 minutes)
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = cacheExpiration
                };

                // Cache the data
                _cache.Set(cacheKey, cachedData, cacheEntryOptions);
            }

            return Ok(cachedData);
        }
    }
}
