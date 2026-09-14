using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AtelieDosPontinhos.Desktop.Helpers
{
    public static class FormAnimator
    {
        private static readonly HashSet<Form> _animatingForms = new();
        private static readonly HashSet<Form> _allowedToClose = new();

        public static void FadeIn(Form form, int durationMs = 300)
        {
            if (form == null || form.IsDisposed)
                return;

            durationMs = Math.Max(1, durationMs);

            form.Opacity = 0;

            var start = Environment.TickCount;
            var timer = new System.Windows.Forms.Timer { Interval = 12 };

            timer.Tick += (_, _) =>
            {
                if (form.IsDisposed)
                {
                    timer.Stop();
                    timer.Dispose();
                    return;
                }

                var elapsed = Environment.TickCount - start;
                var progress = Math.Min(1f, elapsed / (float)durationMs);
                var easedProgress = EaseInOutCubic(progress);

                form.Opacity = easedProgress;

                if (progress >= 1f)
                {
                    timer.Stop();
                    timer.Dispose();
                    form.Opacity = 1;
                }
            };

            if (!form.Visible)
                form.Show();

            timer.Start();
        }

        public static void SlideFadeIn(
            Form form,
            int durationMs = 360,
            int offset = 40)
        {
            if (form == null || form.IsDisposed)
                return;

            durationMs = Math.Max(1, durationMs);

            var targetTop = form.Top;
            var initialTop = targetTop - offset;

            form.Top = initialTop;
            form.Opacity = 0;

            var start = Environment.TickCount;
            var timer = new System.Windows.Forms.Timer { Interval = 12 };

            timer.Tick += (_, _) =>
            {
                if (form.IsDisposed)
                {
                    timer.Stop();
                    timer.Dispose();
                    return;
                }

                var elapsed = Environment.TickCount - start;
                var progress = Math.Min(1f, elapsed / (float)durationMs);
                var easedProgress = EaseOutCubic(progress);

                form.Top = (int)(
                    initialTop +
                    (targetTop - initialTop) * easedProgress);

                form.Opacity = Math.Min(1, easedProgress);

                if (progress >= 1f)
                {
                    timer.Stop();
                    timer.Dispose();

                    form.Top = targetTop;
                    form.Opacity = 1;
                }
            };

            if (!form.Visible)
                form.Show();

            timer.Start();
        }

        public static DialogResult ShowDialogWithSlideFade(
            Form form,
            IWin32Window? owner = null,
            int durationMs = 360,
            int offset = 40)
        {
            if (form == null || form.IsDisposed)
                return DialogResult.OK;

            durationMs = Math.Max(1, durationMs);

            void OnShown(object? sender, EventArgs e)
            {
                form.Shown -= OnShown;

                var targetTop = form.Top;
                var initialTop = targetTop - offset;

                form.Top = initialTop;
                form.Opacity = 0;

                var start = Environment.TickCount;
                var timer = new System.Windows.Forms.Timer { Interval = 12 };

                timer.Tick += (_, _) =>
                {
                    if (form.IsDisposed)
                    {
                        timer.Stop();
                        timer.Dispose();
                        return;
                    }

                    var elapsed = Environment.TickCount - start;
                    var progress = Math.Min(1f, elapsed / (float)durationMs);
                    var easedProgress = EaseOutCubic(progress);

                    form.Top = (int)(
                        initialTop +
                        (targetTop - initialTop) * easedProgress);

                    form.Opacity = Math.Min(1, easedProgress);

                    if (progress >= 1f)
                    {
                        timer.Stop();
                        timer.Dispose();

                        form.Top = targetTop;
                        form.Opacity = 1;
                    }
                };

                timer.Start();
            }

            form.Shown += OnShown;

            return owner == null
                ? form.ShowDialog()
                : form.ShowDialog(owner);
        }

        public static void CloseWithFade(
            Form form,
            int durationMs = 240)
        {
            StartCloseAnimation(
                form,
                durationMs,
                offset: 0,
                slide: false);
        }

        public static void CloseWithSlideFade(
            Form form,
            int durationMs = 320,
            int offset = 40)
        {
            StartCloseAnimation(
                form,
                durationMs,
                offset,
                slide: true);
        }

        public static void AnimateOnClosing(
            Form form,
            FormClosingEventArgs e,
            bool closeWithSlide = true,
            int durationMs = 320,
            int offset = 40)
        {
            if (form == null || form.IsDisposed)
                return;

            lock (_allowedToClose)
            {
                // Segunda chamada do Close(): permite o fechamento real.
                if (_allowedToClose.Remove(form))
                {
                    e.Cancel = false;
                    return;
                }
            }

            // Primeira chamada: cancela o fechamento imediato.
            e.Cancel = true;

            lock (_animatingForms)
            {
                // Evita iniciar outra animação.
                if (_animatingForms.Contains(form))
                    return;
            }

            StartCloseAnimation(
                form,
                durationMs,
                offset,
                closeWithSlide);
        }

        public static void AnimateCloseOverlay(
            Form form,
            FormClosingEventArgs e,
            bool closeWithSlide = true,
            int durationMs = 320,
            int offset = 40)
        {
            // Mantém o método para compatibilidade com os outros forms.
            // O overlay antigo causava a piscada porque escondia o form
            // e criava outro Form sobre ele.
            AnimateOnClosing(
                form,
                e,
                closeWithSlide,
                durationMs,
                offset);
        }

        private static void StartCloseAnimation(
            Form form,
            int durationMs,
            int offset,
            bool slide)
        {
            if (form == null || form.IsDisposed)
                return;

            durationMs = Math.Max(1, durationMs);

            lock (_animatingForms)
            {
                if (!_animatingForms.Add(form))
                    return;
            }

            var initialTop = form.Top;
            var targetTop = slide
                ? initialTop + offset
                : initialTop;

            var initialOpacity = Math.Max(0.01, form.Opacity);
            var start = Environment.TickCount;
            var timer = new System.Windows.Forms.Timer { Interval = 15 };

            timer.Tick += (_, _) =>
            {
                if (form.IsDisposed)
                {
                    StopAnimation(form, timer);
                    return;
                }

                var elapsed = Environment.TickCount - start;
                var progress = Math.Min(1f, elapsed / (float)durationMs);
                var easedProgress = EaseInOutCubic(progress);

                try
                {
                    if (slide)
                    {
                        form.Top = (int)(
                            initialTop +
                            (targetTop - initialTop) * easedProgress);
                    }

                    form.Opacity = Math.Max(
                        0,
                        initialOpacity * (1 - easedProgress));
                }
                catch
                {
                    StopAnimation(form, timer);
                    return;
                }

                if (progress < 1f)
                    return;

                timer.Stop();
                timer.Dispose();

                lock (_animatingForms)
                {
                    _animatingForms.Remove(form);
                }

                if (form.IsDisposed)
                    return;

                form.Top = targetTop;
                form.Opacity = 0;

                lock (_allowedToClose)
                {
                    _allowedToClose.Add(form);
                }

                // Dispara FormClosing novamente.
                // Na segunda chamada, e.Cancel será explicitamente false.
                form.Close();
            };

            timer.Start();
        }

        private static void StopAnimation(Form form, System.Windows.Forms.Timer timer)
        {
            timer.Stop();
            timer.Dispose();

            lock (_animatingForms)
            {
                _animatingForms.Remove(form);
            }
        }

        private static float EaseInOutCubic(float value)
        {
            return value < 0.5f
                ? 4f * value * value * value
                : 1f - MathF.Pow(-2f * value + 2f, 3f) / 2f;
        }

        private static float EaseOutCubic(float value)
        {
            return 1f - MathF.Pow(1f - value, 3f);
        }

    }


}