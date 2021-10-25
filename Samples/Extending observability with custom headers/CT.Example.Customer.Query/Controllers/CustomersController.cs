using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace CT.Example.Customer.Query.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ILogger<CustomersController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{name}")]
        public IActionResult GetCustomer([FromRoute] string name)
        {
            _logger.LogInformation($"Getting customer by name: {name}");

            return Ok(new { Id = Guid.NewGuid().ToString() });
        }
    }
}
