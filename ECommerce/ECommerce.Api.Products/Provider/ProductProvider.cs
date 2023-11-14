using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Interfaces;
using ECommerce.Api.Products.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ECommerce.Api.Products.Provider
{
    public class ProductProvider : IProductProvider
    {
        private readonly ProductsDbContext _context;
        private readonly ILogger<ProductProvider> _logger;
        private readonly IMapper _mapper;

        public ProductProvider(ProductsDbContext context,
            ILogger<ProductProvider> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!_context.Products.Any())
            {
                _context.Products.Add(new Product() { Id = 1, Name = "Keyboard", Inventory = 100, Price = 20 });
                _context.Products.Add(new Product() { Id = 2, Name = "Mouse", Inventory = 200, Price = 5 });
                _context.Products.Add(new Product() { Id = 3, Name = "Monitor", Inventory = 300, Price = 150 });
                _context.Products.Add(new Product() { Id = 4, Name = "CPU", Inventory = 400, Price = 200 });
                _context.Products.Add(new Product() { Id = 5, Name = "Laptop", Inventory = 500, Price = 350 });
                _context.Products.Add(new Product() { Id = 6, Name = "Notebook", Inventory = 600, Price = 400 });
                _context.Products.Add(new Product() { Id = 7, Name = "Tablet", Inventory = 700, Price = 450 });
                _context.Products.Add(new Product() { Id = 8, Name = "Phone", Inventory = 800, Price = 500 });
                _context.Products.Add(new Product() { Id = 9, Name = "Watch", Inventory = 900, Price = 550 });
                _context.Products.Add(new Product() { Id = 10, Name = "Camera", Inventory = 1000, Price = 600 });
                _context.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<ProductModel> Products,
            string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                if (products != null && products.Any())
                {
                    _logger?.LogInformation("Products were retrieved successfully");
                    var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductModel>>(products);
                    return (true, result, null);
                }

                return (true, null, "Not Found");
            }
            catch (Exception e)
            {
                _logger?.LogError(e.ToString());
                return (false, null, e.Message);
            }
        }

        public async Task<(bool IsSuccess, ProductModel Product, string ErrorMessage)> GetProductAsync(int id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product != null)
                {

                    _logger?.LogInformation("Product was retrieved successfully");
                    var result = _mapper.Map<Product, ProductModel>(product);
                    return (true, result, null);
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
