using CT.Examples.SimplifiedCA.Api.Middleware;
using CT.Examples.SimplifiedCA.Domain.Services;
using CT.Examples.SimplifiedCA.Models.Output;
using Microsoft.AspNetCore.Mvc;

namespace CT.Examples.SimplifiedCA.Api.Controllers;

[ApiController]
[Route("/weather-forecast")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherForecastController(IWeatherService weatherService)
	{
        _weatherService = weatherService;
    }

    [HttpGet("/latitude/{latitude}/longitude/{longitude}")]
    public async Task<ActionResult<WeatherDto>> GetWeatherDataAsync(double latitude, double longitude)
    {
        // Simple validations could be done here or in the service
        if (latitude < 0 || longitude < 0)
        {
            return BadRequest("Invalid latitude or longitude");
        }

        return await _weatherService.GetWeatherDataAsync(Guid.NewGuid(), latitude, longitude).ToActionResult();
    }
}