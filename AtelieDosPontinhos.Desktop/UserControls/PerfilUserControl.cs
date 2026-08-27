using AtelieDosPontinhos.Desktop.Helpers;
using AtelieDosPontinhos.Desktop.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtelieDosPontinhos.Desktop.UserControls
{
    public partial class PerfilUserControl : UserControl
    {
        private AuthApiService _authService = null!;
        public PerfilUserControl()
        {
            InitializeComponent();
        }

        private void PerfilUserControl_Load(object sender, EventArgs e)
        {
            //guard
            if (DesignMode) return;

            //inicializa o serviço de autenticação
            _authService = new AuthApiService();

            //Preenche os dados  se sessão no User Control
            var displayName = SessionManager.Instance.GetDisplayName();

            var email = SessionManager.Instance.GetEmail();
            var isAdmin = SessionManager.Instance.IsAdmin;

            btnAvatar.Text = displayName.Length > 0 ? displayName.Substring(0, 1).ToUpper() : "U";

            lblNome.Text = displayName;
            lblEmailValor.Text = email;
            lblApiValor.Text = AppConfig.ApiBaseUrl;

            var perfil = isAdmin ? "🔑 Administrador" : "👀 Usuário comum";
            var corBadge = isAdmin ? Color.Orange : Color.Blue;

            lblBadge.Text = perfil;
            lblBadge.BackColor = corBadge;

            var roles = SessionManager.Instance.CurrentUser?.Roles ?? new List<string>();

            lblRolesValor.Text = roles.Count > 0 ? string.Join(", ", roles) : "sem perfil atribuido";
        }

      
    }
}
