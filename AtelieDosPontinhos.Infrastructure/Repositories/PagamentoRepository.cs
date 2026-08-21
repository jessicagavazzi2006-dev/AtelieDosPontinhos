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
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly AtelieDosPontinhosDbContext _context;

        public PagamentoRepository(AtelieDosPontinhosDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pagamento>> GetAllAsync()
        {
            return await _context.Pagamentos
                .AsNoTracking()
                .OrderByDescending(p => p.DataPagamento)
                .ToListAsync();
        }

        public async Task<Pagamento?> GetByIdAsync(int id)
        {
            return await _context.Pagamentos
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Implementação razoável para "destaques" em pagamentos: retorna pagamentos dos últimos 30 dias.
        // Ajuste a regra conforme a necessidade do domínio.
        public async Task<IEnumerable<Pagamento>> GetFeaturedAsync()
        {
            var cutoff = DateTime.UtcNow.AddDays(-30);
            return await _context.Pagamentos
                .AsNoTracking()
                .Where(p => p.DataPagamento >= cutoff)
                .OrderByDescending(p => p.DataPagamento)
                .ToListAsync();
        }

        public async Task AddAsync(Pagamento pagamento)
        {
            await _context.Pagamentos.AddAsync(pagamento);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Pagamento pagamento)
        {
            _context.Pagamentos.Update(pagamento);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var pagamento = await _context.Pagamentos.FindAsync(id);
            if (pagamento != null)
            {
                _context.Pagamentos.Remove(pagamento);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CountAsync()
        {
            return await _context.Pagamentos.CountAsync();
        }
    }
}