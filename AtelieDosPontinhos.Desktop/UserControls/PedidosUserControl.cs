using AtelieDosPontinhos.Desktop.DTOs;
using AtelieDosPontinhos.Desktop.Services;
using AtelieDosPontinhos.Desktop.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtelieDosPontinhos.Desktop.UserControls
{
    public partial class PedidosUserControl : UserControl
    {
        private PedidosApiService _pedidosService = null!;
        private ProdutosApiService _produtosService = null!;
        private UsuariosApiService _usuariosService = null!;

        private List<Pedido> _todosPedidos = new();
        private List<ProductResponseDto> _todosProdutos = new();
        private List<UsuarioResponseDto> _todosUsuarios = new();

        public PedidosUserControl()
        {
            InitializeComponent();
        }

        private async void PedidosUserControl_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            _pedidosService = new PedidosApiService();
            _produtosService = new ProdutosApiService();
            _usuariosService = new UsuariosApiService();

            // Se preferir, configure colunas programaticamente aqui
            ConfigurarColunasGrid();

            AtelieDosPontinhosTheme.AplicarEstiloGrid(gridPedidos);

            await CarregarDadosAsync();
        }

        private void ConfigurarColunasGrid()
        {
            gridPedidos.Columns.Clear();
            gridPedidos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "Id" });
            gridPedidos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colUsuario", HeaderText = "Usuário" });
            gridPedidos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colData", HeaderText = "Data" });
            gridPedidos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCidade", HeaderText = "Cidade" });
            gridPedidos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPagamento", HeaderText = "Pagamento" });
            gridPedidos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colValor", HeaderText = "Valor" });
            gridPedidos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colStatus", HeaderText = "Status" });
            gridPedidos.Columns.Add(new DataGridViewTextBoxColumn { Name = "colItens", HeaderText = "Itens" }); // contagem
        }

        private async Task CarregarDadosAsync()
        {
            gridPedidos.Rows.Clear();

            try
            {
                var tPedidos = _pedidosService.GetAllAsync();
                var tUsuarios = _usuariosService.GetAllAsync();
                var tProdutos = _produtosService.GetAllAsync();

                await Task.WhenAll(tPedidos, tUsuarios, tProdutos);

                _todosPedidos = tPedidos.Result;
                _todosUsuarios = tUsuarios.Result;
                _todosProdutos = tProdutos.Result;

                PopularGrid(_todosPedidos);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar pedidos: {ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void PopularGrid(List<Pedido> pedidos)
        {
            gridPedidos.Rows.Clear();
            foreach (var p in pedidos)
            {
                // Busca nome do usuário (assumindo que UsuarioResponseDto tem uma propriedade identificadora igual a Pedido.UserId)
                var usuarioNome = _todosUsuarios.FirstOrDefault(u => u.Id == p.UserId)?.UserName ?? p.UserId;

                // Apenas contagem de itens; se PedidoItem tiver ProductId é possível montar string com nomes dos produtos
                var itensCount = p.Itens?.Count ?? 0;

                gridPedidos.Rows.Add(
                    p.Id,
                    usuarioNome,
                    p.DataPedido.ToString("dd/MM/yyyy"),
                    p.Cidade,
                    p.MetodoPagamento,
                    p.ValorTotal.ToString("C"),
                    p.Status,
                    itensCount
                );
            }
        }
    }
}