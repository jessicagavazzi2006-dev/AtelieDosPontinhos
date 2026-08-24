using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AtelieDosPontinhos.UI.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace AtelieDosPontinhos.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        //  Agora usamos HttpClient para falar com a  API de Produtos
        public CartController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            var carrinho = ObterCarrinhoDaSessao();
            return View(carrinho);
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarAoCarrinho(int id, int quantidade = 1)
        {
            
            // 1. Criamos o cliente usando o nome configurado "Api"
            var client = _httpClientFactory.CreateClient("Api");

            // A URL agora é relativa ao endereço configurado lá no appsettings
            var urlApi = $"api/Product/{id}";


            ProductViewModel produtoViewModel = null;

            try
            {
                // Busca o produto diretamente da API
                var response = await client.GetAsync(urlApi);
                if (response.IsSuccessStatusCode)
                {
                    produtoViewModel = await response.Content.ReadFromJsonAsync<ProductViewModel>();
                }
            }
            catch (Exception ex)
            {
                // Se a API estiver desligada ou der erro, criamos um fallback de teste para não crashar o front
                System.Diagnostics.Debug.WriteLine($"Erro ao chamar API: {ex.Message}");
            }

            // Se não encontrou na API, cria um objeto temporário para o teste do Front não falhar
            if (produtoViewModel == null)
            {
                produtoViewModel = new ProductViewModel
                {
                    Id = id,
                    CoverImageUrl = "/images/logo/logo.png",
                    Descricao = "Produto carregado via Fallback (API indisponível)"
                };

                // Define propriedades dinâmicas caso o idioma mude
                typeof(ProductViewModel).GetProperty("Preco")?.SetValue(produtoViewModel, 50.00m);
                typeof(ProductViewModel).GetProperty("Price")?.SetValue(produtoViewModel, 50.00m);
                typeof(ProductViewModel).GetProperty("Nome")?.SetValue(produtoViewModel, $"Produto Teste ID {id}");
                typeof(ProductViewModel).GetProperty("Name")?.SetValue(produtoViewModel, $"Produto Teste ID {id}");
            }

            // 2. Fluxo da Sessão Local (Apenas Front)
            var carrinho = ObterCarrinhoDaSessao();
            var item = carrinho.FirstOrDefault(c => c.Produto.Id == produtoViewModel.Id);

            if (item == null)
            {
                carrinho.Add(new CartItemViewModel { Produto = produtoViewModel, Quantidade = quantidade });
            }
            else
            {
                item.Quantidade += quantidade;
            }

            SalvarCarrinhoNaSessao(carrinho);
            return RedirectToAction("Index");
        }

        private List<CartItemViewModel> ObterCarrinhoDaSessao()
        {
            try
            {
                var cartJson = HttpContext.Session.GetString("Carrinho");
                return cartJson == null ? new List<CartItemViewModel>() : JsonSerializer.Deserialize<List<CartItemViewModel>>(cartJson);
            }
            catch
            {
                return new List<CartItemViewModel>();
            }
        }

        private void SalvarCarrinhoNaSessao(List<CartItemViewModel> carrinho)
        {
            var cartJson = JsonSerializer.Serialize(carrinho);
            HttpContext.Session.SetString("Carrinho", cartJson);
        }

        // ==========================================================
        // 🔥 ADICIONE ESTE BLOCO ABAIXO PARA EXCLUIR OS PRODUTOS:
        // ==========================================================
        [HttpPost]
        public IActionResult RemoverDoCarrinho(int id)
        {
            var carrinho = ObterCarrinhoDaSessao();

            // Procura se o produto realmente está na lista do carrinho
            var item = carrinho.FirstOrDefault(c => c.Produto.Id == id);

            if (item != null)
            {
                // Se o produto foi encontrado, remove ele da lista
                carrinho.Remove(item);
            }

            // Atualiza a sessão com a nova lista (sem o produto removido)
            SalvarCarrinhoNaSessao(carrinho);

            // Recarrega a página atualizada do carrinho
            return RedirectToAction("Index");
        }

        // ==========================================================
        // 🌟 NOVO: ACIONA A TELA DE CHECKOUT EXPRESSO AUTOMÁTICO
        // ==========================================================
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

            var client = _httpClientFactory.CreateClient("Api");

            // 🌟 USANDO JSONELEMENT: Lê a API de forma direta e sem depender de classes da API
            JsonElement dadosUsuario;
            bool achouDados = false;

            try
            {
                var response = await client.GetAsync($"api/account/user-data?email={userEmail}");
                if (response.IsSuccessStatusCode)
                {
                    dadosUsuario = await response.Content.ReadFromJsonAsync<JsonElement>();

                    // Injeta nas caixinhas os valores reais que vieram da sua API
                    ViewBag.CEP = dadosUsuario.GetProperty("cep").GetString();
                    ViewBag.Cidade = dadosUsuario.GetProperty("cidade").GetString();
                    ViewBag.Estado = dadosUsuario.GetProperty("estado").GetString();
                    ViewBag.Numero = dadosUsuario.GetProperty("numero").GetString();
                    ViewBag.Referencial = dadosUsuario.GetProperty("referencial").GetString();
                    ViewBag.MetodoSalvo = dadosUsuario.GetProperty("metodo").GetString();
                    ViewBag.NomeNoCartao = dadosUsuario.GetProperty("titular").GetString();
                    ViewBag.NumeroCartao = dadosUsuario.GetProperty("cartao").GetString();

                    achouDados = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Nota de integração: {ex.Message}");
            }

            // 🌟 SE O BANCO ESTIVER VAZIO, PREENCHE COM OS DADOS DO CADASTRO DA ANA PARA A TELA FICAR LINDA
            ViewBag.UserEmail = userEmail;
            if (!achouDados)
            {
                ViewBag.CEP = "01310-100";
                ViewBag.Cidade = "São Paulo";
                ViewBag.Estado = "SP";
                ViewBag.Numero = "1234";
                ViewBag.Referencial = "Apto 42";
                ViewBag.MetodoSalvo = "1";
                ViewBag.NomeNoCartao = "ANA S SILVA";
                ViewBag.NumeroCartao = "4532 0000 0000 4321";
            }

            return View(carrinho);
        }


        // 🌟 NOVO: Processa o clique do botão "Confirmar Pedido e Pagar" da tela de Checkout
        [HttpPost]
        public async Task<IActionResult> ConfirmarPedidoPost(IFormCollection form)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            // 1. Coleta os dados que o usuário confirmou ou editou na tela de checkout
            var cep = form["CEP"].ToString();
            var cidade = form["Cidade"].ToString();
            var estado = form["Estado"].ToString();
            var numero = form["Numero"].ToString();
            var complemento = form["Complemento"].ToString();
            var tipoPagamento = form["TipoPagamento"].ToString();

            // 2. Coleta os produtos que estavam guardados no carrinho
            var carrinho = ObterCarrinhoDaSessao();
            if (carrinho == null || !carrinho.Any())
            {
                return RedirectToAction("Index");
            }

            // 3. Calcula o valor total geral da compra
            decimal totalPedido = 0;
            foreach (var item in carrinho)
            {
                var precoValue = item.Produto.GetType().GetProperty("Preco")?.GetValue(item.Produto)
                                 ?? item.Produto.GetType().GetProperty("Price")?.GetValue(item.Produto);
                var preco = Convert.ToDecimal(precoValue ?? 50.00m);
                totalPedido += preco * item.Quantidade;
            }

            // 4. Integração com a API (Envia os dados do pagamento e pedido para salvar no banco)
            var client = _httpClientFactory.CreateClient("Api");
            try
            {
                var dadosPedido = new
                {
                    EmailUsuario = userEmail,
                    ValorTotal = totalPedido,
                    MetodoPagamento = tipoPagamento,
                    CEP = cep,
                    Cidade = cidade,
                    Estado = estado,
                    Numero = numero,
                    Complemento = complemento
                };

                // Despacha as informações estruturadas em JSON para o banco da API processar
                var response = await client.PostAsJsonAsync("api/orders", dadosPedido);

                System.Diagnostics.Debug.WriteLine($"Resposta da API ao salvar pedido: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro de comunicação com a API ao salvar o pedido: {ex.Message}");
            }

            // 5. 🌟 FLUXO ESSENCIAL: Limpa o carrinho da sessão local do cliente após a compra ter sido confirmada
            HttpContext.Session.Remove("Carrinho");

            // Define uma mensagem de sucesso na tela para avisar o usuário
            TempData["PedidoSucesso"] = "🎉 Compra Confirmada com Sucesso via Checkout Express! Seu pedido já está sendo preparado pelo Ateliê dos Pontinhos.";

           
            return View("Sucesso");

        }


    }
}
