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
        }
    }

}
