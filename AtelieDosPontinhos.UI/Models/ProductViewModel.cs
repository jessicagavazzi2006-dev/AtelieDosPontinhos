namespace AtelieDosPontinhos.UI.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string CoverImageUrl { get; set; }
        public bool IsFeatured { get; set; }

        // Aliases em português usados pelas Views existentes
        public string Nome { get => Name; set => Name = value; }
        public decimal Preco { get => Price; set => Price = value; }
        public string Descricao { get => Description; set => Description = value; }
        public string Imagem { get => CoverImageUrl; set => CoverImageUrl = value; }
    }
}