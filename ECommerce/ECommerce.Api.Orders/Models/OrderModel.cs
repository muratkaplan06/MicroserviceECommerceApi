using System.Collections.Generic;
using System;

namespace ECommerce.Api.Orders.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemModel> Items { get; set; }
    }
}
