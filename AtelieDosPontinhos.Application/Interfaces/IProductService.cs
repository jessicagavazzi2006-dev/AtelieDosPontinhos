using AtelieDosPontinhos.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetByCategoryAsync (int categoryId);
        Task<ProductDto> CreateAsync (CreateProductDto Dto);
        Task<ProductDto?> UpdateAsync (int id, UpdateProductDto Dto);
        Task<bool> DeleteAsync (int id);
        Task<int>CountAsync();
    }
}
