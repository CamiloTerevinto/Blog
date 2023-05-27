namespace CT.Examples.B2cClientCredentials;

public interface ITestQueryApiClient
{
    Task<string> GetDataAsync();
}

public class TestQueryApiClient : ITestQueryApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpRequestFactory _httpRequestFactory;

    public TestQueryApiClient(HttpClient httpClient, IHttpRequestFactory httpRequestFactory)
    {
        _httpClient = httpClient;
        _httpRequestFactory = httpRequestFactory;
    }

    public async Task<string> GetDataAsync()
    {
        var requestMessage = await _httpRequestFactory.GetRequestMessageAsync(HttpMethod.Get, "testquery", ClientApiType.TestQueryApi);

        var response = await _httpClient.SendAsync(requestMessage);

        return await response.Content.ReadAsStringAsync();
    }
}
