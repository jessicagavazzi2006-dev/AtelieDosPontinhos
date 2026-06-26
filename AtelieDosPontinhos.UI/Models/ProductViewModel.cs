namespace AtelieDosPontinhos.UI.Models
{
    public class ProductViewModel
    {
        // Propriedades principais em Português (padrão do seu banco)
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;

        // 🔄 ATALHOS EM INGLÊS (Para as Views que usam termos antigos não quebrarem)
        public string Name { get => Nome; set => Nome = value; }
        public decimal Price { get => Preco; set => Preco = value; }
        public string Description { get => Descricao; set => Descricao = value; }
        public string Image { get => CoverImageUrl; set => CoverImageUrl = value; }
    }
}