using System;
using System.Drawing;
using System.Windows.Forms;

namespace AtelieDosPontinhos.Desktop.Helpers
{
    public static class FormAnimator
    {
        private static readonly HashSet<Form> _bypassCloseAnimation = new();
        // Fade-in simples (usado para Forms modais e não-modais)
        public static void FadeIn(Form form, int durationMs = 300)
        {
            if (form == null) return;

            form.Opacity = 0;
            var start = Environment.TickCount;
            var timer = new System.Windows.Forms.Timer { Interval = 12 };
            timer.Tick += (s, e) =>
            {
                var elapsed = Environment.TickCount - start;
                var raw = Math.Min(1f, (float)elapsed / durationMs);
                var t = EaseInOutCubic(raw);
                try { form.Opacity = t; } catch { }
                if (raw >= 1f)
                {
                    timer.Stop();
                    timer.Dispose();
                    form.Opacity = 1;
                }
            };
            // Se o form ainda não estiver mostrado, mostramos e depois animamos
            if (!form.Visible) form.Show();
            timer.Start();
        }

        // Slide-in vertical + fade (não-modal)
        public static void SlideFadeIn(Form form, int durationMs = 360, int offset = 40)
        {
            if (form == null) return;

            var startY = form.Top;
            var fromY = startY - offset;
            form.Top = fromY;
            form.Opacity = 0;

            var start = Environment.TickCount;
            var timer = new System.Windows.Forms.Timer { Interval = 12 };
            timer.Tick += (s, e) =>
            {
                var elapsed = Environment.TickCount - start;
                var raw = Math.Min(1f, (float)elapsed / durationMs);
                var t = EaseOutCubic(raw);

                try
                {
                    form.Top = (int)(fromY + (startY - fromY) * t);
                    form.Opacity = Math.Min(1, t);
                }
                catch { }

                if (raw >= 1f)
                {
                    timer.Stop();
                    timer.Dispose();
                    form.Top = startY;
                    form.Opacity = 1;
                }
            };

            if (!form.Visible) form.Show();
            timer.Start();
        }

        // Slide+Fade para ShowDialog: animação começa no Shown do form
        public static DialogResult ShowDialogWithSlideFade(Form form, IWin32Window owner = null, int durationMs = 360, int offset = 40)
        {
            if (form == null) return DialogResult.None;

            void Handler(object s, EventArgs e)
            {
                form.Shown -= Handler;
                // iniciar animação sem chamar Show (já está sendo mostrado pelo ShowDialog)
                var startY = form.Top;
                var fromY = startY - offset;
                form.Top = fromY;
                form.Opacity = 0;

                var start = Environment.TickCount;
                var timer = new System.Windows.Forms.Timer { Interval = 12 };
                timer.Tick += (ts, te) =>
                {
                    var elapsed = Environment.TickCount - start;
                    var raw = Math.Min(1f, (float)elapsed / durationMs);
                    var t = EaseOutCubic(raw);
                    try
                    {
                        form.Top = (int)(fromY + (startY - fromY) * t);
                        form.Opacity = Math.Min(1, t);
                    }
                    catch { }
                    if (raw >= 1f)
                    {
                        timer.Stop();
                        timer.Dispose();
                        form.Top = startY;
                        form.Opacity = 1;
                    }
                };
                timer.Start();
            }

            form.Shown += Handler;
            return owner == null ? form.ShowDialog() : form.ShowDialog(owner);
        }

        // Fade-out e fechar (animação)
        public static void CloseWithFade(Form form, int durationMs = 240)
        {
            if (form == null || form.IsDisposed) return;

            var startOpacity = form.Opacity;
            var start = Environment.TickCount;
            var timer = new System.Windows.Forms.Timer { Interval = 12 };
            timer.Tick += (s, e) =>
            {
                var elapsed = Environment.TickCount - start;
                var raw = Math.Min(1f, (float)elapsed / durationMs);
                var t = EaseInOutCubic(raw);
                try
                {
                    form.Opacity = Math.Max(0, startOpacity * (1 - t));
                }
                catch { }

                if (raw >= 1f)
                {
                    timer.Stop();
                    timer.Dispose();
                    try { form.Close(); } catch { }
                }
            };
            timer.Start();
        }

        // Slide+Fade para fechar (desce e desaparece)
        public static void CloseWithSlideFade(Form form, int durationMs = 320, int offset = 40)
        {
            if (form == null || form.IsDisposed) return;

            var startOpacity = form.Opacity;
            var startY = form.Top;
            var toY = startY + offset;
            var start = Environment.TickCount;
            var timer = new System.Windows.Forms.Timer { Interval = 12 };
            timer.Tick += (s, e) =>
            {
                var elapsed = Environment.TickCount - start;
                var raw = Math.Min(1f, (float)elapsed / durationMs);
                var t = EaseInOutCubic(raw);
                try
                {
                    form.Top = (int)(startY + (toY - startY) * t);
                    form.Opacity = Math.Max(0, startOpacity * (1 - t));
                }
                catch { }

                if (raw >= 1f)
                {
                    timer.Stop();
                    timer.Dispose();
                    try { form.Close(); } catch { }
                }
            };
            timer.Start();
        }

        // Helper: conectar animação de fechamento ao evento FormClosing (cancela e anima)
        // Uso: this.FormClosing += (s,e) => FormAnimator.AnimateOnClosing(this, e, closeWithSlide: true);
        public static void AnimateOnClosing(Form form, FormClosingEventArgs e, bool closeWithSlide = true, int durationMs = 320, int offset = 40)
        {
            if (form == null) return;
            // evitar loop se já estamos fechando
            if (e.CancelledByAnimator()) return;

            // cancela fechamento e anima; depois fecha
            e.Cancel = true;
            // marcal flag temporária para evitar reentrância
            form.SetClosingFlag(true);
            if (closeWithSlide)
            {
                CloseWithSlideFade(form, durationMs, offset);
            }
            else
            {
                CloseWithFade(form, durationMs);
            }
        }

        // Easing functions
        private static float EaseInOutCubic(float x) => x < 0.5f ? 4f * x * x * x : 1f - (float)Math.Pow(-2f * x + 2f, 3) / 2f;
        private static float EaseOutCubic(float x) => 1f - (float)Math.Pow(1f - x, 3);

        // Small helpers to avoid reentrance using Control.Tag dictionary
        private const string ClosingFlagKey = "__animator_closing__";

        private static void SetClosingFlag(this Control c, bool value)
        {
            try
            {
                if (c == null) return;
                c.Tag = value ? ClosingFlagKey : null;
            }
            catch { }
        }

        public static void AnimateCloseOverlay(Form form, FormClosingEventArgs e, bool closeWithSlide = true, int durationMs = 320, int offset = 40)
        {
            if (form == null || e == null) return;

            // Protege reentrância: se já estamos fechando, permite fechar
            lock (_bypassCloseAnimation)
            {
                if (_bypassCloseAnimation.Contains(form))
                {
                    _bypassCloseAnimation.Remove(form);
                    return;
                }

                // marca que vamos animar e fechar
                _bypassCloseAnimation.Add(form);
            }

            try
            {
                // cancela fechamento imediato
                e.Cancel = true;

                // tenta capturar snapshot do form
                Bitmap snapshot;
                try
                {
                    snapshot = new Bitmap(Math.Max(1, form.Width), Math.Max(1, form.Height));
                    form.DrawToBitmap(snapshot, new Rectangle(0, 0, snapshot.Width, snapshot.Height));
                }
                catch
                {
                    snapshot = new Bitmap(Math.Max(1, form.Width), Math.Max(1, form.Height));
                    using var g = Graphics.FromImage(snapshot);
                    g.Clear(form.BackColor);
                }

                // cria overlay no mesmo lugar
                var overlay = new Form
                {
                    FormBorderStyle = FormBorderStyle.None,
                    StartPosition = FormStartPosition.Manual,
                    ShowInTaskbar = false,
                    Size = form.Size,
                    Location = form.PointToScreen(Point.Empty),
                    BackgroundImage = snapshot,
                    BackgroundImageLayout = ImageLayout.Stretch,
                    TopMost = true,
                    Opacity = 1.0
                };

                // tenta associar owner (melhora Z-order e evita flash na taskbar)
                try { overlay.Owner = form; } catch { }

                // mostra overlay ANTES de esconder o form
                overlay.Show();
                overlay.BringToFront();

                // agora escondemos o form — overlay já visível, assim evitamos piscar
                try { form.Hide(); } catch { form.Opacity = 0; }

                // anima overlay (slide para baixo + fade)
                var startY = overlay.Top;
                var toY = startY + offset;
                var startOpacity = overlay.Opacity;
                var start = Environment.TickCount;
                var timer = new System.Windows.Forms.Timer { Interval = 12 };

                timer.Tick += (s, ev) =>
                {
                    var elapsed = Environment.TickCount - start;
                    var raw = Math.Min(1f, (float)elapsed / durationMs);
                    var t = EaseInOutCubic(raw);

                    try
                    {
                        overlay.Top = (int)(startY + (toY - startY) * t);
                        overlay.Opacity = Math.Max(0, startOpacity * (1 - t));
                    }
                    catch { }

                    if (raw >= 1f)
                    {
                        timer.Stop();
                        timer.Dispose();

                        try { overlay.Close(); } catch { }

                        // Fecha o form na UI thread. Como já adicionamos ao bypass, o handler permitirá fechar.
                        try
                        {
                            if (!form.IsDisposed && form.IsHandleCreated)
                                form.BeginInvoke((Action)(() =>
                                {
                                    try { form.Close(); } catch { }
                                }));
                        }
                        finally
                        {
                            try { snapshot.Dispose(); } catch { }
                        }
                    }
                };

                timer.Start();
            }
            catch
            {
                // em caso de erro, remove o bypass e permite fechar normalmente
                lock (_bypassCloseAnimation)
                {
                    _bypassCloseAnimation.Remove(form);
                }
                e.Cancel = false;
            }
        }


        private static bool CancelledByAnimator(this FormClosingEventArgs e)
        {
            // Check caller's Form via reflection (we can't access form easily from event args),
            // so this helper is best used inline in the form's FormClosing handler as shown below.
            return false;
        }
    }
}