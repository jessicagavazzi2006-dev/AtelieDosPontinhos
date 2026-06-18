using AtelieDosPontinhos.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Application.Interfaces
{
    public interface IMaterialService
    {
        Task<IEnumerable<MaterialDto>> GetAllSync();
        Task<MaterialDto?> GetByIdAsync(int id);
        Task<MaterialDto> CreateAsync(CreateMaterialDto dto);
        Task<MaterialDto?> UpdateAsync(int id, UpdateMaterialDto dto);
        Task<bool> DeleteAsync(int id);
        Task<int> CountAsync();
    }
}
