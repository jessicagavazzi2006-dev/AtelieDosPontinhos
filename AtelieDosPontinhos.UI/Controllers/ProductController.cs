using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AtelieDosPontinhos.UI.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace AtelieDosPontinhos.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpClientFactory _httpClientFactory;

        // Configuração para aceitar propriedades minúsculas/maiúsculas do JSON da API
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public ProductController(IWebHostEnvironment environment, IHttpClientFactory httpClientFactory)
        {
            _environment = environment;
            _httpClientFactory = httpClientFactory;
        }

        // LISTA GLOBAL NA MEMÓRIA (Mantida como fallback)
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

        // 📦 GET: Lista os produtos da API e exibe a tabela
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("Api");
            List<ProductViewModel> produtos = null;

            try
            {
                var response = await client.GetAsync("api/Product");
                if (response.IsSuccessStatusCode)
                {
                    produtos = await response.Content.ReadFromJsonAsync<List<ProductViewModel>>(_jsonOptions);
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

        // 👁️ GET: Detalhes do produto
        [HttpGet]
        public async Task<IActionResult> Detalhes(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            ProductViewModel product = null;

            try
            {
                // Tenta buscar no endpoint padrão de produto
                var response = await client.GetAsync($"api/Product/{id}");

                if (response.IsSuccessStatusCode)
                {
                    product = await response.Content.ReadFromJsonAsync<ProductViewModel>(_jsonOptions);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"API retornou HTTP Status: {response.StatusCode} para o ID {id}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao conectar na API para detalhes do produto {id}: {ex.Message}");
            }

            // Fallback para lista em memória caso não encontre na API
            if (product == null)
            {
                product = _products.FirstOrDefault(p => p.Id == id);
            }

            // Garante o mapeamento do nome caso a propriedade venha vazia do JSON
            if (product != null && string.IsNullOrEmpty(product.Nome) && !string.IsNullOrEmpty(product.Name))
            {
                product.Nome = product.Name;
            }

            if (product == null)
            {
                return NotFound($"O produto de código #{id} não foi localizado nem na API e nem na lista de fallback.");
            }

            return View(product);
        }

        // ➕ GET: Redireciona para o formulário na DashboardController
        [HttpGet]
        public IActionResult Create()
        {
            return RedirectToAction("CriarProduto", "Dashboard");
        }

        // 💾 POST: Envia o novo produto para salvar na API
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

        // 🗑️ GET: Confirmação de exclusão
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            ProductViewModel product = null;

            try
            {
                var response = await client.GetAsync($"api/Product/{id}");
                if (response.IsSuccessStatusCode)
                {
                    product = await response.Content.ReadFromJsonAsync<ProductViewModel>(_jsonOptions);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao buscar produto para deleção: {ex.Message}");
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

        // 🗑️ POST: Executa a exclusão na API
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");

            try
            {
                var response = await client.DeleteAsync($"api/Product/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var localItem = _products.FirstOrDefault(p => p.Id == id);
                    if (localItem != null)
                    {
                        _products.Remove(localItem);
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao deletar produto na API: {ex.Message}");
            }

            var fallbackItem = _products.FirstOrDefault(p => p.Id == id);
            if (fallbackItem != null)
            {
                _products.Remove(fallbackItem);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}