using CT.Examples.CqrsWithAspire.Command.Api;
using CT.Examples.CqrsWithAspire.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CT.Examples.CqrsWithAspire.Tests.Internal;

internal class CommandApiWebAppFactory(string postgresConnectionString) : WebApplicationFactory<Marker>
{
    private readonly string _postgresConnectionString = postgresConnectionString;

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTesting");

        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "ConnectionStrings:Shop", _postgresConnectionString }
            });

            // Replace services as needed
        });

        return base.CreateHost(builder);
    }

    internal static async Task<CommandApiWebAppFactory> CreateInitializedAsync(string postgresConnectionString)
    {
        var factory = new CommandApiWebAppFactory(postgresConnectionString);

        await using var scope = factory.Services.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
        await context.Database.EnsureCreatedAsync();

        return factory;
    }
}