namespace CT.Examples.CqrsWithAspire.Tests.Internal;

public class AspireBaseTest
{
    private AspireResourcesFactory _testBase;

    public HttpClient CommandApiClient { get => _testBase.CommandApiClient; }
    public HttpClient QueryApiClient { get => _testBase.QueryApiClient; }

    [OneTimeSetUp]
    public async Task SetUp()
    {
        _testBase = await AspireResourcesFactory.StartAsync();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await _testBase.DisposeAsync();
    }
}
