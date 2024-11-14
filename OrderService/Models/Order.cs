

namespace OrderService.Models;
public class Order
{
    public int Id { get; set; }
    public int ProductId { get; set; }

    public int MerchantId { get; set; }

    public int BuyerId { get; set; }

    public string CardNumberMasked { get; set; } = "";

    public decimal TotalPrice { get; set;}

    public DateTime CreatedAt { get; set; }
}