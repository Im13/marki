using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Extensions
{
    public static class UserManagerExtensions
{
    public static async Task<List<AppUser>> GetUsersInRolesAsync(this UserManager<AppUser> userManager, params string[] roles)
    {
        var users = new List<AppUser>();

        foreach (var role in roles)
        {
            var inRole = await userManager.GetUsersInRoleAsync(role);
            users.AddRange(inRole);
        }

        return users.Distinct().ToList();
    }
}

}