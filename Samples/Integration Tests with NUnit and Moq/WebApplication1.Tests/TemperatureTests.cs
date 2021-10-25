using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace WebApplication1.Tests
{
    [TestFixture]
    public class TemperatureTests : CustomBaseTest
    {
        [Test]
        public async Task GetTemperatureTest()
        {
            var expected = "test";
            ExternalServicesMock.TemperatureApiClient
                .Setup(x => x.GetTemperatureAsync())
                .ReturnsAsync(expected);

            var client = GetClient();

            var response = await client.GetAsync("/weatherforecast/temperature");
            var responseMessage = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(expected, responseMessage);
            ExternalServicesMock.TemperatureApiClient.Verify(x => x.GetTemperatureAsync(), Times.Once);
        }
    }
}
