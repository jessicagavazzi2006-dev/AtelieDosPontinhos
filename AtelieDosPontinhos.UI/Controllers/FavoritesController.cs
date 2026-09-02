using Microsoft.AspNetCore.Mvc;
using AtelieDosPontinhos.UI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

namespace AtelieDosPontinhos.UI.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string SESSION_KEY = "UserFavorites";

        public FavoritesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // 📋 GET: Favorites/Index (Renderiza a lista de favoritos)
        [HttpGet]
        public IActionResult Index()
        {
            var favoritos = GetFavoritesFromSession();
            return View(favoritos);
        }

        // 🤍/❤️ POST: Favorites/ToggleFavorite/5 (Adiciona ou Remove dos favoritos via AJAX)
        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int id)
        {
            var favoritos = GetFavoritesFromSession();
            var item = favoritos.FirstOrDefault(p => p.Id == id);

            bool isFavorited;

            if (item != null)
            {
                // Se já existir na sessão, remove
                favoritos.Remove(item);
                isFavorited = false;
            }
            else
            {
                // Se não existir, busca os dados atualizados na API e insere na lista
                var client = _httpClientFactory.CreateClient("Api");
                try
                {
                    var response = await client.GetAsync($"api/Product/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var produto = await response.Content.ReadFromJsonAsync<ProductViewModel>();
                        if (produto != null)
                        {
                            favoritos.Add(produto);
                        }
                    }
                }
                catch
                {
                    // Tratamento silencioso para caso a API falhe na busca pontual
                }
                isFavorited = true;
            }

            // Salva a lista atualizada de volta na Sessão do usuário
            SaveFavoritesToSession(favoritos);

            return Json(new { success = true, isFavorited = isFavorited, count = favoritos.Count });
        }

        // 🛠️ Métodos Auxiliares para Manipulação do JSON da Sessão
        private List<ProductViewModel> GetFavoritesFromSession()
        {
            var json = HttpContext.Session.GetString(SESSION_KEY);
            return string.IsNullOrEmpty(json)
                ? new List<ProductViewModel>()
                : JsonSerializer.Deserialize<List<ProductViewModel>>(json) ?? new List<ProductViewModel>();
        }

        private void SaveFavoritesToSession(List<ProductViewModel> favoritos)
        {
            var json = JsonSerializer.Serialize(favoritos);
            HttpContext.Session.SetString(SESSION_KEY, json);
        }
    }
}