namespace EmailService.Models
{
    public class Email
    {
        public int Id { get; set; } 
        public int OrderId { get; set; } 
        public decimal TotalPrice { get; set; } 
        public string Status { get; set; } = ""; 
    }
}
