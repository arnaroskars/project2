
using MerchantService.Data.Interfaces;
using MerchantService.Models;
using MerchantService.OrderDb;

namespace MerchantService.Data.Implementations
{
    public class MerchantRepository : IMerchantRepository
    {
        private readonly MerchantDbContext _dbContext;

        public MerchantRepository(MerchantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateMerchantAsync(Merchant merchant)
        {
            await _dbContext.Merchants.AddAsync(merchant);
            await _dbContext.SaveChangesAsync();
            return merchant.Id;
        }

        public async Task<MerchantDto> GetMerchantByIdAsync(int merchantId)
        {
            var merchant = await _dbContext.Merchants.FindAsync(merchantId);

            if (merchant == null)
            {
                return null!;
            }
            var merchantDto = new MerchantDto
            {
                name = merchant.name,
                ssn = merchant.ssn,
                email = merchant.email,
                phoneNumber = merchant.phoneNumber,
                allowsDiscount = merchant.allowsDiscount
            };

            return merchantDto;
        }
    }
}