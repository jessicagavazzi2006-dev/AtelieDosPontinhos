using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AtelieDosPontinhos.Domain.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime DataPedido { get; set; } = DateTime.Now;
        public decimal ValorTotal { get; set; }
        public string Status { get; set; } = "Pendente"; // Pendente, Pago, Enviado, Concluído
        public string MetodoPagamento { get; set; } = string.Empty;

        // Dados de Entrega usados no momento da compra
        public string CEP { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;

        // Relacionamento com os itens comprados
        public List<PedidoItem> Itens { get; set; } = new List<PedidoItem>();
    }
}
