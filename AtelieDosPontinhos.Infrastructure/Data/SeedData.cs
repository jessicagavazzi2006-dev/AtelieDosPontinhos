using AtelieDosPontinhos.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace AtelieDosPontinhos.Infrastructure.Data
{


    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AtelieDosPontinhosDbContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Aplica migrations pendentes automaticamente
            await context.Database.MigrateAsync();

            // 🔐 CRIAR ROLES
            string[] roles = { "Admin", "Client" };

            foreach (var role in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);

                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 👤 CRIAR ADMIN
            var adminEmail = "admin@site.com";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // 👤 CRIAR CLIENTE
            var clientEmail = "cliente@site.com";

            var clientUser = await userManager.FindByEmailAsync(clientEmail);

            if (clientUser == null)
            {
                var user = new IdentityUser
                {
                    UserName = clientEmail,
                    Email = clientEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Cliente@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Client");
                }
            }
        }
    }
}