// =============================================================================
// SenacGames.UI - Services/HttpGameService.cs
// =============================================================================

using System.Net.Http.Json;
using SenacGames.Application.DTOs;
using SenacGames.Application.Interfaces;

namespace SenacGames.UI.Services
{
    public class HttpGameService : IGameService
    {
        private readonly HttpClient _httpClient;

        public HttpGameService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> CountAsync()
        {
            var games = await GetAllAsync();
            return games.Count();
        }

        public async Task<GameDto> CreateAsync(CreateGameDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/games", dto);
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<GameDto>())!;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/games/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<GameDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<GameDto>>("/api/games") ?? new List<GameDto>();
        }

        public async Task<IEnumerable<GameDto>> GetByCategoryAsync(int categoryId)
        {
            var all = await GetAllAsync();
            return all.Where(g => g.CategoryId == categoryId);
        }

        public async Task<GameDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<GameDto>($"/api/games/{id}");
        }

        public async Task<IEnumerable<GameDto>> GetFeaturedAsync()
        {
            // O endpoint /api/games retorna todos. Poderíamos criar um endpoint específico,
            // mas para manter igual, vamos pegar os 3 primeiros
            var all = await GetAllAsync();
            return all.Take(3);
        }

        public async Task<GameDto?> UpdateAsync(int id, UpdateGameDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/games/{id}", dto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<GameDto>();
            }
            return null;
        }
    }
}
