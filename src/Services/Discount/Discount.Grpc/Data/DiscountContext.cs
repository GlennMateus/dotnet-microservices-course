using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountContext : DbContext
{
    public DbSet<Cupon> Cupons { get; set; } = default!;
    public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cupon>()
            .HasData(
                new Cupon
                {
                    Id = 1,
                    ProductName = "IPhone",
                    Description = "IPhone Discount",
                    Amount = 150
                },
                new Cupon
                {
                    Id = 2,
                    ProductName = "Samsung 10",
                    Description = "Samsung 10 Discount",
                    Amount = 100
                }
            );
    }
}
