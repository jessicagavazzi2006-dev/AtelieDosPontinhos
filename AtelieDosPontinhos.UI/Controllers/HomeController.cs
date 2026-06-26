using AtelieDosPontinhos.Infrastructure.Context;
using AtelieDosPontinhos.UI.Models; // Garante o acesso ao ProductViewModel
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class HomeController : Controller
{
    private readonly AtelieDosPontinhosDbContext _context;

    public HomeController(AtelieDosPontinhosDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        // 1. Busca os produtos originais do banco de dados
        var productsFromDb = _context.Products.ToList();

        // 2. Converte a lista de 'Product' para 'ProductViewModel' esperada pela View
        var productViewModels = productsFromDb.Select(p => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            CoverImageUrl = p.CoverImageUrl // Use o nome exato da propriedade de imagem do seu modelo
        }).ToList();

        // 3. Envia a lista convertida e correta para a View
        return View(productViewModels);
    }
}
