using Microsoft.Identity.Client;
using System.Net.Http.Headers;

namespace CT.Examples.B2cClientCredentials;

public interface IHttpRequestFactory
{
    Task<HttpRequestMessage> GetRequestMessageAsync(HttpMethod method, string url, ClientApiType clientApiType);
}

public class HttpRequestFactory : IHttpRequestFactory
{
    private readonly IConfidentialClientApplication _confidentialClientApplication;
    private readonly Dictionary<ClientApiType, string> _scopes;

    public HttpRequestFactory(IConfidentialClientApplication confidentialClientApplication, Dictionary<ClientApiType, string> scopes)
    {
        _confidentialClientApplication = confidentialClientApplication;
        _scopes = scopes;
    }

    public async Task<HttpRequestMessage> GetRequestMessageAsync(HttpMethod method, string url, ClientApiType clientApiType)
    {
        var requestMessage = new HttpRequestMessage(method, url);

        await AddAuthorizationHeaderAsync(requestMessage, clientApiType);

        return requestMessage;
    }

    private async Task AddAuthorizationHeaderAsync(HttpRequestMessage requestMessage, ClientApiType clientApiType)
    {
        var authenticationResult = await _confidentialClientApplication.AcquireTokenForClient(new[] { _scopes[clientApiType] }).ExecuteAsync();

        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
    }
}
