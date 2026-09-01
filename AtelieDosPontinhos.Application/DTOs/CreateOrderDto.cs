using System.Collections.Generic;

namespace AtelieDosPontinhos.Application.DTOs
{
    public class CreateOrderDto
    {
        public string? EmailUsuario { get; set; }
        public decimal ValorTotal { get; set; }
        public string? MetodoPagamento { get; set; }
        public string? CEP { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }

        /// <summary>
        /// Items enviado pela UI (com 'I' maiúsculo, como no JSON do frontend)
        /// </summary>
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}
