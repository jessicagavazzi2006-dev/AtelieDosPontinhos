using AtelieDosPontinhos.Desktop.DTOs;
using AtelieDosPontinhos.Desktop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtelieDosPontinhos.Desktop.Services
{
    public class UsuariosApiService
    {
        private readonly HttpClientHelper _http;

        public UsuariosApiService()
        {
            _http = HttpClientHelper.Instance;
        }

        public async Task<List<UsuarioResponseDto>> GetAllAsync()
        {
            try
            {
                var usuarios = await _http.GetAsync<List<UsuarioResponseDto>>("/api/User");
                return usuarios ?? new List<UsuarioResponseDto>();
            }
            catch
            {
                return new List<UsuarioResponseDto>();
            }
        }

        public async Task<(bool Success, UsuarioResponseDto? Usuario, string ErrorMessage)> CreateAsync(CreateUsuarioDto dto)
        {
            try
            {
                var (success, data, errorMessage) = await _http.PostAsync<UsuarioResponseDto>("/api/User", dto);
                return (success, data, errorMessage);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool Success, UsuarioResponseDto? Usuario, string ErrorMessage)> UpdateAsync(string id, UpdateUsuarioDto dto)
        {
            try
            {
                var (success, data, errorMessage) = await _http.PutAsync<UsuarioResponseDto>($"/api/User/{id}", dto);
                return (success, data, errorMessage);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool Success, string ErrorMessage)> DeleteAsync(string id)
        {
            try
            {
                await _http.DeleteAsync($"/api/User/{id}");
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<List<string>> GetPerfisAsync()
        {
            try
            {
                var perfis = await _http.GetAsync<List<string>>("/api/User/perfis");
                return perfis ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }
    }
}
