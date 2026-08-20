using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Application.DTOs;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Application.Interfaces
{
    public interface IEnderecoService
    {
        Task<IEnumerable<EnderecoDto>> GetAllAsync();
        Task<EnderecoDto?> GetByIdAsync(int id);
        Task<EnderecoDto> CreateAsync(CreateEnderecoDto dto);
        Task<EnderecoDto?> UpdateAsync(int id, UpdateEnderecoDto dto);
        Task<bool> DeleteAsync(int id);
        Task<int> CountAsync();
    }
}
