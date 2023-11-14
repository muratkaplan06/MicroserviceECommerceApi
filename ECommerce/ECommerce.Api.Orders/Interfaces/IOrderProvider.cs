using ECommerce.Api.Orders.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Interfaces
{
    public interface IOrderProvider
    {
        Task<(bool IsSuccess, IEnumerable<OrderModel> Orders, string ErrorMessage)> GetOrdersAsync(int customerId);
    }
}
