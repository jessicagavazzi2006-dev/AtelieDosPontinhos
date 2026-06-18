using AtelieDosPontinhos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AtelieDosPontinhos.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p  => p.Id);

            builder.Property(p => p.Name)
                .IsRequired() // Define que este campo é OBRIGATORIO preencher
                .HasMaxLength(200); // Define um tamanho máximo para preencher o campo

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(p => p.CoverImageUrl)
                .HasMaxLength(500);

            builder.Property(p => p.Price)
                .IsRequired()
                .HasPrecision(10, 2); // Define que este campo todo número será 10 números e 2 casas

            builder.Property(p => p.Stock)
                .IsRequired();

            builder.Property(p => p.Type_Product)
                .IsRequired()
                .HasMaxLength (200);

            builder.HasOne(p => p.Category) // UM produto tem UMA categoria
                .WithMany(p => p.Products) // UMA categoria tem MUITOS pordutos
                .HasForeignKey(g => g.CategoryId) // a FK é CategoryId
                .OnDelete(DeleteBehavior.Restrict); // Esse método vai dar um bloqueio ao tentar excluir a tabela com couteúdo 

        }
    }
}
