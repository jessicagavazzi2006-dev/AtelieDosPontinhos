using AtelieDosPontinhos.Application.DTOs;
using AtelieDosPontinhos.Application.Interfaces;
using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Application.Services
{
    public class MaterialService :IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;
        public MaterialService(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }
        public async Task<IEnumerable<MaterialDto>> GetAllAsync()
        {
            var materials = await _materialRepository.GetAllAsync();
            return materials.Select(MapToDto);
        }
        public async Task<MaterialDto?> GetByIdAsync(int id)
        {
            var material = await _materialRepository.GetByIdAsync(id);
            return material == null ? null : MapToDto(material);
        }
        public async
    }
}
