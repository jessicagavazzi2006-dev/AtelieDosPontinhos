using AtelieDosPontinhos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Domain.Interfaces
{
    public interface IEnderecoRepository
    {
        Task<IEnumerable<Endereco>> GetAllAsync();
        Task<Endereco?> GetByIdAsync(int id);
        Task<IEnumerable<Endereco>> GetFeaturedAsync();
        Task AddAsync(Endereco endereco);
        Task UpdateAsync(Endereco endereco);
        Task DeleteAsync(int id);
        Task<int >CountAsync();
            
    }
}
