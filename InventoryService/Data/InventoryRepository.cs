
using InventoryService.Data.Interfaces;
using InventoryService.InventoryDb;
using InventoryService.Models;

namespace InventoryService.Data.Implementations
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly InventoryDbContext _dbContext;

        public InventoryRepository(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        } 

        public async Task<int> CreateProductAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product.Id;
        }

        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);

            if (product == null)
            {
                return null!;
            }

            return new ProductDto{
                merchantId = product.merchantId,
                productName = product.productName,
                price = product.price,
                quantity = product.quantity,
                reserved = product.reserved
            };

        }
    }
}