namespace FastCarSales.Data.Seeding
{
    using System;
    using System.Threading.Tasks;
    using FastCarSales.Data;
    using FastCarSales.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
   using static FastCarSales.GlobalConstants.GlobalConstants;

    public class AdministratorSeeder : ISeeder
    {
        public async Task SeedAsync(FastCarSalesDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedAdminAsync(roleManager, userManager, AdministratorRoleName);
        }

        private static async Task SeedAdminAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, string adminRoleName)
        {
            if (await roleManager.RoleExistsAsync(adminRoleName))
            {
                return;
            }

            var adminRole = new IdentityRole { Name = adminRoleName };

            await roleManager.CreateAsync(adminRole);

            const string adminEmail = "dpmangwiro@gmail.com";
            const string adminPassword = "dponqqdr123";

            var adminUser = new ApplicationUser()
            {
                Email = adminEmail,
                UserName = adminEmail
            };

            await userManager.CreateAsync(adminUser, adminPassword);
            await userManager.AddToRoleAsync(adminUser, adminRoleName);
        }
    }
}