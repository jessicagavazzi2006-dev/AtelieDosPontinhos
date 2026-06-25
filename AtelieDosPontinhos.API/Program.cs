using AtelieDosPontinhos.Infrastructure.Context;
using AtelieDosPontinhos.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region SERVICES
// Controllers (API)
builder.Services.AddControllers();

// MVC (Views)
builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<AtelieDosPontinhosDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AtelieDosPontinhosDbContext>()
    .AddDefaultTokenProviders();

// 🔐 CORREÇÃO IMPORTANTE (API não pode redirecionar pra login)
builder.Services.ConfigureApplicationCookie(options => {
    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = 403;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };
});

// Authorization
builder.Services.AddAuthorization();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region DEPENDENCY INJECTION
// 🌟 CORREÇÃO FEITA: Ajustado para usar o arquivo correto no plural "ProductServices"
builder.Services.AddScoped<AtelieDosPontinhos.Application.Interfaces.IProductService, AtelieDosPontinhos.Application.Services.ProductServices>();
builder.Services.AddScoped<AtelieDosPontinhos.Domain.Interfaces.IProductRepository, AtelieDosPontinhos.Infrastructure.Repositories.ProductRepository>();
#endregion

var app = builder.Build();

#region PIPELINE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// MVC routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 🌟 ADICIONADO: Mapeia os endpoints de API (AccountController) para que o Swagger consiga lê-los!
app.MapControllers();
#endregion

#region SEED DATA
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}
#endregion

#region DEBUG DB CONNECTION
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AtelieDosPontinhosDbContext>();
    Console.WriteLine("DB CONECTADO: " + db.Database.GetConnectionString());
}
#endregion

app.Run();