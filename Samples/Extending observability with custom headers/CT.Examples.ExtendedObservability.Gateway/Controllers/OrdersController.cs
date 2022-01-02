using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CT.Examples.ExtendedObservability.Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderApiClient _orderApiClient;

        public OrdersController(IOrderApiClient orderApiClient)
        {
            _orderApiClient = orderApiClient;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder()
        {
            await _orderApiClient.CreateOrder(new OrderDto { Name = "a", LastName = "b" });
            return Ok("Order created");
        }
    }
}
