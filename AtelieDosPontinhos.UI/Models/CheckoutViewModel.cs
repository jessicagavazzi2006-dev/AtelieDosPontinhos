namespace AtelieDosPontinhos.UI.Models
{
    public class CheckoutViewModel
    {
        public string EmailUsuario { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Referencial { get; set; } = string.Empty;
        public string Metodo { get; set; } = string.Empty;
        public string Titular { get; set; } = string.Empty;
        public string Cartao { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public List<CartItemViewModel> Items { get; set; } = new();
    }
}