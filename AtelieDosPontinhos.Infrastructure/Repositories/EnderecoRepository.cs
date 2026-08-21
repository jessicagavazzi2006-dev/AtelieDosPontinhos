using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Domain.Interfaces;
using AtelieDosPontinhos.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtelieDosPontinhos.Infrastructure.Repositories
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly AtelieDosPontinhosDbContext _context;

        public EnderecoRepository(AtelieDosPontinhosDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Endereco>> GetAllAsync()
        {
            return await _context.Enderecos
                .AsNoTracking()
                .OrderBy(e => e.Id)
                .ToListAsync();
        }

        public async Task<Endereco?> GetByIdAsync(int id)
        {
            return await _context.Enderecos
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // Regra simples para "destaques": retorna endereços que tenham campo Referencial preenchido.
        // Se nenhum existir, retorna 20 ordenados por Id e para  evitar trafego no BD.
        public async Task<IEnumerable<Endereco>> GetFeaturedAsync()
        {
            var featured = await _context.Enderecos
                .AsNoTracking()
                .Where(e => !string.IsNullOrEmpty(e.Referencial))
                .OrderBy(e => e.Id)
                .Take(20)
                .ToListAsync();

            if (featured.Count > 0) return featured;

            return await _context.Enderecos
                .AsNoTracking()
                .OrderByDescending(e => e.Id)
                .Take(20)
                .ToListAsync();
        }

        public async Task AddAsync(Endereco endereco)
        {
            await _context.Enderecos.AddAsync(endereco);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Endereco endereco)
        {
            _context.Enderecos.Update(endereco);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var endereco = await _context.Enderecos.FindAsync(id);
            if (endereco != null)
            {
                _context.Enderecos.Remove(endereco);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CountAsync()
        {
            return await _context.Enderecos.CountAsync();
        }
    }
}