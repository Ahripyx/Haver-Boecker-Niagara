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
            if (SeedSampleData)
            {
                using (var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>())
                {
                    try
                    {
                        var users = new[]
                        {
                            new { UserName = "admin@haver.com", Password = "Admin@123", Role = "Admin" },
                            new { UserName = "sales@haver.com", Password = "Sales@123", Role = "Sales" },
                            new { UserName = "engineering@haver.com", Password = "Engineering@123", Role = "Engineering" },
                            new { UserName = "procurement@haver.com", Password = "Procurement@123", Role = "Procurement" },
                            new { UserName = "production@haver.com", Password = "Production@123", Role = "Production" },
                            new { UserName = "pic@haver.com", Password = "PIC@123", Role = "PIC" },
                            new { UserName = "readonly@haver.com", Password = "ReadOnly@123", Role = "Read Only" }
                        };

                        foreach (var user in users)
                        {
                            var existingUser = await userManager.FindByEmailAsync(user.UserName);
                            if (existingUser == null)
                            {
                                var identityUser = new IdentityUser { UserName = user.UserName, Email = user.UserName };
                                var result = await userManager.CreateAsync(identityUser, user.Password);
                                if (result.Succeeded)
                                {
                                    await userManager.AddToRoleAsync(identityUser, user.Role);
                                }
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
        }
    }

}
