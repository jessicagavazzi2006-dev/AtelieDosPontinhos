using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AtelieDosPontinhos.Desktop.DTOs

using AtelieDosPontinhos.Desktop.Forms;

namespace AtelieDosPontinhos.Desktop.UserControls
{
    public partial class ProdutosUserControl : UserControl
    {
        private ProdutosApiService _produtosService = null;
        private CategoriasApiService _categoriasService = null;

        private List<ProductResponseDto> _todosPrdoutos = new();

        private List<CategoryDto> _categorias = new();
        public ProdutosUserControl()
        {
            InitializeComponent();
        }

        private void ProdutosUserControl_Load(object sender, EventArgs e)
        {
            //Guard não executa em tempo de desing
            if (DesignMode) return;

            _produtosService = new ProdutosApiService();
            _categoriasService = new CategoriasApiService();



            ConfigurarPermissoes();

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
            gridProdutos.Rows.Clear();

            try
            {
                var tarefaGames = _produtosService.GetAllAsync();
                var tarefaCategorias = _categoriasService.GetAllAsync();
                await Task.WhenAll(tarefaCategorias, tarefaGames);

                _todosPrdoutos = tarefaGames.Result;
                _categorias = tarefaCategorias.Result;

                PopularGrid(_todosPrdoutos);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar games : {ex.Message}", "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

            }
        }

        private void PopularGrid(List<ProductResponseDto> Produtos)
        {
            gridProdutos.Rows.Clear();
            foreach (var p in Produtos)
            {
                gridProdutos.Rows.Add(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.CategoryId,
                    p.Stock,
                    p.IsFeatured

                );
            }
        }

        private void btnPesquisar_Click(object sender, EventArgs e) => FiltrarGames();

        private void FiltrarGames()
        {
            var termo = txtPesquisa.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(termo))
            {
                PopularGrid(_todosPrdoutos);
                return;
            }
            var filtrados = _todosPrdoutos.Where(p => p.Name.Contains(termo, StringComparison.OrdinalIgnoreCase)
                || p.Description.Contains(termo, StringComparison.OrdinalIgnoreCase)).ToList();

            PopularGrid(filtrados);
        }

        private async void btnNovo_Click(object sender, EventArgs e)
        {
            using var form = new ProdutoFormDialog(_categorias, null);
            if (form.ShowDialog() == DialogResult.OK && form.ProdutoDto != null)
            {
                var (success, _, error) = await _produtosService.CreateAsync(form.ProdutoDto);
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

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            var produto = ObterGameSelecionado();
            if (produto == null)
            {
                MessageBox.Show($"Selecione um game para editar", "Avisar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using var form = new ProdutoFormDialog(_categorias, produto);
            if (form.ShowDialog() == DialogResult.OK && form.UpdateDto != null)
            {
                var (success, _, error) = await _produtosService.UpdateAsync(produto.Id, form.UpdateDto);
                if (success)
                {
                    MessageBox.Show("game atualizado", "sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await CarregarDadosAsync();
                }
                else
                {
                    MessageBox.Show($"X {error}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private ProductResponseDto? ObterGameSelecionado()
        {
            if (gridProdutos.SelectedRows.Count == 0) return null;
            var row = gridProdutos.SelectedRows[0];
            var id = Convert.ToInt32(row.Cells["colId"].Value);
            return _todosPrdoutos.FirstOrDefault(g => g.Id == id);
        }

        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            var gam = ObterGameSelecionado();
            if (gam == null)
            {
                MessageBox.Show("Selecione uma categoria para excluir.", "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }


            var conf = MessageBox.Show($"Excluir O GAME \"{gam.Name}\"?",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);


            if (conf != DialogResult.Yes) return;

            var (sucess, error) = await _produtosService.DeleteAsync(gam.Id);
            if (sucess)
            {
                MessageBox.Show($"GAME Excluída!",
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

    }
}
