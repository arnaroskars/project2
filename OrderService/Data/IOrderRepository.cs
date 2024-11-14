using System.Threading.Tasks;
using OrderService.Models;

namespace OrderService.Data.Interfaces
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderAsync(Order order);
        Task<OrderDto> GetOrderByIdAsync(int id);
    }
}
