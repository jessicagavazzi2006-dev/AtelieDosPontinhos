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
        private ProdutoApiService _produtoService = null;
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
            _produtoService = new ProdutoApiService();
            _categoriasService = new CategoriasApiService();


            //Preenche dados dinamicos da sessão
            cardCategoriasLblTitulo.Text = $"Olá, {SessionManager.Instance.GetDisplayName()!}";
            lblSubtitulo.Text = $"Bem-vindo ao SenacGames Desktop - {DateTime.Now:dddd, dd 'de' MMM 'de' yyyy}";

            //aplica estilo no DataGridView(tabela)
           

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

                AtualizarNumeroCard(cardProdutos, produto.Count().ToString());
                AtualizarNumeroCard(cardCategorias, categorias.Count().ToString());

                //atuliza os dadods do card
                gridUltimosProdutos.Rows.Clear();
                foreach (var game in produto.OrderByDescending(x => x.CreatedAt).Take(10))
                {
                    gridUltimosProdutos.Rows.Add(
                        game.Id,
                        game.Title,
                        game.CategoryName,
                        game.ReleaseYear,
                        game.IsFeatured,
                        game.CreatedAt.ToString("dd/MM/yyyy HH:mm")
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
            //reservado
            gridUltimosProdutos.Visible = !carregando;
        }
    }
}
