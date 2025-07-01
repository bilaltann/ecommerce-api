
using Microsoft.EntityFrameworkCore;

namespace Product.API.Models.Entities;

public class ProductAPIDbContext : DbContext
{
    public ProductAPIDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
   
}
