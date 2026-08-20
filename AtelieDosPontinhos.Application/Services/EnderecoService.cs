using AtelieDosPontinhos.Application.DTOs;
using AtelieDosPontinhos.Application.Interfaces;
using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Application.Services
{
    internal class EnderecoService : IEnderecoService
    {
        private readonly IEnderecoRepository _enderecoRepository;

        public EnderecoService(IEnderecoRepository enderecoRepository)
        {
            _enderecoRepository = enderecoRepository;
        }

        public async Task<IEnumerable<EnderecoDto>> GetAllAsync()
        {
            var enderecos = await _enderecoRepository.GetAllAsync();
            return enderecos.Select(MapToDto);

        }

        public async Task<EnderecoDto?> GetByIdAsync(int id)
        {
            var enderecu = await _enderecoRepository.GetByIdAsync(id);
            return enderecu == null ? null : MapToDto(enderecu);
        }

        public async Task<EnderecoDto> CreateAsync(CreateEnderecoDto dto)
        {
            var enderecu = new Endereco { CEP = dto.CEP};
            await _enderecoRepository.AddAsync(enderecu);
            return MapToDto(enderecu);
        }

        public async Task<EnderecoDto?> UpdateAsync (int id, UpdateEnderecoDto dto)
        {
            var enderecu = await _enderecoRepository.GetByIdAsync(id);
            if (enderecu == null)  return null;

            enderecu.CEP = dto.CEP;
            await _enderecoRepository.UpdateAsync(enderecu);
            return MapToDto(enderecu);
        }

        public async Task<bool> DeleteAsync (int id)
        {
            var enderecu = await _enderecoRepository.GetByIdAsync(id);
            if (enderecu == null) return false;

            await _enderecoRepository.DeleteAsync(id);
            return true;
        }
        public async Task<int> CountAsync()
        {
            return await _enderecoRepository.CountAsync();
        }

    private static EnderecoDto MapToDto(Endereco enderecu)
        {
            return new EnderecoDto
            {
                Id = enderecu.Id,
                CEP = enderecu.CEP,
                Cidade = enderecu.Cidade,
                Estado = enderecu.Estado,
                NUMERO = enderecu.NUMERO,
                Referencial = enderecu.Referencial
            };
        }




    }
}
