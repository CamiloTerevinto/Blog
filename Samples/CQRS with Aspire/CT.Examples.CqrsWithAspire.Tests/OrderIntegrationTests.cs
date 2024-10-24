using CT.Examples.CqrsWithAspire.Command.Api.Controllers;
using CT.Examples.CqrsWithAspire.Tests.Internal;
using System.Net.Http.Json;

namespace CT.Examples.CqrsWithAspire.Tests;

public class OrderIntegrationTests : AspireBaseTest
{
    [Test]
    public async Task OrderCanBeCreated()
    {
        // Arrange
        var commandData = new CreateOrderCommand
        {
            CustomerName = "Test"
        };

        // Act
        var commandResponse = await CommandApiClient.PostAsJsonAsync("/orders", commandData);
        var commandDto = await commandResponse.Content.ReadFromJsonAsync<OrderDto>();

        var queryResponse = await QueryApiClient.GetAsync($"/orders/{commandDto.Id}");
        var queryDto = await queryResponse.Content.ReadFromJsonAsync<OrderDto>();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(commandResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(queryResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(commandDto.Id, Is.EqualTo(queryDto.Id));
            Assert.That(commandDto.CustomerName, Is.EqualTo(commandData.CustomerName));
            Assert.That(commandDto.CustomerName, Is.EqualTo(queryDto.CustomerName));
        });
    }
}
