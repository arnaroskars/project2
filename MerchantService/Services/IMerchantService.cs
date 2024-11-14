
using MerchantService.Models;

namespace MerchantService.Services.Interfaces
{
    public interface IMerchantService
    {
        Task<int> CreateMerchantAsync(MerchantRequestModel merchant);

        Task<MerchantDto> GetMerchantByIdAsync(int merchantId);
    }
}