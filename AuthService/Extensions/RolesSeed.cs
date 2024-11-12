using AuthService.Data;
using AuthService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Extensions
{
    public class RolesSeed
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AuthDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), "AuthDbContext is not registered in the service provider.");
            }

            // Seed roles
            string[] roles = new string[] { "Admin", "User" };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(role));

                    if (!roleResult.Succeeded)
                    {
                        throw new Exception($"Failed to create role {role}: " +
                            string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                    }
                }
            }

            // Ensure that changes are saved before assigning roles to users
            await context.SaveChangesAsync();
            var user = await userManager.FindByNameAsync("admin");

            // Create admin user if it doesn't exist yet (with default password "admin")
            if (user == null)
            {
                user = new User
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                    NormalizedUserName = "ADMIN",
                    EmailConfirmed = true,
                    PhoneNumber = "1234567890",
                    name = "Admin User"
                };

                // Use UserManager to create the user with a password
                var result = await userManager.CreateAsync(user, "P@ssw0rd!");
                if (!result.Succeeded)
                {
                    // Handle errors (e.g., log or throw an exception)
                    Console.WriteLine("Failed to create admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            // Ensure "Admin" role is assigned to admin user
            if (!await userManager.IsInRoleAsync(user, "Admin"))
            {
                var addToRoleResult = await userManager.AddToRoleAsync(user, "Admin");

                if (!addToRoleResult.Succeeded)
                {
                    throw new Exception("Failed to assign Admin role to created admin user: " +
                        string.Join(", ", addToRoleResult.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
