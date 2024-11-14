
using InventoryService.Models;

namespace InventoryService.Data.Interfaces
{
    public interface IInventoryRepository
    {
        Task<ProductDto> GetProductByIdAsync(int productId);
        Task<int> CreateProductAsync(Product product);
    }
}