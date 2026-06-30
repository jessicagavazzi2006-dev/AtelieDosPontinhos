using AtelieDosPontinhos.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace AtelieDosPontinhos.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiUrl = "http://localhost:5006/api/account";

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 🔐 LOGIN (POST) - PROCESSA O LOGON E VALIDA PERMISSÕES DO SISTEMA
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();

            try
            {
                var response = await client.PostAsJsonAsync($"{_apiUrl}/login", new
                {
                    Email = model.Email,
                    Password = model.Password
                });

                if (response.IsSuccessStatusCode)
                {
                    // Configuração para mapear chaves vindas em camelCase (minúsculo) ou PascalCase
                    var jsonOptions = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var loginResult = await response.Content.ReadFromJsonAsync<LoginApiResponse>(jsonOptions);

                    if (loginResult != null && loginResult.Succeeded)
                    {
                        // Salva os dados na Sessão para o Layout usar
                        HttpContext.Session.SetString("UserEmail", loginResult.Email);

                        if (loginResult.Roles != null && loginResult.Roles.Any())
                        {
                            HttpContext.Session.SetString("UserRoles", string.Join(",", loginResult.Roles));

                            // 👑 Identifica o Administrador de forma segura independente da formatação do banco/API
                            bool ehAdmin = loginResult.Roles.Any(r => r.Trim().Equals("Admin", System.StringComparison.OrdinalIgnoreCase));

                            if (ehAdmin)
                            {
                                // Redireciona para o Painel Administrativo do Dashboard
                                return RedirectToAction("AdminPanel", "Dashboard");
                            }
                        }

                        // 🛍️ SE FOR CLIENTE: Vai para a vitrine da loja
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    var errorObj = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
                    if (errorObj != null && errorObj.ContainsKey("message"))
                    {
                        ModelState.AddModelError("", errorObj["message"]?.ToString() ?? "Credenciais inválidas.");
                        return View(model);
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("", "Não foi possível conectar ao servidor de autenticação (API).");
                return View(model);
            }

            ModelState.AddModelError("", "E-mail ou senha inválidos.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterClienteViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var apiModel = new { Email = model.Email, Password = model.Password, Role = "Cliente" };
            var client = _httpClientFactory.CreateClient();

            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(apiModel), Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{_apiUrl}/register", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cadastro realizado com sucesso! Faça seu login.";
                    return RedirectToAction("Login");
                }
                ModelState.AddModelError(string.Empty, "A API recusou o cadastro. Verifique os requisitos de senha.");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Erro ao se comunicar com a API para salvar o cliente.");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session?.Clear();
            return RedirectToAction("Login");
        }
    }
}