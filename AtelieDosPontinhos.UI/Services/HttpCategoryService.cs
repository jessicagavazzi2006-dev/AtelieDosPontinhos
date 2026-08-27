// =============================================================================
// SenacGames.UI - Services/HttpCategoryService.cs
// =============================================================================

using System.Net.Http.Json;
using SenacGames.Application.DTOs;
using SenacGames.Application.Interfaces;

namespace SenacGames.UI.Services
{
    public class HttpCategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public HttpCategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> CountAsync()
        {
            var cats = await GetAllAsync();
            return cats.Count();
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

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<CategoryDto>($"/api/categories/{id}");
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
