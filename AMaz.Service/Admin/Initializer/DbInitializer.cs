using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using AMaz.DB;
using AMaz.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using AMaz.Common;

namespace AMaz.Service
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AMazDbContext>();

                try
                {
                    if (context.Database.GetPendingMigrations().Count() > 0)
                    {
                        context.Database.Migrate();
                    }

                    await CreateRolesWithAdmin(serviceScope.ServiceProvider);
                }
                catch (Exception ex)
                {

                }

            }

        }

        private static async Task CreateRolesWithAdmin(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var PowerUserConfig = serviceProvider.GetRequiredService<IOptions<PowerUserConfiguration>>();
            
            //initializing custom roles 
            string[] roleNames = { "Admin", "Manager", "Student", "Coordinator" };
            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //Make Initial Admin User
            var poweruser = new User
            {
                UserName = PowerUserConfig.Value.Email,
                Email = PowerUserConfig.Value.Email,
            };
            string userPWD = PowerUserConfig.Value.Password;

            var _user = await UserManager.FindByEmailAsync(PowerUserConfig.Value.Email);
            if (_user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
                if (createPowerUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(poweruser, "Admin");

                }
            }
        }
    }
}
