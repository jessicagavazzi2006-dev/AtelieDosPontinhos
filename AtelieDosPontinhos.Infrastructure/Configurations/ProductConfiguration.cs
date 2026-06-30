using AtelieDosPontinhos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AtelieDosPontinhos.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Define a chave primária da tabela Product
            builder.HasKey(p => p.Id);

            // Nome do produto
            builder.Property(p => p.Name)
                .IsRequired() // campo obrigatório
                .HasMaxLength(200); // limite de caracteres

            // Descrição do produto
            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(2000);

            // URL da imagem do produto
            builder.Property(p => p.CoverImageUrl)
                .HasMaxLength(2000);

            // Preço do produto
            builder.Property(p => p.Price)
                .IsRequired()
                .HasPrecision(10, 2); // 10 dígitos no total, 2 casas decimais

            // Estoque do produto
            builder.Property(p => p.Stock)
                .IsRequired();


            // Relacionamento: um Product pertence a uma Category
            builder.HasOne(p => p.Category) // UM Product tem UMA Category
                .WithMany(c => c.Products)  // UMA Category tem MUITOS Products
                .HasForeignKey(p => p.CategoryId) // chave estrangeira
                .OnDelete(DeleteBehavior.Restrict); // impede exclusão em cascata
        }
    }
}