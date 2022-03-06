using CT.Examples.OffloadToHangfire.Shared.Jobs;
using CT.Examples.OffloadToHangfire.Shared.Models;
using System.Text;

namespace CT.Examples.OffloadToHangfire.Processor.Services
{
    internal class CallApiExampleJob : ICallApiExampleJob
    {
        private readonly HttpClient _httpClient;

        public CallApiExampleJob(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Execute(CallApiExampleData data)
        {
            var response = await _httpClient.PostAsync(data.Url, new StringContent(data.SomeOtherImportantData, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
        }
    }
}
