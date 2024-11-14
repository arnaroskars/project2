

using System.ComponentModel.DataAnnotations;

namespace InventoryService.Models
{
    public class Product
    {
        public int Id { get; set; }

        public int merchantId { get; set; }

        public string productName { get; set; } = "";

        public decimal price { get; set; }

        public int quantity { get; set; }

        public int reserved { get; set; } = 0;
    }
}