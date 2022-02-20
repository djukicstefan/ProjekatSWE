using DAL.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace MNP_main.Helpers
{
    public static class MyIdentityDataInitializer
    {
        public static void SeedData(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<User> userManager)
        {
            EnsureAdminUserCreated(userManager);
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            EnsureRoleCreatedAsync(roleManager, "Admin");
            EnsureRoleCreatedAsync(roleManager, "DeliveryGuy");
            EnsureRoleCreatedAsync(roleManager, "Worker");
        }

        private static bool EnsureRoleCreatedAsync(RoleManager<IdentityRole> manager, string roleName)
        {
            if (!manager.RoleExistsAsync(roleName).Result)
               return manager.CreateAsync(new IdentityRole(roleName)).Result.Succeeded;
            return true;
        }

        private static bool EnsureAdminUserCreated(UserManager<User> userManager)
        {
            var adminUser = userManager.FindByNameAsync("admin").Result;

            if (adminUser == null)
            {
                var user = new User
                {
                    UserName = "admin",
                    Email = "admin@mnp.com",
                    EmailConfirmed = true
                };

                if (userManager.CreateAsync(user, "Admin123!").Result.Succeeded)
                    return userManager.AddToRoleAsync(user, "Admin").Result.Succeeded;
                return false;
            }
            return true;
        }
    }
}
