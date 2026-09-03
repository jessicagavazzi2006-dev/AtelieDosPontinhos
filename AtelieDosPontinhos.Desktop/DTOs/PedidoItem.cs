using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtelieDosPontinhos.Desktop.DTOs
{
    public class PedidoItem
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; }

        public int ProductId { get; set; }
        public ProductResponseDto? Product { get; set; }

        // Campos que podem vir do endpoint /api/orders (projeção) e
        // não mapeiam diretamente para a entidade; mantemos para
        // compatibilidade com a resposta usada pelo Desktop.
        public string? NomeProduto { get; set; }
        public decimal PrecoUnitario { get; set; }

        public int Quantidade { get; set; }

    }
}