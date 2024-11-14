public class PaymentResult
{
    public int OrderId { get; set; }
    public string Status { get; set; } // "Success" or "Failure"
    public decimal TotalPrice { get; set; }
}
