using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace AtelieDosPontinhos.Desktop.Themes
{
    public static class ThemeTransitionAnimator
    {
        private class AnimatedColor
        {
            public Action<Color> Setter = null!;
            public Color From;
            public Color To;
        }

        public static void Transition(Form form, bool toDark, int durationMs = 360)
        {
            var items = new List<AnimatedColor>();

            var targetFormBg = toDark ? AtelieDosPontinhosDarkTheme.FundoEscuro : AtelieDosPontinhosTheme.CinzaFundo;
            var targetPanelBg = toDark ? AtelieDosPontinhosDarkTheme.Surface : AtelieDosPontinhosTheme.Branco;
            var targetLabelFore = toDark ? AtelieDosPontinhosDarkTheme.TextoClaro : AtelieDosPontinhosTheme.TextoPrincipal;
            var targetBtnFill = toDark ? AtelieDosPontinhosDarkTheme.AccentRoxo : AtelieDosPontinhosTheme.BotaoPrimarioFundo;
            var targetBtnFore = Color.White;
            var targetTxtFill = toDark ? Color.FromArgb(40, 40, 44) : AtelieDosPontinhosTheme.Branco;
            var targetGridBg = toDark ? AtelieDosPontinhosDarkTheme.LinhaPar : AtelieDosPontinhosTheme.GridLinhaPar;
            var targetGridAlt = toDark ? AtelieDosPontinhosDarkTheme.LinhaImpar : AtelieDosPontinhosTheme.GridLinhaImpar;
            var targetGridHeader = toDark ? AtelieDosPontinhosDarkTheme.Cabecalho : AtelieDosPontinhosTheme.GridCabecalhoFundo;
            var targetGridFore = toDark ? AtelieDosPontinhosDarkTheme.TextoClaro : AtelieDosPontinhosTheme.TextoPrincipal;

            // form background + fore
            items.Add(new AnimatedColor { Setter = c => form.BackColor = c, From = form.BackColor, To = targetFormBg });
            items.Add(new AnimatedColor { Setter = c => form.ForeColor = c, From = form.ForeColor, To = targetLabelFore });

            void Collect(Control parent)
            {
                foreach (Control c in parent.Controls)
                {
                    // preserve controls flagged to keep custom background color but still animate forecolor
                    var keepBack = c?.Tag?.ToString() == "KeepBackColor";

                    // Animate BackColor where sensible
                    if (!keepBack)
                    {
                        // Panel-like controls
                        if (c is Panel pnl)
                            items.Add(new AnimatedColor { Setter = col => pnl.BackColor = col, From = pnl.BackColor, To = targetPanelBg });
                        if (c is Guna2Panel gp)
                            items.Add(new AnimatedColor { Setter = col => gp.FillColor = col, From = gp.FillColor, To = targetPanelBg });
                    }

                    // Animate ForeColor for most controls (labels, buttons, textboxes, etc.)
                    if (c is Label lbl)
                    {
                        items.Add(new AnimatedColor { Setter = col => lbl.ForeColor = col, From = lbl.ForeColor, To = targetLabelFore });
                    }
                    else if (c is Guna2Button gbtn)
                    {
                        items.Add(new AnimatedColor { Setter = col => gbtn.FillColor = col, From = gbtn.FillColor, To = targetBtnFill });
                        items.Add(new AnimatedColor { Setter = col => gbtn.ForeColor = col, From = gbtn.ForeColor, To = targetBtnFore });
                    }
                    else if (c is Button btn)
                    {
                        items.Add(new AnimatedColor { Setter = col => btn.BackColor = col, From = btn.BackColor, To = targetBtnFill });
                        items.Add(new AnimatedColor { Setter = col => btn.ForeColor = col, From = btn.ForeColor, To = targetBtnFore });
                    }
                    else if (c is Guna2TextBox gtxt)
                    {
                        items.Add(new AnimatedColor { Setter = col => gtxt.FillColor = col, From = gtxt.FillColor, To = targetTxtFill });
                        items.Add(new AnimatedColor { Setter = col => gtxt.ForeColor = col, From = gtxt.ForeColor, To = targetLabelFore });
                    }
                    else if (c is Guna2ComboBox gcombo)
                    {
                        items.Add(new AnimatedColor { Setter = col => gcombo.FillColor = col, From = gcombo.FillColor, To = targetTxtFill });
                        items.Add(new AnimatedColor { Setter = col => gcombo.ForeColor = col, From = gcombo.ForeColor, To = targetLabelFore });
                    }
                    else if (c is Guna2CheckBox gchk)
                    {
                        items.Add(new AnimatedColor { Setter = col => gchk.ForeColor = col, From = gchk.ForeColor, To = targetLabelFore });
                    }
                    else if (c is DataGridView dgv)
                    {
                        items.Add(new AnimatedColor { Setter = col => dgv.BackgroundColor = col, From = dgv.BackgroundColor, To = targetGridBg });
                        items.Add(new AnimatedColor { Setter = col => dgv.DefaultCellStyle.BackColor = col, From = dgv.DefaultCellStyle.BackColor, To = targetGridBg });
                        items.Add(new AnimatedColor { Setter = col => dgv.AlternatingRowsDefaultCellStyle.BackColor = col, From = dgv.AlternatingRowsDefaultCellStyle.BackColor, To = targetGridAlt });
                        items.Add(new AnimatedColor { Setter = col => dgv.ColumnHeadersDefaultCellStyle.BackColor = col, From = dgv.ColumnHeadersDefaultCellStyle.BackColor, To = targetGridHeader });
                        items.Add(new AnimatedColor { Setter = col => dgv.DefaultCellStyle.ForeColor = col, From = dgv.DefaultCellStyle.ForeColor, To = targetGridFore });
                        items.Add(new AnimatedColor { Setter = col => dgv.ColumnHeadersDefaultCellStyle.ForeColor = col, From = dgv.ColumnHeadersDefaultCellStyle.ForeColor, To = targetGridFore });
                    }
                    else
                    {
                        // generic attempt: animate ForeColor (most controls support)
                        try
                        {
                            items.Add(new AnimatedColor { Setter = col => c.ForeColor = col, From = c.ForeColor, To = targetLabelFore });
                        }
                        catch { /* ignore controls that don't allow ForeColor change */ }
                    }

                    if (c.HasChildren) Collect(c);
                }
            }

            Collect(form);

            if (items.Count == 0)
            {
                FinalizeTheme(form, toDark);
                return;
            }

            var timer = new System.Windows.Forms.Timer { Interval = 12 };
            var start = Environment.TickCount;

            timer.Tick += (s, e) =>
            {
                var elapsed = Environment.TickCount - start;
                var t = Math.Min(1f, (float)elapsed / durationMs);

                foreach (var a in items)
                {
                    var col = LerpColor(a.From, a.To, t);
                    try { a.Setter(col); } catch { }
                }

                if (t >= 1f)
                {
                    timer.Stop();
                    timer.Dispose();
                    // Finalize only non-color properties and ensure exact final colors (preserves KeepBackColor)
                    FinalizeTheme(form, toDark);
                }
            };

            timer.Start();
        }

        private static void FinalizeTheme(Form form, bool toDark)
        {
            // Aplica propriedades não-visuais e garante que cores finais estejam corretas.
            // Ambos os temas respeitam o Tag = "KeepBackColor".
            if (toDark)
                AtelieDosPontinhosDarkTheme.AplicarEstiloFormulario(form);
            else
                AtelieDosPontinhosTheme.AplicarEstiloFormulario(form);
        }

        private static Color LerpColor(Color a, Color b, float t)
        {
            int A = (int)(a.A + (b.A - a.A) * t);
            int R = (int)(a.R + (b.R - a.R) * t);
            int G = (int)(a.G + (b.G - a.G) * t);
            int B = (int)(a.B + (b.B - a.B) * t);
            return Color.FromArgb(A, Clamp(R), Clamp(G), Clamp(B));
        }

        private static int Clamp(int v) => Math.Max(0, Math.Min(255, v));
    }
}