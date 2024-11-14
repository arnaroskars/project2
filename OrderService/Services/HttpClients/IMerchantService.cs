
using OrderService.Models;

namespace OrderService.Services.HttpClients.Interfaces;
public interface IMerchantServiceClient
{
    Task<MerchantDto?> GetMerchantByIdAsync(int merchantId);
}
