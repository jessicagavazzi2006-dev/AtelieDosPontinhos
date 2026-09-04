using AtelieDosPontinhos.Desktop.DTOs;
using AtelieDosPontinhos.Desktop.Forms;
using AtelieDosPontinhos.Desktop.Helpers;
using AtelieDosPontinhos.Desktop.Services;
using AtelieDosPontinhos.Desktop.Themes;
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
        private List<string> _perfil = new();
        public UsuariosUserControl()
        {
            InitializeComponent();
        }

        private async void UsuariosUserControl_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            _usuarioService = new UsuariosApiService();
            ConfigurarPermissoes();

            AtelieDosPontinhosTheme.AplicarEstiloGrid(gridUsuarios);

            //reservado para CarregarDados
            await CarregarDadosAsync();
        }

        private void ConfigurarPermissoes()
        {
            bool isAdmin = SessionManager.Instance.IsAdmin;
            btnNovo.Visible = isAdmin;
            btnEditar.Visible = isAdmin;
            btnExcluir.Visible = isAdmin;
        }

        private async Task CarregarDadosAsync()
        {
            gridUsuarios.Rows.Clear();

            try
            {
                var tarefaUsuarios = _usuarioService.GetAllAsync();
                var tarefaPerfis = _usuarioService.GetPerfisAsync();
                await Task.WhenAll(tarefaUsuarios, tarefaPerfis);

                _todosUsuarios = tarefaUsuarios.Result;
                _perfil = tarefaPerfis.Result;

                PopularGrid(_todosUsuarios);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar usuários: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PopularGrid(List<UsuarioResponseDto> usuarios)
        {
            gridUsuarios.Rows.Clear();
            foreach (var u in usuarios)
            {
                gridUsuarios.Rows.Add(
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.PerfilPrincipal
                );
            }

        }
        private void txtPesquisa_TextChanged(object sender, EventArgs e) => FiltrarUsuarios();

        private void FiltrarUsuarios()
        {
            var termo = txtPesquisa.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(termo))
            {
                PopularGrid(_todosUsuarios);
                return;
            }

            var filtrados = _todosUsuarios
                .Where(u => u.UserName.Contains(termo, StringComparison.OrdinalIgnoreCase)
                         || u.Email.Contains(termo, StringComparison.OrdinalIgnoreCase))
                .ToList();

            PopularGrid(filtrados);
        }

        private async void btnNovo_Click(object sender, EventArgs e)
        {
            using var form = new UsuarioFormDialog(_perfil, null);
            if (form.ShowDialog() == DialogResult.OK && form.CreateDto != null)
            {
                var (success, _, error) = await _usuarioService.CreateAsync(form.CreateDto);
                if (success)
                {
                    MessageBox.Show("✅ Usuário criado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await CarregarDadosAsync();
                }
                else
                {
                    MessageBox.Show($"❌ {error}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            var usuario = ObterUsuarioSelecionado();
            if (usuario == null)
            {
                MessageBox.Show("Selecione um usuário para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var form = new UsuarioFormDialog(_perfil, usuario);
            if (form.ShowDialog() == DialogResult.OK && form.UpdateDto != null)
            {
                var (success, _, error) = await _usuarioService.UpdateAsync(usuario.Id, form.UpdateDto);
                if (success)
                {
                    MessageBox.Show("✅ Usuário atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await CarregarDadosAsync();
                }
                else
                {
                    MessageBox.Show($"❌ {error}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            var usuario = ObterUsuarioSelecionado();
            if (usuario == null)
            {
                MessageBox.Show("Selecione um usuário para excluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var conf = MessageBox.Show($"Tem certeza que deseja excluir o usuário:\n\"{usuario.UserName}\"?",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (conf != DialogResult.Yes) return;

            var (success, error) = await _usuarioService.DeleteAsync(usuario.Id);
            if (success)
            {
                MessageBox.Show("✅ Usuário excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CarregarDadosAsync();
            }
            else
            {
                MessageBox.Show($"❌ {error}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAtualizar_Click(object sender, EventArgs e) => await CarregarDadosAsync();

        private UsuarioResponseDto? ObterUsuarioSelecionado()
        {
            if (gridUsuarios.SelectedRows.Count == 0) return null;
            var row = gridUsuarios.SelectedRows[0];
            var id = row.Cells["colId"].Value?.ToString();
            return _todosUsuarios.FirstOrDefault(u => u.Id == id);
        } 
    }
}
