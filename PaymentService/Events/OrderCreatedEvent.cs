using PaymentService.Models;

namespace PaymentService.Events
{
    public class OrderCreatedEvent
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int MerchantId { get; set; }
        public int BuyerId { get; set; }
        public decimal TotalPrice { get; set; }
        public CreditCardModel CreditCard { get; set; } = null!;
    }
}
