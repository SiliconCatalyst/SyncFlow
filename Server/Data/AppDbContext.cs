using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Server.Models;

namespace Server.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<ProductEntry> ProductEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.EntryDateTime).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            });

            // Seed dummy data
            modelBuilder.Entity<ProductEntry>().HasData(
                new ProductEntry
                {
                    Id = 1,
                    UserName = "John Doe",
                    EntryDateTime = DateTime.Now,
                    ProductModel = "Widget Pro 3000",
                    PartNumber = "WP-3000-BLK",
                    Quantity = 50,
                    Price = 299.99m,
                    CreatedAt = DateTime.Now
                }
            );
        }
    }
}