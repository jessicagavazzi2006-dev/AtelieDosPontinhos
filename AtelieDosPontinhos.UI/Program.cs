using AtelieDosPontinhos.Application.Interfaces;
using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Infrastructure;
using AtelieDosPontinhos.Infrastructure.Context;
using AtelieDosPontinhos.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.Data.SqlClient;
using SenacGames.UI.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// =====================================================================
// AUTENTICAÇÃO MVC NATIVA
// =====================================================================
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// Permite acessar o HttpContext (necessário para o ApiCookieHandler)
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(); // 🌟 ATIVA O SERVIÇO DE SESSÃO NA MEMÓRIA

// =====================================================================
// HTTP CLIENTS & SERVIÇOS DA API
// =====================================================================
// Registra o Handler que injeta o Cookie
builder.Services.AddTransient<ApiCookieHandler>();

// Resolve a URL dinamicamente via ApiEndpointResolver
var apiBaseUrl = AppConfig.ApiBaseUrl;

// Cliente para autenticação (sem interceptador)
builder.Services.AddHttpClient("ApiClientAuth", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// Cliente padrão para serviços (com interceptador de cookie)
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
})
.AddHttpMessageHandler<ApiCookieHandler>();

// =====================================================================
// MVC
// =====================================================================
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AtelieDosPontinhosDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
    {
        // Habilita retry em transient failures
        sqlOptions.EnableRetryOnFailure();
    }));

// Identity (UI)
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AtelieDosPontinhosDbContext>()
.AddDefaultTokenProviders();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Serve arquivos estáticos adicionais (ex.: mapeamento para /images em uma pasta customizada)
// Exemplo: se as imagens estiverem em uma pasta "StaticFiles/Images" no ContentRoot,
// descomente o bloco abaixo e ajuste o caminho conforme necessário.
/*
var imagesPath = Path.Combine(app.Environment.ContentRootPath, "StaticFiles", "Images");
if (Directory.Exists(imagesPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(imagesPath),
        RequestPath = "/images"
    });
}
*/

app.UseRouting();

app.UseSession(); // 🌟 ATIVA O MIDDLEWARE QUE PERMITE O HTML LER A SESSÃO

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ====================================================================
// 🚀 APLICAR MIGRATIONS E POPULAR DADOS (SEED)
// ====================================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Atenção: rotina de criação do banco (apenas em Development).
        // Evita erro quando o banco já existe, verificando antes de criar.
        if (app.Environment.IsDevelopment())
        {
            var db = services.GetRequiredService<AtelieDosPontinhosDbContext>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                // Usar a ExecutionStrategy do EF para operações de migração/criação, isso aplica retry automático
                var strategy = db.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    try
                    {
                        // Use MigrateAsync unicamente: EF cria o DB e a tabela __EFMigrationsHistory quando necessário
                        await db.Database.MigrateAsync();
                        logger.LogInformation("Migrations aplicadas/criadas com sucesso.");
                    }
                    catch (Exception migrateEx)
                    {
                        logger.LogWarning(migrateEx, "Falha ao aplicar migrations no banco (ExecutionStrategy).");
                    }
                });
            }
            catch (Exception dbEx)
            {
                logger.LogWarning(dbEx, "Falha ao verificar/criar o banco de dados usando ExecutionStrategy. Ignorando recriação.");
            }
        }

        // SeedData.SeedAsync espera um IServiceProvider. Opcionalmente passamos WebRootPath para permitir
        // que a camada de infraestrutura carregue imagens estáticas se estiverem presentes.
        await SeedData.SeedAsync(services, app.Environment.WebRootPath);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao aplicar seed/migrations durante inicialização.");
        throw;
    }
}

app.Run();