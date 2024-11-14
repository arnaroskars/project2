
using OrderService.Models;

namespace OrderService.Services.HttpClients.Interfaces
{
    public interface IInventoryServiceClient
    {
        Task<ProductDto?> GetProductByIdAsync(int productId);
    }

}
