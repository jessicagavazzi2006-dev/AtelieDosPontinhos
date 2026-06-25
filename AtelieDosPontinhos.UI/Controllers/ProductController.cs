using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using AtelieDosPontinhos.UI.Models;

namespace AtelieDosPontinhos.UI.Controllers
{
    public class ProductController : Controller
    {
        // 🛑 LISTA GLOBAL NA MEMÓRIA: Agora ela mantém os produtos salvos enquanto o app estiver rodando!
        private static List<ProductViewModel> _products = new List<ProductViewModel>
        {
            new ProductViewModel
            {
                Id = 1,
                Nome = "Fronha Floral Rosa",
                Preco = 29.90M,
                Descricao = "Fronha artesanal feita com tecido de alta qualidade.",
                CoverImageUrl = "/images/categorias/cama/fronha/fronha floral rosa.jpg"
            },
            new ProductViewModel
            {
                Id = 2,
                Nome = "Pano de Prato Café",
                Preco = 19.90M,
                Descricao = "Pano de prato decorado estilo café.",
                CoverImageUrl = "/images/categorias/mesa/panos de prato/pano de prato café.jpg"
            }
        };

        // 🟢 1. LISTA DE PRODUTOS (Para o botão "Ver Todos")
        public IActionResult Index()
        {
            // Retorna a lista que agora aceita novas inserções
            return View(_products);
        }

        // 🟡 2. DETALHES DO PRODUTO
        public IActionResult Detalhes(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // ➕ 3. CARREGA A PÁGINA DE CADASTRO
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 💾 4. RECEBE OS DADOS E SALVA DE VERDADE NA MEMÓRIA
        [HttpPost]
        public IActionResult Create(ProductViewModel novoProduto)
        {
            if (ModelState.IsValid)
            {
                // Gera um novo ID baseado no último item da lista
                novoProduto.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;

                // ADICIONA O PRODUTO NA LISTA!
                _products.Add(novoProduto);

                // Redireciona para a página de listagem para você ver o produto adicionado na tabela
                return RedirectToAction("Index");
            }

            // Se faltou preencher algo, recarrega o formulário
            return View(novoProduto);
        }
    }
}