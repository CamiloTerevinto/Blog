using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CT.Examples.ExtendedObservability.Order.Command
{
    public class OrderDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
    }

    public class CustomerDto
    {
        public Guid Id { get; set; }
    }

    public interface ICustomerQueryClient
    {
        Task<CustomerDto> GetCustomerData(string name);
    }

    public class CustomerQueryClient : ICustomerQueryClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CustomerQueryClient> _logger;

        public CustomerQueryClient(HttpClient httpClient, ILogger<CustomerQueryClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<CustomerDto> GetCustomerData(string name)
        {
            _logger.LogInformation("Sending order to OrderCommand");

            return await _httpClient.GetFromJsonAsync<CustomerDto>($"/customers/{name}");
        }
    }
}
