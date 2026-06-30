using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Infrastructure;
using AtelieDosPontinhos.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Força a URL de escuta para evitar conflito de porta em desenvolvimento
builder.WebHost.UseUrls("http://localhost:5012");

// 1. Configuração do Banco de Dados SQL Server
builder.Services.AddDbContext<AtelieDosPontinhosDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Configuração do Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AtelieDosPontinhosDbContext>()
    .AddDefaultTokenProviders();

// Configuração das Opções de Senha/Login
builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
});

// Configuração de redirecionamento de Cookies para as telas da UI
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/Login";
});

// ====================================================================
// 🛒 CONFIGURAÇÃO DE SESSÃO COMPATÍVEL COM IDENTITY
// ====================================================================
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddControllersWithViews();

// 🌟 CONFIGURADO: Registra o HttpClientFactory para o AccountController da UI consumir a API!
builder.Services.AddHttpClient();

var app = builder.Build();

// Configurações de ambiente pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// ====================================================================
// 🛒 ATIVAÇÃO DO MIDDLEWARE DE SESSÃO (ANTES DO ROTEAMENTO/AUTH)
// ====================================================================
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ====================================================================
// 🚀 INICIALIZAÇÃO E CARGA DINÂMICA DE DADOS (AUTO-ID)
// ====================================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AtelieDosPontinhosDbContext>();

    context.Database.EnsureCreated();

    // Carga de Categorias
    if (!context.Categories.Any())
    {
        var categoriesReais = new List<Category>
        {
            new Category { Name = "Banho" },
            new Category { Name = "Cama" },
            new Category { Name = "Infantil" },
            new Category { Name = "Materiais" },
            new Category { Name = "Mesa" }
        };

        context.Categories.AddRange(categoriesReais);
        context.SaveChanges();
    }

    // Carga Inicial de Produtos 
    if (!context.Products.Any())
    {
        var idBanho = context.Categories.FirstOrDefault(c => c.Name == "Banho")?.Id ?? 1;
        var idCama = context.Categories.FirstOrDefault(c => c.Name == "Cama")?.Id ?? 2;
        var idInfantil = context.Categories.FirstOrDefault(c => c.Name == "Infantil")?.Id ?? 3;
        var idMateriais = context.Categories.FirstOrDefault(c => c.Name == "Materiais")?.Id ?? 4;
        var idMesa = context.Categories.FirstOrDefault(c => c.Name == "Mesa")?.Id ?? 5;

        var produtosReais = new List<Product>
        {
            // === CATEGORIA: BANHO ===
            new Product { Name = "Kit Toalhas Beges", Price = 149.90m, CoverImageUrl = "/images/categorias/banho/Kit toalhas/kit toalhas beges.jpg", IsFeatured = true, Description = "Lindo kit de toalhas beges com bordado artesanal refinado.", CategoryId = idBanho },
            new Product { Name = "Kit Toalhas Brancas com Flores", Price = 159.90m, CoverImageUrl = "/images/categorias/banho/Kit toalhas/kit toalhas brancas com flo.jpg", IsFeatured = true, Description = "Kit de toalhas brancas com delicados motifs florais bordados.", CategoryId = idBanho },
            new Product { Name = "Toalha de Banho Azul", Price = 69.90m, CoverImageUrl = "/images/categorias/banho/Toalha de banho/toalha azul.jpg", IsFeatured = true, Description = "Toalha de banho azul macia com detalhes em ponto cruz.", CategoryId = idBanho },
            new Product { Name = "Toalha de Banho Branca", Price = 69.90m, CoverImageUrl = "/images/categorias/banho/Toalha de banho/toalha branca.jpg", IsFeatured = true, Description = "Toalha de banho branca clássica com barrado trabalhado.", CategoryId = idBanho },
            new Product { Name = "Toalha Lavabo Branca", Price = 29.90m, CoverImageUrl = "/images/categorias/banho/Toalha lavabo/toalha lavabo branca.jpg", IsFeatured = true, Description = "Toalha de lavabo branca para decoração de banheiros.", CategoryId = idBanho },
            new Product { Name = "Toalha Lavabo Rosa", Price = 29.90m, CoverImageUrl = "/images/categorias/banho/Toalha lavabo/toalha lavabo rosa.jpg", IsFeatured = true, Description = "Toalha de lavabo rosa com bordados delicados.", CategoryId = idBanho },

            // === CATEGORIA: CAMA ===
            new Product { Name = "Fronha Floral Rosa", Price = 39.90m, CoverImageUrl = "/images/categorias/cama/fronha/fronha floral rosa.jpg", IsFeatured = true, Description = "Fronha de algodao com estampa floral rosa exclusiva.", CategoryId = idCama },
            new Product { Name = "Fronha Passarinhos", Price = 39.90m, CoverImageUrl = "/images/categorias/cama/fronha/fronha passarinhos.jpg", IsFeatured = true, Description = "Fronha artesanal decorada com passarinhos.", CategoryId = idCama },
            new Product { Name = "Kit Lençol Frozen Solteiro", Price = 119.90m, CoverImageUrl = "/images/categorias/cama/kit lençol/lençol frozen solteiro.jpg", IsFeatured = true, Description = "Kit lençol infantil solteiro Frozen.", CategoryId = idCama },
            new Product { Name = "Kit Lençol Vermelho Casal", Price = 189.90m, CoverImageUrl = "/images/categorias/cama/kit lençol/lençol vermelho casal.jpg", IsFeatured = true, Description = "Kit de lençol casal vermelho vibrante.", CategoryId = idCama },

            // === CATEGORIA: INFANTIL ===
            new Product { Name = "Fralda Bordada Menina", Price = 34.90m, CoverImageUrl = "/images/categorias/infantil/fraldas/fralda menina.jpg", IsFeatured = true, Description = "Fralda com bordado personalizado para menina.", CategoryId = idInfantil },
            new Product { Name = "Fralda Bordada Menino", Price = 34.90m, CoverImageUrl = "/images/categorias/infantil/fraldas/fralda menino.jpg", IsFeatured = true, Description = "Fralda de pano com bordado artesanal para menino.", CategoryId = idInfantil },
            new Product { Name = "Kit Fralda e Babador", Price = 59.90m, CoverImageUrl = "/images/categorias/infantil/kit infantil/kit fralda e babador.jpg", IsFeatured = true, Description = "Kit contendo fralda de boca e babador coordenados.", CategoryId = idInfantil },
            new Product { Name = "Kit Fraldas de Boca", Price = 49.90m, CoverImageUrl = "/images/categorias/infantil/kit infantil/kit fraldas.jpg", IsFeatured = true, Description = "Conjunto com fraldas de boca essenciais.", CategoryId = idInfantil },
            new Product { Name = "Kit Toalhas Infantil", Price = 89.90m, CoverImageUrl = "/images/categorias/infantil/kit infantil/kit toalhas.jpg", IsFeatured = true, Description = "Kit de toalhas infantis com capuz macio.", CategoryId = idInfantil },
            new Product { Name = "Toalha Infantil Menina", Price = 45.00m, CoverImageUrl = "/images/categorias/infantil/tolhas/toalha menina.jpg", IsFeatured = true, Description = "Toalha infantil com capuz bordado para menina.", CategoryId = idInfantil },
            new Product { Name = "Toalha Infantil Menino", Price = 45.00m, CoverImageUrl = "/images/categorias/infantil/tolhas/toalha menino.jpg", IsFeatured = true, Description = "Toalha infantil com capuz bordado para menino.", CategoryId = idInfantil },

            // === CATEGORIA: MATERIAIS ===
            new Product { Name = "Agulhas de Costura Premium", Price = 15.00m, CoverImageUrl = "/images/categorias/materiais/agulhas/agulhas.webp", IsFeatured = false, Description = "Kit de agulhas de alta resistencia para costura.", CategoryId = idMateriais },
            new Product { Name = "Bastidores de Madeira para Bordado", Price = 24.90m, CoverImageUrl = "/images/categorias/materiais/bastidor/bastidores.jpg", IsFeatured = false, Description = "Bastidor de madeira com regulagem para bordado.", CategoryId = idMateriais },
            new Product { Name = "Kit de Meadas para Bordar", Price = 48.00m, CoverImageUrl = "/images/categorias/materiais/meada/kit meadas.webp", IsFeatured = false, Description = "Sortimento de meadas coloridas para bordar.", CategoryId = idMateriais },
            new Product { Name = "Meada 127 - Azul", Price = 4.50m, CoverImageUrl = "/images/categorias/materiais/meada/meada 127 (azul).webp", IsFeatured = false, Description = "Linha meada para bordado azul codigo 127.", CategoryId = idMateriais },
            new Product { Name = "Tecido Etamine para Ponto Cruz", Price = 19.90m, CoverImageUrl = "/images/categorias/materiais/tecidos/etamine.webp", IsFeatured = false, Description = "Tecido etamine ideal para ponto cruz.", CategoryId = idMateriais },

            // === CATEGORIA: MESA ===
            new Product { Name = "Pano de Prato Café", Price = 18.00m, CoverImageUrl = "/images/categorias/mesa/panos de prato/pano de prato café.jpg", IsFeatured = true, Description = "Pano de prato com estampa de café e croche.", CategoryId = idMesa },
            new Product { Name = "Pano de Prato Cerejinha", Price = 18.00m, CoverImageUrl = "/images/categorias/mesa/panos de prato/pano de prato cerejinha.jpg", IsFeatured = true, Description = "Pano de prato decorado com padrão de cerejas.", CategoryId = idMesa },
            new Product { Name = "Caminho de Mesa Rendado", Price = 79.90m, CoverImageUrl = "/images/categorias/mesa/toalha de mesa/caminho de mesa.jpg", IsFeatured = true, Description = "Caminho de mesa rendado para refeições.", CategoryId = idMesa },
            new Product { Name = "Toalha de Mesa Estampada", Price = 98.90m, CoverImageUrl = "/images/categorias/mesa/toalha de mesa/toalha de mesa.jpg", IsFeatured = true, Description = "Toalha de mesa retangular com estampa exclusiva.", CategoryId = idMesa }
        };

        context.Products.AddRange(produtosReais);
        context.SaveChanges();
    }

    // Execução assíncrona limpa no encerramento do escopo
    await AtelieDosPontinhos.Infrastructure.Identity.SeedData.SeedAsync(scope.ServiceProvider);
}

app.Run();