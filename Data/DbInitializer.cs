using Microsoft.AspNetCore.Identity;
using HotelApplication.Models;

namespace HotelApplication.Data
{
    public class DbInitializer
    {
        public static async Task SeedRolesAndUsersAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            string[] roles = new[] { "Admin", "Amministrativo", "AddettoPrenotazioni" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var users = new[]
            {
                new { UserName = "admin", Email = "admin@hotel.com", Password = "Admin123!", Role = "Admin" },
                new { UserName = "amministrativo", Email = "amministrativo@hotel.com", Password = "Amministrativo123!", Role = "Amministrativo" },
                new { UserName = "prenotazioni", Email = "prenotazioni@hotel.com", Password = "Prenotazioni123!", Role = "AddettoPrenotazioni" }
            };

            foreach (var userInfo in users)
            {
                var user = new ApplicationUser { UserName = userInfo.UserName, Email = userInfo.Email, EmailConfirmed = true };
                if (userManager.Users.All(u => u.UserName != user.UserName))
                {
                    var result = await userManager.CreateAsync(user, userInfo.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, userInfo.Role);
                    }
                }
            }
        }

        internal static async Task SeedRolesAndUsersAsync(IServiceProvider services)
        {
            throw new NotImplementedException();
        }
    }
}


