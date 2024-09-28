using EfCoreTips.Database;
using EfCoreTips.Database.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfCoreTips.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        private readonly DatabaseContext _db;
        public ValueController(DatabaseContext _db)
        {
            this._db = _db;
        }


        [HttpGet("EfCore")]
        public async Task<IActionResult> efCoreTips()
        {

            //Project -- Select specific columns that you need
            var projection = await _db.products
            .Where(prod => prod.catId == 5)
            .Select(
                prod => new {
                    prod.productId,
                    prod.productName
                }
            )
            .ToListAsync();


            //Split Query -- breaks down a query involving a large number of related entities into multiple SQL queries, reducing the size of the result set.
            var splitQuery = await _db.products
            .Include(prod => prod.categories)
            .Select(prod => new 
            {
                productId = prod.productId,
                productName = prod.productName,
                catId = prod.catId,
                categories = prod.categories,
                isActive = prod.isActive,
            })
            .AsSplitQuery()
            .ToListAsync();



            //If you want to have a more responsive application, use the asynchronous methods that EF Core provides.
            //They free up threads to handle other requests while waiting for I/O operations to complete.
            var asyncProducts = await _db.products
            .Where(x => x.productId < 50)
            .ToListAsync();


            //AsNoTracking() tells EF Core not to track the entities retrieved
            //It saves memory and improves performance for read-only operations.
            var noTrackedProducts = await _db.products
            .AsNoTracking()
            .Where(x => x.productId < 50)
            .ToListAsync();


            //Using methods like Count() and Any() allows EF Core to generate more efficient SQL queries.
            //They are more efficient than retrieving and counting entities in memory.

            bool hasAnyActiveProduct = await _db.products
            .AnyAsync(x => x.isActive);

            int activeProductsCount = await _db.products
            .CountAsync(x=>x.isActive);


            //By calling SaveChangesAsync() once after all updates, you reduce the number of trips to the database.
            //This process dramatically improves performance in bulk operations.

            var productsToUpdate = await _db.products.Where(x=>x.isActive).ToListAsync().ConfigureAwait(false);
            foreach(var product in productsToUpdate)
            {
                product.isActive = false;
            }
            await _db.SaveChangesAsync().ConfigureAwait(false);


            //create an initial IQueryable query.
            //Then, use it in different ways to avoid multiple query variables.

            IQueryable<Products> query = _db.products.Where(x => x.isActive);

            int count = await query.CountAsync();
            if(count > 0)
            {
                var productsList = await query.Where(x=> x.productId > 100).ToListAsync().ConfigureAwait(false);
            }





            return Ok();
        }

    }

    
    record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }

}
