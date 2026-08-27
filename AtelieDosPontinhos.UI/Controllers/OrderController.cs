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

        // 👤 ROTA: /Order/Index -> Histórico do Cliente Logado
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("Api");
            var listaPedidos = new List<JsonElement>();

            try
            {
                // Faz a chamada para a rota de pedidos do usuário na API que criamos no OrdersController
                var response = await client.GetAsync($"api/orders/my?email={userEmail}");
                if (response.IsSuccessStatusCode)
                {
                    listaPedidos = await response.Content.ReadFromJsonAsync<List<JsonElement>>() ?? new List<JsonElement>();
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

            var client = _httpClientFactory.CreateClient("Api");
            var todosPedidos = new List<JsonElement>();

            try
            {
                // Chama a rota administrativa da API para trazer TODAS as vendas do sistema
                var response = await client.GetAsync($"api/orders?email={userEmail}");
                if (response.IsSuccessStatusCode)
                {
                    todosPedidos = await response.Content.ReadFromJsonAsync<List<JsonElement>>() ?? new List<JsonElement>();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    // Se o cliente tentar burlar a URL e não for admin, bloqueia o acesso
                    return Unauthorized("Acesso restrito para administradores.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao listar painel admin: {ex.Message}");
            }

            return View(todosPedidos);
        }
    }
}
