namespace AtelieDosPontinhos.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        public required string Description { get; set; }

        public required string CoverImageUrl { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}