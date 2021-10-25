using System.Net.Http;
using System.Threading.Tasks;

namespace WebApplication1
{
    public interface ITemperatureApiClient
    {
        Task<string> GetTemperatureAsync();
    }

    public class TemperatureApiClient : ITemperatureApiClient
    {
        private readonly HttpClient _httpClient;

        public TemperatureApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<string> GetTemperatureAsync()
        {
            return Task.FromResult("Temperature came from a real service");
        }
    }
}
