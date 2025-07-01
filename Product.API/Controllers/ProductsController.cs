using Microsoft.AspNetCore.Mvc;
using Product.API.Models.Entities;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
     private readonly ProductAPIDbContext _context;
     public ProductsController(ProductAPIDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product.API.Models.Entities.Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }



    }
}
