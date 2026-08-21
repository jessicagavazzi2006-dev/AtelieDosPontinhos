using AtelieDosPontinhos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Infrastructure.Configurations
{
    internal class EnderecoConfiguration : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.CEP)
                .IsRequired()
                .HasMaxLength(8);

            builder.Property(e => e.NUMERO)
                .IsRequired();

            builder.Property(e => e.Estado)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Referencial)
                .HasMaxLength (200);

            builder.Property(e => e.Cidade)
                .IsRequired()
                .HasMaxLength(200);

        }
    }
}
