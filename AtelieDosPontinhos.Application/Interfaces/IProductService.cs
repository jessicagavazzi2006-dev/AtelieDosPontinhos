using AtelieDosPontinhos.Application.DTOs;

namespace AtelieDosPontinhos.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetByCategoryAsync(int categoryId);

        Task<ProductDto> CreateAsync(CreateProductDto dto);
        Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto);

        Task<bool> DeleteAsync(int id);
        Task<int> CountAsync();

        Task<IEnumerable<ProductDto>> SearchAsync(string term);
    }
}