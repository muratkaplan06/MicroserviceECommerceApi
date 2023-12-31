﻿using ECommerce.Api.Search.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace ECommerce.Api.Search.Services
{
    public class CustomersService : ICustomersService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CustomersService> _logger;

        public CustomersService(IHttpClientFactory httpClientFactory,
            ILogger<CustomersService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<(bool IsSuccess, dynamic Customer, string ErrorMessage)> GetCustomerAsync(int customerId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CustomersService");
                var response = await client.GetAsync($"api/customers/{customerId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var result = JsonSerializer.Deserialize<dynamic>(content, new JsonSerializerOptions
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
