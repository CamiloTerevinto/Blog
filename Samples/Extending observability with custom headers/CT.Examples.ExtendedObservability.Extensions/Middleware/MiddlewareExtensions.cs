using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CT.Examples.ExtendedObservability.Extensions.Middleware
{
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Adds the middleware and forces validation of the custom headers
        /// </summary>
        /// <param name="services">The service collection to inject the middleware</param>
        /// <returns>The service collection for further chaining</returns>
        public static IServiceCollection AddCustomHeadersLoggerMiddleware(this IServiceCollection services)
        {
            return AddCustomHeadersLoggerMiddleware(services, null);
        }

        /// <summary>
        /// Adds the middleware and skips validation of the custom headers, setting this service as the entry point.
        /// </summary>
        /// <param name="services">The service collection to inject the middleware</param>
        /// <param name="serviceName">The name of the service to have as a starting point</param>
        /// <returns>The service collection for further chaining</returns>
        public static IServiceCollection AddCustomHeadersLoggerMiddleware(this IServiceCollection services, string serviceName)
        {
            services.AddSingleton(new CustomHeadersLoggerOptions { DefaultServiceName = serviceName });
            return services.AddScoped<CustomHeadersLoggerMiddleware>();
        }

        public static IApplicationBuilder UseCustomHeadersLoggerMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomHeadersLoggerMiddleware>();
        }
    }
}
