using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using AMaz.Service;
using AMaz.DB;
using AMaz.Entity;

namespace AMaz.Service
{
    public static class DbInitializer
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AMazDbContext>();
                context.Database.EnsureCreated();

                var userService =
                    serviceScope.ServiceProvider.GetService<IAdminService>();
                try
                {
                    if (context.Database.GetPendingMigrations().Count() > 0)
                    {
                        context.Database.Migrate();
                    }
                }
                catch (Exception ex)
                {

                }
                if (userService.AdminCheck())
                    return;
                var user = new CreateRequest()
                {
                    FirstName = "Admin",
                    LastName = "Nguyen1",
                    Role = Role.Admin,
                    Email = "Admin@gmail.com",
                    Password = "admin123@",
                    
                };
                userService.Create(user);

            }

        }
    }
}
