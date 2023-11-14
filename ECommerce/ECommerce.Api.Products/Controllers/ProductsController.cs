using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ECommerce.Api.Products.Interfaces;

namespace ECommerce.Api.Products.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductProvider _productProvider;

        public ProductsController(IProductProvider productProvider)
        {
            _productProvider = productProvider;
        }
        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            var result = await _productProvider.GetProductsAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Products);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductAsync(int id)
        {
            var result = await _productProvider.GetProductAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Product);
            }
            return NotFound();
        }
    }
}
