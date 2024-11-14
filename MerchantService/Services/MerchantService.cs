
using MerchantService.Services.Interfaces;
using MerchantService.Models;
using MerchantService.Data.Interfaces;

namespace MerchantService.Services.Implementations
{
    public class MerchantServiceClass : IMerchantService
    {
        private readonly IMerchantRepository _merchantRepository;

        public MerchantServiceClass(IMerchantRepository merchantRepository)
        {
            _merchantRepository = merchantRepository;
        }

        public async Task<int> CreateMerchantAsync(MerchantRequestModel merchantInputModel)
        {
            var merchant = new Merchant
            {
                name = merchantInputModel.name,
                ssn = merchantInputModel.ssn,
                email = merchantInputModel.email,
                phoneNumber = merchantInputModel.phoneNumber,
                allowsDiscount = merchantInputModel.allowsDiscount
            };

            var createdMerchantId = await _merchantRepository.CreateMerchantAsync(merchant);

            return createdMerchantId;

        }

        public async Task<MerchantDto> GetMerchantByIdAsync(int merchantId)
        {
            var merchant = await _merchantRepository.GetMerchantByIdAsync(merchantId);
            return merchant;
        }
    }
}