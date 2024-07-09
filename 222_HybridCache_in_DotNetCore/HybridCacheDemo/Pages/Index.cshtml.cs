using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
    private readonly CacheService _cacheService;

    public IndexModel(CacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public string CachedData { get; private set; }

    public async Task OnGetAsync()
    {
        CachedData = await _cacheService.GetDataAsync("sampleKey");
    }
}
