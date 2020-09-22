using Microsoft.EntityFrameworkCore;

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

            // Configure Cascade Delete using Fluent API
            //modelBuilder.Entity<Models.Entities.Category>()
            //    .HasMany<Models.Entities.Product>(c => c.Products)
            //    .WithOne(s => s.Category)
            //    .HasForeignKey(s => s.CategoryId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
