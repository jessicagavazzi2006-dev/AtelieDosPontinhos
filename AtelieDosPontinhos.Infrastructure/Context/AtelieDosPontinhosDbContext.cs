using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AtelieDosPontinhos.Infrastructure.Context
{
    public class AtelieDosPontinhosDbContext : IdentityDbContext
    {
        public AtelieDosPontinhosDbContext(DbContextOptions<AtelieDosPontinhosDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Dbset representa a tabela Product no banco de dados
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// DbSet representa a tabela categoria no banco de dados
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Dbset representa a tabela material no banco de dados
        /// </summary>
        public DbSet<Material> Materials { get; set; }

        /// <summary>
        /// Dbset representa a tabela Product
        /// </summary>
        public DbSet<Product_Material> ProductMaterials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProductConfiguration()); 
            modelBuilder.ApplyConfiguration(new CategoryConfiguration()); 
            modelBuilder.ApplyConfiguration(new MaterialConfiguration()); 
            modelBuilder.ApplyConfiguration(new Product_MaterialConfiguration()); 
        }
    }
}
