

namespace InventoryService.Models
{
    public class ProductDto
    {
        public int merchantId { get; set; }

        public string productName { get; set; } = "";

        public decimal price { get; set; }

        public int quantity { get; set; }

        public int reserved { get; set; }
    }
}