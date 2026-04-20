using Microsoft.EntityFrameworkCore;
using order_service.Models;

namespace order_service.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>().HasData(new Order
        {
            Id = 1,
            ProductIds = [1],
            OrderDate =  DateTime.Now,
        });
    }
}