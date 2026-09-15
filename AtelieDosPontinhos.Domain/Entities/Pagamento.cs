using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

public class Pagamento
{
    public int Id { get; set; }
    public PaymentMethod Metodo { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataPagamento { get; set; }

    // Novas propriedades para pré-preenchimento
    public string? NomeCartao { get; set; }
    public string? NumeroCartao { get; set; }

    public string? UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser? User { get; set; }
    public int? PedidoId { get; set; }
}