using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Infrastructure.Context;

namespace AtelieDosPontinhos.Infrastructure.Identity
{
    public static class SeedData
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            
            var context = scope.ServiceProvider.GetRequiredService<AtelieDosPontinhosDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

          
            try
            {
                var strategy = context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    try
                    {
                        // Use apenas MigrateAsync: EF Core cria o DB e a tabela __EFMigrationsHistory automaticamente
                        await context.Database.MigrateAsync();
                    }
                    catch (Exception migrateEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Falha ao aplicar migrations (Identity.Seed): {migrateEx.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Falha ao executar ExecutionStrategy (Identity.Seed): {ex.Message}");
            }

            // =================================================================
            // Seed de Roles (Papéis de Usuário)
            // =================================================================
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // =================================================================
            // Seed do Usuário Administrador
            // =================================================================
            var adminEmail = "admin@site.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                // Cria o usuário com a senha padrão mapeada
                var result = await userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
