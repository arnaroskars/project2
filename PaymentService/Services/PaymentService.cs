using System.Threading.Tasks;
using PaymentService.Models;
using PaymentService.OrderDb;

namespace PaymentService.Services
{
    public interface IPaymentService
    {
        Task SavePaymentAsync(int orderId, decimal totalPrice, string status);
    }

    public class PaymentServiceClass : IPaymentService
    {
        private readonly PaymentDbContext _dbContext;

        public PaymentServiceClass(PaymentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SavePaymentAsync(int orderId, decimal totalPrice, string status)
        {
            var newPayment = new Payment
            {
                OrderId = orderId,
                TotalPrice = totalPrice,
                Status = status
            };

            await _dbContext.Payments.AddAsync(newPayment);
            await _dbContext.SaveChangesAsync();
        }
    }
}
