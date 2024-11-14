
using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using OrderService.Services.Interfaces; // Assuming you have a service layer for handling logic
using System.Threading.Tasks;


namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestModel order)
        {
            try
            {
                var createdOrderId = await _orderService.CreateOrderAsync(order);
                return CreatedAtAction(nameof(GetOrder), new { id = createdOrderId }, createdOrderId);
            }
            catch (Exception ex)
            {
                // Catch any exception and return a 400 Bad Request with the error message
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            return Ok("OrderService is running and responding.");
        }

    }
}