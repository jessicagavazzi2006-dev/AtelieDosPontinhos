using AtelieDosPontinhos.Desktop.DTOs;
using AtelieDosPontinhos.Desktop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtelieDosPontinhos.Desktop.Services
{
    /// <summary>
    /// Serviço de comunicação com os endpoints de Categorias da API.
    /// </summary>
    public class CategoriasApiService
    {
        private readonly HttpClientHelper _http;

        public CategoriasApiService()
        {
            _http = HttpClientHelper.Instance;
        }

        /// <summary>
        /// Lista todas as categorias via GET /api/categories.
        /// </summary>
        public async Task<List<CategoriaResponseDto>> GetAllAsync()
        {
            try
            {
                var categorias = await _http.GetAsync<List<CategoriaResponseDto>>("/api/categories");
                return categorias ?? new List<CategoriaResponseDto>();
            }
            catch
            {
                return new List<CategoriaResponseDto>();
            }
        }

        /// <summary>
        /// Cria uma nova categoria via POST /api/categories.
        /// Requer perfil Admin.
        /// </summary>
        public async Task<(bool Success, CategoriaResponseDto? Categoria, string ErrorMessage)>
            CreateAsync(CreateCategoryDto dto)
        {
            return await _http.PostAsync<CategoriaResponseDto>("/api/categories", dto);
        }

        /// <summary>
        /// Atualiza uma categoria via PUT /api/categories/{id}.
        /// Requer perfil Admin.
        /// </summary>
        public async Task<(bool Success, CategoriaResponseDto? Categoria, string ErrorMessage)>
            UpdateAsync(int id, UpdateCategoryDto dto)
        {
            return await _http.PutAsync<CategoriaResponseDto>($"/api/categories/{id}", dto);
        }

        /// <summary>
        /// Exclui uma categoria via DELETE /api/categories/{id}.
        /// Requer perfil Admin.
        /// </summary>
        public async Task<(bool Success, string ErrorMessage)> DeleteAsync(int id)
        {
            return await _http.DeleteAsync($"/api/categories/{id}");
        }
    }
}
