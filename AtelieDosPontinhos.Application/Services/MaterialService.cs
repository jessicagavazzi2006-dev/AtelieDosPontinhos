using AtelieDosPontinhos.Application.DTOs;
using AtelieDosPontinhos.Application.Interfaces;
using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Application.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;
        public MaterialService(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }
        public async Task<IEnumerable<MaterialDto>> GetAllAsync()
        {
            var material = await _materialRepository.GetAllAsync();
            return material.Select(MapToDto);
        }
        public async Task<MaterialDto?> GetByIdAsync(int id)
        {
            var material = await _materialRepository.GetByIdAsync(id);
            return material == null ? null : MapToDto(material);
        }
        public async Task<MaterialDto> CreateAsync(CreateMaterialDto dto)
        {
            var material = new Material { Name = dto.Name,
            Amount = dto.Amount,
            Unit = dto.Unit};
            await _materialRepository.AddAsync(material);
            return MapToDto(material);
        }

        public async Task<MaterialDto?> UpdateAsync(int id, UpdateMaterialDto dto)
        {
            var material = await _materialRepository.GetByIdAsync(id);
            if (material == null) return null;
            
            material.Amount = dto.Amount;
            material.Unit = dto.Unit;
            material.Name = dto.Name;
            await _materialRepository.UpdateAsync(material);
            return MapToDto(material);

        }
        
        public async Task<bool> DeleteAsync(int id) 
        {
            var material = await _materialRepository.GetByIdAsync(id);
            if (material == null) return false;

            await _materialRepository.DeleteAsync(id);
            return true;
        }

        public async Task<int> CountAsync()
        {
            return await _materialRepository.CountAsync();
        }

        private static MaterialDto MapToDto(Material material)
        {
            return new MaterialDto
            {
                Amount = material.Amount,
                Name = material.Name,
                Id = material.Id,
                Unit = material.Unit,
            };
        }
    }
}
