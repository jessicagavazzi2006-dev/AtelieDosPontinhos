using AtelieDosPontinhos.Desktop.Helpers;
using AtelieDosPontinhos.Desktop.Services;
using AtelieDosPontinhos.Desktop.UserControls;
using AtelieDosPontinhos.Desktop.Themes;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtelieDosPontinhos.Desktop.Forms
{
    public partial class MainForm : Form
    {
        private AuthApiService _authService = null!;

        private UserControl? _controleAtual;

        private Guna2Button? _botaoAtivo;

        // flag de modo (false = tema padrão claro, true = dark)
        private bool _isDarkMode = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            _authService = new AuthApiService();

            this.Text = $"Ateliê dos Pontinhos Desktop - {AppConfig.Version}";

            lblUsuario.Text = $"{SessionManager.Instance.GetDisplayName()}";
            lblPerfil.Text = SessionManager.Instance.IsAdmin ? "🔑 Administrador" : "👀 usuario comum";
            lblPerfil.ForeColor = SessionManager.Instance.IsAdmin ? Color.Orange : Color.Blue;
            lblSessao.Text = $"🟢 {SessionManager.Instance.GetEmail()}";

            ConfigurarPermissoes();

            // aplica tema padrão (claro) ao carregar
            AtelieDosPontinhosTheme.AplicarEstiloFormulario(this);

            pnlSidebar.BackColor = Color.FromArgb(155, 113, 206);
            pnlSidebar.Tag = "KeepBackColor";
            pnlLogo.BackColor = Color.FromArgb(108, 58, 169);
            pnlLogo.Tag = "KeepBackColor";
            lblSidebarLogo.ForeColor = Color.White;
            lblSidebarLogo.Tag = "KeepBackColor";
            lblSidebarSub.ForeColor = Color.White;
            lblSidebarSub.Tag = "KeepBackColor";
            NavegarParaDashboard();
        }

        private void ConfigurarPermissoes()
        {
            var isAdmin = SessionManager.Instance.IsAdmin;

            btnCategorias.Visible = isAdmin;
            btnUsuarios.Visible = isAdmin;
        }

        private void AtualizarBotaoAtivo(Guna2Button? Botao)
        {
            if (_botaoAtivo != null)
            {
                _botaoAtivo.FillColor = Color.Transparent;
                _botaoAtivo.ForeColor = Color.White;

                _botaoAtivo = Botao;
                if (_botaoAtivo != null)
                {
                    _botaoAtivo.FillColor = Color.FromArgb(0, 50, 110);
                    _botaoAtivo.ForeColor = Color.White;

                }


            }
        }

        private void NavegarParaDashboard()
        {
            Navegar(new DashboardUserControl(), btnDashboard);
        }

        private void Navegar(UserControl Control, Guna2Button? botao = null)
        {
            //Remove o UserControl anterior
            if (_controleAtual != null)
            {
                pnlConteudo.Controls.Remove(_controleAtual);
                _controleAtual.Dispose();
                _controleAtual = null;
            }

            //Adiona o novo UserControl(Tela interna)
            Control.Dock = DockStyle.Fill;
            pnlConteudo.Controls.Add(Control);
            _controleAtual = Control;

            AtualizarBotaoAtivo(botao);

            // Reaplica tema ao formulário (garante que o novo UserControl receba o estilo)
            if (_isDarkMode)
                AtelieDosPontinhosDarkTheme.AplicarEstiloFormulario(this);
            else
                AtelieDosPontinhosTheme.AplicarEstiloFormulario(this);
        }

        private async void btnLogout_Click(object sender, EventArgs e)
        {
            var resposta = MessageBox.Show("Deseja sair do sistema?",
                "confirmar Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resposta != DialogResult.Yes) return;

            try
            {
                await _authService.LogoutAsync();
            }
            catch
            {
                //mesmo se a API falahar, limpa a sessão local
            }
            finally
            {
                SessionManager.Instance.Clear();
                this.Close();
            }
        }

        private void btnDashboard_Click_1(object sender, EventArgs e) => Navegar(new DashboardUserControl(), btnDashboard);

        private void btnProdutos_Click(object sender, EventArgs e) => Navegar(new ProdutosUserControl(), btnProdutos);

        private void btnCategorias_Click(object sender, EventArgs e) => Navegar(new CategoriasUserControl(), btnCategorias);

        private void btnUsuarios_Click(object sender, EventArgs e) => Navegar(new UsuariosUserControl(), btnUsuarios);

        private void btnPerfil_Click(object sender, EventArgs e) => Navegar(new PerfilUserControl(), btnPerfil);

        private void btnPedidos_Click(object sender, EventArgs e) => Navegar(new PedidosUserControl(), btnPedidos);

        private void darkModebtn_Click(object sender, EventArgs e)
        {
            // alterna o modo e reaplica o tema correspondente
            _isDarkMode = !_isDarkMode;

            //if (_isDarkMode)
            //{
            //    AtelieDosPontinhosDarkTheme.AplicarEstiloFormulario(this);
            //    // opcional: ajustar texto do botão

            //}
            //else
            //{
            //    AtelieDosPontinhosTheme.AplicarEstiloFormulario(this);

            //}
            ThemeTransitionAnimator.Transition(this, _isDarkMode, 200);
            

        }
    }
}