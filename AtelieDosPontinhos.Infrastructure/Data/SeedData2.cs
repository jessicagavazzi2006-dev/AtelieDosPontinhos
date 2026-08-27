// =============================================================================
// SenacGames.Infrastructure - Seed Data (Dados Iniciais)
// =============================================================================
//  CONCEITO IMPORTANTE: Seed Data
// Seed Data são dados iniciais que são inseridos no banco de dados
// quando a aplicação é executada pela primeira vez.
// Isso é útil para:
// - Ter dados de demonstração
// - Criar o usuário administrador inicial
// - Popular categorias padrão
//
// Este método é chamado no Program.cs durante a inicialização da aplicação.
// =============================================================================

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Infrastructure.Context;

namespace AtelieDosPontinhos.Infrastructure.Identity
{
    /// <summary>
    /// Classe responsável por popular o banco de dados com dados iniciais.
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// Popula o banco de dados com categorias, games e o usuário admin.
        /// Este método é idempotente — pode ser chamado várias vezes sem duplicar dados.
        /// </summary>
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            // Obtém o DbContext do container de Dependency Injection
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AtelieDosPontinhosDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Aplica migrations pendentes automaticamente
            await context.Database.MigrateAsync();

            // =====================================================================
            // 1. SEED DE CATEGORIAS
            // =====================================================================
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Ação" },
                    new Category { Name = "Aventura" },
                    new Category { Name = "RPG" },
                    new Category { Name = "Corrida" },
                    new Category { Name = "Esportes" },
                    new Category { Name = "Terror" },
                    new Category { Name = "Multiplayer" },
                    new Category { Name = "Indie" }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            // =====================================================================
            // 2. SEED DE GAMES
            // =====================================================================
            if (!context.Games.Any())
            {
                // Busca as categorias recém-criadas para obter os IDs
                var acao = await context.Categories.FirstAsync(c => c.Name == "Ação");
                var aventura = await context.Categories.FirstAsync(c => c.Name == "Aventura");
                var rpg = await context.Categories.FirstAsync(c => c.Name == "RPG");
                var corrida = await context.Categories.FirstAsync(c => c.Name == "Corrida");
                var terror = await context.Categories.FirstAsync(c => c.Name == "Terror");
                var indie = await context.Categories.FirstAsync(c => c.Name == "Indie");

                var games = new List<Game>
                {
                    new Game
                    {
                        Title = "God of War Ragnarök",
                        Description = "Embarque em uma jornada épica e comovente onde Kratos e Atreus lutam para se segurar e se soltar. Testemunhe a mudança da dinâmica pai e filho à medida que eles se preparam para a guerra.",
                        ReleaseYear = 2022,
                        CoverImageUrl = "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/2322010/library_600x900_2x.jpg",
                        CategoryId = acao.Id,
                        IsFeatured = true,
                        CreatedAt = DateTime.Now
                    },
                    new Game
                    {
                        Title = "Elden Ring",
                        Description = "Um jogo de RPG de ação desenvolvido pela FromSoftware e publicado pela Bandai Namco. Criado em colaboração com George R.R. Martin, oferece um vasto mundo aberto repleto de desafios.",
                        ReleaseYear = 2022,
                        CoverImageUrl = "https://image.api.playstation.com/vulcan/ap/rnd/202110/2000/aGhopp3MHppi7kooGE2Dtt8C.png",
                        CategoryId = rpg.Id,
                        IsFeatured = true,
                        CreatedAt = DateTime.Now
                    },
                    new Game
                    {
                        Title = "Spider-Man 2",
                        Description = "Marvel's Spider-Man 2 é um jogo de aventura em mundo aberto onde Peter Parker e Miles Morales enfrentam novos vilões em uma Nova York expandida.",
                        ReleaseYear = 2023,
                        CoverImageUrl = "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1817070/library_600x900_2x.jpg",
                        CategoryId = aventura.Id,
                        IsFeatured = true,
                        CreatedAt = DateTime.Now
                    },
                    new Game
                    {
                        Title = "Zelda: Tears of the Kingdom",
                        Description = "A sequência de Breath of the Wild leva Link aos céus de Hyrule em uma aventura épica com novas mecânicas de construção e exploração vertical.",
                        ReleaseYear = 2023,
                        CoverImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co5vmg.jpg",
                        CategoryId = rpg.Id,
                        IsFeatured = true,
                        CreatedAt = DateTime.Now
                    },
                    new Game
                    {
                        Title = "Final Fantasy XVI",
                        Description = "A mais recente entrada na lendária franquia Final Fantasy traz um RPG de ação sombrio e maduro com combate espetacular e uma história envolvente.",
                        ReleaseYear = 2023,
                        CoverImageUrl = "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/2515020/library_600x900_2x.jpg",
                        CategoryId = rpg.Id,
                        IsFeatured = false,
                        CreatedAt = DateTime.Now
                    },
                    new Game
                    {
                        Title = "Forza Horizon 5",
                        Description = "Explore os vibrantes e contrastantes cenários do México no maior e mais diverso mundo aberto de Forza Horizon. Corridas emocionantes em um festival de velocidade.",
                        ReleaseYear = 2021,
                        CoverImageUrl = "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1551360/library_600x900_2x.jpg",
                        CategoryId = corrida.Id,
                        IsFeatured = false,
                        CreatedAt = DateTime.Now
                    },
                    new Game
                    {
                        Title = "Resident Evil 4",
                        Description = "O remake do clássico survival horror traz Leon S. Kennedy em uma missão de resgate na Europa rural, com gráficos modernos e gameplay atualizado.",
                        ReleaseYear = 2023,
                        CoverImageUrl = "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/2050650/library_600x900_2x.jpg",
                        CategoryId = terror.Id,
                        IsFeatured = false,
                        CreatedAt = DateTime.Now
                    },
                    new Game
                    {
                        Title = "Hades",
                        Description = "Um roguelike aclamado pela crítica onde você desafia o deus da morte enquanto tenta escapar do Submundo. Combate rápido e narrativa envolvente da Supergiant Games.",
                        ReleaseYear = 2020,
                        CoverImageUrl = "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1145360/library_600x900_2x.jpg",
                        CategoryId = indie.Id,
                        IsFeatured = false,
                        CreatedAt = DateTime.Now
                    }
                };

                await context.Games.AddRangeAsync(games);
                await context.SaveChangesAsync();
            }

            // =====================================================================
            // 3. SEED DE ROLES (Papéis de Usuário)
            // =====================================================================
            //  CONCEITO: Roles no Identity
            // Roles são papéis que definem o nível de acesso do usuário.
            // Exemplo: "Admin" pode gerenciar games, "User" só pode visualizar.
            // =====================================================================
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // =====================================================================
            // 4. SEED DO USUÁRIO ADMINISTRADOR
            // =====================================================================
            //  CONCEITO: UserManager
            // O UserManager é o serviço do Identity para gerenciar usuários.
            // Ele permite criar, buscar, atualizar e deletar usuários.
            // =====================================================================
            var adminEmail = "admin@senacgames.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true // Confirma o email automaticamente
                };

                // Cria o usuário com a senha padrão
                var result = await userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    // Atribui a role "Admin" ao usuário
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
