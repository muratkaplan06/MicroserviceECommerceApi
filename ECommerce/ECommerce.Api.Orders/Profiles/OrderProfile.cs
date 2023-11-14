using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Models;

namespace ECommerce.Api.Orders.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderModel>();
            CreateMap<OrderItem, OrderItemModel>();
        }
    }
}
