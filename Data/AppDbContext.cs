using Microsoft.EntityFrameworkCore;
using RainbowProject.Models.Domain;

namespace ColorApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<ColorItem> Colors => Set<ColorItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure User table
            modelBuilder.Entity<User>(e =>
            {
                e.HasIndex(u => u.Email).IsUnique();           // ensures each email appears only once
                e.Property(u => u.UserName).HasMaxLength(64);  // limits username length
                e.Property(u => u.Email).IsRequired().HasMaxLength(128); // required email
            });

            // Configure ColorItem table
            modelBuilder.Entity<ColorItem>(e =>
            {
                e.Property(c => c.Hex).IsRequired().HasMaxLength(9); // e.g. "#RRGGBB"

                // define the relationship between ColorItem and User
                e.HasOne(c => c.User)
                 .WithMany() // (no Colors navigation in User, but you could change to .WithMany(u => u.Colors))
                 .HasForeignKey(c => c.UserId)
                 .OnDelete(DeleteBehavior.Cascade); // delete colors when the user is deleted
            });
        }

    }
}
