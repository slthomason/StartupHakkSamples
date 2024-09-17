using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCheck.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValueController: ControllerBase
    {
        [HttpGet("Test")]
        public async Task<IActionResult> test()
        {
            var list = new List<string>(){"US","CA"};
            return Ok(list);
        }
    }
}
