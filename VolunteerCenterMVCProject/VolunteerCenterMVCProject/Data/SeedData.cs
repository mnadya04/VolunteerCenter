
using Microsoft.AspNetCore.Identity;
using VolunteerCenterMVCProject.Common;
using VolunteerCenterMVCProject.Data;
using VolunteerCenterMVCProject.Models;

namespace HousekeeperApp.Data.Seeding
{
    public class SeedData
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            // Seed roles
            await SeedRolesAsync(roleManager);

            // Seed admin user
            await SeedUserAsync(dbContext, userManager, roleManager, "Admin", "Admin", "admin@gmail.com", "adminPass_01", Constants.AdminRole);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            await SeedRoleAsync(roleManager, Constants.AdminRole);
            await SeedRoleAsync(roleManager, Constants.VolunteerRole);
        }

        private static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }

        private static async Task SeedUserAsync(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, string firstName, string lastName, string email, string password, string roleName)
        {
            User user = await userManager.FindByNameAsync(email);
            if (user == null)
            {
                IdentityResult result = await userManager.CreateAsync(
                    new User()
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        UserName = email,
                        Email = email,
                    }, password);

                if (!result.Succeeded)
                {
                    throw new Exception();
                }
            }

            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                throw new Exception($"Role {roleName} does not exist.");
            }

            var isInRole = await userManager.IsInRoleAsync(user, roleName);
            if (!isInRole)
            {
                var result = await userManager.AddToRoleAsync(user, roleName);
                if (!result.Succeeded)
                {
                    throw new Exception();
                }
            }
            else
            {
                Console.WriteLine($"User {email} is already in the {roleName} role.");
            }
        }

         
    }
}
