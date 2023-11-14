using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Api.Products.Models;

namespace ECommerce.Api.Products.Interfaces
{
    public interface IProductProvider
    {
        Task<(bool IsSuccess, IEnumerable<ProductModel> Products, string ErrorMessage)> GetProductsAsync();
        Task<(bool IsSuccess, ProductModel Product, string ErrorMessage)> GetProductAsync(int id);
    }
}
