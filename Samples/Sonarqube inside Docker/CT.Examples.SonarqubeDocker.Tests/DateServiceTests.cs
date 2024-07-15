using CT.Examples.SonarqubeDocker.Api.Services;

namespace CT.Examples.SonarqubeDocker.Tests;

public class Tests
{
    private DateService _dateService;

    [SetUp]
    public void Setup()
    {
        _dateService = new DateService();
    }

    [Test]
    public async Task GetCurrentDateAsync_ReturnsValueOnSuccess()
    {
        var result = await _dateService.GetCurrentDateAsync();

        Assert.That(result, Is.Not.Null);
    }
}