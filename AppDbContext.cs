using Microsoft.EntityFrameworkCore;
using RPG_BD.Models;

namespace RPG_BD.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Item> Items => Set<Item>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Item>()
            .HasIndex(i => i.Name)
            .IsUnique(); // impede nomes duplicados

        modelBuilder.Entity<Item>()
            .Property(i => i.Price)
            .HasPrecision(10, 2);
    }
}