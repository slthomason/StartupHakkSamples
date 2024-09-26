using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspnetCoreWithDocker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult getUsers()
        {
            var users = new List<object>()
            {
                new
                {
                    name = "Jon Doe",
                    email = "jondoe@gmail.com"
                },
                new
                {
                    name = "Jon Doe1",
                    email = "jondoe1@gmail.com"
                },

            };
            return Ok(users);
        }
    }
}
