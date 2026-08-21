using AtelieDosPontinhos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity;

namespace AtelieDosPontinhos.Infrastructure.Configurations
{
    internal class PagamentoConfiguration : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Metodo)
                   .IsRequired();

            builder.Property(p => p.Valor)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(p => p.DataPagamento)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne<IdentityUser>()
                   .WithMany()
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}