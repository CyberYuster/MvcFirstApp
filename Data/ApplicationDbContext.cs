using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MvcDatabaseApp.Models;

namespace MvcDatabaseApp.Data
{
    public class ApplicationDbContext : DbContext

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Configure Entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(p=>p.Name).IsUnique();
                entity.Property(p => p.Price).HasPrecision(18,2);
                entity.Property(p => p.CreateDate).HasDefaultValueSql("GETUTCDATE()");
            });
            //Seed initial data
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Price = 999.99m, Description = "High performance HP", Category = "Accessories", CreateDate = DateTime.UtcNow, isActive = true },
                new Product { Id = 2, Name = "Hp", Price = 78.99m, Description = "well definition", Category = "Electronics", CreateDate = DateTime.UtcNow, isActive = true }
                );
        }
    }
}
