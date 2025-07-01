using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Product.API.Models.Entities;
using System.IO;

namespace Product.API.Models
{
    public class ProductAPIDbContextFactory : IDesignTimeDbContextFactory<ProductAPIDbContext>
    {
        public ProductAPIDbContext CreateDbContext(string[] args)
        {
            // appsettings.json'u okuyacak şekilde ayarla
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ProductAPIDbContext>();
            var connectionString = configuration.GetConnectionString("SQLServer");

            optionsBuilder.UseSqlServer(connectionString);

            return new ProductAPIDbContext(optionsBuilder.Options);
        }
    }
}
