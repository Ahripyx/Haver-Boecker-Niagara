using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Haver_Boecker_Niagara.Data
{
    public static class ApplicationDbInitializer
    {
        public static async void Initialize(IServiceProvider serviceProvider,
            bool UseMigrations = true, bool SeedSampleData = true)
        {
            #region Prepare the Database
            if (UseMigrations)
            {
                using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
                {
                    try
                    {
                        context.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.GetBaseException().Message);
                    }
                }
            }
            #endregion

            #region Seed Roles
            if (SeedSampleData)
            {
                using (var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>())
                {
                    try
                    {
                        string[] roleNames = { "Admin", "Sales", "Engineering", "Procurement", "Production", "PIC", "Read Only" };

                        foreach (var roleName in roleNames)
                        {
                            var roleExist = await roleManager.RoleExistsAsync(roleName);
                            if (!roleExist)
                            {
                                await roleManager.CreateAsync(new IdentityRole(roleName));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.GetBaseException().Message);
                    }
                }
            }
            #endregion

            #region Seed Users
            using (var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>())
            {
                try
                {
                    string defaultPassword = "Pa55w@rd";

                    if (userManager.FindByEmailAsync("admin@outlook.com").Result == null)
                    {
                        IdentityUser user = new IdentityUser
                        {
                            UserName = "admin@outlook.com",
                            Email = "admin@outlook.com",
                            EmailConfirmed = true
                        };

                        IdentityResult result = userManager.CreateAsync(user, defaultPassword).Result;

                        if (result.Succeeded)
                        {
                            userManager.AddToRoleAsync(user, "Admin").Wait();
                        }
                    }
                    if (userManager.FindByEmailAsync("sales@outlook.com").Result == null)
                    {
                        IdentityUser user = new IdentityUser
                        {
                            UserName = "sales@outlook.com",
                            Email = "sales@outlook.com",
                            EmailConfirmed = true
                        };

                        IdentityResult result = userManager.CreateAsync(user, defaultPassword).Result;

                        if (result.Succeeded)
                        {
                            userManager.AddToRoleAsync(user, "sales").Wait();
                        }
                    }
                    if (userManager.FindByEmailAsync("Engineering@outlook.com").Result == null)
                    {
                        IdentityUser user = new IdentityUser
                        {
                            UserName = "Engineering@outlook.com",
                            Email = "Engineering@outlook.com",
                            EmailConfirmed = true
                        };

                        IdentityResult result = userManager.CreateAsync(user, defaultPassword).Result;

                        if (result.Succeeded)
                        {
                            userManager.AddToRoleAsync(user, "Engineering").Wait();
                        }
                    }
                    if (userManager.FindByEmailAsync("Procurement@outlook.com").Result == null)
                    {
                        IdentityUser user = new IdentityUser
                        {
                            UserName = "Procurement@outlook.com",
                            Email = "Procurement@outlook.com",
                            EmailConfirmed = true
                        };

                        IdentityResult result = userManager.CreateAsync(user, defaultPassword).Result;

                        if (result.Succeeded)
                        {
                            userManager.AddToRoleAsync(user, "Procurement").Wait();
                        }
                    }
                    if (userManager.FindByEmailAsync("Production@outlook.com").Result == null)
                    {
                        IdentityUser user = new IdentityUser
                        {
                            UserName = "Production@outlook.com",
                            Email = "Production@outlook.com",
                            EmailConfirmed = true
                        };

                        IdentityResult result = userManager.CreateAsync(user, defaultPassword).Result;

                        if (result.Succeeded)
                        {
                            userManager.AddToRoleAsync(user, "PIC").Wait();
                        }
                    }
                    if (userManager.FindByEmailAsync("PIC@outlook.com").Result == null)
                    {
                        IdentityUser user = new IdentityUser
                        {
                            UserName = "PIC@outlook.com",
                            Email = "PIC@outlook.com",
                            EmailConfirmed = true
                        };

                        IdentityResult result = userManager.CreateAsync(user, defaultPassword).Result;

                        if (result.Succeeded)
                        {
                            userManager.AddToRoleAsync(user, "").Wait();
                        }
                    }
                    if (userManager.FindByEmailAsync("super@outlook.com").Result == null)
                    {
                        IdentityUser user = new IdentityUser
                        {
                            UserName = "super@outlook.com",
                            Email = "super@outlook.com",
                            EmailConfirmed = true
                        };

                        IdentityResult result = userManager.CreateAsync(user, defaultPassword).Result;

                        if (result.Succeeded)
                        {
                            userManager.AddToRoleAsync(user, "Supervisor").Wait();
                        }
                    }
                    if (userManager.FindByEmailAsync("user@outlook.com").Result == null)
                    {
                        IdentityUser user = new IdentityUser
                        {
                            UserName = "user@outlook.com",
                            Email = "user@outlook.com",
                            EmailConfirmed = true
                        };

                        IdentityResult result = userManager.CreateAsync(user, defaultPassword).Result;

                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.GetBaseException().Message);
                }
            }
        }
        #endregion
    }
}

