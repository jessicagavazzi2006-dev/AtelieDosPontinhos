using AtelieDosPontinhos.Application.DTOs;
using AtelieDosPontinhos.Application.Interfaces;
using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AtelieDosPontinhos.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var Categorias = await _categoryRepository.GetAllAsync();
            return Categorias.Select(MapToDto);
        }
        public async Task<CategoryDto?> GetByIdAsync(int id) 
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category == null ? null : MapToDto(category);
        } 
        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                ImageLocal = dto.ImageLocal
            };
            await _categoryRepository.AddAsync(category);
            return MapToDto(category); 
            
        }
        public async Task<CategoryDto?> UpdateAsync (int id, UpdateCategoryDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            category.Name = dto.Name;
            category.ImageLocal = dto.ImageLocal;
            await _categoryRepository.UpdateAsync(category);
            return MapToDto(category);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return false;

            await _categoryRepository.DeleteAsync(id);
            return true;
        }
        public async Task<int> CountAsync()
        {
            return await _categoryRepository.CountAsync();
        }
        private static CategoryDto MapToDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                ImageLocal = category.ImageLocal,
                ProductCount = category.Products?.Count ?? 0
            };
        }
    }
}
