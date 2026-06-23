using Microsoft.AspNetCore.Mvc;

namespace SeuProjeto.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Detalhes(int id)
        {
            // simulação de banco (depois vira API)
            var Product = ObterProductPorId(id);

            return View(Product);
        }

        private ProductViewModel ObterProductPorId(int id)
        {
            var Products = new List<ProductViewModel>
            {
                new ProductViewModel
                {
                    Id = 1,
                    Nome = "Fronha Floral Rosa",
                    Preco = 29.90M,
                    Descricao = "Fronha artesanal feita com tecido de alta qualidade.",
                    Imagem = "~/images/categorias/cama/fronha/fronha floral rosa.jpg"
                },
                new ProductViewModel
                {
                    Id = 2,
                    Nome = "Pano de Prato Café",
                    Preco = 19.90M,
                    Descricao = "Pano de prato decorado estilo café.",
                    Imagem = "~/images/categorias/mesa/panos de prato/pano de prato café.jpg"
                }
            };

            return Products.FirstOrDefault(p => p.Id == id);
        }
    }

    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
    }
}
