
using BuyerService.Models;

namespace BuyerService.Services.Interfaces
{
    public interface IBuyerService
    {
        Task<int> CreateBuyerAsync(BuyerRequestModel buyer);

        Task<BuyerDto> GetBuyerByIdAsync(int buyerId);
    }
}