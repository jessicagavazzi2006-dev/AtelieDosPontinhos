using AtelieDosPontinhos.Application.DTOs;
using AtelieDosPontinhos.Application.Interfaces;
using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AtelieDosPontinhos.Application.Services
{
    public class ProductServices : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductServices (IProductRepository productRepositoory)
        {
            _productRepository = productRepositoory;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var Produtos = await _productRepository.GetAllAsync();
            return Produtos.Select(MapToDto);
        }
        public async Task<ProductDto?> GetByIdAsync(int id) 
        {
            var Produtos = await _productRepository.GetByIdAsync(id);
            return Produtos == null ? null : MapToDto(Produtos);
        }
        public async Task<IEnumerable<ProductDto>> GetByCategoryAsync(int categoryId) 
        {
            var Produtos = await _productRepository.GetByCategoryAsync(categoryId);
            return Produtos.Select(MapToDto);
        }
        public async Task<ProductDto> CreateAsync(CreateProductDto Dto)
        {
            var produto = new Product
            {
                Name = Dto.name,
                Description = Dto.Description,
                CoverImageUrl = Dto.CoverImageUrl,
                Price = Dto.Price,
                Stock = Dto.Stock,
                CategoryId = Dto.CategoryId,
            };
            await _productRepository.AddAsync(produto);

            return MapToDto(produto);
        }
        public async Task<ProductDto> UpdateAsync(int id, UpdateProductDto Dto)
        {
            var produto = await _productRepository.GetByIdAsync(id);
            if (produto == null) return null;

            {
                produto.Name = Dto.name;
                produto.Description = Dto.Description;
                produto.CoverImageUrl = Dto.CoverImageUrl;
                produto.Price = Dto.Price;
                produto.Stock = Dto.Stock;
                produto.CategoryId = Dto.CategoryId;
            };
            await _productRepository.UpdateAsync(produto);

            return MapToDto(produto);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var produto = await _productRepository.GetByIdAsync(id);
            if(produto == null)
            {
                return false;
            }
            await _productRepository.DeleteAsync(id);
            return true;
        }
        public async Task<int> CountAsync()
        {
            return await _productRepository.CountAsync();
        }
        private static ProductDto MapToDto(Product produto)
        {
            return new ProductDto
            {
                Name = produto.Name,
                Description = produto.Description,
                CoverImageUrl = produto.CoverImageUrl,
                Price = produto.Price,
                Stock = produto.Stock,
                CategoryId = produto.CategoryId,
            };
        }
    }
}

