using AtelieDosPontinhos.Desktop.DTOs;
using AtelieDosPontinhos.Desktop.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AtelieDosPontinhos.Desktop.Services
{
    public class PedidosApiService
    {
        private readonly HttpClientHelper _http;

        public PedidosApiService()
        {
            _http = HttpClientHelper.Instance;
        }

        // GET /api/orders
        public async Task<List<Pedido>> GetAllAsync()
        {
            try
            {
                var pedidos = await _http.GetAsync<List<Pedido>>("/api/orders");
                return pedidos ?? new List<Pedido>();
            }
            catch
            {
                return new List<Pedido>();
            }
        }

        // GET /api/orders/{id}
        public async Task<Pedido?> GetByIdAsync(int id)
        {
            try
            {
                return await _http.GetAsync<Pedido>($"/api/orders/{id}");
            }
            catch
            {
                return null;
            }
        }

        // PUT /api/orders/{id}  -> atualiza o objeto completo
        public async Task<(bool Success, Pedido? pedido, string ErrorMessage)> UpdateAsync(int id, Pedido dto)
        {
            return await _http.PutAsync<Pedido>($"/api/orders/{id}", dto);
        }

        // PUT /api/orders/{id}/status  -> atualiza somente o status (conforme OrderController)
        public async Task<(bool Success, Pedido? pedido, string ErrorMessage)> UpdateStatusAsync(int id, string status)
        {
            try
            {
                // envia { Status = status } como payload (mesma forma que o controlador UI usa)
                return await _http.PutAsync<Pedido>($"/api/orders/{id}/status", new { Status = status });
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        // GET /api/orders/search?term=...
        public async Task<List<Pedido>> SearchAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return new List<Pedido>();

            try
            {
                var encoded = Uri.EscapeDataString(term);
                var pedidos = await _http.GetAsync<List<Pedido>>($"/api/orders/search?term={encoded}");
                return pedidos ?? new List<Pedido>();
            }
            catch
            {
                return new List<Pedido>();
            }
        }
    }
}