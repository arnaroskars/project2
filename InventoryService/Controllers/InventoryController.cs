
using Microsoft.AspNetCore.Mvc;
using InventoryService.Models;
using InventoryService.Services.Interfaces;

namespace InventoryService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public ProductsController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductRequestModel product)
        {
            var createdProductId = await _inventoryService.CreateProductAsync(product);
            return Ok(new { ProductId = createdProductId });
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            return Ok("InventoryService is running and responding.");
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _inventoryService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }


    }
}