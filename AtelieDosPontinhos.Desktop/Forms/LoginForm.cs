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
    }

}
