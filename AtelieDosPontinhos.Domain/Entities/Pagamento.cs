using AtelieDosPontinhos.Domain.Enums;
using Microsoft.AspNetCore.Identity;

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtelieDosPontinhos.Domain.Entities
{
    public class Pagamento
    {
        public int Id { get; set; }

        // Mapeia o Enum (Credito, Debito, Pix) criado no passo anterior
        public PaymentMethod Metodo { get; set; }

        // Valor monetário corretamente como decimal
        public decimal Valor { get; set; }

        public DateTime DataPagamento { get; set; }

        // Chave Estrangeira (FK) para vincular o pagamento ao Usuário Logado (Identity)
        public string? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser? User { get; set; }

        // Opcional: vínculo com pedido/carrinho
        public int? PedidoId { get; set; }
    }
}
