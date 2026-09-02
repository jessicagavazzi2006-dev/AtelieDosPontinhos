using System.Text.Json.Serialization;
using AtelieDosPontinhos.Infrastructure.Context;
using AtelieDosPontinhos.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region SERVICES

// Controllers (API) + Configuração para ignorar ciclos de serialização JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// 🔥 Libera o acesso CORS para chamadas externas/UI
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// DbContext
builder.Services.AddDbContext<AtelieDosPontinhosDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure();
        }));

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AtelieDosPontinhosDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
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

// Serve arquivos estáticos (wwwroot) para permitir que imagens/arquivos sejam acessados
app.UseStaticFiles();

app.UseRouting();

// Ativa a política de CORS
app.UseCors("PermitirTudo");

app.UseAuthentication();
app.UseAuthorization();

// Mapeia os endpoints de API para controllers
app.MapControllers();

#endregion

#region SEED DATA

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await SeedData.SeedAsync(services, app.Environment.WebRootPath);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogWarning(ex, "Falha ao executar SeedData na API durante inicialização. Ignorando.");
    }
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