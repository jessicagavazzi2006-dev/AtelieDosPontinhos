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
    public partial class DashboardUserControl : UserControl
    {
        private ProdutosApiService _produtoService = null;
        private CategoriasApiService _categoriasService = null;
        public DashboardUserControl()
        {
            InitializeComponent();
        }

        private async void DashboardUserControl_Load(object sender, EventArgs e)
        {
            //guard não executa em tempo de design
            if (DesignMode) return;

            //inicializa serviços 
            _produtoService = new ProdutosApiService();
            _categoriasService = new CategoriasApiService();


            //Preenche dados dinamicos da sessão
            lblTitulo.Text = $"Olá, {SessionManager.Instance.GetDisplayName()!}";
            lblSubtitulo.Text = $"Bem-vindo ao Ateliê dos Pontinhos Desktop - {DateTime.Now:dddd, dd 'de' MMM 'de' yyyy}";

            //aplica estilo no DataGridView(tabela)
            AtelieDosPontinhosTheme.AplicarEstiloGrid(gridUltimosProdutos);

            await CarregarDadosAsync();
        }
        private async Task CarregarDadosAsync()
        {
            SetCarregando(true);

            try
            {
                var tarefaProduto = _produtoService.GetAllAsync();
                var tarefasCategorias = _categoriasService.GetAllAsync();
                await Task.WhenAll(tarefaProduto, tarefasCategorias);

                var produto = tarefaProduto.Result;
                var categorias = tarefasCategorias.Result;

                cardProdutosLblNumero.Text = produto.Count.ToString();
                cardCategoriasLblNumero.Text = categorias.Count.ToString();

                //Atualiza os dados do card
                //AtualizarNumeroCard(cardProdutos, produto.Count().ToString());
                //AtualizarNumeroCard(cardCategorias, categorias.Count().ToString());

                //Popula o DataGridView(tabela) com os últimos 10 produtos.
                gridUltimosProdutos.Rows.Clear();
                foreach (var p in produto.OrderByDescending(x => x.Id).Take(10))
                {
                    gridUltimosProdutos.Rows.Add(
                        p.Id,
                        p.Name,
                        p.Price,
                        p.Stock,
                        p.CategoryId,
                        p.IsFeatured
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao Carregar dados: {ex.Message}",
                "Erro",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            }
            finally
            {
                SetCarregando(false);
            }


        }

        private void AtualizarNumeroCard(Guna.UI2.WinForms.Guna2Panel card, String numero)
        {
            var lblNumero = card.Controls.OfType<Label>().FirstOrDefault(l => l.Tag?.ToString() == "numero");
            if (lblNumero != null)
            {
                lblNumero.Text = numero;
            }
        }

        private void SetCarregando(bool carregando)
        {
            lblCarregando.Visible = carregando;
            cardProdutos.Visible = !carregando;
            cardCategorias.Visible = !carregando;
            lblUltimosProdutos.Visible = !carregando;
            gridUltimosProdutos.Visible = !carregando;
        }
    }
}
