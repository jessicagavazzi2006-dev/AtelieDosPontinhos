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

namespace AtelieDosPontinhos.Desktop.Forms
{
    public partial class DetalhesPedidosForm : Form
    {
        public Pedido? Pedido { get; set; }

        private List<PedidoItem> _itens = new();
        private readonly UsuariosApiService _usuariosService = new();

        public DetalhesPedidosForm()
        {
            InitializeComponent();
        }

        // Construtor que recebe um Pedido para facilitar a abertura a partir do UserControl
        public DetalhesPedidosForm(Pedido pedido)
        {
            InitializeComponent();
            Pedido = pedido;
            _ = PreencherDadosAsync();
        }

        private async void DetalhesPedidosForm_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            await PreencherDadosAsync();

            AtelieDosPontinhosTheme.AplicarEstiloGrid(itemCompradosGrid);
        }

        private async Task PreencherDadosAsync()
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

            // Cliente: tenta buscar o nome do usuário via API. Se não encontrado, mostra o UserId
            var nomeCliente = Pedido.UserId;
            try
            {
                var usuarios = await _usuariosService.GetAllAsync();
                var usuario = usuarios.FirstOrDefault(u => u.Id == Pedido.UserId);
                if (usuario != null && !string.IsNullOrWhiteSpace(usuario.UserName))
                    nomeCliente = usuario.UserName;
            }
            catch
            {
                // se falhar, mantemos o UserId
            }

            ClienteLbl.Text = $"Cliente: {nomeCliente}";

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
            // Itens: mostra nome, quantidade, preço unitário e preço total por item
            _itens = Pedido.Itens ?? new List<PedidoItem>();
            foreach (var item in _itens)
            {
                // o backend tem duas formas de retornar os itens:
                // - quando retorna a entidade completa, item.Product está preenchido
                // - quando retorna uma projeção (AllOrders) ele inclui NomeProduto e PrecoUnitario
                // então damos preferência a Product, se estiver ausente usamos os campos projetados
                var productName = item.Product?.Name ?? item.NomeProduto ?? $"Produto #{item.ProductId}";
                var quantidade = item.Quantidade;
                var precoUnitario = item.Product?.Price ?? item.PrecoUnitario;
                var valorItem = precoUnitario * quantidade;

                var precoUnitarioTexto = precoUnitario.ToString("C");
                var valorItemTexto = valorItem.ToString("C");

                itemCompradosGrid.Rows.Add(productName, quantidade, precoUnitarioTexto, valorItemTexto);
            }

            // Total do pedido (usa ValorTotal do Pedido)
            totalLbl.Text = Pedido.ValorTotal.ToString("C");
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void EnderecoLbl_Click(object sender, EventArgs e)
        {

        }
    }

}
