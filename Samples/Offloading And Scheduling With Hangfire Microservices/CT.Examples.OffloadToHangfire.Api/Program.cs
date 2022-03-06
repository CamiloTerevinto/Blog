using CT.Examples.OffloadToHangfire.Api.Services;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IApiExampleService, ApiExampleService>();

builder.Services.AddHangfire(configuration =>
{
    var currentPath = builder.Environment.ContentRootPath;
    var solutionPath = Path.GetFullPath(Path.Combine(currentPath, ".."));
    var pathToDb = Path.Combine(solutionPath, "CT.Examples.OffloadToHangfire.Processor", "Hangfire.mdf");

    configuration.UseSqlServerStorage($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"{pathToDb}\";Integrated Security=True");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();
app.UseAuthorization();

app.MapControllers();

app.Run();
