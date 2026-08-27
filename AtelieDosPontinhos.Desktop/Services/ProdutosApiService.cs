using AtelieDosPontinhos.Desktop.DTOs;
using AtelieDosPontinhos.Desktop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtelieDosPontinhos.Desktop.Services
{
    public class ProdutosApiService
    {
        private readonly HttpClientHelper _http;

        public ProdutosApiService()
        {
            _http = HttpClientHelper.Instance;
        }

        /// <summary>
        /// Lista todos os produtos via GET /api/produtos.
        /// Disponível para qualquer usuário autenticado.
        /// </summary>
        /// <returns>Lista de produtos ou lista vazia em caso de erro</returns>
        public async Task<List<ProductResponseDto>> GetAllAsync()
        {
            try
            {
                var produtos = await _http.GetAsync<List<ProductResponseDto>>("/api/Product");
                return produtos ?? new List<ProductResponseDto>();
            }
            catch
            {
                return new List<ProductResponseDto>();
            }
        }

        /// <summary>
        /// Busca um produto específico por ID via GET /api/produtos/{id}.
        /// </summary>
        public async Task<ProductResponseDto?> GetByIdAsync(int id)
        {
            return await _http.GetAsync<ProductResponseDto>($"/api/Product/{id}");
        }

        /// <summary>
        /// Cria um novo produto via POST /api/produtos.
        /// Requer perfil Admin (verificado pela API).
        /// </summary>
        /// <param name="dto">Dados do produto a ser criado</param>
        /// <returns>Produto criado ou null em caso de erro</returns>
        public async Task<(bool Success, ProductResponseDto? product, string ErrorMessage)>
            CreateAsync(CreateProductDto dto)
        {
            return await _http.PostAsync<ProductResponseDto>("/api/Product", dto);
        }

        /// <summary>
        /// Atualiza um produto existente via PUT /api/produtos/{id}.
        /// Requer perfil Admin (verificado pela API).
        /// </summary>
        public async Task<(bool Success, ProductResponseDto? product, string ErrorMessage)>
            UpdateAsync(int id, UpdateProductDto dto)
        {
            return await _http.PutAsync<ProductResponseDto>($"/api/Product/{id}", dto);
        }

        /// <summary>
        /// Exclui um produto via DELETE /api/produtos/{id}.
        /// Requer perfil Admin (verificado pela API).
        /// </summary>
        public async Task<(bool Success, string ErrorMessage)> DeleteAsync(int id)
        {
            return await _http.DeleteAsync($"/api/Product/{id}");
        }
    }
}
