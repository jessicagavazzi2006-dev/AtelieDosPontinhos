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
                        var apiCookie = response.Headers.FirstOrDefault(h => h.Key == "Set-Cookie").Value?.FirstOrDefault() ?? string.Empty;

                        if (string.IsNullOrEmpty(apiCookie) && response.Headers.TryGetValues("set-cookie", out var cookieValues))
                        {
                            apiCookie = string.Join("; ", cookieValues);
                        }

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email, loginResult.Email),
                            new Claim(ClaimTypes.Name, loginResult.Email),
                            new Claim("ApiCookie", apiCookie)
                        };

                        if (loginResult.Roles != null && loginResult.Roles.Any())
                        {
                            foreach (var role in loginResult.Roles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, role));
                            }
                        }

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        HttpContext.Session.SetString("UserEmail", loginResult.Email);

                        if (!string.IsNullOrEmpty(apiCookie))
                        {
                            HttpContext.Session.SetString("ApiCookie", apiCookie);
                        }

                        if (loginResult.Roles != null && loginResult.Roles.Any())
                        {
                            HttpContext.Session.SetString("UserRoles", string.Join(",", loginResult.Roles));
                        }

                        // Redirecionamento inteligente: Admin vai para o Dashboard, Cliente vai para a Home
                        if (loginResult.Roles != null && loginResult.Roles.Contains("Admin"))
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
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

            var apiModel = new
            {
                Nome = model.UserName,
                Email = model.Email,
                Password = model.Password,
                Role = "Cliente",
                CEP = model.CEP,
                Logradouro = model.Logradouro,
                Numero = model.Numero,
                Complemento = model.Complemento ?? "",
                Bairro = model.Bairro,
                Cidade = model.Cidade,
                Estado = model.Estado,
                TipoPagamento = model.TipoPagamento,
                NomeNoCartao = model.NomeNoCartao ?? "",
                NumeroCartaoMascarado = model.NumeroCartaoMascarado ?? ""
            };

            var client = _httpClientFactory.CreateClient();

            try
            {
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(apiModel, options);
                var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{_apiUrl}/register", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cadastro realizado com sucesso! Faça seu login.";
                    return RedirectToAction("Login");
                }

                var erroApi = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"A API recusou o cadastro. Detalhe: {erroApi}");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Erro ao se comunicar com a API para salvar o cliente.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("ApiClient");

            var apiCookie = HttpContext.Session.GetString("ApiCookie") ?? User.FindFirst("ApiCookie")?.Value;
            if (!string.IsNullOrEmpty(apiCookie))
            {
                client.DefaultRequestHeaders.Remove("Cookie");
                client.DefaultRequestHeaders.Add("Cookie", apiCookie);
            }

            var viewModel = new UserProfileViewModel { Email = userEmail };

            try
            {
                string rotaPerfil = client.BaseAddress != null && client.BaseAddress.ToString().EndsWith("api/")
                    ? "User/profile"
                    : "api/User/profile";

                var response = await client.GetAsync(rotaPerfil);

                if (response.IsSuccessStatusCode)
                {
                    var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var dadosPerfil = await response.Content.ReadFromJsonAsync<UserProfileViewModel>(jsonOptions);

                    if (dadosPerfil != null)
                    {
                        viewModel = dadosPerfil;
                        viewModel.Email = userEmail;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao carregar perfil: {ex.Message}");
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            model.Email = userEmail;

            var client = _httpClientFactory.CreateClient("ApiClient");

            var apiCookie = HttpContext.Session.GetString("ApiCookie") ?? User.FindFirst("ApiCookie")?.Value;
            if (!string.IsNullOrEmpty(apiCookie))
            {
                client.DefaultRequestHeaders.Remove("Cookie");
                client.DefaultRequestHeaders.Add("Cookie", apiCookie);
            }

            try
            {
                string rotaUpdate = client.BaseAddress != null && client.BaseAddress.ToString().EndsWith("api/")
                    ? "User/update-profile"
                    : "api/User/update-profile";

                var response = await client.PutAsJsonAsync(rotaUpdate, model);

                if (response.IsSuccessStatusCode || ((int)response.StatusCode >= 200 && (int)response.StatusCode < 300))
                {
                    TempData["Sucesso"] = "Dados alterados com sucesso!";
                    return RedirectToAction("Profile");
                }
                else
                {
                    var erroDetalhe = await response.Content.ReadAsStringAsync();
                    TempData["Erro"] = $"Erro da API: {erroDetalhe}";
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao comunicar com a API: {ex.Message}";
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session?.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}