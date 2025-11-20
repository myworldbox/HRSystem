using HRSystem.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static HRSystem.Domain.Enums;

namespace HRSystem.Data
{
    public class MockDataSeed
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public MockDataSeed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            // Seed roles
            List<string> roleNames = Enum.GetNames(typeof(Role)).ToList();
            foreach (var roleName in roleNames)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            string user1 = "clerk01";
            string user2 = "clerk02";
            string user3 = "super01";
            string user4 = "super02"; // Fixed duplicate (was "super01")
            string user5 = "mgr01";
            string user6 = "admin01";
            string password = "Pw@12345";

            var staffs = new RegisterModel.InputModel[]
            {
                new RegisterModel.InputModel { Email = $"{user1}@gmail.com", Password = password, ConfirmPassword = password },
                new RegisterModel.InputModel { Email = $"{user2}@gmail.com", Password = password, ConfirmPassword = password },
                new RegisterModel.InputModel { Email = $"{user3}@gmail.com", Password = password, ConfirmPassword = password },
                new RegisterModel.InputModel { Email = $"{user4}@gmail.com", Password = password, ConfirmPassword = password },
                new RegisterModel.InputModel { Email = $"{user5}@gmail.com", Password = password, ConfirmPassword = password },
                new RegisterModel.InputModel { Email = $"{user6}@gmail.com", Password = password, ConfirmPassword = password }
            };

            var users = new[] {
                new { staff = staffs[0], Role = nameof(Role.Clerk), Accessible = new[] { user1 } },
                new { staff = staffs[1], Role = nameof(Role.Clerk), Accessible = new[] { user2 } },
                new { staff = staffs[2], Role = nameof(Role.Supervisor), Accessible = new[] { user1 } },
                new { staff = staffs[3], Role = nameof(Role.Supervisor), Accessible = new[] { user1, user2 } },
                new { staff = staffs[4], Role = nameof(Role.Manager), Accessible = new[] { user1, user2, user3, user4, user5 } },
                new { staff = staffs[5], Role = nameof(Role.Admin), Accessible = Array.Empty<string>() }
            };

            foreach (var u in users)
            {
                var email = u.staff.Email;
                var existingUser = await _userManager.FindByEmailAsync(email);

                if (existingUser == null)
                {
                    var newUser = new IdentityUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };

                    var result = await _userManager.CreateAsync(newUser, password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(newUser, u.Role);

                        foreach (var account in u.Accessible)
                        {
                            await _userManager.AddClaimAsync(newUser, new Claim("AccessibleAccount", account));
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Failed to create user {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }
    }
}
