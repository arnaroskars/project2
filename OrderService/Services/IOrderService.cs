using System.Threading.Tasks;
using OrderService.Models;

namespace OrderService.Services.Interfaces
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(OrderRequestModel orderRequest);
        Task<OrderDto> GetOrderByIdAsync(int id);
    }
}
