using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace WebApplication19
{
    [Route("api/[controller]")]
    [ApiController]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByHeader = "Authorization")]
    public class CountryController : ControllerBase
    {
        Country country;
        IDistributedCache _distributedCache;
        public CountryController(IDistributedCache distributedCache)
        {
            country = new Country();
            _distributedCache = distributedCache;
        }
        [HttpGet("countries")]
        public async Task<IActionResult> Countries()
        {
            string cachedCountry = await _distributedCache.GetStringAsync("country");
            if (!string.IsNullOrEmpty(cachedCountry))
            {
                return Ok(new { cachedData = cachedCountry });
            }
            //add to redis cache
            var countries = country.getData();
            var serialize = JsonSerializer.Serialize(countries);
            await _distributedCache.SetStringAsync("country", serialize);
            return Ok(countries);
        }
        [HttpGet("cached/remove")]
        public async Task<IActionResult> ClearCachedCountry()
        {
            await _distributedCache.RemoveAsync("country");
            return Ok("Removed");
        }
    }
}
