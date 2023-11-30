using CT.Examples.SimplifiedCA.Entities;
using CT.Examples.SimplifiedCA.Infrastructure.Services;
using CT.Examples.SimplifiedCA.Models;
using CT.Examples.SimplifiedCA.Models.Output;
using Microsoft.EntityFrameworkCore;

namespace CT.Examples.SimplifiedCA.Domain.Services;

public interface IWeatherService
{
    Task<OperationResult<WeatherDto>> GetWeatherDataAsync(Guid currentAccountId, double latitude, double longitude);
}

internal class WeatherService : IWeatherService
{
    private readonly SimplifiedCAContext _dbContext;
    private readonly IWeatherApiClient _weatherApiClient;

    public WeatherService(SimplifiedCAContext dbContext, IWeatherApiClient weatherApiClient)
    {
        _dbContext = dbContext;
        _weatherApiClient = weatherApiClient;
    }

    public async Task<OperationResult<WeatherDto>> GetWeatherDataAsync(Guid currentAccountId, double latitude, double longitude)
    {
        if (!await _dbContext.Accounts.AnyAsync(x => x.Id == currentAccountId && x.Enabled))
        {
            // Case 1: return a status directly
            return OperationResultStatus.Forbidden;
        }

        var existingData = await _dbContext.WeatherForecasts
            .SingleOrDefaultAsync(x => x.AccountId == currentAccountId && x.Latitude == latitude && x.Longitude == longitude);

        if (existingData != null)
        {
            // Case 2: return the generic T directly
            return new WeatherDto
            {
                Latitude = latitude,
                Longitude = longitude,
                Temperature = existingData.Temperature,
                Unit = existingData.Unit,
            };
        }

        var weatherAnalyticsData = await _weatherApiClient.GetWeatherDataAsync(latitude, longitude);

        if (weatherAnalyticsData == null)
        {
            // Case 3: return a status and an error message
            return new OperationResult<WeatherDto>(OperationResultStatus.NotFound, $"Weather data not found for the latitude {latitude} and longitude {longitude}");
        }

        var weatherForecast = new WeatherForecast
        {
            AccountId = currentAccountId,
            Latitude = latitude,
            Longitude = longitude,
            Temperature = weatherAnalyticsData.Temperature,
            Unit = weatherAnalyticsData.Unit,
            Time = DateTimeOffset.UtcNow,
        };

        _dbContext.WeatherForecasts.Add(weatherForecast);
        await _dbContext.SaveChangesAsync();

        // Case 2 again
        return new WeatherDto
        {
            Latitude = latitude,
            Longitude = longitude,
            Temperature = weatherAnalyticsData.Temperature,
            Unit = weatherAnalyticsData.Unit ?? ""
        };
    }
}
