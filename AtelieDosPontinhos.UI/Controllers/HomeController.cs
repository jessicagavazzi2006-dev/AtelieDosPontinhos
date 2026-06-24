

using AtelieDosPontinhos.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;

namespace AtelieDosPontinhos.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly AtelieDosPontinhosDbContext _context;

        public HomeController(AtelieDosPontinhosDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
    }
}