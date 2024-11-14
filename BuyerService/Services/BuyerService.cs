
using BuyerService.Services.Interfaces;
using BuyerService.Models;
using BuyerService.Data.Interfaces;

namespace BuyerService.Services.Implementations
{
    public class BuyerServiceClass : IBuyerService
    {
        private readonly IBuyerRepository _buyerRepository;

        public BuyerServiceClass(IBuyerRepository buyerRepository)
        {
            _buyerRepository = buyerRepository;
        }

        public async Task<int> CreateBuyerAsync(BuyerRequestModel buyerInputModel)
        {
            var buyer = new Buyer
            {
                name = buyerInputModel.name,
                ssn = buyerInputModel.ssn,
                email = buyerInputModel.email,
                phoneNumber = buyerInputModel.phoneNumber,
            };

            var createdBuyerId = await _buyerRepository.CreateBuyerAsync(buyer);

            return createdBuyerId;

        }

        public async Task<BuyerDto> GetBuyerByIdAsync(int merchantId)
        {
            var buyer = await _buyerRepository.GetBuyerByIdAsync(merchantId);
            return buyer;
        }
    }
}