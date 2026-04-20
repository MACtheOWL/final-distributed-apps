using Microsoft.EntityFrameworkCore;
using product_service.Models;

namespace product_service.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().HasData(new Product
        {
            Id = 1,
            Name = "Samsung Galaxy S5",
            Description = "Samsung Galaxy S5",
            Price = 100.00m,
        });
    }
}