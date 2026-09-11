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
        // 1. Busca os produtos em destaque aceitando tanto a flag Destaque quanto IsFeatured (evita falha por nome do campo na entidade DB)
        var destaqueFromDb = _context.Products.Where(p => p.Destaque || p.IsFeatured).ToList();
        var todosFromDb = _context.Products.ToList();

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

        // 3. Converte os produtos em destaque para ViewModel mapeando status de favorito e flags de destaque
        var destaqueViewModels = destaqueFromDb.Select(p => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            CoverImageUrl = p.CoverImageUrl,
            Description = p.Description ?? string.Empty,
            Destaque = true,
            IsFeatured = true,
            IsFavorited = favoriteIds.Contains(p.Id)
        }).ToList();

        // 4. Converte todos os produtos para ViewModel mapeando status de favorito e flags de destaque
        var todosViewModels = todosFromDb.Select(p => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            CoverImageUrl = p.CoverImageUrl,
            Description = p.Description ?? string.Empty,
            Destaque = p.Destaque || p.IsFeatured,
            IsFeatured = p.Destaque || p.IsFeatured,
            IsFavorited = favoriteIds.Contains(p.Id)
        }).ToList();

        // 5. Disponibiliza ambas as listas para a View através da ViewBag
        ViewBag.ProdutosDestaque = destaqueViewModels;
        ViewBag.TodosProdutos = todosViewModels;

        // Retorna a View passando a lista de destaques como modelo principal
        return View(destaqueViewModels);
    }
}