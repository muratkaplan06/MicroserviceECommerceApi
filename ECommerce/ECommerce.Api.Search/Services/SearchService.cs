using System.Linq;
using ECommerce.Api.Search.Interfaces;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService _ordersService;
        private readonly IProductsService _productService;
        private readonly ICustomersService _customersService;

        public SearchService(IOrdersService ordersService,
            IProductsService productService,
            ICustomersService customersService)
        {
            _ordersService = ordersService;
            _productService = productService;
            _customersService = customersService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResult)> SearchAsync(int customerId)
        {
            var customerResult = await _customersService.GetCustomerAsync(customerId);
            var ordersResult = await _ordersService.GetOrdersAsync(customerId);
            var productsResult = await _productService.GetProductsAsync();
            if (ordersResult.IsSuccess)
            {
                foreach (var order in ordersResult.Orders)
                {
                    foreach (var item in order.Items)
                    {
                        item.ProductName = productsResult.IsSuccess ?
                            productsResult.Products.FirstOrDefault(x => x.Id == item.Id)?.Name
                            : "Product information is not available";
                    }


                }

                var result = new
                {
                    Customer = customerResult.IsSuccess ?
                        customerResult.Customer
                        : new { Name = "Customer information is not available" },
                    Orders = ordersResult.Orders
                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
