using Microsoft.EntityFrameworkCore;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Data
{
    public class ProductCatalogueDataContext : DbContext
    {
        public ProductCatalogueDataContext(DbContextOptions<ProductCatalogueDataContext> options)
            : base (options)
        {

        }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductOption> ProductOptions { get; set; }
    }
}
