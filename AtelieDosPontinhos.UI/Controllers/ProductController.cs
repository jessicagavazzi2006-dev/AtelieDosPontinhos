using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace SeuProjeto.Controllers
{
    public class ProductController : Controller
    {
        // 🟢 LISTA DE PRODUTOS (vai alimentar o foreach da View)
        public IActionResult Index()
        {
            var products = ObterProducts();

            return View(products);
        }

        // 🟡 DETALHES DO PRODUTO
        public IActionResult Detalhes(int id)
        {
            var product = ObterProducts()
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // 🔵 SIMULA BANCO (depois vira banco real)
        private List<ProductViewModel> ObterProducts()
        {
            return new List<ProductViewModel>
            {
                new ProductViewModel
                {
                    Id = 1,
                    Name = "Fronha Floral Rosa",
                    Price = 29.90M,
                    Description = "Fronha artesanal feita com tecido de alta qualidade.",
                    Image = "~/images/categorias/cama/fronha/fronha floral rosa.jpg"
                },
                new ProductViewModel
                {
                    Id = 2,
                    Name = "Pano de Prato Café",
                    Price = 19.90M,
                    Description = "Pano de prato decorado estilo café.",
                    Image = "~/images/categorias/mesa/panos de prato/pano de prato café.jpg"
                }
            };
        }
    }

    // 🧠 VIEWMODEL (melhor prática: fora do controller na vida real)
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}