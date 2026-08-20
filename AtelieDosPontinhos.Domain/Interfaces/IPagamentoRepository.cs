using AtelieDosPontinhos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Domain.Interfaces
{
    public interface IPagamentoRepository 
    {
        Task<IEnumerable<Pagamento>> GetAllAsync();

        Task<Pagamento?> GetByIdAsync(int id);

        Task<IEnumerable<Pagamento>> GetFeaturedAsync();

        Task AddAsync(Pagamento pagamento);
        Task UpdateAsync(Pagamento pagamento);
        Task DeleteAsync(int id);
        Task<int> CountAsync();
    }
}
