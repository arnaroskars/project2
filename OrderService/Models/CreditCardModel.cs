using System.ComponentModel.DataAnnotations;

namespace OrderService.Models;
public class CreditCardModel
{
    [Required]
    public string cardNumber { get; set; } = "";

    [Required]
    public int expirationMonth { get; set;}

    [Required]
    public int expirationYear { get; set; }

    [Required]
    [Range(100, 999, ErrorMessage = "CVC must be a 3 digit number.")]
    public int cvc { get; set; }
    
}