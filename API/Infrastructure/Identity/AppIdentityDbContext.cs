using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser, AppRole, int, 
        IdentityUserClaim<int>, 
        AppUserRole,
        IdentityUserLogin<int>, 
        IdentityRoleClaim<int>, 
        IdentityUserToken<int>>
    {
        public AppIdentityDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(b => {
                b.HasKey(u => u.Id);
                b.HasMany(ur => ur.UserRoles).WithOne(u => u.User).HasForeignKey(ur => ur.UserId).IsRequired();
                b.HasOne(u => u.Address).WithOne(x => x.AppUser).HasForeignKey<Address>(x => x.AppUserId).IsRequired();
            });

            builder.Entity<AppRole>(b => {
                b.HasKey(r => r.Id);
                b.HasMany(ur => ur.UserRoles).WithOne(r => r.Role).HasForeignKey(ur => ur.RoleId).IsRequired();
            });
        }
    }
}