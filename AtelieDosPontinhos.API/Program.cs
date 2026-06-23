using AtelieDosPontinhos.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Ativa suporte a MVC (Controllers + Views Razor)
builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<AtelieDosPontinhosDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔐 Identity (LOGIN REAL)
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AtelieDosPontinhosDbContext>()
    .AddDefaultTokenProviders();

// 🔐 Authorization (necessário para Roles funcionar corretamente)
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure pipeline
app.UseHttpsRedirection();

// 🔐 AUTH PIPELINE (ordem obrigatória)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 🔐 SEED DE ROLES (Admin / Client)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}

app.Run();