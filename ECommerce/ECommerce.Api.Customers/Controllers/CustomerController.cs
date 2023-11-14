using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ECommerce.Api.Customers.Interfaces;

namespace ECommerce.Api.Customers.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomersProvider _provider;

        public CustomerController(ICustomersProvider provider)
        {
            _provider = provider;
        }
        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var result = await _provider.GetCustomersAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Customers);
            }

            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerAsync(int id)
        {
            var result = await _provider.GetCustomerAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Customer);
            }

            return NotFound();
        }
    }
}
