using CT.Examples.AspNetCoreWithAngular.Domain.Configuration;
using CT.Examples.AspNetCoreWithAngular.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CT.Examples.AspNetCoreWithAngular;

public static class DomainExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration,
        AuthenticationOptions authenticationOptions)
    {
        services.AddSingleton(authenticationOptions);
        
        services
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<IAccountsService, AccountsService>();

        return services;
    }
}
