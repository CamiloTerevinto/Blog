using Microsoft.EntityFrameworkCore;

namespace CT.Examples.SimplifiedCA.Entities;

public class SimplifiedCAContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<WeatherForecast> WeatherForecasts { get; set; }

    public SimplifiedCAContext(DbContextOptions<SimplifiedCAContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Some important stuff here :)
    }
}
