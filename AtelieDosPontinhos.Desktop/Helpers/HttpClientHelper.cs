using AtelieDosPontinhos.Desktop.Helpers;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace AtelieDosPontinhos.Desktop.Helpers
{
    /// <summary>
    /// Helper centralizado para comunicação HTTP com a API.
    /// Gerencia cookies de sessão, serialização JSON e tratamento de erros.
    /// A URL base é descoberta automaticamente pelo <see cref="ApiEndpointResolver"/>.
    /// </summary>
    public sealed class HttpClientHelper
    {
        // Instância Singleton (thread-safe)
        private static readonly Lazy<HttpClientHelper> _instance =
            new(() => new HttpClientHelper());

        /// <summary>Ponto de acesso global ao HttpClientHelper.</summary>
        public static HttpClientHelper Instance => _instance.Value;

        // =====================================================================
        // CAMPOS PRIVADOS
        // =====================================================================

        /// <summary>
        /// CookieContainer: armazena os cookies recebidos da API.
        /// Quando a API retorna um cookie de autenticação após o login,
        /// o CookieContainer o armazena automaticamente.
        /// Nas próximas requisições, o cookie é enviado de volta para a API.
        /// </summary>
        private readonly CookieContainer _cookieContainer;

        /// <summary>
        /// HttpClientHandler: configuração de baixo nível do HttpClient.
        /// Permite configurar cookies, certificados SSL, proxies, etc.
        /// </summary>
        private readonly HttpClientHandler _handler;

        /// <summary>
        /// HttpClient: o cliente HTTP principal para todas as requisições.
        /// Reutilizado em todas as chamadas para evitar esgotamento de sockets.
        /// A BaseAddress é definida a partir do ApiEndpointResolver.
        /// </summary>
        private readonly HttpClient _client;

        /// <summary>
        /// Opções de serialização JSON.
        /// PropertyNameCaseInsensitive: aceita "title" e "Title" como iguais.
        /// </summary>
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        // =====================================================================
        // CONSTRUTOR
        // =====================================================================

        private HttpClientHelper()
        {
            // Cria o container de cookies para manter a sessão
            _cookieContainer = new CookieContainer();

            // Configura o handler com suporte a cookies
            _handler = new HttpClientHandler
            {
                CookieContainer = _cookieContainer,
                // true = o handler gerencia cookies automaticamente
                UseCookies = true,
                // false = não seguir redirects (a API retorna 401/403 diretamente)
                AllowAutoRedirect = false,
                // Aceita certificados SSL inválidos em desenvolvimento.
                // Em produção com HTTPS válido, remova esta linha.
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            // ── URL base resolvida automaticamente ────────────────────────────
            // ApiEndpointResolver descobre a URL a partir do launchSettings.json
            // da API ou do appsettings.json do Desktop (fallback).
            // Nunca há porta hardcoded aqui.
            var baseUrl = AppConfig.ApiBaseUrl;
            System.Diagnostics.Debug.WriteLine($"[HttpClientHelper] ApiBaseUrl = '{baseUrl}'");

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                // Sem URL: cria o client sem BaseAddress.
                // Program.cs já deve ter tratado esse caso antes de chegar aqui.
                _client = new HttpClient(_handler)
                {
                    Timeout = TimeSpan.FromSeconds(AppConfig.Timeout)
                };
            }
            else
            {
                // Garante que a URL termina com "/"
                if (!baseUrl.EndsWith('/'))
                    baseUrl += "/";

                _client = new HttpClient(_handler)
                {
                    BaseAddress = new Uri(baseUrl),
                    Timeout = TimeSpan.FromSeconds(AppConfig.Timeout)
                };
            }

            // Cabeçalho padrão: aceita JSON como resposta
            _client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        // =====================================================================
        // VALIDAÇÃO DE DISPONIBILIDADE
        // =====================================================================

        /// <summary>
        /// Verifica se a API está disponível fazendo uma requisição simples.
        /// Retorna (true, "") se disponível, ou (false, mensagem_amigável) se não.
        ///
        /// Usado pelo Program.cs antes de abrir o LoginForm.
        ///
        /// IMPORTANTE: Esta verificação é opcional e não-bloqueante.
        /// Se a API não responder, o usuário pode tentar de qualquer forma —
        /// as mensagens de erro específicas aparecerão na tela de Login.
        /// </summary>
        public async Task<(bool IsAvailable, string ErrorMessage)> PingApiAsync()
        {
            if (_client.BaseAddress == null)
            {
                return (false, "URL da API não configurada. Verifique o launchSettings.json " +
                               "do projeto AtelieDosPontinhos.API ou o appsettings.json do Desktop.");
            }

            try
            {
                // Faz uma requisição HEAD simples para verificar conectividade
                // GET /api/produtos é público e leve o suficiente para um ping
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                var response = await _client.GetAsync("/api/Product", cts.Token);

                // Qualquer resposta HTTP (mesmo 401/403) indica que a API está rodando
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, CategorizeConnectionError(ex, _client.BaseAddress?.ToString() ?? ""));
            }
        }

        // =====================================================================
        // MÉTODOS HTTP
        // =====================================================================

        /// <summary>
        /// Realiza uma requisição GET e desserializa o resultado em T.
        /// Uso: var produtos = await http.GetAsync&lt;List&lt;ProdutoResponseDto&gt;&gt;("/api/produtos");
        /// </summary>
        /// <typeparam name="T">Tipo de retorno esperado</typeparam>
        /// <param name="endpoint">Caminho do endpoint (ex: "/api/produtos")</param>
        /// <returns>Objeto desserializado ou null em caso de erro</returns>
        public async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
                }

                return default;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GET] Erro em {endpoint}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Realiza uma requisição POST com corpo JSON e retorna T.
        /// Uso: var produto = await http.PostAsync&lt;ProdutosResponseDto&gt;("/api/produtos", createDto);
        /// </summary>
        public async Task<(bool Success, T? Data, string ErrorMessage)> PostAsync<T>(
            string endpoint, object body)
        {
            try
            {
                var json = JsonSerializer.Serialize(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(endpoint, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var data = JsonSerializer.Deserialize<T>(responseBody, _jsonOptions);
                    return (true, data, string.Empty);
                }

                var error = TryExtractErrorMessage(responseBody);
                return (false, default, error);
            }
            catch (Exception ex)
            {
                var friendly = CategorizeConnectionError(ex, endpoint);
                return (false, default, friendly);
            }
        }

        /// <summary>
        /// Realiza uma requisição PUT (atualização) e retorna T.
        /// </summary>
        public async Task<(bool Success, T? Data, string ErrorMessage)> PutAsync<T>(
            string endpoint, object body)
        {
            try
            {
                var json = JsonSerializer.Serialize(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync(endpoint, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var data = JsonSerializer.Deserialize<T>(responseBody, _jsonOptions);
                    return (true, data, string.Empty);
                }

                var error = TryExtractErrorMessage(responseBody);
                return (false, default, error);
            }
            catch (Exception ex)
            {
                var friendly = CategorizeConnectionError(ex, endpoint);
                return (false, default, friendly);
            }
        }

        /// <summary>
        /// Realiza uma requisição DELETE.
        /// </summary>
        public async Task<(bool Success, string ErrorMessage)> DeleteAsync(string endpoint)
        {
            try
            {
                var response = await _client.DeleteAsync(endpoint);

                if (response.IsSuccessStatusCode)
                    return (true, string.Empty);

                var body = await response.Content.ReadAsStringAsync();
                return (false, TryExtractErrorMessage(body));
            }
            catch (Exception ex)
            {
                return (false, CategorizeConnectionError(ex, endpoint));
            }
        }

        /// <summary>
        /// Realiza um POST sem corpo e retorna apenas sucesso/erro.
        /// Útil para logout e ações simples.
        /// </summary>
        public async Task<(bool Success, string ErrorMessage)> PostEmptyAsync(string endpoint)
        {
            try
            {
                var response = await _client.PostAsync(endpoint, null);

                if (response.IsSuccessStatusCode)
                    return (true, string.Empty);

                var body = await response.Content.ReadAsStringAsync();
                return (false, TryExtractErrorMessage(body));
            }
            catch (Exception ex)
            {
                return (false, CategorizeConnectionError(ex, endpoint));
            }
        }

        // =====================================================================
        // MÉTODOS AUXILIARES
        // =====================================================================

        /// <summary>
        /// Limpa os cookies de sessão (logout local).
        /// Após isso, as próximas requisições não terão autenticação.
        /// </summary>
        public void ClearCookies()
        {
            var baseUri = _client.BaseAddress;
            if (baseUri != null)
            {
                var cookies = _cookieContainer.GetCookies(baseUri);
                foreach (Cookie cookie in cookies)
                    cookie.Expired = true;
            }
        }

        /// <summary>
        /// Tenta extrair a mensagem de erro de um corpo JSON de resposta da API.
        /// A API retorna: { "message": "..." }
        /// </summary>
        private string TryExtractErrorMessage(string json)
        {
            try
            {
                var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("message", out var msg))
                    return msg.GetString() ?? "Erro desconhecido.";
                if (doc.RootElement.TryGetProperty("name", out var name))
                    return name.GetString() ?? "Erro desconhecido.";
            }
            catch { }

            return string.IsNullOrEmpty(json) ? "Erro desconhecido." : json;
        }

        /// <summary>
        /// Categoriza exceções de conexão em mensagens amigáveis para o usuário.
        ///
        /// Tratamento específico por tipo de erro:
        ///   - Conexão recusada  API não está rodando
        ///   - Timeout          API lenta ou sobrecarregada
        ///   - SSL              Problema de certificado
        ///   - DNS              URL inválida ou sem rede
        ///   - Genérico         Mensagem original com contexto
        /// </summary>
        private string CategorizeConnectionError(Exception ex, string endpoint)
        {
            System.Diagnostics.Debug.WriteLine(
                $"[HttpClientHelper] Erro em '{endpoint}': {ex.GetType().Name} — {ex.Message}");

            // ── Timeout ──────────────────────────────────────────────────────
            if (ex is TaskCanceledException or OperationCanceledException)
            {
                return "⏱ A requisição excedeu o tempo limite.\n" +
                       "Verifique se a API está respondendo normalmente.";
            }

            if (ex is HttpRequestException httpEx)
            {
                var msg = httpEx.Message.ToLowerInvariant();

                // ── Conexão recusada (API desligada) ─────────────────────────
                if (msg.Contains("connection refused") ||
                    msg.Contains("actively refused") ||
                    msg.Contains("no connection could be made"))
                {
                    var apiUrl = _client.BaseAddress?.ToString() ?? "URL não configurada";
                    return $"❌ A API não está em execução.\n\n" +
                           $"URL configurada: {apiUrl}\n\n" +
                           $"Verifique se o projeto AtelieDosPontinhos.API está rodando no Visual Studio.";
                }

                // ── SSL / Certificado ─────────────────────────────────────────
                if (msg.Contains("ssl") || msg.Contains("certificate") ||
                    msg.Contains("https"))
                {
                    return "🔒 Erro de conexão SSL.\n\n" +
                           "Tente usar HTTP em vez de HTTPS.\n" +
                           "No launchSettings.json, selecione o perfil 'http'.";
                }

                // ── DNS / Host não encontrado ─────────────────────────────────
                if (msg.Contains("name or service not known") ||
                    msg.Contains("no such host") ||
                    msg.Contains("getaddrinfo"))
                {
                    return $"🌐 Host não encontrado.\n\n" +
                           $"Verifique a URL da API: {_client.BaseAddress}";
                }

                // ── Erro HTTP genérico ────────────────────────────────────────
                return $"⚠ Erro de comunicação com a API:\n{httpEx.Message}";
            }

            // ── URL inválida ──────────────────────────────────────────────────
            if (ex is UriFormatException or InvalidOperationException)
            {
                return "⚠ URL da API inválida.\n\n" +
                       "Verifique o appsettings.json → ApiSettings.BaseUrl\n" +
                       "ou o launchSettings.json do projeto AtelieDosPontinhos.API.";
            }

            // ── Erro genérico ─────────────────────────────────────────────────
            return $"⚠ Erro inesperado:\n{ex.Message}";
        }
    }
}
