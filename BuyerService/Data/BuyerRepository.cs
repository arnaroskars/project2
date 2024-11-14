
using BuyerService.Data.Interfaces;
using BuyerService.Models;
using BuyerService.OrderDb;

namespace BuyerService.Data.Implementations
{
    public class BuyerRepository : IBuyerRepository
    {
        private readonly BuyerDbContext _dbContext;

        public BuyerRepository(BuyerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateBuyerAsync(Buyer buyer)
        {
            await _dbContext.Buyers.AddAsync(buyer);
            await _dbContext.SaveChangesAsync();
            return buyer.Id;
        }

        public async Task<BuyerDto> GetBuyerByIdAsync(int buyerId)
        {
            var buyer = await _dbContext.Buyers.FindAsync(buyerId);

            if (buyer == null)
            {
                return null!;
            }
            var buyerDto = new BuyerDto
            {
                name = buyer.name,
                ssn = buyer.ssn,
                email = buyer.email,
                phoneNumber = buyer.phoneNumber,
            };

            return buyerDto;
        }
    }
}