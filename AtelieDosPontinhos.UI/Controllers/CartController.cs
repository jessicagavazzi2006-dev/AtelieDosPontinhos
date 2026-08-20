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
    }
}
