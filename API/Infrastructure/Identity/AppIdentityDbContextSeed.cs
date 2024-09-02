using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) 
        {
            if(await userManager.Users.AnyAsync()) return;
            
            var user = new AppUser
            {
                DisplayName = "Bob",
                Email = "bob@test.com",
                UserName = "bob@test.com",
                Address = new Address 
                {
                    Fullname = "Bob Muller",
                    CityOrProvinceId = 1,
                    DistrictId = 1,
                    Street = "Hong Ha",
                    WardId = 1,
                }
            };

            var admin = new AppUser
            {
                DisplayName = "Marki",
                Email = "admin@marki.vn",
                UserName = "marki",
                Address = new Address 
                {
                    Fullname = "Marki Admin",
                    CityOrProvinceId = 1,
                    DistrictId = 1,
                    Street = "Hong Ha",
                    WardId = 1,
                }
            };

            var roles = new List<AppRole>
            {
                new AppRole {Name = "Customer"},
                new AppRole {Name = "SuperAdmin"},
                new AppRole {Name = "Admin"},
                new AppRole {Name = "Employee"}
            };
            
            foreach(var role in roles) 
            {
                await roleManager.CreateAsync(role);
            }

            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRolesAsync(user, new[] {"Customer"});

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] {"Admin", "SuperAdmin"});
        }
    }
}