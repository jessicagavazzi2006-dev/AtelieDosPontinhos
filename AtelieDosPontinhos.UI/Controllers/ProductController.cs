using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AtelieDosPontinhos.UI.Models;
using System.Net.Http; // 🌟 NOVO
using System.Net.Http.Json; // 🌟 NOVO

namespace AtelieDosPontinhos.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpClientFactory _httpClientFactory; // 🌟 NOVO

        // Construtor recebendo o HttpClientFactory para conversar com a API
        public ProductController(IWebHostEnvironment environment, IHttpClientFactory httpClientFactory)
        {
            _environment = environment;
            _httpClientFactory = httpClientFactory; // 🌟 NOVO
        }

        // LISTA GLOBAL NA MEMÓRIA (Mantida apenas como segurança/fallback)
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

        // 🌟 MUDANÇA 1: O método Index agora busca TODOS os produtos reais direto da API!
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("Api");
            List<ProductViewModel> produtos = null;

            try
            {
                var response = await client.GetAsync("api/Product");
                if (response.IsSuccessStatusCode)
                {
                    produtos = await response.Content.ReadFromJsonAsync<List<ProductViewModel>>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao listar produtos da API: {ex.Message}");
            }

            if (produtos == null)
            {
                produtos = _products;
            }

            return View(produtos);
        }

        // Método de detalhes buscando da API (que já havíamos arrumado)
        public async Task<IActionResult> Detalhes(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            ProductViewModel product = null;

            try
            {
                var response = await client.GetAsync($"api/Product/{id}");
                if (response.IsSuccessStatusCode)
                {
                    product = await response.Content.ReadFromJsonAsync<ProductViewModel>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao buscar detalhes na API: {ex.Message}");
            }

            if (product == null)
            {
                product = _products.FirstOrDefault(p => p.Id == id);
            }

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 🌟 MUDANÇA 2: O método Create agora envia o novo produto para salvar no banco real da API!
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel novoProduto, IFormFile FotoArquivo)
        {
            if (FotoArquivo != null && FotoArquivo.Length > 0)
            {
                string pastaUploads = Path.Combine(_environment.WebRootPath, "uploads");

                if (!Directory.Exists(pastaUploads))
                {
                    Directory.CreateDirectory(pastaUploads);
                }

                string nomeUnicoArquivo = Guid.NewGuid().ToString() + "_" + Path.GetFileName(FotoArquivo.FileName);
                string caminhoCompletoNoPC = Path.Combine(pastaUploads, nomeUnicoArquivo);

                using (var stream = new FileStream(caminhoCompletoNoPC, FileMode.Create))
                {
                    await FotoArquivo.CopyToAsync(stream);
                }

                novoProduto.CoverImageUrl = "/uploads/" + nomeUnicoArquivo;
            }

            ModelState.Remove(nameof(novoProduto.CoverImageUrl));

            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("Api");

                try
                {
                    // Faz o POST enviando o produto em formato JSON para a API
                    var response = await client.PostAsJsonAsync("api/Product", novoProduto);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Erro ao salvar o produto na API.");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Erro ao enviar produto para a API: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Não foi possível conectar à API.");
                }
            }

            return View(novoProduto);
        }
    }
}
