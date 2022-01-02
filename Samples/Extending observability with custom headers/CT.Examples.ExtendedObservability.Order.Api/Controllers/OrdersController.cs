using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CT.Examples.ExtendedObservability.Order.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderCommandClient _orderCommandClient;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderCommandClient orderCommandClient, ILogger<OrdersController> logger)
        {
            _orderCommandClient = orderCommandClient;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody]OrderDto orderDto)
        {
            await _orderCommandClient.CreateOrder(orderDto);
            _logger.LogInformation($"Order created: Name = {orderDto.Name}, LastName = {orderDto.LastName}");
            return Ok();
        }
    }
}
