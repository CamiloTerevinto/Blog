using CT.Examples.B2cClientCredentials;
using Microsoft.Identity.Client;

var builder = WebApplication.CreateBuilder(args);

var clientCredentialsConfiguration = builder.Configuration.GetSection("AzureB2C").Get<ClientCredentialsConfiguration>();

var confidentialClient = ConfidentialClientApplicationBuilder.Create(clientCredentialsConfiguration.ClientId)
    .WithClientSecret(clientCredentialsConfiguration.ClientSecret)
    .WithTenantId(clientCredentialsConfiguration.TenantId)
    .WithB2CAuthority(clientCredentialsConfiguration.Authority)
    .Build();

var scopes = clientCredentialsConfiguration.Apis.ToDictionary(x => Enum.Parse<ClientApiType>(x.Key, ignoreCase: true), x => x.Value.Scope);

builder.Services.AddSingleton<IHttpRequestFactory>(sp => new HttpRequestFactory(confidentialClient, scopes));

builder.Services.AddHttpClient<ITestQueryApiClient, TestQueryApiClient>(client =>
{
    client.BaseAddress = new Uri(clientCredentialsConfiguration.Apis[ClientApiType.TestQueryApi.ToString()].Url);
});

builder.Services.AddHttpClient<ITestCommandApiClient, TestCommandApiClient>(client =>
{
    client.BaseAddress = new Uri(clientCredentialsConfiguration.Apis[ClientApiType.TestCommandApi.ToString()].Url);
});

var app = builder.Build();

app.MapGet("/test", async (ITestCommandApiClient testCommandApiClient, ITestQueryApiClient testQueryApiClient) =>
{
    var data = new { test = "yes this a test!" };
    await testCommandApiClient.SendSomeDataAsync(data);

    var result = await testQueryApiClient.GetDataAsync();

    return Results.Ok(result);
})
.WithName("test");

app.Run();
