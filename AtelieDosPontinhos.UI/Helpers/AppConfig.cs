// =============================================================================
// SenacGames.UI - Helpers/AppConfig.cs
// =============================================================================

namespace SenacGames.UI.Helpers
{
    public static class AppConfig
    {
        public static string ApiBaseUrl => ApiEndpointResolver.Resolve() ?? string.Empty;
    }
}
