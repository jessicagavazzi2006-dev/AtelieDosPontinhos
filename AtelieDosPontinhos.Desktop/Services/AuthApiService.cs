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
    /// Serviço de comunicação com os endpoints de autenticação da API.
    /// </summary>
    public class AuthApiService
    {
        // Referência ao helper HTTP (singleton)
        private readonly HttpClientHelper _http;

        /// <summary>
        /// Construtor: obtém a instância singleton do HttpClientHelper.
        /// </summary>
        public AuthApiService()
        {
            _http = HttpClientHelper.Instance;
        }

        /// <summary>
        /// Realiza o login chamando POST /api/auth/login.
        ///
        /// O que acontece internamente:
        /// 1. Envia email + senha para a API em formato JSON
        /// 2. A API valida as credenciais com o ASP.NET Core Identity
        /// 3. Se válido, a API retorna um cookie de sessão + dados do usuário
        /// 4. O CookieContainer do HttpClient armazena o cookie automaticamente
        /// 5. Retornamos os dados do usuário para o LoginForm
        /// </summary>
        /// <param name="email">E-mail do usuário</param>
        /// <param name="password">Senha do usuário</param>
        /// <returns>Tupla com sucesso, dados do usuário e mensagem de erro</returns>
        public async Task<(bool Success, UserDto? User, string ErrorMessage)>
            LoginAsync(string email, string password)
        {
            // Cria o objeto de requisição (DTO de login)
            var loginDto = new loginDto
            {
                Email = email,
                Password = password
            };

            // Envia para POST /api/auth/login
            var (success, data, error) = await _http.PostAsync<UserDto>(
                "/api/Account/login", loginDto);

            return (success, data, error);
        }

        /// <summary>
        /// Realiza o logout chamando POST /api/auth/logout.
        /// Também limpa os cookies de sessão localmente.
        /// </summary>
        public async Task<(bool Success, string ErrorMessage)> LogoutAsync()
        {
            var result = await _http.PostEmptyAsync("/api/Account/logout");

            // Limpa os cookies locais independentemente do resultado da API
            _http.ClearCookies();

            return result;
        }

        /// <summary>
        /// Busca os dados do usuário autenticado via GET /api/auth/me.
        /// Útil para verificar se a sessão ainda está ativa.
        /// </summary>
        public async Task<UserDto?> GetCurrentUserAsync()
        {
            return await _http.GetAsync<UserDto>("/api/Account/me");
        }

        /// <summary>
        /// Registra um novo usuário via POST /api/auth/register.
        /// </summary>
        public async Task<(bool Success, string ErrorMessage)> RegisterAsync(
            string email, string password, string confirmPassword)
        {
            var dto = new RegisterDto
            {
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            var (success, _, error) = await _http.PostAsync<object>("/api/Account/register", dto);
            return (success, error);
        }
    }
}
