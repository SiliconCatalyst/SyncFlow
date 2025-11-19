using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductEntry> ProductEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ProductEntry configurations
            modelBuilder.Entity<ProductEntry>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ProductModel)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.PartNumber)
                    .IsRequired()
                    .HasMaxLength(100);

                // Price value can take 18 digits, with 2 decimal points
                entity.Property(e => e.Price)
                    .HasPrecision(18, 2);
            });
        }
    }
}