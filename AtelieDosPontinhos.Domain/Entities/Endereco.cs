using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtelieDosPontinhos.Domain.Entities
{
    public class Endereco
    {
        public int Id { get; set; }
        public string CEP { get; set; } = string.Empty;
        public int Numero { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Referencia { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;

        public string? UserId { get; set; }

        // Alterado de IdentityUser para ApplicationUser
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser? User { get; set; }
    }
}