using AtelieDosPontinhos.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AtelieDosPontinhos.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

#region SERVICES

// Controllers
builder.Services.AddControllers();

// MVC (Views, se você usa UI junto)
builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<AtelieDosPontinhosDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AtelieDosPontinhosDbContext>()
    .AddDefaultTokenProviders();

// Authorization (roles)
builder.Services.AddAuthorization();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region PIPELINE

// Swagger (DEVE vir antes do routing)
app.UseSwagger();
app.UseSwaggerUI();

// HTTPS
app.UseHttpsRedirection();

// Routing
app.UseRouting();

// Auth (ordem obrigatória)
app.UseAuthentication();
app.UseAuthorization();

// Rotas MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

#endregion

#region SEED DATA

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}

#endregion

#region DEBUG DB CONNECTION (opcional)

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AtelieDosPontinhosDbContext>();
    Console.WriteLine("DB CONECTADO: " + db.Database.GetConnectionString());
}

#endregion

app.Run();