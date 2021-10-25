using System.Threading.Tasks;

namespace WebApplication1
{
    public interface ITemperatureService
    {
        Task<string> GetTemperatureFromExternalApiAsync();
    }

    public class TemperatureService : ITemperatureService
    {
        private readonly ITemperatureApiClient _temperatureApiClient;

        public TemperatureService(ITemperatureApiClient temperatureApiClient)
        {
            _temperatureApiClient = temperatureApiClient;
        }

        public async Task<string> GetTemperatureFromExternalApiAsync()
        {
            return await _temperatureApiClient.GetTemperatureAsync();
        }
    }
}
