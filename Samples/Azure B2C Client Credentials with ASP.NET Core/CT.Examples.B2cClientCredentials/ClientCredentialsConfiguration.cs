using System.Text.Json.Serialization;

namespace CT.Examples.B2cClientCredentials;

public class ClientCredentialsConfiguration
{
    public string TenantId { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string Authority { get; set; }

    public Dictionary<string, ClientCredentialsApiConfiguration> Apis { get; set; } = new(StringComparer.InvariantCultureIgnoreCase);
}

public class ClientCredentialsApiConfiguration
{
    public string Url { get; set; }
    public string Scope { get; set; }
}

public enum ClientApiType
{
    TestQueryApi,
    TestCommandApi
}