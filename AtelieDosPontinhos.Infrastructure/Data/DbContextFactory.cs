using AtelieDosPontinhos.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AtelieDosPontinhos.Infrastructure.Data
{
    public class DbContextFactory : IDesignTimeDbContextFactory<AtelieDosPontinhosDbContext>
    {
        public AtelieDosPontinhosDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AtelieDosPontinhosDbContext>();

            // 💡 Cole aqui a sua String de Conexão do SQL Server (a mesma do seu appsettings.json)
            // Se o nome do seu banco for diferente, ajuste no "Database=..."
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=AtelieDosPontinhosDB;Trusted_Connection=True;MultipleActiveResultSets=true";

            optionsBuilder.UseSqlServer(connectionString);

            return new AtelieDosPontinhosDbContext(optionsBuilder.Options);
        }
    }
}