using AtelieDosPontinhos.Desktop.DTOs;
using AtelieDosPontinhos.Desktop.Forms;
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
    public partial class UsuariosUserControl : UserControl
    {
        private UsuariosApiService _usuarioService = null!;

        private List<UsuarioResponseDto> _todosUsuarios = new();
        public UsuariosUserControl()
        {
            InitializeComponent();
        }

        private void UsuariosUserControl_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            _usuarioService = new UsuariosApiService();

            bool isAdmin = SessionManager.Instance.IsAdmin;
            btnNovo.Visible = isAdmin;
            btnEditar.Visible = isAdmin;

            //reservado para CarregarDados
            await CarregarDadosAsync();
        }

        private async Task CarregarDadosAsync()
        {
            gridUsuarios.Rows.Clear();

            try
            {
                var tarefaUsuarios = await _usuarioService.GetAllAsync();

                // armazenar resultado no campo usado pelo filtro / grid
                _todosUsuarios = tarefaUsuarios ?? new List<UsuarioResponseDto>();

                PopularGrid(_todosUsuarios);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"deu ruim!: {ex.Message}");
            }


        }

        private void PopularGrid(List<UsuarioResponseDto> usuarios)
        {
            gridUsuarios.Rows.Clear();
            foreach (var u in usuarios)
            {
                gridUsuarios.Rows.Add(
                    u.Id,
                    u.Email,
                    u.PerfilPrincipal
                );
            }

        }

        private async void btnNovo_Click(object sender, EventArgs e)
        {
            using var form = new UsuarioFormDialog();
            if (form.ShowDialog() == DialogResult.OK && form.CreateDto != null)
            {
                var (success, _, error) = await _usuarioService.CreateAsync(form.CreateDto);
                if (success)
                {
                    MessageBox.Show("game criado com sucesso", "sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await CarregarDadosAsync();
                }
                else
                {
                    MessageBox.Show($"X {error}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        ///reservado













    }
}
