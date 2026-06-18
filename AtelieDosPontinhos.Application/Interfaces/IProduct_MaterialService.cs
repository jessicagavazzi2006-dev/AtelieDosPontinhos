using AtelieDosPontinhos.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Application.Interfaces
{
    public interface IProduct_MaterialService
    {
        Task<IEnumerable<Product_MaterialDto>> GetAllSync();
        Task<Product_MaterialDto?> GetByIdAsync(int id);
        Task<Product_MaterialDto> CreateAsync(CreateProduct_MaterialDto dto);
        Task<Product_MaterialDto?> UpdateAsync(int id, UpdateProduct_MaterialDto dto);
        Task<bool> DeleteAsync(int id);
        Task<int> CountAsync();
    }
}
