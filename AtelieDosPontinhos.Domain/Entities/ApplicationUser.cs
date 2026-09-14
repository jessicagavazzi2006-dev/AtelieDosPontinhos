using Microsoft.AspNetCore.Identity;

namespace AtelieDosPontinhos.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Nome { get; set; } = string.Empty;
    }
}