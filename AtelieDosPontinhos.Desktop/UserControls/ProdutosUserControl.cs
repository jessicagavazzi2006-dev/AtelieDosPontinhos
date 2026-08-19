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

        private List<CategoriaResponseDto> _categorias = new();
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
        
        private void btnNovo_Click(object sender, EventArgs e)
        {
            using var form = new ProdutoFormDialog(_categorias, null);
            if (form.ShowDialog() == DialogResult.OK && form.GameDto != null)
            {
                var (success, _, error) = await _gameService.CreateAsync(form.GameDto);
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
    }
}
