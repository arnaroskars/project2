
using MerchantService.Models;

namespace MerchantService.Data.Interfaces
{
    public interface IMerchantRepository
    {
        Task<int> CreateMerchantAsync(Merchant merchant);

        Task<MerchantDto> GetMerchantByIdAsync(int merchantId);
    }
}