using System.ComponentModel.DataAnnotations;

namespace AtelieDosPontinhos.UI.Models
{
    public class RegisterClienteViewModel
    {
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, ErrorMessage = "A senha deve ter pelo menos {2} caracteres.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar senha")]
        [Compare("Password", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        // Novos campos mapeados diretamente
        public string CEP { get; set; } = string.Empty;

        public string Logradouro { get; set; } = string.Empty;

        public string Numero { get; set; } = string.Empty;

        public string? Complemento { get; set; }

        public string Bairro { get; set; } = string.Empty;

        public string Cidade { get; set; } = string.Empty;

        public string Estado { get; set; } = string.Empty;

        public string TipoPagamento { get; set; } = string.Empty;
        public string? NomeNoCartao { get; set; }
        public string? NumeroCartaoMascarado { get; set; }
    }
}