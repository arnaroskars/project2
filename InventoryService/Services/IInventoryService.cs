using System.Threading.Tasks;
using InventoryService.Models;

namespace InventoryService.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<int> CreateProductAsync(ProductRequestModel productRequest);
        Task<ProductDto> GetProductByIdAsync(int productId);
    }
}
