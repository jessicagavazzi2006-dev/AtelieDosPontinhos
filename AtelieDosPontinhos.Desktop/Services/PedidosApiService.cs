using AtelieDosPontinhos.Desktop.DTOs;
using AtelieDosPontinhos.Desktop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtelieDosPontinhos.Desktop.Services
{
    public class PedidosApiService
    {
        private readonly HttpClientHelper _http;

        private PedidosApiService()
        {
            _http = HttpClientHelper.Instance;
        }

        public async Task<List<Pedido>> GetAllAsync()
        {
            try
            {
                var pedidos = await _http.GetAsync<List<Pedido>>("/api/Pedido");
                return pedidos ?? new List<Pedido>();
            }
            catch
            {
                return new List<Pedido>();
            }

        }

        public async Task<Pedido?> GetByIdAsync(int id)
        {
           return await _http.GetAsync<Pedido>($"/api/Pedido/{id}");
        }

        public async Task<(bool Success, Pedido? pedido, string ErrorMessage)> UpdateAsync(int id, Pedido dto)
        {
            return await _http.PutAsync<Pedido>($"/api/Pedido/{id}", dto);
        }

        public async Task<List<Pedido>> SearchAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return new List<Pedido>();

            try
            {
                var encoded = Uri.EscapeDataString(term);
                var pedidos = await _http.GetAsync<List<Pedido>>($"/api/Pedido/search?term={encoded}");
                return pedidos ?? new List<Pedido>();
            }
            catch
            {
                return new List<Pedido>();
            }
        }

    }
}
