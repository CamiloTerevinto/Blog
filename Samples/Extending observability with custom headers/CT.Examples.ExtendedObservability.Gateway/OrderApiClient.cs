using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CT.Examples.ExtendedObservability.Gateway
{
    public class OrderDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
    }

    public interface IOrderApiClient
    {
        Task CreateOrder(OrderDto orderDto);
    }

    public class OrderApiClient : IOrderApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OrderApiClient> _logger;

        public OrderApiClient(HttpClient httpClient, ILogger<OrderApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task CreateOrder(OrderDto orderDto)
        {
            _logger.LogInformation("Sending order to OrderApi");
            
            var response = await _httpClient.PostAsJsonAsync("/orders", orderDto);
            
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(content);
            }
        }
    }
}
