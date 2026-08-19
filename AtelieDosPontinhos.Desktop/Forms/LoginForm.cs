using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtelieDosPontinhos.Desktop.Forms
{
    public partial class LoginForm : Form
    {
        private AuthApiService _authService = null!;
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            _authService = new AuthApiService();

            lblVersao.Text = $"Versão {AppConfig.Version} | @{DateTime.Now.Year} Senac-sp ";
            lblApi.Text = $" API: {AppConfig.ApiBaseUrl}";

            txtEmail.Text = "admin@SenacGames.com";
            txtSenha.Text = "Admin@123";
        }

        private async Task btnEntrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                ExibirErro("informe o email");
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                ExibirErro("informe o email");
                txtSenha.Focus();
                return;
            }

            SetCarregando(true);

            try
            {
                var (success, user, errorMessage) = await _authService.LoginAsync(txtEmail.Text.Trim(), txtSenha.Text);

                if (success && user != null)
                {
                    sessionManager.Instance.SetUser(user)

                    this.Hide();

                    using var mainForm = new MainForm();
                    mainForm.ShowDialog();

                    this.Close();
                }
                else
                {
                    ExibirErro($"X{errorMessage}");
                }
            }
            catch (HttpRequestException)
            {
                ExibirErro("Xnao foi conectatr a API \n verifique se a API esta em execução ")
            }
            catch (Exception ex)
            {
                ExibirErro($"X erro inesperado{ex.Message}");
            }
            finally
            {
                SetCarregando(false);
            }

        }
        private void ExibirErro(string Mensagem)
        {
            if (string.IsNullOrEmpty(Mensagem))
            {
                lblErro.Visible = false;
                lblErro.Text = string.Empty;
            }
            else
            {
                lblErro.Text = Mensagem;
                lblErro.Visible = true;
            }
        }
        private void SetCarregando(bool carregando)
        {
            btnEntrar.Enabled = !carregando;
            txtEmail.Enabled = !carregando;
            txtSenha.Enabled = !carregando;
            lblCarregando.Visible = carregando;

            if (carregando)
            {
                btnEntrar.Text = "aguarde...";
                lblErro.Visible = false;
            }
            else
            {
                btnEntrar.Text = "ENTRAR";
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}


