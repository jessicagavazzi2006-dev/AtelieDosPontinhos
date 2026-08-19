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

namespace AtelieDosPontinhos.Desktop.UserControls
{
    public partial class CategoriasUserControl : UserControl
    {

        private CategoriasApiService _categoriaService = null;
        private List<CategoriaRsponseDto> _categorias = new();
        private int? _editandoId = null;
        public CategoriasUserControl()
        {
            InitializeComponent();
        }
        private async Task CarregarDadosAsync()
        {
            gridCategorias.Rows.Clear();
            try
            {
                _categorias = await _categoriaService.GetAllAsync();
                foreach (var c in _categorias)
                {
                    gridCategorias.Rows.Add(c.Id, c.Name, c.ProductCount);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private async void CategoriasUserControl_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            _categoriaService = new CategoriasApiService();

            await CarregarDadosAsync();
        }
        private void MostrarFormulario(CategoriaResponseDto? categoria)
        {
            _editandoId = categoria?.Id;
            txtNome.Text = categoria?.Name ?? string.Empty;
            lblFormTitulo.Text = categoria == null ? "Nova Categoria" : "Editar Categoria";
            pnlForm.Visible = true;
            txtNome.Focus();
        }

        private void OcultarFormulario()
        {
            pnlForm.Visible = false;
            _editandoId = null;
            txtNome.Text = string.Empty;
        }

        private CategoriaResponseDto? ObterCategoriaSelecionada()
        {
            if (gridCategorias.SelectedRows.Count == 0) return null;
            var id = Convert.ToInt32(gridCategorias.SelectedRows[0].Cells["colId"].Value);
            return _categorias.FirstOrDefault(c => c.Id == id);
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            var cat = ObterCategoriaSelecionada();
            if (cat == null)
            {
                MessageBox.Show("selecione uam categoria para editar", "avisa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MostrarFormulario(cat);
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            var cat = ObterCategoriaSelecionada();
            if (cat == null)
            {
                MessageBox.Show("Selecione uma categoria para excluir.", "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
            if (cat.ProductCount > 0)
            {
                MessageBox.Show($"A categoria \"{cat.Name}\" possui {cat.ProductCount} game(s) vinculado(s). \nRemova os games antes de excluir a categoria",
                    "Não é possivel excluir",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var conf = MessageBox.Show($"Excluir a categoria \"{cat.Name}\"?",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);


            if (conf != DialogResult.Yes) return;

            var (sucess, error) = await _categoriaService.DeleteAsync(cat.Id);
            if (sucess)
            {
                MessageBox.Show($"Categoria Excluída!",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                await CarregarDadosAsync();
            }
            else
            {
                MessageBox.Show($"{error}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void btnAtualizar_Click(object sender, EventArgs e) => await CarregarDadosAsync();

        private async void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show($"Categoria Excluída!",
                "validação",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
                return;
            }
            bool sucess;
            string error;

            if (_editandoId == null)
            {
                var dto = new CreateCategoryDto { Name = txtNome.Text.Trim() };
                var result = await _categoriaService.CreateAsync(dto);
                sucess = result.Success;
                error = result.ErrorMessage;
            }
            else
            {
                var dto = new UpdateCategoryDto { Name = txtNome.Text.Trim() };
                var result = await _categoriaService.UpdateAsync(_editandoId.Value, dto);
                sucess = result.Success;
                error = result.ErrorMessage;
            }

            if (sucess)
            {
                MessageBox.Show($"salvo com sucesso",
                "validação",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
                OcultarFormulario();
                await CarregarDadosAsync();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e) => OcultarFormulario();

    }
}
