using Microsoft.AspNetCore.Identity;

namespace AtelieDosPontinhos.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        // Endereço de Entrega
        public string? Cep { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Referencia { get; set; }

        // Preferências de Pagamento
        public string? Metodo { get; set; }
        public string? Titular { get; set; }
        public string? Cartao
        {
            get; set; }
        }
    }