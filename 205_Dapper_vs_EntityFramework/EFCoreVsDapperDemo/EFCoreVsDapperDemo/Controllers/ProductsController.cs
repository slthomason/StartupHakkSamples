using EFCoreVsDapperDemo.DataAccess;
using EFCoreVsDapperDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreVsDapperDemo.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ProductRepository _productRepository;
        public ProductsController(AppDbContext context, ProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var productsEfCore = await _context.Products.ToListAsync();
            var productsDapper = await _productRepository.GetAllProducts();

            // You can return one of the lists or both for comparison
            return View(productsDapper); // or productsDapper
        }
    }
}
