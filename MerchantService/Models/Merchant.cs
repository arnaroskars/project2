

namespace MerchantService.Models
{
    public class Merchant
    {
        public int Id { get; set; }
        public string name { get; set; } = "";
        public string ssn { get; set; } = "";
        public string email { get; set; } = "";

        public string phoneNumber { get; set; } = "";

        public bool allowsDiscount { get; set; }

    }

}