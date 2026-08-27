// =============================================================================
// SenacGames.UI - Helpers/ApiEndpointResolver.cs
// =============================================================================
//  CONCEITO: Descoberta Automática da URL da API
// =============================================================================

using System.Text.Json;

namespace SenacGames.UI.Helpers
{
    public static class ApiEndpointResolver
    {
        private static string? _resolvedUrl;
        private static bool _resolved = false;
        private const string ApiProjectName = "SenacGames.API";
        private const string LaunchSettingsRelativePath = $"{ApiProjectName}/Properties/launchSettings.json";
        private static readonly string[] PreferredProfiles = ["http", "https", "IIS Express"];

        public static string? Resolve()
        {
            if (_resolved) return _resolvedUrl;
            _resolved = true;

            var fromLaunchSettings = TryResolveFromLaunchSettings();
            if (fromLaunchSettings != null)
            {
                _resolvedUrl = fromLaunchSettings;
                return _resolvedUrl;
            }

            var fromAppSettings = TryResolveFromAppSettings();
            if (fromAppSettings != null)
            {
                _resolvedUrl = fromAppSettings;
                return _resolvedUrl;
            }

            return null;
        }

        private static string? TryResolveFromLaunchSettings()
        {
            var candidates = BuildLaunchSettingsCandidatePaths();
            foreach (var candidate in candidates)
            {
                if (File.Exists(candidate))
                {
                    var url = ParseLaunchSettings(candidate);
                    if (url != null) return url;
                }
            }
            return null;
        }

        private static List<string> BuildLaunchSettingsCandidatePaths()
        {
            var paths = new List<string>();
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var relativeLevels = new[] { 4, 5, 3, 6 };
            foreach (var levels in relativeLevels)
            {
                var dir = GoUpDirectories(baseDir, levels);
                if (dir != null) paths.Add(Path.Combine(dir, LaunchSettingsRelativePath));
            }
            var solutionDir = Environment.GetEnvironmentVariable("SolutionDir");
            if (!string.IsNullOrEmpty(solutionDir)) paths.Add(Path.Combine(solutionDir, LaunchSettingsRelativePath));
            paths.Add(Path.Combine(Directory.GetCurrentDirectory(), LaunchSettingsRelativePath));
            return paths;
        }

        private static string? ParseLaunchSettings(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                using var doc = JsonDocument.Parse(json);
                if (!doc.RootElement.TryGetProperty("profiles", out var profiles)) return null;

                foreach (var profileName in PreferredProfiles)
                {
                    if (profiles.TryGetProperty(profileName, out var profile) &&
                        profile.TryGetProperty("applicationUrl", out var urlProp))
                    {
                        var url = ExtractBestUrl(urlProp.GetString() ?? "", profileName);
                        if (url != null) return url;
                    }
                }
            }
            catch { }
            return null;
        }

        private static string? ExtractBestUrl(string applicationUrl, string profileName)
        {
            var urls = applicationUrl.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(u => u.Trim()).ToList();
            if (urls.Count == 0) return null;
            if (profileName == "http") return urls.FirstOrDefault(u => u.StartsWith("http://", StringComparison.OrdinalIgnoreCase)) ?? urls[0];
            return urls.FirstOrDefault(u => u.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) ?? urls[0];
        }

        private static string? TryResolveFromAppSettings()
        {
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                if (!File.Exists(path)) return null;

                var json = File.ReadAllText(path);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("ApiSettings", out var apiSettings) &&
                    apiSettings.TryGetProperty("BaseUrl", out var baseUrl))
                {
                    return baseUrl.GetString();
                }
            }
            catch { }
            return null;
        }

        private static string? GoUpDirectories(string path, int levels)
        {
            var dir = new DirectoryInfo(path);
            for (int i = 0; i < levels; i++)
            {
                dir = dir.Parent;
                if (dir == null) return null;
            }
            return dir.FullName;
        }
    }
}
