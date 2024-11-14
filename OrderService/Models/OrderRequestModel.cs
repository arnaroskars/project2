using System.ComponentModel.DataAnnotations;

namespace OrderService.Models;
public class OrderRequestModel
{
    [Required]
    public int productId { get; set; }
    [Required]
    public int merchantId { get; set; }
    [Required]
    public int buyerId { get; set; }
    [Required]
    public CreditCardModel creditCard { get; set; } = null!;

    [Range(0.1, 1)]
    public decimal discount { get; set; }

    // Navigation

    
}