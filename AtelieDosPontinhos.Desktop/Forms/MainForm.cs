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
        private AuthApiService _authService = null;

        private UserControl? _controleAtual;

        private Guna2Button? _botaoAtivo;



        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            _authService = new AuthApiService();

            this.Text = $"Atelie do pontinhos Desktop - {AppConfig.Version}";

            lblUsuario.Text = $"{SessionManager.Intance.GetDisplayName()}";
            lblPerfil.Text = SessionManager.Instance.IsAdmin ? "Administrador" : "usuario comum";
            lblSessao.Text = $"🟣 {SessionManager.Instance.GetEmail()}";

            ConfigurarPermissoes();

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
                if (Botao != null)
                {
                    _botaoAtivo.FillColor = Color.FromArgb(0, 50, 110);
                    _botaoAtivo.ForeColor = Color.White;

                }


            }
        }

        private void NavegarParaDashboard()
        {
            NavegarParaDashboard(new DashBoardUserControl(), btnDashboard);
        }

        private void Navegar(UserControl Control, Guna2Button? botao = null)
        {
            if (_controleAtual != null)
            {
                pnlConteudo.Controls.Remove(_controleAtual);
                _controleAtual.Dispose();
                _controleAtual = null;
            }

            Control.Dock = DockStyle.Fill;
            pnlConteudo.Controls.Add(Control);
            _controleAtual = Control;

            AtualizarBotaoAtivo(botao);

        }

        private async Task btnLogout_Click(object sender, EventArgs e)
        {
            var resposta = MessageBox.Show("Deseja do sair do sistema?", "confirmar Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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



        private void btnDashboard_Click_1(object sender, EventArgs e) => Navegar(new DashBoardUserControl(), btnDashboard);

        private void btnProdutos_Click(object sender, EventArgs e) => Navegar(new ProdutosUserControl(), btnProdutos);

        private void btnCategorias_Click(object sender, EventArgs e) => Navegar(new CategoriasUserControl(), btnCategorias);

        private void btnUsuarios_Click(object sender, EventArgs e) => Navegar(new UsuarioUserControl(), btnUsuarios);

        private void btnPerfil_Click(object sender, EventArgs e) => Navegar(new PerfilUserControl(), btnPerfil);

    }
}
