using System;
using System.Windows.Forms;

namespace AtelieDosPontinhos.Desktop.Themes
{
    public static class ThemeManager
    {
        // Estado atual
        public static bool IsDarkMode { get; private set; }

        // Evento para que outros forms/usercontrols reajam à troca de tema
        public static event Action<bool>? ThemeChanged;

        // Define o modo e notifica assinantes (não aplica animação — MainForm pode animar)
        public static void SetDarkMode(bool isDark)
        {
            if (IsDarkMode == isDark) return;
            IsDarkMode = isDark;
            ThemeChanged?.Invoke(IsDarkMode);
        }

        // Aplica o tema ao formulário (opcionalmente com animação)
        public static void ApplyTheme(Form form, bool animate = false, int durationMs = 360)
        {
            if (animate)
            {
                ThemeTransitionAnimator.Transition(form, IsDarkMode, durationMs);
                return;
            }

            if (IsDarkMode)
                AtelieDosPontinhosDarkTheme.AplicarEstiloFormulario(form);
            else
                AtelieDosPontinhosTheme.AplicarEstiloFormulario(form);
        }
    }
}
