using WebApplication24.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication24.Models;
using Microsoft.VisualBasic;
namespace WebApplication24.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProduct _product;
        public ProductsController(IProduct product)
        {
            _product = product;
        }
        [HttpGet]
        public IActionResult Get([FromQuery] bool? isActive = null)
        {
            return Ok(_product.GetAllProducts(isActive));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_product.GetProductsByID(id));
        }

        [HttpPost]
        public IActionResult Post(AddUpdateProducts productObject)
        {
            var item = _product.AddProducts(productObject);

            if (item == null)
            {
                return BadRequest();
            }

            return Ok(new
            {
                message = "Product Created Successfully!!!",
                id = item!.Id
            });
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] AddUpdateProducts productObject)
        {
            var item = _product.UpdateProducts(id,productObject);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                message = "Product Updated Successfully!!!",
                id = item!.Id
            });
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            if (!_product.DeleteProductsByID(id))
            {
                return NotFound();
            }
            return Ok(new
            {
                message = "Product Deleted Successfully!!!",
                id = id
            });
        }
    }
}
