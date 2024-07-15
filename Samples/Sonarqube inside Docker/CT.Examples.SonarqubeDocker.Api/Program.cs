using CT.Examples.SonarqubeDocker.Api.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IDateService, DateService>();

var app = builder.Build();

app.UseHttpsRedirection();

app
    .MapGet("/dateforecast", async ([FromServices] IDateService dateService) =>
    {
        return Results.Ok(await dateService.GetCurrentDateAsync());
    })
    .WithName("GetDateForecast");

app.Run();

