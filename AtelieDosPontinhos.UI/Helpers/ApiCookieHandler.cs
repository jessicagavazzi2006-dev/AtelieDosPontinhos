// =============================================================================
// SenacGames.UI - Helpers/ApiCookieHandler.cs
// =============================================================================

using System.Net.Http.Headers;

namespace SenacGames.UI.Helpers
{
    /// <summary>
    /// Intercepta as requisições HTTP saindo da UI para a API e adiciona o
    /// cookie de autenticação da API, caso o usuário esteja logado.
    /// Isso garante que a API reconheça o usuário autenticado.
    /// </summary>
    public class ApiCookieHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiCookieHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
            {
                // A Claim "ApiCookie" armazena o cookie retornado pela API no momento do Login
                var cookieClaim = user.FindFirst("ApiCookie");
                if (cookieClaim != null && !string.IsNullOrEmpty(cookieClaim.Value))
                {
                    request.Headers.Add("Cookie", cookieClaim.Value);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
