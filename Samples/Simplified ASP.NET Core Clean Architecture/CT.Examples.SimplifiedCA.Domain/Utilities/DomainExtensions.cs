using CT.Examples.SimplifiedCA.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CT.Examples.SimplifiedCA;

public static class DomainExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IWeatherService, WeatherService>();

        return services;
    }
}
