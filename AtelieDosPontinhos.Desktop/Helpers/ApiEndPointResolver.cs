using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AtelieDosPontinhos.Desktop.Helpers
{
    public static class ApiEndPointResolver
    {
        private static string? _resolvedUrl;
        private static bool _resolved = false;

        private const string ApiProjectName = "AtelieDosPontinhos.API";
        private const string LaunchSettingsRelativePath = $"{ApiProjectName}/Properties/launchSettings.json";
        private static readonly string[] PreferredProfiles = ["http"];

        public static string? Resolve()
        {
            if (_resolved) return _resolvedUrl;

            _resolved = true;

            var fromLaunchSettings = TryResolveFromLaunchSettings();
            if (fromLaunchSettings != null)
            {
                _resolvedUrl = fromLaunchSettings;
                Log($"✅ API localizada em: {_resolvedUrl}");
                Log($"   Origem: launchSettings.json do {ApiProjectName}");
                return _resolvedUrl;
            }

            var fromAppSettings = TryResolveFromAppSettings();
            if (fromAppSettings != null)
            {
                _resolvedUrl = fromAppSettings;
                Log($"✅ API localizada em: {_resolvedUrl}");
                Log($"   Origem: appsettings.json (configuração manual)");
                return _resolvedUrl;
            }

            Log("❌ URL da API não foi localizada.");
            Log("   Verifique se AtelieDosPontinhos.API/Properties/launchSettings.json existe");
            Log("   ou configure manualmente em appsettings.json → ApiSettings.BaseUrl");
            _resolvedUrl = null;
            return null;
        }

        public static void Reset()
        {
            _resolved = false;
            _resolvedUrl = null;
        }

        private static string? TryResolveFromLaunchSettings()
        {
            var candidates = BuildLaunchSettingsCandidatePaths();

            foreach (var candidate in candidates)
            {
                Log($"   🔍 Testando: {candidate}");

                if (!File.Exists(candidate)) continue;

                Log($"   📄 launchSettings.json encontrado em: {candidate}");

                var url = ParseLaunchSettings(candidate);
                if (url != null) return url;
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
                if (dir != null)
                {
                    paths.Add(Path.Combine(dir, LaunchSettingsRelativePath));
                }
            }

            paths.Add(Path.Combine(Directory.GetCurrentDirectory(), LaunchSettingsRelativePath));
            return paths;
        }

        private static string? ParseLaunchSettings(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (!root.TryGetProperty("profiles", out var profiles))
                {
                    Log("  ⚠ launchSettings.json não contém seção 'profiles'");
                    return null;
                }

                foreach (var profileName in PreferredProfiles)
                {
                    if (!profiles.TryGetProperty(profileName, out var profile)) continue;

                    // CORREÇÃO: usar o objeto do perfil aqui (profile), não "profiles"
                    if (!profile.TryGetProperty("applicationUrl", out var urlProp)) continue;

                    var applicationUrl = urlProp.GetString();
                    if (string.IsNullOrWhiteSpace(applicationUrl)) continue;

                    // applicationUrl pode conter múltiplas entradas separadas por ';'
                    var parts = applicationUrl.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    // Prioriza endereço que começa com "http://", caso exista
                    var httpPart = parts.FirstOrDefault(p => p.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                                   ?? parts.FirstOrDefault();

                    if (httpPart != null)
                    {
                        // Remove barra final se existir
                        var cleaned = httpPart.TrimEnd('/');
                        return cleaned;
                    }
                }

                Log("  ⚠ Nenhum perfil preferido continha 'applicationUrl' válido.");
                return null;
            }
            catch (Exception ex)
            {
                Log($"  ❌ Erro ao parsear launchSettings.json: {ex.Message}");
                return null;
            }
        }

        private static string? TryResolveFromAppSettings()
        {
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                if (!File.Exists(path)) return null;
                var json = File.ReadAllText(path);
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("ApiSettings", out var apiSettings))
                {
                    if (apiSettings.TryGetProperty("BaseUrl", out var baseUrl))
                    {
                        var url = baseUrl.GetString();
                        if (!string.IsNullOrWhiteSpace(url))
                            return url.TrimEnd('/');
                    }
                }
            }
            catch { }
            return null;
        }

        private static string? GoUpDirectories(string path, int levels)
        {
            try
            {
                var di = new DirectoryInfo(path);
                for (int i = 0; i < levels; i++)
                {
                    if (di.Parent == null) return null;
                    di = di.Parent;
                }
                return di.FullName;
            }
            catch { return null; }
        }

        private static void Log(string msg)
        {
            try { System.Diagnostics.Debug.WriteLine($"[ApiEndPointResolver] {msg}"); } catch { }
        }
    }
}