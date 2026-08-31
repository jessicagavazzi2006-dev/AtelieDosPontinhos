// =============================================================================
// AtelieDosPontinhos.UI - Services/HttpGameService.cs
// =============================================================================

using System.Net.Http.Json;
using AtelieDosPontinhos.Application.DTOs;
using AtelieDosPontinhos.Application.Interfaces;

namespace AtelieDosPontinhos.UI.Services
{
    public class HttpGameService : ICategoryService
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

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/categories", dto);
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<CategoryDto>())!;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/categories/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<CategoryDto>>("/api/categories") ?? new List<CategoryDto>();
        }

        public async Task<IEnumerable<CategoryDto>> GetByCategoryAsync(int categoryId)
        {
            var all = await GetAllAsync();
            return all.Where(g => g.Id == categoryId);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<CategoryDto>($"/api/categories/{id}");
        }

        public async Task<IEnumerable<CategoryDto>> GetFeaturedAsync()
        {
            // O endpoint /api/categories retorna todos. Poderíamos criar um endpoint específico,
            // mas para manter igual, vamos pegar os 3 primeiros
            var all = await GetAllAsync();
            return all.Take(3);
        }

        public async Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/categories/{id}", dto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CategoryDto>();
            }
            return null;
        }
    }
}
