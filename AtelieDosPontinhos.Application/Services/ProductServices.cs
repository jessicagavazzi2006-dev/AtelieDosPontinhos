using AtelieDosPontinhos.Application.DTOs;
using AtelieDosPontinhos.Application.Interfaces;
using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtelieDosPontinhos.Application.Services
{
    public class ProductServices : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductServices(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(MapToDto);
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new KeyNotFoundException("Product not found");

            return MapToDto(product);
        }

        public async Task<IEnumerable<ProductDto>> GetByCategoryAsync(int categoryId)
        {
            var products = await _productRepository.GetByCategoryAsync(categoryId);
            return products.Select(MapToDto);
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                CoverImageUrl = dto.CoverImageUrl,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId
            };

            await _productRepository.AddAsync(product);

            return MapToDto(product);
        }

        public async Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new KeyNotFoundException("Product not found");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.CoverImageUrl = dto.CoverImageUrl;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.CategoryId = dto.CategoryId;

            await _productRepository.UpdateAsync(product);

            return MapToDto(product);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return false;

            await _productRepository.DeleteAsync(id);
            return true;
        }

        public async Task<int> CountAsync()
        {
            return await _productRepository.CountAsync();
        }

        private static ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Name = product.Name,
                Description = product.Description,
                CoverImageUrl = product.CoverImageUrl,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId
            };
        }

        public async Task<IEnumerable<ProductDto>> SearchAsync(string term)
        {
            var products = await _productRepository.SearchAsync(term);
            return products.Select(MapToDto);
        }

        public async Task<IEnumerable<ProductDto>> SearchAsync(string term)
        {
            var products = await _productRepository.SearchAsync(term);

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CoverImageUrl = p.CoverImageUrl
            });
        }
    }
}