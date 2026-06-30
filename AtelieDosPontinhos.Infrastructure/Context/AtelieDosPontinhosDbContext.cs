using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AtelieDosPontinhos.Domain;
using AtelieDosPontinhos.Domain.Entities; // Ajuste se suas entidades usarem outro namespace
using AtelieDosPontinhos.Infrastructure.Configurations;

namespace AtelieDosPontinhos.Infrastructure.Context
{
    public class AtelieDosPontinhosDbContext : IdentityDbContext
    {
        public AtelieDosPontinhosDbContext(DbContextOptions<AtelieDosPontinhosDbContext> options) : base(options)
        {
        }

        // 🛠️ TODAS AS SUAS TABELAS DE VOLTA PARA NÃO QUEBRAR OS REPOSITÓRIOS:
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Material> Materials { get; set; }
        /// <summary>
        /// Tabela de junção entre Product e Material
        /// </summary>
        public DbSet<Product_Material> ProductMaterials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🖼️ CONFIGURAÇÃO DA IMAGEM LONGA EM BASE64:
            modelBuilder.Entity<Product>()
                .Property(p => p.CoverImageUrl)
                .HasColumnType("nvarchar(max)");

            // Aplicar configurações específicas de entidade (inclui chave composta para Product_Material)
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new MaterialConfiguration());
            modelBuilder.ApplyConfiguration(new Product_MaterialConfiguration());
        }
    }
}