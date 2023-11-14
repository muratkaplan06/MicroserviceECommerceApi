using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Models;

namespace ECommerce.Api.Products.Profile
{
    public class ProductProfile : AutoMapper.Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductModel>();
        }
    }
}
