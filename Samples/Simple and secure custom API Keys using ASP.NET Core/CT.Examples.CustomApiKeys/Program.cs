using CT.Examples.CustomApiKeys.Services;
using TerevintoSoftware.AspNetCore.Authentication.ApiKeys;
using TerevintoSoftware.AspNetCore.Authentication.ApiKeys.Abstractions;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSimpleConsole(options => options.IncludeScopes = true);

builder.Services.AddControllers();

builder.Services.AddLocalization(opt => opt.ResourcesPath = "Resources");

builder.Services.AddSwaggerGen(setup =>
{
    setup.AddApiKeySupport();
});

builder.Services
    .AddDefaultApiKeyGenerator(new ApiKeyGenerationOptions
    {
        KeyPrefix = "CT-",
        GenerateUrlSafeKeys = true,
        LengthOfKey = 36
    })
    .AddDefaultClaimsPrincipalFactory()
    .AddApiKeys(options => { options.InvalidApiKeyLog = (LogLevel.Warning, "Someone attempted to use an invalid API Key: {ApiKey}"); }, true)
    .AddSingleton<IClientsService, InMemoryClientsService>()
    .AddMemoryCache()
    .AddSingleton<IApiKeysCacheService, CacheService>();

var app = builder.Build();

app.UseRequestLocalization(opt =>
{
    opt.AddSupportedCultures("en", "es");
    opt.AddSupportedUICultures("en", "es");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
