using CT.Examples.SimplifiedCA;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSimpleConsole(options => options.IncludeScopes = true);

builder.Services.AddControllers();

builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddDomainServices();

var app = builder.Build();

app.MapControllers();

app.Run();
