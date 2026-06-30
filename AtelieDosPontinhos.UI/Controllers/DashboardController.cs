using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AtelieDosPontinhos.UI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiUrl = "http://localhost:5006/api/Product";
        private const string ApiUsuariosUrl = "http://localhost:5006/api/Account/Users";

        public DashboardController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index() => View();
        public IActionResult AdminPanel() => View();

        // ==========================================
        // GERENCIAMENTO DE PRODUTOS
        // ==========================================

        // LISTAGEM
        [HttpGet]
        public async Task<IActionResult> GerenciarProdutos()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var produtos = await httpClient.GetFromJsonAsync<List<ProdutoViewModel>>(ApiUrl);
            return View(produtos);
        }

        // CRIAÇÃO
        [HttpGet]
        public IActionResult CriarProduto() => View();

        [HttpPost]
        public async Task<IActionResult> CriarProdutoPost(ProdutoViewModel model, IFormFile FotoArquivo)
        {
            // Processa o arquivo carregado do computador
            if (FotoArquivo != null && FotoArquivo.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await FotoArquivo.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    // Transforma em Base64 com o prefixo correto para tags <img> do HTML reconhecerem
                    model.CoverImageUrl = $"data:{FotoArquivo.ContentType};base64,{System.Convert.ToBase64String(fileBytes)}";
                }
            }
            else
            {
                ModelState.AddModelError("CoverImageUrl", "A foto do produto é obrigatória.");
            }

            if (!ModelState.IsValid) return View("CriarProduto", model);

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.PostAsJsonAsync(ApiUrl, model);

            if (response.IsSuccessStatusCode) return RedirectToAction("GerenciarProdutos");

            ModelState.AddModelError(string.Empty, "Erro ao salvar o produto na API.");
            return View("CriarProduto", model);
        }

        // EDIÇÃO
        [HttpGet]
        public async Task<IActionResult> EditarProduto(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{ApiUrl}/{id}");

            if (!response.IsSuccessStatusCode) return NotFound();

            var produtoApi = await response.Content.ReadFromJsonAsync<ProdutoViewModel>();
            return View(produtoApi);
        }

        [HttpPost]
        public async Task<IActionResult> EditarProdutoPost(ProdutoViewModel model)
        {
            if (!ModelState.IsValid) return View("EditarProduto", model);

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.PutAsJsonAsync($"{ApiUrl}/{model.Id}", model);

            if (response.IsSuccessStatusCode) return RedirectToAction("GerenciarProdutos");

            ModelState.AddModelError(string.Empty, "Erro ao atualizar o produto na API.");
            return View("EditarProduto", model);
        }

        // EXCLUSÃO
        [HttpPost]
        public async Task<IActionResult> ExcluirProduto(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            await httpClient.DeleteAsync($"{ApiUrl}/{id}");
            return RedirectToAction("GerenciarProdutos");
        }

        // ==========================================
        // GERENCIAMENTO DE USUÁRIOS
        // ==========================================

        // LISTAGEM DE USUÁRIOS
        [HttpGet]
        public async Task<IActionResult> GerenciarUsuarios()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var usuarios = await httpClient.GetFromJsonAsync<List<UsuarioViewModel>>(ApiUsuariosUrl);
                return View(usuarios);
            }
            catch (Exception)
            {
                // Evita crash caso o endpoint da API ainda esteja fora do ar
                ModelState.AddModelError(string.Empty, "Não foi possível carregar os usuários da API.");
                return View(new List<UsuarioViewModel>());
            }
        }

        // CRIAÇÃO (TELA DO FORMULÁRIO)
        [HttpGet]
        public IActionResult CriarUsuario() => View();

        // CRIAÇÃO (SUBMISSÃO DO FORMULÁRIO)
        [HttpPost]
        public async Task<IActionResult> CriarUsuarioPost(CriarUsuarioViewModel model)
        {
            if (!ModelState.IsValid) return View("CriarUsuario", model);

            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                // Observação: Ajuste a rota se a sua API usar '/api/Account/Register' para novos cadastros
                var response = await httpClient.PostAsJsonAsync("http://localhost:5006/api/Account/Register", model);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("GerenciarUsuarios");

                ModelState.AddModelError(string.Empty, "Erro ao salvar o usuário na API. Verifique as credenciais ou se o e-mail já existe.");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível se comunicar com o servidor de autenticação.");
            }

            return View("CriarUsuario", model);
        }

        // EXCLUSÃO DE USUÁRIO
        [HttpPost]
        public async Task<IActionResult> ExcluirUsuario(string id)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                await httpClient.DeleteAsync($"{ApiUsuariosUrl}/{id}");
            }
            catch (Exception)
            {
                TempData["ErroExcluir"] = "Não foi possível excluir o usuário.";
            }

            return RedirectToAction("GerenciarUsuarios");
        }
    }

    // ==========================================
    // VIEW MODELS
    // ==========================================

    public class ProdutoViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public int Stock { get; set; } = 10;
        public int CategoryId { get; set; } = 1;
    }

    public class UsuarioViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
    }

    public class CriarUsuarioViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}