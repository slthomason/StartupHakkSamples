using EfCoreTips.Database;
using EFCoreTips.Database.RawQueryResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTips.Controllers
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
        
        [HttpGet("AsSplitQuery")]
        public async Task<IActionResult> asSplitQuery()
        {
            var getProducts = await _db.products
                                .AsNoTracking() // IT WILL MAKE QUERY MORE OPTIMISED
                                .Where(x => x.isActive)
                                .Include(x=>x.categories)
                                .AsSplitQuery()
                                .ToListAsync()
                                .ConfigureAwait(false);
            
            //IN SQL WHAT IT WILL DO IS 
            // First Query = Select * FROM Products WHERE isActive = 1;
            // Second Query = Select * FROM Categories WHERE CatId IN (select catId from first query result);

            return Ok(getProducts);
        }

        [HttpGet("bulkUpdatesAndDeletes")]
        public async Task<IActionResult> bulkUpdatesAndDeletes()
        {

            // WE WILL CALL DATABSE ONLY ONE TIME TO UPDATE ALL THE PRODUCTS IN BULK TO SAVE TIME WITHOUT RETRIVING IT
            await _db.products
                .Where(product => product.productId < 50)
                .ExecuteUpdateAsync(x => x.SetProperty(o => o.catId, 2));


            //WHAT IT WILL DO IN SQL IS
            // UPDATE products SET catId = 2 WHERE productId < 50;



            //BULK DELETION
            await _db.products
                .Where(product => product.isActive == false)
                .ExecuteDeleteAsync();
            
            //WHAT IT WILL DO IN SQL IS
            // DELETE FROM products WHERE isActive = 0;
                
            

            return Ok();
        }

        [HttpGet("rawSQLQuery")]
        public async Task<IActionResult> rawSQLQuery()
        {

            //IF you want to write raw SQL query and you do not want to use the built-In EF CORE functions or if you want to use the stored procedure
            var prods = await _db.Set<RawProductsResponse>() // You have to register your responses in dbcontext (OnModelCreating method)
            .FromSqlRaw("SELECT * FROM PRODUCTS")
            .ToListAsync()
            .ConfigureAwait(false);
            

            return Ok();
        }


        [HttpGet("queryFilters")]
        public async Task<IActionResult> queryFilters()
        {

            //ADD Any filter you want in your DbContext class in OnModelCreating method like i have set to fetch only active products
            var activeProducts = await _db.products
            .ToListAsync()
            .ConfigureAwait(false);

            //You can bypass the queryFilter too

            var allProducts = await _db.products
                            .IgnoreQueryFilters()
                            .ToListAsync(); // Retrieves all products, including deleted ones

            

            return Ok();
        }
    }
}
