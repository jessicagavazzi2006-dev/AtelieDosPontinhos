using AtelieDosPontinhos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Product_MaterialConfiguration : IEntityTypeConfiguration<Product_Material>
{
    public void Configure(EntityTypeBuilder<Product_Material> builder)
    {
        // Define chave composta (tabela de junção)
        builder.HasKey(pm => new { pm.ProductId, pm.MaterialId });

        // Relacionamento: Product_Material -> Product
        builder.HasOne(pm => pm.Product)
            .WithMany(p => p.Product_Materials) // sem navegação no Product
            .HasForeignKey(pm => pm.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento: Product_Material -> Material
        builder.HasOne(pm => pm.Material)
            .WithMany(m => m.Product_Materials) // sem navegação no Material
            .HasForeignKey(pm => pm.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);

        // Quantidade usada do material no produto
        builder.Property(pm => pm.UnitUsed)
            .IsRequired();
    }
}
