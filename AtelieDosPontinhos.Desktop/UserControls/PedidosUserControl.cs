using AtelieDosPontinhos.Desktop.DTOs;
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
    public partial class PedidosUserControl : UserControl
    {
        //=================================================
        // SERVIÇOS (Inicilizados no Load)
        //=================================================
        private PedidosService _pedidosService = null!;

        //=================================================
        // DADOS
        //=================================================
        private List<Pedido> _todosPedidos = new();

        //=================================================
        // CONSTRUTOR
        //=================================================
        public PedidosUserControl()
        {
            InitializeComponent();
        }

        private async void PedidosUserControl_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            _pedidosService = new PedidosService();

            AtelieDosPontinhosTheme.AplicarEstiloGrid(gridPedidos);

            await CarregarDadosAsync();
        }

        private async Task CarregarDadosAsync()
        {
            gridPedidos.Rows.Clear();

            try
            {
                var tarefaPedidos = _pedidosService.GetAllAsync();
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
                gridPedidos.Rows.Add(
                    p.Id,
                    p.UserId,
                    p.DataPedido.ToString("dd/MM/yyyy"),
                    p.Cidade,
                    p.MetodoPagamento,
                    p.ValorTotal.ToString("C"),
                    p.Status
                );
            }
        }
    }
}
