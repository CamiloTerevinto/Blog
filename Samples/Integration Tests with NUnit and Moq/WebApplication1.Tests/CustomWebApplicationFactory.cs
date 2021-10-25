using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace WebApplication1.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        private readonly ExternalServicesMock _externalServicesMock;

        public CustomWebApplicationFactory(ExternalServicesMock externalServicesMock)
        {
            _externalServicesMock = externalServicesMock;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("IntegrationTesting");
            base.ConfigureWebHost(builder);

            builder
                .ConfigureServices(services =>
                {
                    foreach ((var interfaceType, var serviceMock) in _externalServicesMock.GetMocks())
                    {
                        services.Remove(services.SingleOrDefault(d => d.ServiceType == interfaceType));

                        services.AddSingleton(interfaceType, serviceMock);
                    }
                });
        }
    }
}
