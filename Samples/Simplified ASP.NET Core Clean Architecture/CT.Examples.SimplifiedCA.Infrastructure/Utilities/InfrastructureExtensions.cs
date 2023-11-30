using CT.Examples.SimplifiedCA.Entities;
using CT.Examples.SimplifiedCA.Infrastructure.Configuration;
using CT.Examples.SimplifiedCA.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace CT.Examples.SimplifiedCA;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var dbOptions = configuration.GetRequiredSection("Database").Get<WeatherDbOptions>() ?? throw new InvalidOperationException("Missing Database configuration");
        dbOptions.Validate();

        services.AddDbContext<SimplifiedCAContext>(opt =>
        {
            // Note: does **not** attempts to represent a valid connection string.
            var connectionString = $"Server={dbOptions.Server};Database={dbOptions.Database};Username={dbOptions.Username};Password={dbOptions.Password}";

            opt.UseSqlServer(connectionString);
        });

        var weatherApiOptions = configuration.GetRequiredSection("WeatherApi").Get<WeatherApiOptions>() ?? throw new InvalidOperationException("Missing WeatherApi configuration");
        weatherApiOptions.Validate();

            // Note: this is just a random example of an "Infrastructure" service - i.e. a service that consumes/facilitates consuming 3rd party services/APIs/etc.
        services.AddHttpClient<IWeatherApiClient, WeatherApiClient>(nameof(IWeatherApiClient), client =>
        {
            client.BaseAddress = new Uri(weatherApiOptions.BaseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", weatherApiOptions.Token);
        });

        return services;
    }
}
