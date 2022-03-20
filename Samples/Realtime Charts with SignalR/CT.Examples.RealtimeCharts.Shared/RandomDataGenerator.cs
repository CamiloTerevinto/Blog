using Azure;
using Azure.Messaging.EventGrid;

namespace CT.Examples.RealtimeCharts.Shared
{
    public static class RandomDataGenerator
    {
        public static async Task GenerateDataAsync()
        {
            var eventGridClient = new EventGridPublisherClient(
                new Uri(MainFunctionConfiguration.EventGridTopic),
                new AzureKeyCredential(MainFunctionConfiguration.EventGridKey));

            var random = new Random();

            for (int i = 1; i < 11; i++)
            {
                var data = new DataPoint[]
                {
                    new() { X = i, Y = random.Next(1, 100) },
                    new() { X = i, Y = random.Next(1, 100) }
                };

                await eventGridClient.SendEventAsync(new EventGridEvent("test", "test", "1.0", data));
                await Task.Delay(500); // Allow some time between data points
            }
        }
    }
}
