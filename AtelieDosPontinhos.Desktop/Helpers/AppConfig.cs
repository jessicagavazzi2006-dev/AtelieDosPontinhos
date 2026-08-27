using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AtelieDosPontinhos.Desktop.Helpers
{
    public static class AppConfig
    {
        // Cache das configurações do appsettings.json
        private static JsonDocument? _config;

        // =====================================================================
        // PROPRIEDADES DE CONFIGURAÇÃO
        // =====================================================================
        /// <summary>
        /// URL base da API. Exemplo: "http://localhost:5223"
        ///
        /// Resolvida na seguinte ordem pelo ApiEndpointResolver:
        ///   1. launchSettings.json do SenacGames.API (desenvolvimento automático)
        ///   2. appsettings.json  ApiSettings.BaseUrl (fallback configurável)
        ///   3. String vazia se não encontrada (Program.cs exibe mensagem amigável)
        ///
        /// Nunca contém porta hardcoded no código.
        /// </summary>
        public static string ApiBaseUrl => ApiEndPointResolver.Resolve() ?? string.Empty;

        /// <summary>Nome do aplicativo.</summary>
        public static string AppName => GetNestedValue("AppSettings", "AppName") ?? "AtelieDosPontinhos Desktop";

        /// <summary>Versão do aplicativo.</summary>
        public static string Version => GetNestedValue("AppSettings", "Timeout") ?? "1.0.0";

        /// <summary>Timeout das requisições HTTP em segundos.</summary>
        public static int Timeout
        {
            get
            {
                var raw = GetNestedValue("AppSettings", "Timeout");
                return int.TryParse(raw, out var t) ? t : 30;
            }
        }

        // =====================================================================
        // MÉTODOS PRIVADOS — Leitura do appsettings.json
        // =====================================================================
        /// <summary>
        /// Carrega o arquivo appsettings.json (com cache).
        /// Usado apenas para AppName, Version e Timeout —
        /// a URL da API é resolvida pelo ApiEndpointResolver.
        /// </summary>
        private static JsonDocument GetConfig()
        {
            if(_config != null) return _config;

            try
            {
                var path = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, 
                    "appsettings.json");

                if(File.Exists(path))
                {
                    var json = File.ReadAllText(path);
                    // Remove comentários (appsettings.json pode ter "// ...")
                    json = RemoveJsonComments(json);
                    _config = JsonDocument.Parse(json);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[AppConfig] Erro ao ler appsettings.json: {ex.Message}");
            }

            return _config ?? JsonDocument.Parse("{}");
        }

        private static string? GetNestedValue(string section, string key)
        {
            try
            {
                var config = GetConfig();
                if (config.RootElement.TryGetProperty(section, out var sectionEl))
                    if (sectionEl.TryGetProperty(key, out var value))
                        return value.GetString() ?? value.ToString();
            }
            catch { }
            return null;

        }

        /// <summary>
        /// Remove comentários de linha // do JSON.
        /// O appsettings.json do VS pode conter comentários não-padrão.
        /// </summary>
        private static string RemoveJsonComments(string json)
        {
            var lines = json.Split('\n');
            var sb = new System.Text.StringBuilder();
            foreach (var line in lines)
            {
                var trimmed = line.TrimStart();
                if (trimmed.StartsWith("//")) continue;
                var commentIdx = line.IndexOf("//", StringComparison.Ordinal);
                sb.AppendLine(commentIdx > 0 ? line[..commentIdx] : line);
            }
            return sb.ToString();
        }
    }
}
