using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Models;

namespace ECommerce.Api.Customers.Profile
{
    public class CustomerProfile : AutoMapper.Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerModel>();
        }
    }
}
