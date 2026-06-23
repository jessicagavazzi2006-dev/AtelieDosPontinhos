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
            // Obtém o DbContext do container de Dependency Injection
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AtelieDosPontinhosDbContext>();
            var userManeger = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Aplica migrations pendentes automaticamente
            await context.Database.MigrateAsync();

            // =================================================================
            // Criação de categoria (PARA TESTE)
            // =================================================================

            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category {Name = "Teste"}
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            // =================================================================
            // Criação de produto (PARA TESTE)
            // =================================================================

            if (!context.Products.Any())
            {
                // Busca as categorias recém-criadas para obter os IDs
                var teste = await context.Categories.FirstAsync(c => c.Name == "Teste");

                var product = new List<Product>
                {
                    new Product
                    {
                        Name = "Teste de Produto",
                        Description = "é apenas um teste para confirmar o funcionamento da criação no SeedData",
                        CoverImageUrl =  "",
                        CategoryId = teste.Id
                    }
                };

                await context.Products.AddRangeAsync(product);
                await context.SaveChangesAsync();
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

            var adminEmail = "admin@AtelieDosPontinhos.com";
            var adminUser = await userManeger.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true // Confirma Emial automaticamente
                };

                // Cria o usuário com senha padrão
                var result = await userManeger.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    // Atribui a role "Admin" ao usuário
                    await userManeger.AddToRoleAsync(adminUser, "Admin");
                }
            }

        }
    }
}
