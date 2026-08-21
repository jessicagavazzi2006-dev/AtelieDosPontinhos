using System;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

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

        // FK para IdentityUser (string)
        public string? UserId { get; set; }

        // Navegação (opcional)
        [ForeignKey(nameof(UserId))]
        public IdentityUser? User { get; set; }
    }
}
