using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace AtelieDosPontinhos.Infrastructure.Data
{
    public static class SeedData
    {
        /// <summary>
        /// Popula o banco de dados com roles, usuários, categorias e produtos iniciais.
        /// Idempotente: pode ser chamado várias vezes sem duplicar dados.
        /// </summary>
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AtelieDosPontinhosDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Aplica migrations pendentes (recomendado para ambientes com Identity)
            //await context.Database.MigrateAsync();

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
            // 👤 2. Users (Admin e Cliente) - 🌟 BLINDADO CONTRA CONCORRÊNCIA
            var adminEmail = "admin@site.com";
            try
            {
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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Admin já existente ou erro de concorrência: {ex.Message}");
            }

            // 👤 3. CRIAR CLIENTE - 🌟 BLINDADO CONTRA CONCORRÊNCIA
            var clientEmail = "cliente@site.com";
            try
            {
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Cliente já existente ou erro de concorrência: {ex.Message}");
            }


            // 3. Categorias
            if (!context.Categories.Any())
            {
                var categorias = new List<Category>
                {
                    new Category { Name = "Banho", ImageLocal = "/images/categorias/banho.png" },
                    new Category { Name = "Cama", ImageLocal = "/images/categorias/cama.png" },
                    new Category { Name = "Infantil", ImageLocal = "/images/categorias/infantil.png" },
                    new Category { Name = "Materiais", ImageLocal = "/images/categorias/materiais.png" },
                    new Category { Name = "Mesa", ImageLocal = "/images/categorias/mesa.png" }
                };

                await context.Categories.AddRangeAsync(categorias);
                await context.SaveChangesAsync();
            }

            // 4. Produtos iniciais
            if (!context.Products.Any())
            {
                var banho = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Banho");
                var cama = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Cama");
                var infantil = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Infantil");

                var produtos = new List<Product>
                {
                    new Product
                    {
                        Name = "Kit de Toalhas Bordadas",
                        Description = "Lindo kit contendo duas toalhas de banho e uma de rosto com bordados feitos à mão.",
                        CoverImageUrl = "https://unsplash.com",
                        Price = 159.90m,
                        Stock = 20,
                        IsFeatured = true,
                        CategoryId = banho?.Id ?? 1
                    },
                    new Product
                    {
                        Name = "Toalha de Banho Azul",
                        Description = "Toalha de banho com acabamento artesanal em crochê azul.",
                        CoverImageUrl ="https://unsplash.com",
                        Price = 89.90m,
                        Stock = 30,
                        IsFeatured = false,
                        CategoryId = banho?.Id ?? 1
                    },
                    new Product
                    {
                        Name = "Jogo de Cama Duplo",
                        Description = "Jogo de cama casal 200 fios com bordado artesanal.",
                        CoverImageUrl = "/images/products/lençol vermelho casal.jpg",
                        Price = 249.90m,
                        Stock = 10,
                        IsFeatured = true,
                        CategoryId = cama?.Id ?? 2
                    },
                    new Product
                    {
                        Name = "Manta Infantil Bordada",
                        Description = "Manta leve para berço com bordados decorativos.",
                        CoverImageUrl = "/images/products/fralda menina.jpg",
                        Price = 129.90m,
                        Stock = 15,
                        IsFeatured = false,
                        CategoryId = infantil?.Id ?? 3
                    }
                };

                await context.Products.AddRangeAsync(produtos);
                await context.SaveChangesAsync();
            }
            // 4. Produtos iniciais
            if (!context.Products.Any())
            {
                // Obtém uma categoria padrão para associar os produtos gerados
                var defaultCategory = await context.Categories.FirstOrDefaultAsync();
                var defaultCategoryId = defaultCategory?.Id ?? 1;

                // Tenta obter IWebHostEnvironment para localizar wwwroot
                var env = scope.ServiceProvider.GetService<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>();
                var produtos = new List<Product>();

                if (env != null)
                {
                    var imagesDir = System.IO.Path.Combine(env.WebRootPath ?? string.Empty, "images", "products");
                    if (System.IO.Directory.Exists(imagesDir))
                    {
                        var allowed = new[] { ".png", ".jpg", ".jpeg", ".webp", ".gif" };
                        var files = System.IO.Directory.GetFiles(imagesDir)
                            .Where(f => allowed.Contains(System.IO.Path.GetExtension(f).ToLowerInvariant()))
                            .ToList();

                        foreach (var file in files)
                        {
                            var fileName = System.IO.Path.GetFileName(file);
                            var name = System.IO.Path.GetFileNameWithoutExtension(fileName).Replace('-', ' ').Replace('_', ' ');
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

                // Se nenhuma imagem foi encontrada, mantém o seed manual mínimo (fallback)
                if (!produtos.Any())
                {
                    produtos.Add(new Product
                    {
                        Name = "Kit de Toalhas Bordadas",
                        Description = "Lindo kit contendo duas toalhas de banho e uma de rosto com bordados feitos à mão.",
                        CoverImageUrl = "/images/products/toalha1.png",
                        Price = 159.90m,
                        Stock = 20,
                        IsFeatured = true,
                        CategoryId = defaultCategoryId
                    });
                }

                await context.Products.AddRangeAsync(produtos);
                await context.SaveChangesAsync();
            }
        }
    }
}