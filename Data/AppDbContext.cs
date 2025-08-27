using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProductInventory.Api.Models;

namespace ProductInventory.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(b =>
            {
                b.Property(p => p.Name).HasMaxLength(100).IsRequired();
                b.Property(p => p.Price).HasPrecision(18, 2);
            });


            modelBuilder.Entity<Category>(b =>
            {
                b.Property(c => c.Name).HasMaxLength(50).IsRequired();
            });


            // Seed a few categories
            modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics" },
            new Category { Id = 2, Name = "Groceries" },
            new Category { Id = 3, Name = "Clothing" }
            );
        }
    }
}
