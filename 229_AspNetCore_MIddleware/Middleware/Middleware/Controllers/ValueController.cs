using Microsoft.AspNetCore.Mvc;

namespace Middleware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        [HttpGet]
        public IActionResult fetchUsers()
        {
            throw new InvalidOperationException("Sample Exception"); // Pas Exception message here to GlobalMiddleware.
            return Ok(Users.getUsers());
        }
    }


    class Users
    {
        public required string name {get;set;}
        public required string email {get;set;}

        public static List<Users> getUsers()
        {
            List<Users> users = new List<Users>();
            var user = new Users()
            {
                name = "Jon Doe",
                email = "jondoe@gmail.com"
            };
            users.Add(user);
            return users;
        }
    }
}
