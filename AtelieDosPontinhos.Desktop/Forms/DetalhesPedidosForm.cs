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

namespace AtelieDosPontinhos.Desktop.Forms
{
    public partial class DetalhesPedidosForm : Form
    {
        public Pedido? Pedido { get; set; }

        private List<PedidoItem> _itens = new();

        public DetalhesPedidosForm()
        {
            InitializeComponent();
        }

        private void DetalhesPedidosForm_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            PreencherDados();
        }

        private void PreencherDados()
        {
            // Limpa estado anterior
            itemCompradosGrid.Rows.Clear();
            totalLbl.Text = "R$ 0,00";

            if (Pedido == null)
            {
                ClienteLbl.Text = "Cliente: -";
                DataLbl.Text = "Data: -";
                EnderecoLbl.Text = "Endereço: -";
                return;
            }

            // Cliente (aqui temos apenas UserId; ajuste se tiver nome do cliente)
            ClienteLbl.Text = $"Cliente: {Pedido.UserId}";

            // Data do pedido formatada
            DataLbl.Text = $"Data: {Pedido.DataPedido:dd/MM/yyyy HH:mm}";

            // Endereço composto
            var enderecoParts = new List<string>();
            if (!string.IsNullOrWhiteSpace(Pedido.CEP)) enderecoParts.Add($"CEP: {Pedido.CEP}");
            var enderecoLocal = new List<string>();
            if (!string.IsNullOrWhiteSpace(Pedido.Cidade)) enderecoLocal.Add(Pedido.Cidade);
            if (!string.IsNullOrWhiteSpace(Pedido.Estado)) enderecoLocal.Add(Pedido.Estado);
            if (!string.IsNullOrWhiteSpace(Pedido.Numero)) enderecoLocal.Add($"Nº {Pedido.Numero}");
            if (!string.IsNullOrWhiteSpace(Pedido.Complemento)) enderecoLocal.Add(Pedido.Complemento);

            var endereco = string.Join(" • ", enderecoLocal);
            var enderecoTexto = string.IsNullOrWhiteSpace(endereco) ? string.Join(" • ", enderecoParts) : $"{endereco} • {string.Join(" • ", enderecoParts)}";
            EnderecoLbl.Text = string.IsNullOrWhiteSpace(enderecoTexto) ? "Endereço: -"
                : $"Endereço: {enderecoTexto}";

            // Itens
            _itens = Pedido.Itens ?? new List<PedidoItem>();
            foreach (var item in _itens)
            {
                var productName = item.Product?.Name ?? $"Produto #{item.ProductId}";
                var quantidade = item.Quantidade;
                var precoUnitario = item.Product?.Price ?? 0m;
                var valorItem = precoUnitario * quantidade;

                var itemTexto = $"{productName} x{quantidade}";
                var precoTexto = valorItem.ToString("C");

                itemCompradosGrid.Rows.Add(itemTexto, precoTexto);
            }

            // Total do pedido (usa ValorTotal do Pedido)
            totalLbl.Text = Pedido.ValorTotal.ToString("C");
        }
    }

}
