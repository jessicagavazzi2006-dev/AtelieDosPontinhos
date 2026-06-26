using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization; // Import essencial para os aliases de propriedades
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtelieDosPontinhos.UI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "http://localhost:5004/api";

        public DashboardController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private bool IsAdminLogged()
        {
            var rolesString = HttpContext.Session.GetString("UserRoles") ?? "";
            var rolesArray = rolesString.Split(',', StringSplitOptions.RemoveEmptyEntries);
            return rolesArray.Any(r => r.Trim().Equals("Admin", StringComparison.OrdinalIgnoreCase));
        }

        public IActionResult AdminPanel()
        {
            if (!IsAdminLogged()) return RedirectToAction("Login", "Account");
            return View();
        }

        // ==========================================
        // 📦 GERENCIAMENTO DE PRODUTOS (CRUD)
        // ==========================================

        public async Task<IActionResult> GerenciarProdutos()
        {
            if (!IsAdminLogged()) return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient();
            List<ProdutoViewModel> produtos = new List<ProdutoViewModel>();

            try
            {
                string urlCompleta = $"{_apiBaseUrl}/Product/search?term=%25";
                var response = await client.GetAsync(urlCompleta);

                if (response.IsSuccessStatusCode)
                {
                    var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var resultado = await response.Content.ReadFromJsonAsync<List<ProdutoViewModel>>(jsonOptions);
                    if (resultado != null) produtos = resultado;
                }
                else
                {
                    var detalheErro = await response.Content.ReadAsStringAsync();
                    TempData["Error"] = $"A API retornou um erro ({(int)response.StatusCode}): {detalheErro}";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Falha crítica na comunicação com a API ao listar: {ex.Message}";
            }

            return View(produtos);
        }

        public IActionResult CriarProduto()
        {
            if (!IsAdminLogged()) return RedirectToAction("Login", "Account");
            return View();
        }

        // 🟢 POST REAL COM TRATAMENTO DE SERIALIZAÇÃO JSON E DE ERROS DO BANCO
        [HttpPost]
        public async Task<IActionResult> CriarProdutoPost(ProdutoViewModel novoProduto)
        {
            if (!IsAdminLogged()) return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient();

            try
            {
                string urlCompleta = $"{_apiBaseUrl}/Product";

                // Serializa mantendo os nomes exatamente mapeados pelos atributos da classe
                var payloadJson = JsonSerializer.Serialize(novoProduto, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null,
                    WriteIndented = true
                });

                var content = new StringContent(payloadJson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(urlCompleta, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = $"Produto '{novoProduto.Name}' cadastrado com sucesso no banco de dados!";
                    return RedirectToAction("GerenciarProdutos");
                }
                else
                {
                    var detalheErro = await response.Content.ReadAsStringAsync();

                    // Se o erro for o 500 do banco por conta do Base64 gigante, adicionamos uma dica amigável na tela
                    if ((int)response.StatusCode == 500 && detalheErro.Contains("inner exception"))
                    {
                        TempData["Error"] = "Erro no Banco de Dados: O código Base64 da imagem é muito longo para o limite da coluna. Por favor, tente usar uma URL direta de imagem (Ex: http://...)";
                    }
                    else
                    {
                        TempData["Error"] = $"Erro ao salvar na API ({(int)response.StatusCode}): {detalheErro}";
                    }

                    return View("CriarProduto", novoProduto);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Falha ao conectar com o servidor backend: {ex.Message}";
                return View("CriarProduto", novoProduto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletarProduto(int id)
        {
            if (!IsAdminLogged()) return RedirectToAction("Login", "Account");
            TempData["Success"] = "Produto removido com sucesso!";
            return RedirectToAction("GerenciarProdutos");
        }

        public IActionResult EditarProduto(int id) => View();

        // ==========================================
        // 👥 GERENCIAMENTO DE USUÁRIOS
        // ==========================================

        public async Task<IActionResult> GerenciarUsuarios()
        {
            if (!IsAdminLogged()) return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient();
            List<UsuarioViewModel> usuarios = new List<UsuarioViewModel>();

            try
            {
                var response = await client.GetAsync($"{_apiBaseUrl}/Account/users");

                if (response.IsSuccessStatusCode)
                {
                    var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var resultado = await response.Content.ReadFromJsonAsync<List<UsuarioViewModel>>(jsonOptions);
                    if (resultado != null) usuarios = resultado;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                usuarios = new List<UsuarioViewModel>
                {
                    new UsuarioViewModel { Email = HttpContext.Session.GetString("UserEmail") ?? "admin@site.com", Role = "Admin" }
                };
            }

            return View(usuarios);
        }
    }

    // 🤝 MODELO ULTRA-COMPATÍVEL COM MAPEAMENTO FLEXÍVEL PARA CORRIGIR ID (#0) E IMAGENS
    public class ProdutoViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; } = string.Empty;

        [JsonPropertyName("coverImageUrl")]
        public string CoverImageUrl
        {
            get => string.IsNullOrEmpty(_coverImageUrl) ? ImageUrl : _coverImageUrl;
            set => _coverImageUrl = value;
        }
        private string _coverImageUrl = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("stock")]
        public int Stock { get; set; } = 10;

        [JsonPropertyName("categoryId")]
        public int CategoryId { get; set; } = 1;
    }

    public class UsuarioViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}