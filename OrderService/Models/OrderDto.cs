namespace OrderService.Models;

public class OrderDto
{
    public int ProductId { get; set; }

    public int MerchantId { get; set; }

    public int BuyerId { get; set; }

    public string CardNumber { get; set; } = "";

    public decimal TotalPrice { get; set; }
}