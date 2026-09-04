using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AtelieDosPontinhos.Domain;
using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Infrastructure.Configurations;
using AtelieDosPontinhos.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace AtelieDosPontinhos.Infrastructure.Context
{
    public class AtelieDosPontinhosDbContext : IdentityDbContext
    {
        public AtelieDosPontinhosDbContext(DbContextOptions<AtelieDosPontinhosDbContext> options) : base(options)
        {
        }

        // 🛠️ TABELAS DO SISTEMA
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Product_Material> ProductMaterials { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoItem> PedidoItens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //// Mapeamento explícito para evitar conflitos de herança com o IdentityUser
            //modelBuilder.Entity<ApplicationUser>(b =>
            //{
            //    b.ToTable("AspNetUsers");
            //});

            //modelBuilder.Entity<Endereco>(eb =>
            //{
            //    eb.HasKey(e => e.Id);
            //    eb.Property(e => e.CEP).HasMaxLength(20);
            //    eb.HasOne<ApplicationUser>()
            //      .WithMany()
            //      .HasForeignKey(e => e.UserId)
            //      .OnDelete(DeleteBehavior.Cascade);
            //});

            //// 🖼️ CONFIGURAÇÃO DA IMAGEM LONGA EM BASE64:
            //modelBuilder.Entity<Product>()
            //    .Property(p => p.CoverImageUrl)
            //    .HasColumnType("nvarchar(max)");

            // Aplicar configurações específicas de entidade
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new MaterialConfiguration());
            modelBuilder.ApplyConfiguration(new PagamentoConfiguration());
            modelBuilder.ApplyConfiguration(new EnderecoConfiguration());
            modelBuilder.ApplyConfiguration(new Product_MaterialConfiguration());

            modelBuilder.Entity<Pedido>().Property(p => p.ValorTotal).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<PedidoItem>().Property(pi => pi.PrecoUnitario).HasColumnType("decimal(18,2)");
        }
    }
}