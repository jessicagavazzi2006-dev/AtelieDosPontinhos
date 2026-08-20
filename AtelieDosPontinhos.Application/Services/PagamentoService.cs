using AtelieDosPontinhos.Application.DTOs;
using AtelieDosPontinhos.Application.Interfaces;
using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtelieDosPontinhos.Application.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoRepository _pagamentoRepository;

        public PagamentoService(IPagamentoRepository pagamentoRepository)
        {
            _pagamentoRepository = pagamentoRepository;
        }

        public async Task<IEnumerable<PagamentoDto>> GetAllAsync()
        {
            var pagamento = await _pagamentoRepository.GetAllAsync();
            return pagamento.Select(MapToDto);
        }

        public async Task<PagamentoDto?> GetByIdAsync(int id)
        {
            var pagament = await _pagamentoRepository.GetByIdAsync(id);
            return pagament == null ? null : MapToDto(pagament);
        }

        public async Task<PagamentoDto> CreateAsync(CreatePagamentoDto dto)
        {
            var pagament = new Pagamento();
            await _pagamentoRepository.AddAsync(pagament);
            return MapToDto(pagament);

        }

        public async Task<PagamentoDto> UpdateAsync(int id, UpdatePagamentoDto dto)
        {
            var pagament = await _pagamentoRepository.GetByIdAsync(id);
            if (pagament == null) return null;

            await _pagamentoRepository.UpdateAsync(pagament);
            return MapToDto(pagament);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var pagament = await _pagamentoRepository.GetByIdAsync(id);
            if (pagament == null) return false;

            await _pagamentoRepository.DeleteAsync(id); 
            return true;
        }

        public async Task<int> CountAsync()
        {
            return await _pagamentoRepository.CountAsync();
        }

        public static PagamentoDto MapToDto(Pagamento pagamento)
        {
            return new PagamentoDto
            {
                Id = pagamento.Id,
                DataPagamento = pagamento.DataPagamento,
                ValorPagamento = pagamento.ValorPagamento,
                FormadePagamento = pagamento.FormadePagamento
            };
        }
    }
}
