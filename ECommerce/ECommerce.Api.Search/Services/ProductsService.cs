using ECommerce.Api.Search.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using ECommerce.Api.Search.Models;

namespace ECommerce.Api.Search.Services
{
    public class ProductService : IProductsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IHttpClientFactory httpClientFactory,
            ILogger<ProductService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        public async Task<(bool IsSuccess, IEnumerable<Product> Products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                _logger?.LogInformation("Querying products");
                var client = _httpClientFactory.CreateClient("ProductsService");
                var response = await client.GetAsync("api/products");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var result = JsonSerializer.Deserialize<IEnumerable<Product>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return (true, result, null);
                }

                return (false, null, response.ReasonPhrase);
            }
            catch (Exception e)
            {
                _logger?.LogError(e.ToString());
                return (false, null, e.Message);
            }

        }
    }
}
