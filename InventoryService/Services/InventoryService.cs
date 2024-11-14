using System;
using System.Threading.Tasks;
using InventoryService.Models;
using InventoryService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using InventoryService.InventoryDb;
using InventoryService.Data.Interfaces;

namespace InventoryService.Services.Implementations
{
    public class InventoryServiceClass : IInventoryService
    {

        private readonly IInventoryRepository _inventoryRepository;

        public InventoryServiceClass(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }
        // Method to create a new product and save it to the database
        public async Task<int> CreateProductAsync(ProductRequestModel productRequest)
        {
            var product = new Product
            {
                merchantId = productRequest.merchantId,
                productName = productRequest.productName,
                price = productRequest.price,
                quantity = productRequest.quantity,
            };

            await _inventoryRepository.CreateProductAsync(product);
            return product.Id;
        }

        // Method to get a product by its ID, returning a ProductDto
        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            return await _inventoryRepository.GetProductByIdAsync(productId);
        }
    }
}
