using AtelieDosPontinhos.Desktop.DTOs;
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
    public partial class UsuarioFormDialog : Form
    {
        // =====================================================================
        // PROPRIEDADES DE SAÍDA
        // =====================================================================
        public CreateUsuarioDto? CreateDto { get; private set; }
        public UpdateUsuarioDto? UpdateDto { get; private set; }

        // =====================================================================
        // CAMPOS PRIVADOS
        // =====================================================================
        private List<string> _perfis = new();
        private UsuarioResponseDto? _usuarioExistente;

        // =====================================================================
        // CONSTRUTORES
        // =====================================================================
        public UsuarioFormDialog()
        {
            InitializeComponent();
        }

        public UsuarioFormDialog(List<string> perfis, UsuarioResponseDto? usuarioExistente = null) : this()
        {
            _perfis = perfis;
            _usuarioExistente = usuarioExistente;

            PreencherComboPerfis();

            if (_usuarioExistente != null)
            {
                lblTituloForm.Text = "✏️ Editar Usuário";
                txtNome.Text = _usuarioExistente.UserName;
                txtEmail.Text = _usuarioExistente.Email;

                if (cmbPerfil.Items.Contains(_usuarioExistente.PerfilPrincipal))
                {
                    cmbPerfil.SelectedItem = _usuarioExistente.PerfilPrincipal;
                }
            }
            else
            {
                lblTituloForm.Text = "➕ Novo Usuário";
                if (cmbPerfil.Items.Count > 0)
                    cmbPerfil.SelectedIndex = 0;
            }
        }

        private void PreencherComboPerfis()
        {
            cmbPerfil.Items.Clear();
            foreach (var p in _perfis)
            {
                cmbPerfil.Items.Add(p);
            }
        }

        // =====================================================================
        // SALVAR
        // =====================================================================
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Nome e Email são obrigatórios.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (_usuarioExistente == null && string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Senha é obrigatória para novos usuários.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (txtSenha.Text != txtConfirmarSenha.Text)
            {
                MessageBox.Show("As senhas não coincidem.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (cmbPerfil.SelectedItem == null)
            {
                MessageBox.Show("Selecione um perfil.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (_usuarioExistente == null)
            {
                CreateDto = new CreateUsuarioDto
                {
                    UserName = txtNome.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Password = txtSenha.Text,
                    ConfirmPassword = txtConfirmarSenha.Text,
                    Role = cmbPerfil.SelectedItem.ToString()!
                };
            }
            else
            {
                UpdateDto = new UpdateUsuarioDto
                {
                    UserName = txtNome.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Password = string.IsNullOrEmpty(txtSenha.Text) ? null : txtSenha.Text,
                    ConfirmPassword = string.IsNullOrEmpty(txtConfirmarSenha.Text) ? null : txtConfirmarSenha.Text,
                    Role = cmbPerfil.SelectedItem.ToString()!
                };
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void UsuarioFormDialog_Load(object sender, EventArgs e)
        {

        }
    }
}
