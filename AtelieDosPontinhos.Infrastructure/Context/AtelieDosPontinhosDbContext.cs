using AtelieDosPontinhos.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AtelieDosPontinhos.Infrastructure.Context
{
    public class AtelieDosPontinhosDbContext : IdentityDbContext<ApplicationUser>
    {
        public AtelieDosPontinhosDbContext(DbContextOptions<AtelieDosPontinhosDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoItem> PedidoItens { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Product_Material> ProductMaterials { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Chave composta para a tabela N:N de produtos e materiais
            builder.Entity<Product_Material>()
                .HasKey(pm => new { pm.ProductId, pm.MaterialId });

            builder.Entity<Product_Material>()
                .HasOne(pm => pm.Product)
                .WithMany(p => p.Product_Materials)
                .HasForeignKey(pm => pm.ProductId);

            builder.Entity<Product_Material>()
                .HasOne(pm => pm.Material)
                .WithMany(m => m.Product_Materials)
                .HasForeignKey(pm => pm.MaterialId);
        }
    }
}