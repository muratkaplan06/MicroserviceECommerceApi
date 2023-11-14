using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext _context;
        private readonly ILogger<CustomersProvider> _logger;
        private readonly IMapper _mapper;

        public CustomersProvider(CustomersDbContext context,
            ILogger<CustomersProvider> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            SeedData();
        }

        private void SeedData()
        {
            if (!_context.Customers.Any())
            {
                _context.Customers.Add(new Customer() { Id = 1, Name = "John", Address = "123 Street" });
                _context.Customers.Add(new Customer() { Id = 2, Name = "Mary", Address = "456 Street" });
                _context.Customers.Add(new Customer() { Id = 3, Name = "Ana", Address = "789 Street" });
                _context.Customers.Add(new Customer() { Id = 4, Name = "Peter", Address = "101 Street" });
                _context.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<CustomerModel> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var customers = await _context.Customers.ToListAsync();
                if (customers != null && customers.Any())
                {
                    _logger?.LogInformation("Customers were retrieved successfully");
                    var result = _mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerModel>>(customers);
                    return (true, result, null);
                }

                return (true, null, "Not Found");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<(bool IsSuccess, CustomerModel Customer, string ErrorMessage)> GetCustomerAsync(int id)
        {

            try
            {
                var result = await _context.Customers.FirstOrDefaultAsync(p => p.Id == id);
                if (result != null)
                {
                    _logger?.LogInformation("Customer was retrieved successfully");
                    var customer = _mapper.Map<Customer, CustomerModel>(result);
                    return (true, customer, null);
                }
                return (true, null, "Not Found");

            }
            catch (Exception e)
            {
                _logger?.LogError(e.ToString());
                return (false, null, e.Message);
            }
        }
    }
}
