using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting; // 👈 Necessário para descobrir as pastas do sistema
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AtelieDosPontinhos.UI.Models;

namespace AtelieDosPontinhos.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        // Construtor para o .NET nos dar acesso às pastas físicas do projeto (wwwroot)
        public ProductController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        // LISTA GLOBAL NA MEMÓRIA
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

        public IActionResult Index()
        {
            return View(_products);
        }

        public IActionResult Detalhes(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel novoProduto, IFormFile FotoArquivo)
        {
            if (FotoArquivo != null && FotoArquivo.Length > 0)
            {
                // 1. Define o caminho da pasta wwwroot/uploads
                string pastaUploads = Path.Combine(_environment.WebRootPath, "uploads");

                // Se a pasta não existir no seu computador, o código cria ela automaticamente
                if (!Directory.Exists(pastaUploads))
                {
                    Directory.CreateDirectory(pastaUploads);
                }

                // 2. Cria um nome único para o arquivo para não dar conflito (Ex: nome-da-foto-123456.jpg)
                string nomeUnicoArquivo = Guid.NewGuid().ToString() + "_" + Path.GetFileName(FotoArquivo.FileName);
                string caminhoCompletoNoPC = Path.Combine(pastaUploads, nomeUnicoArquivo);

                // 3. Salva o arquivo fisicamente na pasta do projeto
                using (var stream = new FileStream(caminhoCompletoNoPC, FileMode.Create))
                {
                    await FotoArquivo.CopyToAsync(stream);
                }

                // 4. Salva o caminho virtual no banco/memória para o HTML conseguir ler depois
                novoProduto.CoverImageUrl = "/uploads/" + nomeUnicoArquivo;
            }

            // Removemos qualquer erro de validação que o .NET coloque automaticamente no campo da imagem
            ModelState.Remove(nameof(novoProduto.CoverImageUrl));

            if (ModelState.IsValid)
            {
                novoProduto.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
                _products.Add(novoProduto);

                return RedirectToAction("Index");
            }

            // Se cair aqui, é porque algum outro campo falhou (ex: preço inválido ou nome em branco)
            return View(novoProduto);
        }
    }
}