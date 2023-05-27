namespace CT.Examples.B2cClientCredentials;

public interface ITestCommandApiClient
{
    Task SendSomeDataAsync(object data);
}

public class TestCommandApiClient : ITestCommandApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpRequestFactory _httpRequestFactory;

    public TestCommandApiClient(HttpClient httpClient, IHttpRequestFactory httpRequestFactory)
    {
        _httpClient = httpClient;
        _httpRequestFactory = httpRequestFactory;
    }

    public async Task SendSomeDataAsync(object data)
    {
        var requestMessage = await _httpRequestFactory.GetRequestMessageAsync(HttpMethod.Post, "testcommand", ClientApiType.TestCommandApi);
        requestMessage.Content = JsonContent.Create(data);

        var response = await _httpClient.SendAsync(requestMessage);

        response.EnsureSuccessStatusCode();
    }
}
