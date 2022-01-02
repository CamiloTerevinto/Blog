using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CT.Examples.ExtendedObservability.Order.Command.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ICustomerQueryClient _customerQueryClient;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ICustomerQueryClient customerQueryClient, ILogger<OrdersController> logger)
        {
            _customerQueryClient = customerQueryClient;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody]OrderDto order)
        {
            var customer = await _customerQueryClient.GetCustomerData(order.Name);
            _logger.LogInformation($"Order created for customer: {customer.Id}");

            return NoContent();
        }
    }
}
