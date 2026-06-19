using AtelieDosPontinhos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Infrastructure.Configurations
{
    public class Product_MaterialConfiguration : IEntityTypeConfiguration<Product_Material>
    {
        public void Configure(EntityTypeBuilder<Product_Material> builder)
        {
            builder.HasKey(pm => new { pm.ProductId, pm.MaterialId });

            builder.HasOne(pm => pm.Product)
                .WithMany(pm => pm.Product_Materials)
                .HasForeignKey(pm => pm.ProductId) // a FK é ProductId
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pm => pm.Material)
                .WithMany(pm => pm.Product_Materials)
                .HasForeignKey(pm => pm.MaterialId) // a FK é MaterialId
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(pm => pm.UnitUsed)
                .IsRequired();


        }
    }
}
