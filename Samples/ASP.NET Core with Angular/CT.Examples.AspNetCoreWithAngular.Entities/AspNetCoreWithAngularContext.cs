using Microsoft.EntityFrameworkCore;

namespace CT.Examples.AspNetCoreWithAngular.Entities;

public class AspNetCoreWithAngularContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }

    public AspNetCoreWithAngularContext(DbContextOptions<AspNetCoreWithAngularContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>(b => { b.HasKey(c => c.Id); b.Property(c => c.Id).ValueGeneratedOnAdd(); });
    }
}
