using System.Reflection;
using Core;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;
using Core.Entities.Recommendation;
using Core.Entities.ShopeeOrder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>,
        AppUserRole,
        IdentityUserLogin<int>,
        IdentityRoleClaim<int>,
        IdentityUserToken<int>>
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
        
        // Recommendation DbSets
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<SessionInteraction> SessionInteractions { get; set; }
        public DbSet<ProductCoOccurrence> ProductCoOccurrences { get; set; }
        public DbSet<ProductTrending> ProductTrendings { get; set; }
        
        // Identity DbSets
        public DbSet<Core.Entities.Identity.Address> Addresses { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Identity configuration
            modelBuilder.Entity<AppUser>(b =>
            {
                b.HasKey(u => u.Id);
                b.HasMany(ur => ur.UserRoles).WithOne(u => u.User).HasForeignKey(ur => ur.UserId).IsRequired();
                b.HasOne(u => u.Address).WithOne(x => x.AppUser).HasForeignKey<Core.Entities.Identity.Address>(x => x.AppUserId).IsRequired();
            });

            modelBuilder.Entity<AppRole>(b =>
            {
                b.HasKey(r => r.Id);
                b.HasMany(ur => ur.UserRoles).WithOne(r => r.Role).HasForeignKey(ur => ur.RoleId).IsRequired();
            });

            modelBuilder.Entity<AppUserRole>(b =>
            {
                b.HasKey(ur => new { ur.UserId, ur.RoleId });

                b.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                b.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            // Store configuration
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Product>().Property(p => p.IsDeleted).HasDefaultValue(false);
            modelBuilder.Entity<Customer>().Property(c => c.IsDeleted).HasDefaultValue(false);
            modelBuilder.Entity<Order>().Property(o => o.Source).HasConversion<string>();
            modelBuilder.Entity<RevenueSummary>().HasIndex(r => r.Date).IsUnique();

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