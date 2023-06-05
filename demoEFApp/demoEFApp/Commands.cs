using demoEFApp.Data;
using demoEFApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace demoEFApp
{
    public static class Commands
    {
        public static bool CreateDatabaseAndAfterSeed(bool onlyIfNoDatabase)
        {
            using var dbContext = new SampleDbContext();
            if (onlyIfNoDatabase && (dbContext.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
            {
                return false;
            }

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            if (!dbContext.Products.Any())
            {
                WriteDataToDb(dbContext);
            }
            return true;
        }

        private static void WriteDataToDb(SampleDbContext dbContext)
        {
            var category = new Category { Name = "Elektronik" };
            var products = new List<Product>()
            {
                 new(){ Name = "Dell XPS 15", Category = category, Description="16 GB Ram 512 SSD", Price=40000},
                 new(){ Name = "Mac Book Pro", Category = category, Description="16 GB Ram 1TB SSD", Price=105000},
                 new(){ Name = "Converse", Category =new Category{ Name="Giyim"}, Description="16 GB Ram 512 SSD", Price=2500}


            };
            dbContext.Products.AddRange(products);

            dbContext.SaveChanges();
        }


        public static void ListProducts()
        {
            using var dbContext = new SampleDbContext();
            foreach (var product in dbContext.Products.AsNoTracking().Include(p => p.Category).AsEnumerable())
            {
                Console.WriteLine($"{product.Name} ({product.Category.Name}) --* {product.Price}  TL");

            }
        }

        public static void AddNewProductToCategory()
        {
            using var dbContext = new SampleDbContext();
            var category = dbContext.Categories.FirstOrDefault(p => p.Id == 1);
            category.Products.Add(new Product { Name = "Lenovo", Description = "Test", Price = 20000 });
            dbContext.SaveChanges();
        }

        public static void ChangeProductPrice()
        {
            using var dbContext = new SampleDbContext();
            var product = dbContext.Products.FirstOrDefault(p => p.Id == 1);
            product.Price *= 1.20M;

            dbContext.SaveChanges();
        }
    }
}
