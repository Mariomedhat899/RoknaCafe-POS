using Microsoft.EntityFrameworkCore;
using Rokna.Domain.Entities;

namespace Rokna.Infrastructure.Data;

public class RoknaDbContext : DbContext
{
    public RoknaDbContext(DbContextOptions<RoknaDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Name)
                  .HasMaxLength(50);
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.Property(e => e.Name)
                  .HasMaxLength(50);
        });
    }
}