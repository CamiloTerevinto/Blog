using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CT.Example.Extensions.Middleware
{
    internal class CustomHeadersLoggerMiddleware : IMiddleware
    {
        private readonly CustomHeadersLoggerOptions _options;
        private readonly ILogger<CustomHeadersLoggerMiddleware> _logger;

        public CustomHeadersLoggerMiddleware(CustomHeadersLoggerOptions options, ILogger<CustomHeadersLoggerMiddleware> logger)
        {
            _options = options;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!string.IsNullOrEmpty(_options.DefaultServiceName))
            {
                await HandleServiceAsEntryPoint(context, next);
            }
            else
            {
                await HandleServiceAsDependency(context, next);
            }
        }

        private async Task HandleServiceAsEntryPoint(HttpContext context, RequestDelegate next)
        {
            /*
             * Here, it's important to define a strategy for determining the name of the operation. 
             * A nice way to achieve this is to use attributes on Controller actions and then building 
             * a list of these at application startup to then be able to reference fast.
             * 
             * For this simple test, I'm just going to append the HTTP verb and the last part of the path to the service's name.
             */

            var operation = context.Request.Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();
            var operationId = _options.DefaultServiceName + "."  
                + context.Request.Method + ":" 
                + operation;

            Activity.Current.AddBaggage(Constants.OperationIdHeader, operationId);

            using var clientNameLogScope = _logger.BeginScope("{ClientName}", _options.DefaultServiceName);
            using var operationIdLogScope = _logger.BeginScope("{OperationId}", operationId);

            _logger.LogInformation($"Starting operation {operation}");

            await next(context);
        }

        private async Task HandleServiceAsDependency(HttpContext context, RequestDelegate next)
        {
            context.Request.Headers.TryGetValue(Constants.ClientNameHeader, out var clientName);

            if (clientName.Count != 1)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var details = new { Error = "A x-client-name header is required for this API" };
                await context.Response.WriteAsync(JsonSerializer.Serialize(details));
                return;
            }

            context.Request.Headers.TryGetValue(Constants.OperationIdHeader, out var operationId);

            if (operationId.Count != 1)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var details = new { Error = "A x-operation-id header is required for this API" };
                await context.Response.WriteAsync(JsonSerializer.Serialize(details));
                return;
            }

            Activity.Current.AddBaggage(Constants.OperationIdHeader, operationId[0]);

            using var clientNameLogScope = _logger.BeginScope("{ClientName}", clientName);
            using var operationIdLogScope = _logger.BeginScope("{OperationId}", operationId);

            _logger.LogInformation($"Starting operation {operationId}");

            await next(context);
        }
    }
}
