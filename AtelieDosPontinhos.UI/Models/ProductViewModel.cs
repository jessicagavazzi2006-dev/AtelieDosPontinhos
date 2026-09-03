namespace AtelieDosPontinhos.UI.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Nome { get => Name; set => Name = value; }
        public decimal Price { get; set; } = 0m;
        public decimal Preco { get => Price; set => Price = value; }
        public string CoverImageUrl { get; set; } = string.Empty;
        public string ImagemUrl { get => CoverImageUrl; set => CoverImageUrl = value; }
        public string Description { get; set; } = string.Empty;
        public string Descricao { get => Description; set => Description = value; }

        // Adicione esta linha para controlar o status do favorito
        public bool IsFavorited { get; set; } = false;
    }
}