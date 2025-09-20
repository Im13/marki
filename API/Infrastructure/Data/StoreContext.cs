using System.Reflection;
using System.Text.Json;
using Core;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Entities.RecommendedData;
using Core.Entities.ShopeeOrder;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<ShopeeOrder> ShopeeOrders { get; set; }
        public DbSet<ShopeeProduct> ShopeeProducts { get; set; }
        public DbSet<ProductOptions> ProductOptions { get; set; }
        public DbSet<ProductOptionValues> ProductOptionValues { get; set; }
        public DbSet<ProductSKUs> ProductSKUs { get; set; }
        public DbSet<ProductSKUValues> ProductSKUValues { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OfflineOrder> OfflineOrders { get; set; }
        public DbSet<OfflineOrderSKUs> OfflineOrderSKUs { get; set; }
        public DbSet<OfflineOrderStatus> OrderStatus { get; set; }
        public DbSet<SlideImage> SlideImages { get; set; }
        public DbSet<RevenueSummary> RevenueSummaries { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationUser> NotificationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Product>().Property(p => p.IsDeleted).HasDefaultValue(false);
            modelBuilder.Entity<Customer>().Property(c => c.IsDeleted).HasDefaultValue(false);
            modelBuilder.Entity<Order>().Property(o => o.Source).HasConversion<string>();
            modelBuilder.Entity<RevenueSummary>().HasIndex(r => r.Date).IsUnique();

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever(); // Manual assignment from Identity
                entity.Property(e => e.DisplayName).HasMaxLength(200);
            });

            // UserProfile configuration
            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.ToTable("UserProfiles");
                entity.HasOne(p => p.User)
                      .WithOne(u => u.Profile)
                      .HasForeignKey<UserProfile>(p => p.UserId);

                entity.Property(e => e.PreferredCategories)
                      .HasConversion(
                          v => string.Join(',', v),
                          v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse).ToList());

                entity.Property(e => e.PreferredTags)
                      .HasConversion(
                          v => string.Join('|', v),
                          v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList());
            });

            // UserInteraction configuration
            modelBuilder.Entity<UserInteraction>(entity =>
            {
                entity.ToTable("UserInteractions");
                entity.HasOne(i => i.User)
                      .WithMany(u => u.Interactions)
                      .HasForeignKey(i => i.UserId);

                entity.HasOne(i => i.Product)
                      .WithMany(p => p.Interactions)
                      .HasForeignKey(i => i.ProductId);

                entity.HasIndex(i => new { i.UserId, i.ProductId, i.Type });
                entity.HasIndex(i => i.Timestamp);
            });

            // Recommendation configuration
            modelBuilder.Entity<Recommendation>(entity =>
            {
                entity.ToTable("Recommendations");
                entity.HasOne(r => r.User)
                      .WithMany()
                      .HasForeignKey(r => r.UserId);

                entity.HasOne(r => r.Product)
                      .WithMany()
                      .HasForeignKey(r => r.ProductId);

                entity.HasIndex(r => new { r.UserId, r.Score });
                entity.HasIndex(r => r.ExpiresAt);
            });

            // ProductFeatures configuration
            modelBuilder.Entity<ProductFeatures>(entity =>
            {
                entity.ToTable("ProductFeatures");
                entity.HasKey(e => e.ProductId);
                
                entity.Property(e => e.CategorySimilarity)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                          v => JsonSerializer.Deserialize<Dictionary<string, double>>(v, (JsonSerializerOptions)null) ?? new Dictionary<string, double>())
                      .HasColumnType("TEXT"); 
            });

            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));

                    foreach (var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                    }
                }
            }
        }
    }
}