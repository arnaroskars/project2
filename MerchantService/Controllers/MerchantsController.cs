
using Microsoft.AspNetCore.Mvc;
using MerchantService.Models;
using MerchantService.Services.Interfaces; // Assuming you have a service layer for handling logic
using System.Threading.Tasks;


namespace MerchantService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MerchantsController : ControllerBase
    {
        private readonly IMerchantService _merchantService;

        public MerchantsController(IMerchantService merchantService)
        {
            _merchantService = merchantService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMerchant([FromBody] MerchantRequestModel merchant)
        {
            if(!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            var createdOrderId = await _merchantService.CreateMerchantAsync(merchant);
            return CreatedAtAction(nameof(GetMerchant), new { id = createdOrderId }, createdOrderId);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetMerchant(int id)
        {
            var order = await _merchantService.GetMerchantByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            return Ok("MerchantService is running and responding.");
        }

    }
}