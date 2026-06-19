using Microsoft.EntityFrameworkCore;
using AtelieDosPontinhos.Domain.Entities;

namespace AtelieDosPontinhos.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Produtos { get; set; }
    }
}
}
