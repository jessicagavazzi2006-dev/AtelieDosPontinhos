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

        // 🌟 Agora usamos HttpClient para falar com a sua API de Produtos
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
            // 1. Criamos o cliente para chamar a sua API
            var client = _httpClientFactory.CreateClient();

            // ⚠️ ATENÇÃO: Substitua a porta (ex: 5001 ou 7001) pela porta REAL onde a sua API (Swagger) está a rodar!
            var urlApi = $"https://localhost:7193/api/Product/{id}";

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
    }
}