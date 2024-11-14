

namespace InventoryService.Models
{
    public class ProductRequestModel
    {
        public int merchantId { get; set; }

        public string productName { get; set; } = "";

        public decimal price { get; set; }

        public int quantity { get; set; }

    }
}