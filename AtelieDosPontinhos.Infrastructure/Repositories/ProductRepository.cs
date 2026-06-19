using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Domain.Interfaces;
using AtelieDosPontinhos.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AtelieDosPontinhosDbContext _context;

        public ProductRepository(AtelieDosPontinhosDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category) // Faz JOIN com a tabela Categoria
                .OrderByDescending(p => p.Price) // Ordenar por preço mais caro 
                .ToListAsync();
        }


    }
}
