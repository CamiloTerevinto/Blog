using CT.Examples.SimplifiedCA.Infrastructure.Models;
using System.Net.Http.Json;

namespace CT.Examples.SimplifiedCA.Infrastructure.Services;

public interface IWeatherApiClient
{
    Task<WeatherAnalyticsDto> GetWeatherDataAsync(double latitude, double longitude);
}

internal class WeatherApiClient : IWeatherApiClient
{
    private readonly HttpClient _httpClient;

    public WeatherApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherAnalyticsDto> GetWeatherDataAsync(double latitude, double longitude)
    {
        return (await _httpClient.GetFromJsonAsync<WeatherAnalyticsDto>($"/something-something?latitude={latitude}&longitude={longitude}"))!;
    }
}
