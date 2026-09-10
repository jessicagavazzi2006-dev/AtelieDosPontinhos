using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro: {ex.Message}");
            }

            ViewBag.UserEmail = userEmail;
            return View(listaPedidos);
        }

        [HttpGet]
        public async Task<IActionResult> AdminDashboard(string statusFiltro)
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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro: {ex.Message}");
            }

            // Aplica o filtro por status se informado
            var pedidosFiltrados = todosPedidos;
            if (!string.IsNullOrEmpty(statusFiltro))
            {
                pedidosFiltrados = todosPedidos.Where(p =>
                {
                    string statusStr = null;

                    if (p.TryGetProperty("status", out var val) && val.ValueKind != JsonValueKind.Null)
                    {
                        statusStr = val.GetString();
                    }
                    else if (p.TryGetProperty("Status", out var val2) && val2.ValueKind != JsonValueKind.Null)
                    {
                        statusStr = val2.GetString();
                    }

                    return statusStr?.Equals(statusFiltro, StringComparison.OrdinalIgnoreCase) == true;
                }).ToList();
            }

            // Calcula o faturamento total com base na lista filtrada, checando 'valorTotal', 'total' ou 'Total'
            decimal faturamentoFiltrado = 0;
            foreach (var p in pedidosFiltrados)
            {
                if (p.TryGetProperty("valorTotal", out var vt) && vt.ValueKind != JsonValueKind.Null)
                {
                    faturamentoFiltrado += vt.GetDecimal();
                }
                else if (p.TryGetProperty("total", out var tot) && tot.ValueKind != JsonValueKind.Null)
                {
                    faturamentoFiltrado += tot.GetDecimal();
                }
                else if (p.TryGetProperty("Total", out var Tot) && Tot.ValueKind != JsonValueKind.Null)
                {
                    faturamentoFiltrado += Tot.GetDecimal();
                }
            }

            ViewBag.FaturamentoFiltrado = faturamentoFiltrado;
            ViewBag.StatusFiltroAtual = statusFiltro;

            return View(pedidosFiltrados);
        }

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
            catch (Exception)
            {
                TempData["Erro"] = "Falha de comunicação com a API.";
            }

            return RedirectToAction("AdminDashboard");
        }
    }
}