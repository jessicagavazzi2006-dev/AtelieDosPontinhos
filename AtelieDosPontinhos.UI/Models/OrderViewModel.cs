using System;
using System.Collections.Generic;

namespace AtelieDosPontinhos.UI.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string EmailUsuario { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public string MetodoPagamento { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public string CEP { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public List<OrderItemViewModel> Itens { get; set; } = new List<OrderItemViewModel>();
    }

    public class OrderItemViewModel
    {
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}