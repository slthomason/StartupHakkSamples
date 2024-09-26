using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultipleDatabases.Database;
using MultipleDatabases.Database.Entities;
using MultipleDatabases.Interface;

namespace MultipleDatabases.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        protected IRepository<Categories, RestaurantContext> restaurantRepository { get; }

        protected IRepository<Users, UsersContext> userRepository { get; }

        public ValueController(IRepository<Categories, RestaurantContext> restaurantRepository, IRepository<Users, UsersContext> userRepository)
        {
            this.userRepository = userRepository;
            this.restaurantRepository = restaurantRepository;
        }


        [HttpGet("getCategories")]
        public async Task<IEnumerable<Categories>> GetCategories()
        {
            return restaurantRepository.GetAll();
        }

        [HttpGet("getUsers")]
        public async Task<IEnumerable<Users>> GetUsers()
        {
            return userRepository.GetAll();
        }
    }
}
