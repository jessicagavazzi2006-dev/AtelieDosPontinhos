namespace AtelieDosPontinhos.UI.Models
{
    public class UserProfileViewModel
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public string? Cep { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Referencial { get; set; }
        public string? Metodo { get; set; }
        public string? Titular { get; set; }
        public string? Cartao { get; set; }
    }
}