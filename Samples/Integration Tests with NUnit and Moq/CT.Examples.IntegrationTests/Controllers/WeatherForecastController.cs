using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CT.Examples.IntegrationTests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ITemperatureService _temperatureService;

        public WeatherForecastController(ITemperatureService temperatureService)
        {
            _temperatureService = temperatureService;
        }

        [HttpGet("temperature")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _temperatureService.GetTemperatureFromExternalApiAsync());
        }
    }
}
