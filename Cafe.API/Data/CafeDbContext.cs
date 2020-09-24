using Microsoft.EntityFrameworkCore;
using Cafe.API.Models.Entities;

namespace Cafe.API.Data
{
    public class CafeDbContext : DbContext
    {
        public CafeDbContext(DbContextOptions<CafeDbContext> options)
            : base(options)
        {
            //ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<Models.Entities.Category> Categories { get; set; }
        public DbSet<Models.Entities.Product> Products { get; set; }
        public DbSet<Models.Entities.Client> Clients { get; set; }
        public DbSet<Models.Entities.ClientProduct> Sales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //// Product - Category (M : 1)
            //modelBuilder.Entity<Models.Entities.Product>()
            //    .HasOne<Models.Entities.Category>()
            //    .WithMany(c => c.Products)
            //    .HasForeignKey(p => p.CurrentCategoryId);


            // Client - Product (M : N)
            modelBuilder.Entity<Models.Entities.ClientProduct>()
                .HasKey(x => new { x.ClientId, x.ProductId });

            modelBuilder.Entity<Models.Entities.ClientProduct>()
                .HasOne(c => c.Client)
                .WithMany(s => s.ClientProducts)
                .HasForeignKey(c => c.ClientId);

            modelBuilder.Entity<Models.Entities.ClientProduct>()
                .HasOne(c => c.Product)
                .WithMany(s => s.ClientProducts)
                .HasForeignKey(c => c.ProductId);

            //modelBuilder.Entity<Category>().HasData(
            //    new Category()
            //    {
            //        Name = "Breakfast",
            //        Description = "The first meal of the day, usually eaten in the morning."
            //    },
            //    new Category()
            //    {
            //        Name = "Drinks",
            //        Description = "A liquid intended for human consumption"
            //    },
            //    new Category()
            //    {
            //        Name = "Soup",
            //        Description = "Be bowled over by our nourishing soups. Our warming recipes range from classic minestrone and vibrant tomato soup to blends such as celeriac, hazelnut and truffle."
            //    },
            //    new Category()
            //    {
            //        Name = "Pizza",
            //        Description = "Dish of Italian origin consisting of a flattened disk of bread dough topped with some combination of olive oil, oregano, tomato, olives, mozzarella or other cheese, and many other ingredients"
            //    },
            //    new Category()
            //    {
            //        Name = "Burger",
            //        Description = "Sandwich consisting of one or more cooked patties of ground meat, usually beef, placed inside a sliced bread roll or bun"
            //    },
            //    new Category()
            //    {
            //        Name = "Salad",
            //        Description = "Dish consisting of pieces of food in a mixture, with at least one raw ingredient"
            //    }                
            //);

            //base.OnModelCreating(modelBuilder);
        }
    }
}
