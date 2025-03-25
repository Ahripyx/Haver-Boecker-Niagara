using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Haver_Boecker_Niagara.Data
{
    public static class ApplicationDbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider,
            bool UseMigrations = true, bool SeedSampleData = true)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            #region Prepare the Database
            if (UseMigrations)
            {
                {
                    await context.Database.EnsureDeletedAsync();
                    await context.Database.MigrateAsync();
                }
            }
            #endregion

            string[] roleNames = { "Admin", "Sales", "Engineering", "Procurement", "Production", "PIC", "Read Only" };

            
            #region Seed Roles
            if (SeedSampleData)
            {
                var roleManager = new RoleStore<IdentityRole>(context);
                
                foreach (var roleName in roleNames)
                {
                    var roleExist = roleManager.Roles.Any(p => p.Name == roleName.ToLower());
                    if (!roleExist)
                    {
                        await roleManager.CreateAsync(new IdentityRole
                        {
                            Name = roleName.ToLower(),
                            NormalizedName = roleName.ToUpper(),
                            ConcurrencyStamp = Guid.NewGuid().ToString()
                        });
                    }
                }
                
            }

            await context.SaveChangesAsync();
            #endregion
            
            #region Seed Users
            var userManager = new UserStore<IdentityUser>(context);

            const string defaultPassword = "Pa55w@rd";
            foreach (var roleName in roleNames)
            {
                var email = $"{roleName.Replace(" ", "").ToLower()}@outlook.com";
                if (!context.Users.Any(p => p.Email == email))
                {
                    var user = new IdentityUser
                    {
                        UserName = email,
                        Email = email,
                        NormalizedEmail = email.ToUpper(),
                        NormalizedUserName = email.ToUpper(),
                        EmailConfirmed = true,
                        ConcurrencyStamp = Guid.NewGuid().ToString("D")
                    };
                
                    var password = new PasswordHasher<IdentityUser>();
                    var hashed = password.HashPassword(user, defaultPassword);
                    user.PasswordHash = hashed;
                
                    await userManager.CreateAsync(user);

                }
            }
            await context.SaveChangesAsync();
            
            // add users to roles AFTER they are created
            foreach (var role in roleNames)
            {
                var email = $"{role.Replace(" ", "").ToLower()}@outlook.com";
                var user = await userManager.FindByEmailAsync(email.ToUpper());
                await userManager.AddToRoleAsync(user, role.ToUpper());
            }

            await context.SaveChangesAsync();
        }
        #endregion
    }
    
}

