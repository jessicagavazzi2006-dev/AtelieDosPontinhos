using AtelieDosPontinhos.Application.DTOs;
using AtelieDosPontinhos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<CategoryDto> CreateAsync(CreateCategoryDto Dto);
        Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto Dto);
        Task<bool> DeleteAsync(int id);
        Task<int> CountAsync();
        
    }
}
