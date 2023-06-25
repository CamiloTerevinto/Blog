using CT.Examples.AspNetCoreWithAngular.Entities;
using CT.Examples.AspNetCoreWithAngular.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CT.Examples.AspNetCoreWithAngular;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var dbOptions = configuration.GetRequiredSection("Database").Get<AspNetCoreWithAngularOptions>();
        dbOptions.Validate();

        services.AddDbContext<AspNetCoreWithAngularContext>(opt =>
        {
            // Note: does **not** attempts to represent a valid connection string.
            var connectionString = $"Server={dbOptions.Server};Port=5432;Database={dbOptions.Database};User Id={dbOptions.Username};Password={dbOptions.Password}";

            opt.UseNpgsql(connectionString);
        });

        return services;
    }
}
