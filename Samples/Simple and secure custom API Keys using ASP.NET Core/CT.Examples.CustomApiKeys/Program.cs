using CT.Examples.CustomApiKeys.Services;
using Microsoft.OpenApi.Models;
using TerevintoSoftware.AspNetCore.Authentication.ApiKeys;
using TerevintoSoftware.AspNetCore.Authentication.ApiKeys.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSimpleConsole(options => options.IncludeScopes = true);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(setup =>
{
    setup.AddSecurityDefinition(ApiKeyAuthenticationOptions.DefaultScheme, new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = ApiKeyAuthenticationOptions.HeaderName,
        Type = SecuritySchemeType.ApiKey
    });

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = ApiKeyAuthenticationOptions.DefaultScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services
    .AddDefaultApiKeyGenerator(new ApiKeyGenerationOptions
    {
        KeyPrefix = "CT-",
        ByteCountToGenerate = 32,
        GenerateUrlSafeKeys = true,
        LengthOfKey = 36
    })
    .AddDefaultClaimsPrincipalFactory()
    .AddApiKeys(options => { options.InvalidApiKeyLog = (LogLevel.Warning, "Someone attempted to use an invalid API Key: {ApiKey}"); }, true)
    .AddSingleton<IClientsService, InMemoryClientsService>()
    .AddMemoryCache()
    .AddSingleton<IApiKeysCacheService, CacheService>();


var app = builder.Build();

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
