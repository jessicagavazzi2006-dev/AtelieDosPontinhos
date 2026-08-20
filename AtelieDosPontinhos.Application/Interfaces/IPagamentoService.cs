using AtelieDosPontinhos.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Application.Interfaces
{
    public interface IPagamentoService
    {
        Task<IEnumerable<PagamentoDto>> GetAllAsync();
        Task<PagamentoDto?> GetByIdAsync(int id);
        Task<PagamentoDto> CreateAsync(CreatePagamentoDto dto);
        Task<PagamentoDto> UpdateAsync(int id, UpdatePagamentoDto dto);
        Task<bool> DeleteAsync(int id);
        Task<int> CountAsync();
    }
}
