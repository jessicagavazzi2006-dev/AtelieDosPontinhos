using AtelieDosPontinhos.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace AtelieDosPontinhos.UI.Controllers
{
    // DTOs locais fortemente tipados para garantir contrato perfeito com a API
    public class CreateOrderDto
    {
        public string EmailUsuario { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public string MetodoPagamento { get; set; } = string.Empty;
        public string? CEP { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }

    public class CartController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CartController(IHttpClientFactory httpClientFactory)
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

            System.Diagnostics.Debug.WriteLine($"🔍 DEBUG COOKIE NA SESSÃO: '{(string.IsNullOrEmpty(apiCookie) ? "VAZIO / NULO" : apiCookie)}'");

            if (!string.IsNullOrEmpty(apiCookie))
            {
                client.DefaultRequestHeaders.Remove("Cookie");
                client.DefaultRequestHeaders.Add("Cookie", apiCookie);
            }
        }

        public IActionResult Index()
        {
            var carrinho = ObterCarrinhoDaSessao();
            decimal total = carrinho?.Sum(i => (i.Produto?.Price ?? 0m) * i.Quantidade) ?? 0m;

            ViewBag.ValorTotal = total;
            return View(carrinho ?? new List<CartItemViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarAoCarrinhoAjax(int id, int quantidade = 1)
        {
            if (id <= 0)
            {
                return Json(new { success = false, message = "ID de produto inválido." });
            }

            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                if (client.BaseAddress == null)
                {
                    return Json(new { success = false, message = "Serviço de API não configurado." });
                }

                string rota = client.BaseAddress.ToString().EndsWith("api/")
                    ? $"Product/{id}"
                    : $"api/Product/{id}";

                var response = await client.GetAsync(rota);

                if (!response.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "Produto não encontrado." });
                }

                var json = await response.Content.ReadFromJsonAsync<JsonElement>();

                int parsedId = id;
                if (json.TryGetProperty("id", out var idProp) && idProp.ValueKind == JsonValueKind.Number)
                    parsedId = idProp.GetInt32();

                string name = null;
                if (json.TryGetProperty("name", out var n1) && n1.ValueKind == JsonValueKind.String)
                    name = n1.GetString();
                else if (json.TryGetProperty("nome", out var n2) && n2.ValueKind == JsonValueKind.String)
                    name = n2.GetString();

                string description = null;
                if (json.TryGetProperty("description", out var d1) && d1.ValueKind == JsonValueKind.String)
                    description = d1.GetString();

                string imagem = null;
                if (json.TryGetProperty("coverImageUrl", out var i1) && i1.ValueKind == JsonValueKind.String)
                    imagem = i1.GetString();
                else if (json.TryGetProperty("imageUrl", out var i2) && i2.ValueKind == JsonValueKind.String)
                    imagem = i2.GetString();

                decimal preco = 0m;
                if (json.TryGetProperty("price", out var p1) && p1.ValueKind == JsonValueKind.Number)
                    preco = p1.GetDecimal();
                else if (json.TryGetProperty("preco", out var p2) && p2.ValueKind == JsonValueKind.Number)
                    preco = p2.GetDecimal();

                var produtoViewModel = new ProductViewModel
                {
                    Id = parsedId,
                    Name = !string.IsNullOrEmpty(name) ? name : $"Produto {id}",
                    Price = preco,
                    CoverImageUrl = !string.IsNullOrEmpty(imagem) ? imagem : "/images/logo/logo.png",
                    Description = description ?? string.Empty
                };

                var carrinho = ObterCarrinhoDaSessao();
                var itemExistente = carrinho.FirstOrDefault(c => c.Produto != null && c.Produto.Id == produtoViewModel.Id);

                if (itemExistente == null)
                {
                    carrinho.Add(new CartItemViewModel { Produto = produtoViewModel, Quantidade = quantidade });
                }
                else
                {
                    itemExistente.Quantidade += quantidade;
                }

                SalvarCarrinhoNaSessao(carrinho);

                int totalCount = carrinho.Sum(x => x.Quantidade);
                return Json(new { success = true, totalCount });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERRO AJAX CARRINHO: {ex.Message}");
                return Json(new { success = false, message = "Erro ao adicionar item ao carrinho." });
            }
        }

        [HttpPost]
        public IActionResult AtualizarQuantidadeAjax(int id, int quantidade)
        {
            var carrinho = ObterCarrinhoDaSessao();
            var item = carrinho.FirstOrDefault(c => c.Produto?.Id == id);

            if (item != null)
            {
                if (quantidade <= 0)
                {
                    carrinho.Remove(item);
                }
                else
                {
                    item.Quantidade = quantidade;
                }

                SalvarCarrinhoNaSessao(carrinho);

                int totalCount = carrinho.Sum(x => x.Quantidade);
                decimal valorTotal = carrinho.Sum(i => (i.Produto?.Price ?? 0m) * i.Quantidade);

                return Json(new { success = true, totalCount, valorTotal });
            }

            return Json(new { success = false, message = "Item não encontrado no carrinho." });
        }

        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> AdicionarAoCarrinho(int id, int produtoId = 0, int quantidade = 1)
        {
            int idFinal = id > 0 ? id : produtoId;

            if (idFinal <= 0)
            {
                TempData["Erro"] = "ID do produto inválido.";
                return RedirectToAction("Index");
            }

            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                if (client.BaseAddress == null)
                {
                    TempData["Erro"] = "Serviço de API não configurado.";
                    return RedirectToAction("Index");
                }

                string rota = client.BaseAddress.ToString().EndsWith("api/")
                    ? $"Product/{idFinal}"
                    : $"api/Product/{idFinal}";

                var response = await client.GetAsync(rota);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Erro"] = "Produto não encontrado na API.";
                    return RedirectToAction("Index");
                }

                var json = await response.Content.ReadFromJsonAsync<JsonElement>();

                int parsedId = idFinal;
                if (json.TryGetProperty("id", out var idProp) && idProp.ValueKind == JsonValueKind.Number)
                    parsedId = idProp.GetInt32();

                string name = null;
                if (json.TryGetProperty("name", out var n1) && n1.ValueKind == JsonValueKind.String)
                    name = n1.GetString();
                else if (json.TryGetProperty("nome", out var n2) && n2.ValueKind == JsonValueKind.String)
                    name = n2.GetString();

                string description = null;
                if (json.TryGetProperty("description", out var d1) && d1.ValueKind == JsonValueKind.String)
                    description = d1.GetString();

                string imagem = null;
                if (json.TryGetProperty("coverImageUrl", out var i1) && i1.ValueKind == JsonValueKind.String)
                    imagem = i1.GetString();
                else if (json.TryGetProperty("imageUrl", out var i2) && i2.ValueKind == JsonValueKind.String)
                    imagem = i2.GetString();

                decimal preco = 0m;
                if (json.TryGetProperty("price", out var p1) && p1.ValueKind == JsonValueKind.Number)
                    preco = p1.GetDecimal();
                else if (json.TryGetProperty("preco", out var p2) && p2.ValueKind == JsonValueKind.Number)
                    preco = p2.GetDecimal();

                var produtoViewModel = new ProductViewModel
                {
                    Id = parsedId,
                    Name = !string.IsNullOrEmpty(name) ? name : $"Produto {idFinal}",
                    Price = preco,
                    CoverImageUrl = !string.IsNullOrEmpty(imagem) ? imagem : "/images/logo/logo.png",
                    Description = description ?? string.Empty
                };

                var carrinho = ObterCarrinhoDaSessao();
                var itemExistente = carrinho.FirstOrDefault(c => c.Produto != null && c.Produto.Id == produtoViewModel.Id);

                if (itemExistente == null)
                {
                    carrinho.Add(new CartItemViewModel { Produto = produtoViewModel, Quantidade = quantidade });
                }
                else
                {
                    itemExistente.Quantidade += quantidade;
                }

                SalvarCarrinhoNaSessao(carrinho);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERRO GERAL: {ex.Message}");
                TempData["Erro"] = "Erro ao processar pedido. Tente novamente.";
                return RedirectToAction("Index");
            }
        }

        private List<CartItemViewModel> ObterCarrinhoDaSessao()
        {
            try
            {
                var cartJson = HttpContext.Session.GetString("Carrinho");
                if (string.IsNullOrEmpty(cartJson)) return new List<CartItemViewModel>();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<CartItemViewModel>>(cartJson, options) ?? new List<CartItemViewModel>();
            }
            catch
            {
                return new List<CartItemViewModel>();
            }
        }

        private void SalvarCarrinhoNaSessao(List<CartItemViewModel> carrinho)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, WriteIndented = false };
            var cartJson = JsonSerializer.Serialize(carrinho, options);
            HttpContext.Session.SetString("Carrinho", cartJson);
        }

        [HttpPost]
        public IActionResult RemoverDoCarrinho(int id)
        {
            var carrinho = ObterCarrinhoDaSessao();
            var item = carrinho.FirstOrDefault(c => c.Produto?.Id == id);

            if (item != null)
            {
                carrinho.Remove(item);
                SalvarCarrinhoNaSessao(carrinho);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            var carrinho = ObterCarrinhoDaSessao();
            if (carrinho == null || !carrinho.Any())
            {
                return RedirectToAction("Index");
            }

            var viewModel = new CheckoutViewModel
            {
                EmailUsuario = userEmail,
                Items = carrinho
            };

            var client = _httpClientFactory.CreateClient("ApiClient");
            InjetarCookieAutenticacao(client);

            try
            {
                string rotaUsuario = client.BaseAddress != null && client.BaseAddress.ToString().EndsWith("api/")
                    ? $"account/user-data?email={userEmail}"
                    : $"api/account/user-data?email={userEmail}";

                var response = await client.GetAsync(rotaUsuario);

                if (response.IsSuccessStatusCode)
                {
                    var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var dadosUsuario = await response.Content.ReadFromJsonAsync<JsonElement>(jsonOptions);

                    viewModel.Cep = ObterPropriedadeString(dadosUsuario, "cep", "CEP");
                    viewModel.Cidade = ObterPropriedadeString(dadosUsuario, "cidade", "Cidade");
                    viewModel.Estado = ObterPropriedadeString(dadosUsuario, "estado", "Estado");
                    viewModel.Numero = ObterPropriedadeString(dadosUsuario, "numero", "Numero");
                    viewModel.Referencial = ObterPropriedadeString(dadosUsuario, "referencia", "Referencia", "referencial");
                    viewModel.Metodo = ObterPropriedadeString(dadosUsuario, "metodo", "Metodo");
                    viewModel.Titular = ObterPropriedadeString(dadosUsuario, "titular", "NomeNoCartao");
                    viewModel.Cartao = ObterPropriedadeString(dadosUsuario, "cartao", "NumeroCartao");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Nota de integração: {ex.Message}");
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarPedidoPost(CheckoutViewModel model, IFormCollection form)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            // Garante que o email venha preenchido
            model.EmailUsuario = userEmail;

            string tipoPagamento = form["TipoPagamento"].ToString();
            if (string.IsNullOrEmpty(tipoPagamento))
            {
                tipoPagamento = model.Metodo;
            }

            bool ehPix = tipoPagamento.Equals("Pix", StringComparison.OrdinalIgnoreCase) || tipoPagamento == "3";

            // 🔒 Validação condicional: Só valida cartão se NÃO for Pix
            if (!ehPix)
            {
                string nomeCartao = !string.IsNullOrEmpty(form["NomeNoCartao"]) ? form["NomeNoCartao"].ToString() : model.Titular;
                string numeroCartao = !string.IsNullOrEmpty(form["NumeroCartao"]) ? form["NumeroCartao"].ToString() : model.Cartao;

                if (string.IsNullOrWhiteSpace(nomeCartao) || string.IsNullOrWhiteSpace(numeroCartao))
                {
                    TempData["Erro"] = "Por favor, preencha todas as informações de pagamento antes de finalizar o pedido.";
                    return RedirectToAction("Checkout");
                }
            }

            var carrinho = ObterCarrinhoDaSessao();
            if (carrinho == null || !carrinho.Any())
            {
                TempData["Erro"] = "Seu carrinho está vazio.";
                return RedirectToAction("Index");
            }

            var client = _httpClientFactory.CreateClient("ApiClient");
            InjetarCookieAutenticacao(client);

            var itemsDto = new List<CreateOrderItemDto>();
            foreach (var item in carrinho)
            {
                var pId = item.Produto?.Id ?? 0;
                var pPreco = item.Produto?.Price ?? 0m;

                if (pId > 0)
                {
                    itemsDto.Add(new CreateOrderItemDto
                    {
                        ProdutoId = pId,
                        Quantidade = item.Quantidade,
                        PrecoUnitario = pPreco
                    });
                }
            }

            if (!itemsDto.Any())
            {
                TempData["Erro"] = "Nenhum produto válido encontrado no carrinho.";
                return RedirectToAction("Index");
            }

            decimal totalPedido = itemsDto.Sum(i => i.PrecoUnitario * i.Quantidade);

            var dadosPedido = new CreateOrderDto
            {
                EmailUsuario = userEmail,
                ValorTotal = totalPedido,
                MetodoPagamento = !string.IsNullOrEmpty(tipoPagamento) ? tipoPagamento : "Cartão de Crédito",
                CEP = !string.IsNullOrEmpty(form["CEP"]) ? form["CEP"].ToString() : model.Cep,
                Cidade = !string.IsNullOrEmpty(form["Cidade"]) ? form["Cidade"].ToString() : model.Cidade,
                Estado = !string.IsNullOrEmpty(form["Estado"]) ? form["Estado"].ToString() : model.Estado,
                Numero = !string.IsNullOrEmpty(form["Numero"]) ? form["Numero"].ToString() : model.Numero,
                Complemento = !string.IsNullOrEmpty(form["Complemento"]) ? form["Complemento"].ToString() : model.Complemento,
                Items = itemsDto
            };

            try
            {
                string rotaPedido = client.BaseAddress != null && client.BaseAddress.ToString().EndsWith("api/")
                    ? "orders"
                    : "api/orders";

                var response = await client.PostAsJsonAsync(rotaPedido, dadosPedido);

                var conteudoResposta = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"🔍 RESPOSTA DA API POST ORDERS [{response.StatusCode}]: {conteudoResposta}");

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Erro"] = $"Erro ao salvar o pedido na API ({response.StatusCode}): {conteudoResposta}";
                    return RedirectToAction("Checkout");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"💥 EXCEÇÃO AO SALVAR PEDIDO: {ex.Message}");
                TempData["Erro"] = $"Exceção ao processar pedido: {ex.Message}";
                return RedirectToAction("Checkout");
            }

            HttpContext.Session.Remove("Carrinho");
            TempData["PedidoSucesso"] = "🎉 Compra Confirmada com Sucesso!";
            return RedirectToAction("Index", "Order");
        }

        private string ObterPropriedadeString(JsonElement json, params string[] nomesPropriedade)
        {
            foreach (var nome in nomesPropriedade)
            {
                if (json.TryGetProperty(nome, out var prop) && prop.ValueKind == JsonValueKind.String)
                {
                    return prop.GetString() ?? string.Empty;
                }
            }
            return string.Empty;
        }
    }
}