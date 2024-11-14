
using OrderService.Models;

namespace OrderService.Services.HttpClients.Interfaces;
public interface IBuyerServiceClient
{
    Task<BuyerDto?> GetBuyerByIdAsync(int buyerId);
}
