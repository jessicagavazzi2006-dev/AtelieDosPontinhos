using AtelieDosPontinhos.Infrastructure.Context;
using AtelieDosPontinhos.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public class HomeController : Controller
{
    private readonly AtelieDosPontinhosDbContext _context;
    private const string SESSION_KEY = "UserFavorites"; // Chave unificada com o FavoritesController

    public HomeController(AtelieDosPontinhosDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        // 1. Busca os produtos do banco de dados
        var productsFromDb = _context.Products.ToList();

        // 2. Recupera os produtos favoritos salvos na sessão
        var favJson = HttpContext.Session.GetString(SESSION_KEY);
        var favoriteIds = new List<int>();

        if (!string.IsNullOrEmpty(favJson))
        {
            try
            {
                var favoriteProducts = JsonSerializer.Deserialize<List<ProductViewModel>>(favJson) ?? new List<ProductViewModel>();
                favoriteIds = favoriteProducts.Select(p => p.Id).ToList();
            }
            catch { }
        }

        // 3. Converte para ProductViewModel e define se está favoritado
        var productViewModels = productsFromDb.Select(p => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            CoverImageUrl = p.CoverImageUrl,
            Description = p.Description ?? string.Empty,
            IsFavorited = favoriteIds.Contains(p.Id) // Verifica se o ID está na lista da sessão
        }).ToList();

        // 4. Retorna para a View
        return View(productViewModels);
    }
}