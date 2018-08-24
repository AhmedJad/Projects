using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobPublisher.Data
{
    public class SampleData
    {
        public static async Task initializeData(IServiceProvider services)
        {
            using (var serviceScope=services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var env = serviceScope.ServiceProvider.GetService<IHostingEnvironment>();
                if (!env.IsDevelopment()) return;
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                //create our roles
                var adminTask = roleManager.CreateAsync(
                    new IdentityRole{Name="Admin"}
                    );
                var publisherUserTask = roleManager.CreateAsync(
                    new IdentityRole { Name = "Publisher" }
                    );
                var ApplierUserTask = roleManager.CreateAsync(
                    new IdentityRole { Name = "Applier" }
                    );
                Task.WaitAll(adminTask, publisherUserTask, ApplierUserTask);
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var user = new ApplicationUser
                {
                    UserName="hossma@gmail.com",
                    Email = "hossam@gmial.com"
                };
                await userManager.CreateAsync(user,"Passw0rd!");
                await userManager.AddToRoleAsync(user,"Admin");
            }
        }
    }
}
