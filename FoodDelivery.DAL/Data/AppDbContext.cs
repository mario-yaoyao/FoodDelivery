using FoodDelivery.Models.Models;
using FoodDelivery.Shared.Contracts.Enums;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.DAL.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Rider> Riders { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<DeliveryAssignment> DeliveryAssignments { get; set; }
        public DbSet<Analytic> Analytics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.FullName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<Rider>()
                .HasIndex(u => u.FullName)
                .IsUnique();

            modelBuilder.Entity<Restaurant>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<Order>()
                .Property(e => e.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(e => e.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(e => e.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MenuItem>()
                .Property(e => e.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Analytic>()
                .Property(a => a.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Rider)
                .WithMany(r => r.Orders)
                .HasForeignKey(o => o.RiderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Analytic>()
                .HasOne(a => a.Order)
                .WithMany(o => o.Analytics)
                .HasForeignKey(a => a.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Analytic>()
                .HasOne(a => a.Customer)
                .WithMany(u => u.Analytics)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Analytic>()
                .HasOne(a => a.Restaurant)
                .WithMany(r => r.Analytics)
                .HasForeignKey(a => a.RestaurantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Analytic>()
                .HasOne(a => a.Rider)
                .WithMany(r => r.Analytics)
                .HasForeignKey(a => a.RiderId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FullName = "Mario Yaoyao II",
                    Email = "Mario.Yaoyao@ext.essilor.com",
                    PhoneNumber = "09171234567",
                    CreatedAt = new DateTime(2026, 10, 6)
                });

            modelBuilder.Entity<Restaurant>().HasData(
                new Restaurant
                {
                    Id = 1,
                    Name = "Jollibee Alabang",
                    Address = "Alabang, Muntinlupa",
                    PhoneNumber = "09181234567",
                    CreatedAt = new DateTime(2026, 10, 6)
                });

            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem
                {
                    Id = 1,
                    RestaurantId = 1,
                    Name = "Chickenjoy",
                    Description = "Fried Chicken Meal",
                    Price = 120.00m,
                    IsAvailable = true,
                    CreatedAt = new DateTime(2026, 10, 6)
                });

            modelBuilder.Entity<Rider>().HasData(
                new Rider
                {
                    Id = 1,
                    FullName = "Juan Dela Cruz",
                    Status = RiderStatus.Available,
                    CreatedAt = new DateTime(2026, 10, 6)
                },
                new Rider
                {
                    Id = 2,
                    FullName = "Bentley Sanders",
                    Status = RiderStatus.Available,
                    CreatedAt = new DateTime(2026, 10, 6)
                },
                new Rider
                {
                    Id = 3,
                    FullName = "Walter Bond",
                    Status = RiderStatus.Available,
                    CreatedAt = new DateTime(2026, 10, 6)
                },
                new Rider
                {
                    Id = 4,
                    FullName = "Franklin Salazar",
                    Status = RiderStatus.Available,
                    CreatedAt = new DateTime(2026, 10, 6)
                });
        }
    }
}
