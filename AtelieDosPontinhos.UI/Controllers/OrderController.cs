using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace AtelieDosPontinhos.UI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Método auxiliar para injetar o cookie de autenticação do ASP.NET Identity nas chamadas da API
        private void InjetarCookieAutenticacao(HttpClient client)
        {
            var apiCookie = HttpContext.Session.GetString("ApiCookie");

            if (string.IsNullOrEmpty(apiCookie))
            {
                apiCookie = User.FindFirst("ApiCookie")?.Value;
            }

            if (!string.IsNullOrEmpty(apiCookie))
            {
                client.DefaultRequestHeaders.Remove("Cookie");
                client.DefaultRequestHeaders.Add("Cookie", apiCookie);
            }
        }

        // 👤 ROTA: /Order/Index -> Histórico do Cliente Logado
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("ApiClient");
            InjetarCookieAutenticacao(client);

            var listaPedidos = new List<JsonElement>();

            try
            {
                string rota = client.BaseAddress != null && client.BaseAddress.ToString().EndsWith("api/")
                    ? "orders/my"
                    : "api/orders/my";

                var response = await client.GetAsync(rota);
                if (response.IsSuccessStatusCode)
                {
                    listaPedidos = await response.Content.ReadFromJsonAsync<List<JsonElement>>() ?? new List<JsonElement>();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Erro ao listar pedidos: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao listar pedidos do cliente: {ex.Message}");
            }

            ViewBag.UserEmail = userEmail;
            return View(listaPedidos);
        }

        // 👑 ROTA: /Order/AdminDashboard -> Painel Geral do Administrador
        [HttpGet]
        public async Task<IActionResult> AdminDashboard()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail)) return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("ApiClient");
            InjetarCookieAutenticacao(client);

            var todosPedidos = new List<JsonElement>();

            try
            {
                string rota = client.BaseAddress != null && client.BaseAddress.ToString().EndsWith("api/")
                    ? "orders"
                    : "api/orders";

                var response = await client.GetAsync(rota);
                if (response.IsSuccessStatusCode)
                {
                    todosPedidos = await response.Content.ReadFromJsonAsync<List<JsonElement>>() ?? new List<JsonElement>();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return Unauthorized("Acesso restrito para administradores.");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Erro ao listar painel admin: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao listar painel admin: {ex.Message}");
            }

            return View(todosPedidos);
        }

        // 🔄 ROTA: /Order/AtualizarStatus -> Altera o status do pedido via API
        [HttpPost]
        public async Task<IActionResult> AtualizarStatus(int id, string status)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            InjetarCookieAutenticacao(client);

            string rota = client.BaseAddress != null && client.BaseAddress.ToString().EndsWith("api/")
                ? $"orders/{id}/status"
                : $"api/orders/{id}/status";

            try
            {
                var response = await client.PutAsJsonAsync(rota, new { Status = status });

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Erro"] = "Erro ao atualizar status do pedido na API.";
                }
                else
                {
                    TempData["Sucesso"] = "Status do pedido atualizado com sucesso!";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao atualizar status: {ex.Message}");
                TempData["Erro"] = "Falha de comunicação com a API.";
            }

            return RedirectToAction("AdminDashboard");
        }
    }
}