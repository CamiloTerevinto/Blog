using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = configuration.GetSection("Auth:Authority").Get<string>();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddControllers(opt => { opt.Filters.Add(new AuthorizeFilter()); });
builder.Services.AddSwaggerGen(options =>
{
    var scheme = new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(configuration.GetSection("Auth:Swagger:AuthorizationUrl").Get<string>()),
                TokenUrl = new Uri(configuration.GetSection("Auth:Swagger:TokenUrl").Get<string>())
            }
        },
        Type = SecuritySchemeType.OAuth2
    };

    options.AddSecurityDefinition("OAuth", scheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { 
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Id = "OAuth", Type = ReferenceType.SecurityScheme }
            }, 
            new List<string> { } 
        }
    });
});

var app = builder.Build();

app.UseStaticFiles();

app.UseSwagger()
    .UseSwaggerUI(options =>
    {
        options.EnablePersistAuthorization();
        options.OAuthClientId("api-swagger");
        options.OAuthScopes("profile", "openid", "api");
        options.OAuthUsePkce();

        options.InjectStylesheet("/content/swagger-extras.css");
    });

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
