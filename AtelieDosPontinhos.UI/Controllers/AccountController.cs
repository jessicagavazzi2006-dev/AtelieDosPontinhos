using AtelieDosPontinhos.UI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

        // LOGIN (POST) - PROCESSA O LOGON E VALIDA PERMISSÕES DO SISTEMA
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
                    var jsonOptions = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var loginResult = await response.Content.ReadFromJsonAsync<LoginApiResponse>(jsonOptions);

                    if (loginResult != null && loginResult.Succeeded)
                    {
                        // 🔐 Extrai o cookie da resposta Set-Cookie
                        var apiCookie = response.Headers.FirstOrDefault(h => h.Key == "Set-Cookie").Value?.FirstOrDefault() ?? string.Empty;

                        // Se não houver Set-Cookie no header padrão, tenta extrair via TryGetValues
                        if (string.IsNullOrEmpty(apiCookie) && response.Headers.TryGetValues("set-cookie", out var cookieValues))
                        {
                            apiCookie = string.Join("; ", cookieValues);
                        }

                        // Cria as claims do usuário autenticado
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email, loginResult.Email),
                            new Claim(ClaimTypes.Name, loginResult.Email),
                            new Claim("ApiCookie", apiCookie)  // Armazena o cookie na Claim
                        };

                        // Adiciona as roles às claims
                        if (loginResult.Roles != null && loginResult.Roles.Any())
                        {
                            foreach (var role in loginResult.Roles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, role));
                            }
                        }

                        // Cria a identidade autenticada da UI
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                        };

                        // Realiza login no contexto MVC da UI
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        // 💾 Grava dados na sessão (incluindo o Cookie da API)
                        HttpContext.Session.SetString("UserEmail", loginResult.Email);

                        if (!string.IsNullOrEmpty(apiCookie))
                        {
                            HttpContext.Session.SetString("ApiCookie", apiCookie);
                        }

                        if (loginResult.Roles != null && loginResult.Roles.Any())
                        {
                            HttpContext.Session.SetString("UserRoles", string.Join(",", loginResult.Roles));
                        }

                        // 🎉 Redireciona para a Tela Inicial
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

        // POST: Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterClienteViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cep = Request.Form["CEP"].ToString();
            var logradouro = Request.Form["Logradouro"].ToString();
            var numero = Request.Form["Numero"].ToString();
            var complemento = Request.Form["Complemento"].ToString();
            var bairro = Request.Form["Bairro"].ToString();
            var cidade = Request.Form["Cidade"].ToString();
            var estado = Request.Form["Estado"].ToString();
            var tipoPagamento = Request.Form["TipoPagamento"].ToString();
            var nomeNoCartao = Request.Form["NomeNoCartao"].ToString();
            var numeroCartaoMascarado = Request.Form["NumeroCartaoMascarado"].ToString();

            var apiModel = new
            {
                Email = model.Email,
                Password = model.Password,
                Role = "Cliente",
                CEP = cep,
                Logradouro = logradouro,
                Numero = numero,
                Complemento = complemento,
                Bairro = bairro,
                Cidade = cidade,
                Estado = estado,
                TipoPagamento = tipoPagamento,
                NomeNoCartao = nomeNoCartao,
                NumeroCartaoMascarado = numeroCartaoMascarado
            };

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
                ModelState.AddModelError(string.Empty, "A API recusou o cadastro. Verifique os requisitos de senha ou se o e-mail já existe.");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Erro ao se comunicar com a API para salvar o cliente com dados Express.");
            }

            return View(model);
        }

        // LOGOUT: Limpa a sessão e desautentica os cookies
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session?.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}