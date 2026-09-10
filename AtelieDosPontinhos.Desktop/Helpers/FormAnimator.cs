using System; // namespaces básicos do .NET
using System.Drawing; // para Bitmap, Graphics, Point, Rectangle
using System.Windows.Forms; // para Form, Control, Timer, etc.

namespace AtelieDosPontinhos.Desktop.Helpers // namespace do projeto
{
    public static class FormAnimator // classe estática que contém utilitários de animação de Forms
    {
        private static readonly HashSet<Form> _bypassCloseAnimation = new(); // conjunto para controlar bypass de animação (evita reentrância)

        // Fade-in simples (usado para Forms modais e não-modais)
        public static void FadeIn(Form form, int durationMs = 300) // inicia opacidade de 0 até 1 em 'durationMs' ms
        {
            if (form == null) return; // valida nulo

            form.Opacity = 0; // garante que comece invisível
            var start = Environment.TickCount; // tempo inicial para cálculo de progresso
            var timer = new System.Windows.Forms.Timer { Interval = 12 }; // timer para atualizar animação (~83FPS)
            timer.Tick += (s, e) => // evento chamado a cada tick
            {
                var elapsed = Environment.TickCount - start; // tempo decorrido
                var raw = Math.Min(1f, (float)elapsed / durationMs); // progresso linear entre 0 e 1
                var t = EaseInOutCubic(raw); // aplica easing para suavizar
                try { form.Opacity = t; } catch { } // atualiza opacidade com segurança
                if (raw >= 1f) // quando finalizado
                {
                    timer.Stop(); // para timer
                    timer.Dispose(); // libera timer
                    form.Opacity = 1; // garante opacidade final
                }
            };
            // Se o form ainda não estiver mostrado, mostramos e depois animamos
            if (!form.Visible) form.Show(); // mostra o form se necessário
            timer.Start(); // inicia o timer
        }

        // Slide-in vertical + fade (não-modal)
        public static void SlideFadeIn(Form form, int durationMs = 360, int offset = 40) // desliza o form de cima para posição com fade
        {
            if (form == null) return; // valida nulo

            var startY = form.Top; // posição final Y desejada
            var fromY = startY - offset; // posição inicial acima da final
            form.Top = fromY; // posiciona inicialmente acima
            form.Opacity = 0; // começa invisível

            var start = Environment.TickCount; // tempo inicial
            var timer = new System.Windows.Forms.Timer { Interval = 12 }; // timer de animação
            timer.Tick += (s, e) => // a cada tick
            {
                var elapsed = Environment.TickCount - start; // tempo decorrido
                var raw = Math.Min(1f, (float)elapsed / durationMs); // progresso normalizado
                var t = EaseOutCubic(raw); // easing para movimento

                try
                {
                    form.Top = (int)(fromY + (startY - fromY) * t); // interpola Y
                    form.Opacity = Math.Min(1, t); // interpola opacidade (fade-in)
                }
                catch { } // ignora erros de UI

                if (raw >= 1f) // final da animação
                {
                    timer.Stop(); // para timer
                    timer.Dispose(); // libera timer
                    form.Top = startY; // garante posição final precisa
                    form.Opacity = 1; // garante opacidade final
                }
            };

            if (!form.Visible) form.Show(); // se necessário, mostra o form antes de animar
            timer.Start(); // inicia o timer
        }

        // Slide+Fade para ShowDialog: animação começa no Shown do form
        public static DialogResult ShowDialogWithSlideFade(Form form, IWin32Window owner = null, int durationMs = 360, int offset = 40)
        {
            if (form == null) return DialogResult.None; // valida nulo

            void Handler(object s, EventArgs e) // handler que roda quando o form é mostrado (Shown)
            {
                form.Shown -= Handler; // remove o handler para não executar novamente
                // iniciar animação sem chamar Show (já está sendo mostrado pelo ShowDialog)
                var startY = form.Top; // posição final
                var fromY = startY - offset; // posição inicial acima
                form.Top = fromY; // posiciona inicialmente
                form.Opacity = 0; // inicia invisível

                var start = Environment.TickCount; // tempo inicial
                var timer = new System.Windows.Forms.Timer { Interval = 12 }; // timer de animação
                timer.Tick += (ts, te) =>
                {
                    var elapsed = Environment.TickCount - start; // tempo decorrido
                    var raw = Math.Min(1f, (float)elapsed / durationMs); // progresso
                    var t = EaseOutCubic(raw); // easing para movimento
                    try
                    {
                        form.Top = (int)(fromY + (startY - fromY) * t); // interpola posição
                        form.Opacity = Math.Min(1, t); // interpola opacidade
                    }
                    catch { } // ignora possíveis exceções
                    if (raw >= 1f) // fim
                    {
                        timer.Stop();
                        timer.Dispose();
                        form.Top = startY; // garante posição final
                        form.Opacity = 1; // garante opacidade final
                    }
                };
                timer.Start(); // inicia animação
            }

            form.Shown += Handler; // anexa handler ao evento Shown
            return owner == null ? form.ShowDialog() : form.ShowDialog(owner); // mostra modal e retorna resultado
        }

        // Fade-out e fechar (animação)
        public static void CloseWithFade(Form form, int durationMs = 240) // reduz opacidade até 0 e depois chama Close()
        {
            if (form == null || form.IsDisposed) return; // valida estado

            var startOpacity = form.Opacity; // opacidade inicial atual
            var start = Environment.TickCount; // tempo inicial
            var timer = new System.Windows.Forms.Timer { Interval = 12 }; // timer
            timer.Tick += (s, e) =>
            {
                var elapsed = Environment.TickCount - start; // decorrido
                var raw = Math.Min(1f, (float)elapsed / durationMs); // progresso
                var t = EaseInOutCubic(raw); // easing suave
                try
                {
                    form.Opacity = Math.Max(0, startOpacity * (1 - t)); // reduz opacidade
                }
                catch { }

                if (raw >= 1f) // quando terminar
                {
                    timer.Stop();
                    timer.Dispose();
                    try { form.Close(); } catch { } // fecha o form
                }
            };
            
        }

        // Slide+Fade para fechar (desce e desaparece)
        public static void CloseWithSlideFade(Form form, int durationMs = 320, int offset = 40) // move form para baixo e desvanece antes de fechar
        {
            if (form == null || form.IsDisposed) return; // valida

            var startOpacity = form.Opacity; // opacidade inicial
            var startY = form.Top; // posição inicial Y
            var toY = startY + offset; // posição final Y (abaixo)
            var start = Environment.TickCount; // tempo inicial
            var timer = new System.Windows.Forms.Timer { Interval = 12 }; // timer
            timer.Tick += (s, e) =>
            {
                var elapsed = Environment.TickCount - start; // decorrido
                var raw = Math.Min(1f, (float)elapsed / durationMs); // progresso
                var t = EaseInOutCubic(raw); // easing
                try
                {
                    form.Top = (int)(startY + (toY - startY) * t); // desloca verticalmente
                    form.Opacity = Math.Max(0, startOpacity * (1 - t)); // reduz opacidade
                }
                catch { }

                if (raw >= 1f) // fim da animação
                {
                    timer.Stop(); // para timer
                    timer.Dispose(); // libera
                    try { form.Close(); } catch { } // fecha o form
                }
            };
            timer.Start(); // inicia
        }

        // Helper: conectar animação de fechamento ao evento FormClosing (cancela e anima)
        // Uso: this.FormClosing += (s,e) => FormAnimator.AnimateOnClosing(this, e, closeWithSlide: true);
        public static void AnimateOnClosing(Form form, FormClosingEventArgs e, bool closeWithSlide = true, int durationMs = 320, int offset = 40)
        {
            if (form == null) return; // valida

            // evitar loop se já estamos fechando (verifica se foi cancelado por nosso animador)
            if (e.CancelledByAnimator()) return; // método placeholder que retorna false por enquanto

            // cancela fechamento e anima; depois fecha
            e.Cancel = true; // cancela a ação de fechamento imediata
            // marca flag temporária no controle para evitar reentrância no handler
            form.SetClosingFlag(true); // marca o controle para indicar que estamos animando
            if (closeWithSlide) // escolhe animação
            {
                CloseWithSlideFade(form, durationMs, offset); // anima slide+fade
            }
            else
            {
                CloseWithFade(form, durationMs); // anima fade
            }
        }

        // Easing functions
        private static float EaseInOutCubic(float x) => x < 0.5f ? 4f * x * x * x : 1f - (float)Math.Pow(-2f * x + 2f, 3) / 2f; // easing in/out cúbico
        private static float EaseOutCubic(float x) => 1f - (float)Math.Pow(1f - x, 3); // easing out cúbico

        // Small helpers to avoid reentrance using Control.Tag dictionary
        private const string ClosingFlagKey = "__animator_closing__"; // chave usada no Tag para sinalizar fechamento em andamento

        private static void SetClosingFlag(this Control c, bool value) // define ou remove a flag no Tag do controle
        {
            try
            {
                if (c == null) return; // valida
                c.Tag = value ? ClosingFlagKey : null; // seta Tag com a chave ou limpa
            }
            catch { } // ignora falhas
        }

        public static void AnimateCloseOverlay(Form form, FormClosingEventArgs e, bool closeWithSlide = true, int durationMs = 320, int offset = 40)
        {
            if (form == null || e == null) return; // valida parâmetros

            // Protege reentrância: se já estamos fechando, permite fechar
            lock (_bypassCloseAnimation) // lock para acessar o conjunto com segurança
            {
                if (_bypassCloseAnimation.Contains(form)) // se já marcado para bypass
                {
                    _bypassCloseAnimation.Remove(form); // remove e permite fechar normalmente
                    return; // sai do método
                }

                // marca que vamos animar e fechar, para evitar double-handling
                _bypassCloseAnimation.Add(form);
            }

            try
            {
                // cancela fechamento imediato (vamos fechar após animação)
                e.Cancel = true;

                // tenta capturar snapshot do form para desenhar overlay idêntico
                Bitmap snapshot;
                try
                {
                    snapshot = new Bitmap(Math.Max(1, form.Width), Math.Max(1, form.Height)); // cria bitmap do tamanho do form
                    form.DrawToBitmap(snapshot, new Rectangle(0, 0, snapshot.Width, snapshot.Height)); // desenha o form no bitmap
                }
                catch
                {
                    // fallback: bitmap com cor de fundo do form
                    snapshot = new Bitmap(Math.Max(1, form.Width), Math.Max(1, form.Height));
                    using var g = Graphics.FromImage(snapshot);
                    g.Clear(form.BackColor); // pinta com BackColor
                }

                // cria overlay no mesmo lugar do form para animação visual contínua
                var overlay = new Form
                {
                    FormBorderStyle = FormBorderStyle.None, // sem borda
                    StartPosition = FormStartPosition.Manual, // posição manual
                    ShowInTaskbar = false, // não aparece na taskbar
                    Size = form.Size, // mesmo tamanho
                    Location = form.PointToScreen(Point.Empty), // mesma posição na tela
                    BackgroundImage = snapshot, // usa snapshot como imagem de fundo
                    BackgroundImageLayout = ImageLayout.Stretch, // adapta a imagem
                    TopMost = true, // fica sobre as outras janelas
                    Opacity = 1.0 // visível inicialmente
                };

                // tenta associar owner (melhora Z-order e evita flash na taskbar)
                try { overlay.Owner = form; } catch { }

                // mostra overlay ANTES de esconder o form para evitar piscada
                overlay.Show();
                overlay.BringToFront();

                // agora escondemos o form — overlay já visível, assim evitamos piscar
                try { form.Hide(); } catch { form.Opacity = 0; } // se Hide falhar, define opacidade 0 como fallback

                // anima overlay (slide para baixo + fade) para dar sensação de fechamento
                var startY = overlay.Top; // posição inicial Y do overlay
                var toY = startY + offset; // posição final Y
                var startOpacity = overlay.Opacity; // opacidade inicial
                var start = Environment.TickCount; // tempo inicial
                var timer = new System.Windows.Forms.Timer { Interval = 12 }; // timer de animação

                timer.Tick += (s, ev) =>
                {
                    var elapsed = Environment.TickCount - start; // decorrido
                    var raw = Math.Min(1f, (float)elapsed / durationMs); // progresso
                    var t = EaseInOutCubic(raw); // easing

                    try
                    {
                        overlay.Top = (int)(startY + (toY - startY) * t); // move overlay para baixo gradualmente
                        overlay.Opacity = Math.Max(0, startOpacity * (1 - t)); // reduz opacidade gradualmente
                    }
                    catch { }

                    if (raw >= 1f) // quando terminar animação
                    {
                        timer.Stop(); // para timer
                        timer.Dispose(); // libera timer

                        try { overlay.Close(); } catch { } // fecha overlay

                        // Fecha o form na UI thread. Como já adicionamos ao bypass, o handler permitirá fechar.
                        try
                        {
                            if (!form.IsDisposed && form.IsHandleCreated) // valida espaço de execução da UI
                                form.BeginInvoke((Action)(() =>
                                {
                                    try { form.Close(); } catch { } // fecha o form de verdade
                                }));
                        }
                        finally
                        {
                            try { snapshot.Dispose(); } catch { } // libera bitmap
                        }
                    }
                };

                timer.Start(); // inicia animação do overlay
            }
            catch
            {
                // em caso de erro, remove o bypass e permite fechar normalmente
                lock (_bypassCloseAnimation)
                {
                    _bypassCloseAnimation.Remove(form); // garante que o form não ficará marcado
                }
                e.Cancel = false; // permite fechamento imediato
            }
        }


        private static bool CancelledByAnimator(this FormClosingEventArgs e)
        {
            // Check caller's Form via reflection (we can't access form easily from event args),
            // so this helper is best used inline in the form's FormClosing handler as shown below.
            return false; // placeholder: atualmente sempre retorna false (pode ser estendido para detectar se cancelado por animador)
        }
    }
}