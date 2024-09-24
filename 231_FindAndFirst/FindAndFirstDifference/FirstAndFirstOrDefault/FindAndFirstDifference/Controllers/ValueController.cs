using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FindAndFirstDifference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        [HttpGet]
        public IActionResult fetchUsers()
        {
            var users = new List<object>();
            return Ok(users);
        }
    }
}
