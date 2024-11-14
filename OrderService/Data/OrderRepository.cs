using System.Threading.Tasks;
using OrderService.Data.Interfaces;
using OrderService.Models;
using Microsoft.EntityFrameworkCore;
using OrderService.OrderDb;

namespace OrderService.Data.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _dbContext;

        public OrderRepository(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Method to create an order in the database
        public async Task<int> CreateOrderAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return order.Id;
        }

        // Method to retrieve an order by ID from the database
        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);

            if (order == null)
            {
                return null!;
            }
            var orderDto = new OrderDto
            {
                ProductId = order.ProductId,
                MerchantId = order.MerchantId,
                BuyerId = order.BuyerId,
                CardNumber = order.CardNumberMasked,
                TotalPrice = order.TotalPrice
            };
            return orderDto;
        }
    }
}
