// =============================================================================
// AtelieDosPontinhos.UI - Helpers/AppConfig.cs
// =============================================================================

namespace AtelieDosPontinhos.UI.Helpers
{
    public static class AppConfig
    {
        public static string ApiBaseUrl => ApiEndpointResolver.Resolve() ?? string.Empty;
    }
}
