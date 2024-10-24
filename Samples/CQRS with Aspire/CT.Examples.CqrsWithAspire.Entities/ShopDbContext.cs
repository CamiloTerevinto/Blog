using Microsoft.EntityFrameworkCore;

namespace CT.Examples.CqrsWithAspire.Entities;

public class ShopDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    public ShopDbContext() { }
    public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options) { }
}
