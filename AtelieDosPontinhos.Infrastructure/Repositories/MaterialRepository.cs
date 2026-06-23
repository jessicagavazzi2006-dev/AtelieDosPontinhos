using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Domain.Interfaces;
using AtelieDosPontinhos.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Infrastructure.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly AtelieDosPontinhosDbContext _context;

        public MaterialRepository(AtelieDosPontinhosDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Material>> GetAllAsync()
        {
            return await _context.Materials
                .Include(m => m.Product_Materials)
                .ThenInclude(m => m.Product)
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        public async Task<Material?> GetByIdAsync(int id)
        {
            return await _context.Materials
                .Include(m => m.Product_Materials)
                .ThenInclude(m => m.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(Material material)
        {
            await _context.Materials.AddAsync(material);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Material material)
        {
            _context.Materials.Update(material);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material != null)
            {
                _context.Materials.Remove(material);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CountAsync()
        {
            return await _context.Materials.CountAsync();
        }
    }
}
