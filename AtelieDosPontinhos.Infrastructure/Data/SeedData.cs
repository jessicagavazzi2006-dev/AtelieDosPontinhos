using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AtelieDosPontinhos.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, string? webRootPath = null)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AtelieDosPontinhosDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            try
            {
                var strategy = context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    try
                    {
                        await context.Database.MigrateAsync();
                    }
                    catch (Exception migrateEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Falha ao aplicar migrations: {migrateEx.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Falha ao executar ExecutionStrategy para migrations: {ex.Message}");
            }

            // 1. Roles
            string[] roles = { "Admin", "Client" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. Users (Admin e Cliente)
            var adminEmail = "admin@site.com";
            try
            {
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true,
                        Nome = "Administrador"
                    };
                    var result = await userManager.CreateAsync(user, "Admin@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Admin já existente ou erro de concorrência: {ex.Message}");
            }

            var clientEmail = "cliente@site.com";
            try
            {
                var clientUser = await userManager.FindByEmailAsync(clientEmail);
                if (clientUser == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = clientEmail,
                        Email = clientEmail,
                        EmailConfirmed = true,
                        Nome = "Cliente Teste"
                    };
                    var result = await userManager.CreateAsync(user, "Cliente@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Client");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Cliente já existente ou erro de concorrência: {ex.Message}");
            }

            // 3. Categorias
            if (!context.Categories.Any())
            {
                var categorias = new List<Category>
                {
                    new Category { Name = "Banho", ImageLocal = "/images/products/default.svg" },
                    new Category { Name = "Cama", ImageLocal = "/images/products/default.svg" },
                    new Category { Name = "Infantil", ImageLocal = "/images/products/default.svg" },
                    new Category { Name = "Materiais", ImageLocal = "/images/products/default.svg" },
                    new Category { Name = "Mesa", ImageLocal = "/images/products/default.svg" }
                };

                await context.Categories.AddRangeAsync(categorias);
                await context.SaveChangesAsync();
            }

            // 4. Produtos iniciais
            if (!context.Products.Any())
            {
                var defaultCategory = await context.Categories.FirstOrDefaultAsync();
                var defaultCategoryId = defaultCategory?.Id ?? 1;

                var produtos = new List<Product>();
                if (!string.IsNullOrWhiteSpace(webRootPath))
                {
                    var imagesDir = Path.Combine(webRootPath, "images", "products");
                    if (Directory.Exists(imagesDir))
                    {
                        var allowed = new[] { ".png", ".jpg", ".jpeg", ".webp", ".gif" };
                        var files = Directory.GetFiles(imagesDir)
                            .Where(f => allowed.Contains(Path.GetExtension(f).ToLowerInvariant()))
                            .ToList();

                        foreach (var file in files)
                        {
                            var fileName = Path.GetFileName(file);
                            var name = Path.GetFileNameWithoutExtension(fileName).Replace('-', ' ').Replace('_', ' ');
                            if (string.IsNullOrWhiteSpace(name)) name = "Produto";

                            produtos.Add(new Product
                            {
                                Name = name,
                                Description = $"Produto gerado automaticamente a partir da imagem {fileName}.",
                                CoverImageUrl = $"/images/products/{fileName}",
                                Price = 49.90m,
                                Stock = 10,
                                IsFeatured = false,
                                CategoryId = defaultCategoryId
                            });
                        }
                    }
                }

                if (!produtos.Any())
                {
                    produtos.Add(new Product
                    {
                        Name = "Kit de Toalhas Bordadas",
                        Description = "Lindo kit contendo duas toalhas de banho e uma de rosto com bordados feitos à mão.",
                        CoverImageUrl = "/images/products/kittoalhas.jpg",
                        Price = 159.90m,
                        Stock = 20,
                        IsFeatured = true,
                        CategoryId = defaultCategoryId
                    });

                    produtos.Add(new Product
                    {
                        Name = "Toalha de Banho Azul",
                        Description = "Toalha de banho com acabamento artesanal em crochê azul.",
                        CoverImageUrl = "/images/products/toalhaazul.jpg",
                        Price = 89.90m,
                        Stock = 30,
                        IsFeatured = false,
                        CategoryId = defaultCategoryId
                    });

                    produtos.Add(new Product
                    {
                        Name = "Jogo de Cama Duplo",
                        Description = "Jogo de cama casal 200 fios com bordado artesanal.",
                        CoverImageUrl = "/images/products/lencolvermelhocasal.jpg",
                        Price = 249.90m,
                        Stock = 10,
                        IsFeatured = true,
                        CategoryId = defaultCategoryId
                    });

                    produtos.Add(new Product
                    {
                        Name = "Manta Infantil Bordada",
                        Description = "Manta leve para berço com bordados decorativos.",
                        CoverImageUrl = "/images/products/default.svg",
                        Price = 129.90m,
                        Stock = 15,
                        IsFeatured = false,
                        CategoryId = defaultCategoryId
                    });
                }

                await context.Products.AddRangeAsync(produtos);
                await context.SaveChangesAsync();
            }
        }
    }
}