using AsisyaApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsisyaApi.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(e =>
            {
                e.HasKey(x => x.CategoryId);
                e.Property(x => x.CategoryName).HasMaxLength(100).IsRequired();
                e.HasIndex(x => x.CategoryName).IsUnique();
            });

            modelBuilder.Entity<Product>(e =>
            {
                e.HasKey(x => x.ProductId);
                e.Property(x => x.ProductName).HasMaxLength(200).IsRequired();
                e.Property(x => x.UnitPrice).HasPrecision(18, 2);
                e.HasIndex(x => x.CategoryId);
                e.HasIndex(x => x.ProductName);
                e.HasOne(x => x.Category)
                    .WithMany(x => x.Products)
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}