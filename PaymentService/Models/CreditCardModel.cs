
namespace PaymentService.Models;
public class CreditCardModel
{
    public string cardNumber { get; set; } = "";

    public int expirationMonth { get; set;}

    public int expirationYear { get; set; }

    public int cvc { get; set; }
    
}