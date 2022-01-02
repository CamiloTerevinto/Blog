using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CT.Examples.ExtendedObservability.Order.Api
{
    public class OrderDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
    }

    public interface IOrderCommandClient
    {
        Task CreateOrder(OrderDto orderDto);
    }

    public class OrderCommandClient : IOrderCommandClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OrderCommandClient> _logger;

        public OrderCommandClient(HttpClient httpClient, ILogger<OrderCommandClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task CreateOrder(OrderDto orderDto)
        {
            _logger.LogInformation("Sending order to OrderCommand");
            var response = await _httpClient.PostAsJsonAsync("/orders", orderDto);
            response.EnsureSuccessStatusCode();
        }
    }
}
