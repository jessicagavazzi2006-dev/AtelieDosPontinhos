using AtelieDosPontinhos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Infrastructure.Configurations
{
    internal class PagamentoConfiguration : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.FormadePagamento)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.ValorPagamento)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(p => p.DataPagamento)
                .IsRequired();

        }
    }
}
