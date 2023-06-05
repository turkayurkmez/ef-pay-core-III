using demoEFApp.Models;
using Microsoft.EntityFrameworkCore;

namespace demoEFApp.Data
{
    public class SampleDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\Mssqllocaldb;Initial Catalog=ProductsCatalogPC3;Integrated Security=True");
        }

    }
}
