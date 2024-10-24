using CT.Examples.CqrsWithAspire.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddNpgsql<ShopDbContext>(builder.Configuration.GetConnectionString("Shop"));

var app = builder.Build();

app.MapControllers();

await app.RunAsync();

namespace CT.Examples.CqrsWithAspire.Query.Api
{
    public class Marker;
}