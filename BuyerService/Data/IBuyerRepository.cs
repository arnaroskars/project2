
using BuyerService.Models;

namespace BuyerService.Data.Interfaces
{
    public interface IBuyerRepository
    {
        Task<int> CreateBuyerAsync(Buyer buyer);

        Task<BuyerDto> GetBuyerByIdAsync(int buyerId);
    }
}