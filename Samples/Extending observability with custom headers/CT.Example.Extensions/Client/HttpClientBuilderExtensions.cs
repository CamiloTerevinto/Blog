using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CT.Example.Extensions.Client
{
    public static class HttpClientBuilderExtensions
    {
        public static IServiceCollection AddMicroserviceClient<TInterface, TClient>(this IServiceCollection services, Action<HttpClient> clientConfiguration, string serviceName)
            where TInterface : class
            where TClient : class, TInterface
        {
            services
                .TryAddSingleton<OperationIdDelegatingHandler>();

            services
                .AddHttpClient<TInterface, TClient>(client =>
                {
                    clientConfiguration(client);

                    client.DefaultRequestHeaders.Add(Constants.ClientNameHeader, serviceName);
                })
                .AddHttpMessageHandler<OperationIdDelegatingHandler>();

            return services;
        }

        private class OperationIdDelegatingHandler: DelegatingHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var activity = Activity.Current;

                if (activity != null)
                {
                    var operationId = activity.GetBaggageItem(Constants.OperationIdHeader);
                    
                    if (!string.IsNullOrEmpty(operationId))
                    {
                        request.Headers.Add(Constants.OperationIdHeader, operationId);
                    }
                }

                return base.SendAsync(request, cancellationToken);
            }
        }
    }
}
