using CT.Examples.AspNetCoreWithAngular;
using CT.Examples.AspNetCoreWithAngular.Domain.Configuration;
using CT.Examples.AspNetCoreWithAngular.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSimpleConsole(options => options.IncludeScopes = true);

builder.Services.AddControllers(c => c.Filters.Add(new AuthorizeFilter()));

var authenticationOptions = new AuthenticationOptions(builder.Configuration["Authentication:JwtKey"]);

builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddDomainServices(builder.Configuration, authenticationOptions);

builder.Services.AddSingleton<IPasswordHasher<Account>, PasswordHasher<Account>>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "Portal",
            ValidAudience = "Portal",
            IssuerSigningKey = authenticationOptions.Key,
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

app.Services.CreateScope().ServiceProvider.GetRequiredService<AspNetCoreWithAngularContext>().Database.EnsureCreated();

app.UseDefaultFiles()
   .UseStaticFiles();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
