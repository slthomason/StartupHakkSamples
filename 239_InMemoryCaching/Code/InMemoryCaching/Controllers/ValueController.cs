using InMemoryCaching.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InMemoryCaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        private readonly ICatRepo _repo;
        public ValueController(ICatRepo _repo)
        {
            this._repo = _repo;
        }

        [HttpGet]
        public async Task<IActionResult> getCategories()
        {
            return Ok(await _repo.getCategories().ConfigureAwait(false));
        }
    }
}
