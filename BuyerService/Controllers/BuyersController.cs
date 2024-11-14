
using Microsoft.AspNetCore.Mvc;
using BuyerService.Models;
using BuyerService.Services.Interfaces; // Assuming you have a service layer for handling logic
using System.Threading.Tasks;


namespace BuyerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuyersController : ControllerBase
    {
        private readonly IBuyerService _buyerService;

        public BuyersController(IBuyerService buyerService)
        {
            _buyerService = buyerService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBuyer([FromBody] BuyerRequestModel buyer)
        {
            if(!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            var createdBuyerId = await _buyerService.CreateBuyerAsync(buyer);
            return CreatedAtAction(nameof(CreateBuyer), new { id = createdBuyerId }, createdBuyerId);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetBuyer(int id)
        {
            var buyer = await _buyerService.GetBuyerByIdAsync(id);

            if (buyer == null)
            {
                return NotFound();
            }
            return Ok(buyer);
        }

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            return Ok("MerchantService is running and responding.");
        }

    }
}