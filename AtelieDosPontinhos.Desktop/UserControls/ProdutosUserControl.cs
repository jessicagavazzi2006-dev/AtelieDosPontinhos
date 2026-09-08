using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AtelieDosPontinhos.Desktop.DTOs;

using AtelieDosPontinhos.Desktop.Forms;
using AtelieDosPontinhos.Desktop.Helpers;
using AtelieDosPontinhos.Desktop.Services;
using AtelieDosPontinhos.Desktop.Themes;

namespace AtelieDosPontinhos.Desktop.UserControls
{
    public partial class ProdutosUserControl : UserControl
    {
        //=================================================
        // SERVIÇOS (Inicilizados no Load)
        //=================================================
        private ProdutosApiService _produtosService = null;
        private CategoriasApiService _categoriasService = null;

        //=================================================
        // DADOS
        //=================================================
        private List<ProductResponseDto> _todosPrdoutos = new();
        private List<CategoriaResponseDto> _categorias = new();

        //=================================================
        // CONSTRUTOR
        //=================================================
        public ProdutosUserControl()
        {
            InitializeComponent();
        }

        private async void ProdutosUserControl_Load(object sender, EventArgs e)
        {
            //Guard não executa em tempo de desing
            if (DesignMode) return;

            _produtosService = new ProdutosApiService();
            _categoriasService = new CategoriasApiService();

            AtelieDosPontinhosTheme.AplicarEstiloGrid(gridProdutos);

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
                MessageBox.Show($"Erro ao carregar os produtos : {ex.Message}", "Erro", MessageBoxButtons.OK,
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
                    p.Price,
                    p.Stock,
                    p.CategoryName,
                    p.IsFeatured

                );
            }
        }

        private void txtPesquisa_TextChanged(object sender, EventArgs e) => FiltrarGames();

        private void FiltrarGames()
        {
            var termo = txtPesquisa.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(termo))
            {
                PopularGrid(_todosPrdoutos);
                return;
            }
            var filtrados = _todosPrdoutos.Where(p => p.Name.Contains(termo, StringComparison.OrdinalIgnoreCase)
                || p.CategoryName.Contains(termo, StringComparison.OrdinalIgnoreCase)).ToList();

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
                    MessageBox.Show("✅ Produto criado com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            var produto = ObterGameSelecionado();
            if (produto == null)
            {
                MessageBox.Show($"Selecione um produto para editar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using var form = new ProdutoFormDialog(_categorias, produto);
            if (form.ShowDialog() == DialogResult.OK && form.UpdateDto != null)
            {
                var (success, _, error) = await _produtosService.UpdateAsync(produto.Id, form.UpdateDto);
                if (success)
                {
                    MessageBox.Show("✅ Produto atualizado", "sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await CarregarDadosAsync();
                }
                else
                {
                    MessageBox.Show($"❌ {error}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Selecione uma produto para excluir.", "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }


            var conf = MessageBox.Show($"Excluir o produto \"{gam.Name}\"?",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);


            if (conf != DialogResult.Yes) return;

            var (sucess, error) = await _produtosService.DeleteAsync(gam.Id);
            if (sucess)
            {
                MessageBox.Show($"✅ Produto excluído com sucesso!",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                await CarregarDadosAsync();
            }
            else
            {
                MessageBox.Show($"❌ {error}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void btnAtualizar_Click(object sender, EventArgs e) => await CarregarDadosAsync();

    }
}
