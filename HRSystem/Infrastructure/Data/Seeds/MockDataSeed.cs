using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace HRSystem.Data.Seeds
{
    public class MockDataSeed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public async Task SeedAsync()
        {
            try
            {
                // Create roles
                string[] roleNames = { "Clerical", "Supervisor", "Manager" };
                foreach (var roleName in roleNames)
                {
                    if (!await _roleManager.RoleExistsAsync(roleName))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                // Create users
                var users = new[]
                {
                    new { Username = "Clerk01", Password = "Password123!", Role = "Clerical" },
                    new { Username = "Clerk02", Password = "Password123!", Role = "Clerical" },
                    new { Username = "Super01", Password = "Password123!", Role = "Supervisor" },
                    new { Username = "Super02", Password = "Password123!", Role = "Supervisor" },
                    new { Username = "Mgr01", Password = "Password123!", Role = "Manager" }
                };

                foreach (var user in users)
                {
                    if (await _userManager.FindByNameAsync(user.Username) == null)
                    {
                        var identityUser = new IdentityUser
                        {
                            UserName = user.Username,
                            Email = $"{user.Username}@example.com"
                        };
                        var result = await _userManager.CreateAsync(identityUser, user.Password);
                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(identityUser, user.Role);
                        }
                        else
                        {
                            throw new Exception($"Failed to create user {user.Username}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // In production, use a proper logger
                Console.WriteLine($"Error during mock seeding: {ex.Message}");
                throw;
            }
        }
    }
}