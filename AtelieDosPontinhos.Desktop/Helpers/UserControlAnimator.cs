using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtelieDosPontinhos.Desktop.Helpers
{
    public static class UserControlAnimator
    {
        public enum SlideDirection { LeftToRight, RightToLeft, TopToBottom, BottomToTop }

        public static async Task SlideSwitchAsync(Control container, UserControl newControl, UserControl? oldControl = null,
            SlideDirection direction = SlideDirection.RightToLeft, int durationMs = 300, bool removeOldImmediately = false)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (newControl == null) throw new ArgumentNullException(nameof(newControl));

            // Se pedir para remover imediatamente, tira o antigo agora
            if (removeOldImmediately && oldControl != null && container.Controls.Contains(oldControl))
            {
                container.Controls.Remove(oldControl);
                try { oldControl.Dispose(); } catch { }
                oldControl = null;
            }

            // Ensure sizes
            newControl.Parent ??= container;
            newControl.Visible = true;
            newControl.BringToFront();
            newControl.Dock = DockStyle.None;
            newControl.Width = container.ClientSize.Width;
            newControl.Height = container.ClientSize.Height;
            newControl.Top = 0;

            // Starting positions
            var startNewLeft = direction switch
            {
                SlideDirection.RightToLeft => container.ClientSize.Width,
                SlideDirection.LeftToRight => -newControl.Width,
                _ => 0
            };
            var endNewLeft = 0;

            var startOldLeft = oldControl != null ? 0 : 0;
            var endOldLeft = direction switch
            {
                SlideDirection.RightToLeft => -oldControl?.Width ?? -newControl.Width,
                SlideDirection.LeftToRight => (oldControl?.Width ?? newControl.Width) + container.ClientSize.Width,
                _ => startOldLeft
            };

            // Vertical directions (optional)
            var startNewTop = direction switch
            {
                SlideDirection.BottomToTop => container.ClientSize.Height,
                SlideDirection.TopToBottom => -newControl.Height,
                _ => 0
            };
            var endNewTop = 0;
            var startOldTop = 0;
            var endOldTop = direction switch
            {
                SlideDirection.BottomToTop => -(oldControl?.Height ?? newControl.Height),
                SlideDirection.TopToBottom => (oldControl?.Height ?? newControl.Height) + container.ClientSize.Height,
                _ => 0
            };

            // Set initial pos
            newControl.Left = startNewLeft;
            newControl.Top = startNewTop;

            // Add newControl to container if not already
            if (!container.Controls.Contains(newControl))
                container.Controls.Add(newControl);

            var start = Environment.TickCount;
            var interval = 12;
            while (true)
            {
                var elapsed = Environment.TickCount - start;
                var raw = Math.Min(1f, (float)elapsed / durationMs);
                var t = EaseInOutCubic(raw);

                // interpolate horizontal
                if (direction == SlideDirection.LeftToRight || direction == SlideDirection.RightToLeft)
                {
                    var newLeft = (int)(startNewLeft + (endNewLeft - startNewLeft) * t);
                    newControl.Left = newLeft;

                    if (oldControl != null)
                    {
                        var oldLeft = (int)(startOldLeft + (endOldLeft - startOldLeft) * t);
                        oldControl.Left = oldLeft;
                    }
                }
                else // vertical
                {
                    var newTop = (int)(startNewTop + (endNewTop - startNewTop) * t);
                    newControl.Top = newTop;

                    if (oldControl != null)
                    {
                        var oldTop = (int)(startOldTop + (endOldTop - startOldTop) * t);
                        oldControl.Top = oldTop;
                    }
                }

                await Task.Delay(interval).ConfigureAwait(true);

                if (raw >= 1f) break;
            }

            // finalizar: garantir layout e dock fill
            newControl.Dock = DockStyle.Fill;
            newControl.Left = 0;
            newControl.Top = 0;

            if (oldControl != null && container.Controls.Contains(oldControl))
            {
                container.Controls.Remove(oldControl);
                try { oldControl.Dispose(); } catch { }
            }
        }

        private static float EaseInOutCubic(float x)
            => x < 0.5f ? 4f * x * x * x : 1f - (float)Math.Pow(-2f * x + 2f, 3) / 2f;
    }
}