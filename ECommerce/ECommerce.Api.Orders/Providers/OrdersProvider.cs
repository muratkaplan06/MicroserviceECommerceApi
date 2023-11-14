using ECommerce.Api.Orders.Interfaces;
using ECommerce.Api.Orders.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using AutoMapper;
using ECommerce.Api.Orders.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrderProvider
    {
        private readonly OrdersDbContext _context;
        private readonly ILogger<OrdersProvider> _logger;
        private readonly IMapper _mapper;

        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this._context = dbContext;
            this._logger = logger;
            this._mapper = mapper;
            SeedData();
        }


        private void SeedData()
        {
            if (!EnumerableExtensions.Any(_context.Orders))
            {
                _context.Orders.Add(new Order()
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = DateTime.Now,
                    Items = new List<OrderItem>()
                    {
                        new OrderItem() { OrderId = 1, ProductId = 1, Quantity = 111, UnitPrice = 10 },
                        new OrderItem() { OrderId = 2, ProductId = 2, Quantity = 112, UnitPrice = 10 },
                        new OrderItem() { OrderId = 3, ProductId = 3, Quantity = 113, UnitPrice = 10 },
                        new OrderItem() { OrderId = 4, ProductId = 2, Quantity = 114, UnitPrice = 10 },
                        new OrderItem() { OrderId = 5, ProductId = 3, Quantity = 115, UnitPrice = 100 }
                    },
                    Total = 100
                });
                _context.Orders.Add(new Order()
                {
                    Id = 2,
                    CustomerId = 1,
                    OrderDate = DateTime.Now.AddDays(-1),
                    Items = new List<OrderItem>()
                    {
                        new OrderItem() { OrderId = 1, ProductId = 1, Quantity = 1112, UnitPrice = 10 },
                        new OrderItem() { OrderId = 2, ProductId = 2, Quantity = 1113, UnitPrice = 10 },
                        new OrderItem() { OrderId = 3, ProductId = 3, Quantity = 1114, UnitPrice = 10 },
                        new OrderItem() { OrderId = 4, ProductId = 4, Quantity = 1115, UnitPrice = 10 },
                        new OrderItem() { OrderId = 5, ProductId = 5, Quantity = 1116, UnitPrice = 100 }
                    },
                    Total = 100
                });
                _context.Orders.Add(new Order()
                {
                    Id = 3,
                    CustomerId = 2,
                    OrderDate = DateTime.Now,
                    Items = new List<OrderItem>()
                    {
                        new OrderItem() { OrderId = 1, ProductId = 1, Quantity = 221, UnitPrice = 10 },
                        new OrderItem() { OrderId = 2, ProductId = 2, Quantity = 222, UnitPrice = 10 },
                        new OrderItem() { OrderId = 3, ProductId = 3, Quantity = 233, UnitPrice = 100 }
                    },
                    Total = 100
                });
                _context.SaveChanges();
            }
        }



        public async Task<(bool IsSuccess, IEnumerable<OrderModel> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                var orders = await _context.Orders
                    .Where(o => o.CustomerId == customerId)
                    .Include(o => o.Items)
                    .ToListAsync();
                if (orders != null && orders.Any())
                {
                    _logger?.LogInformation("Orders were retrieved for CustomerId: {0}", customerId);
                    var result = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderModel>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
