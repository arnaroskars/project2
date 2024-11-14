namespace PaymentService.Models
{
    public class Payment
    {
        public int Id { get; set; } 
        public int OrderId { get; set; } 
        public decimal TotalPrice { get; set; } 
        public string Status { get; set; } = ""; 
    }
}
