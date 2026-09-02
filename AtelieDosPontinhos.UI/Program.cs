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
using AtelieDosPontinhos.UI.Helpers;
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

// Fallback para appsettings.json ou URL padrão se resolver falhar
if (string.IsNullOrEmpty(apiBaseUrl))
{
    apiBaseUrl = builder.Configuration["Api:BaseUrl"] ?? "http://localhost:5006/";
    System.Diagnostics.Debug.WriteLine($"DEBUG: API URL resolvida via appsettings.json: {apiBaseUrl}");
}
else
{
    System.Diagnostics.Debug.WriteLine($"DEBUG: API URL resolvida via ApiEndpointResolver: {apiBaseUrl}");
}

// Validação crítica
if (string.IsNullOrEmpty(apiBaseUrl) || !Uri.IsWellFormedUriString(apiBaseUrl, UriKind.Absolute))
{
    throw new InvalidOperationException($"API Base URL inválida ou não configurada: '{apiBaseUrl}'. Configure 'Api:BaseUrl' em appsettings.json ou libere o acesso ao launchSettings.json do projeto AtelieDosPontinhos.API");
}

System.Diagnostics.Debug.WriteLine($"✅ API URL Configurada: {apiBaseUrl}");

// Cliente para autenticação (sem interceptador)
builder.Services.AddHttpClient("ApiClientAuth", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// 🌟 CORREÇÃO: Adicionado o cliente "Api" utilizado pelo ProductController
builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
})
.AddHttpMessageHandler<ApiCookieHandler>();

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
        if (app.Environment.IsDevelopment())
        {
            var db = services.GetRequiredService<AtelieDosPontinhosDbContext>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                var strategy = db.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    try
                    {
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