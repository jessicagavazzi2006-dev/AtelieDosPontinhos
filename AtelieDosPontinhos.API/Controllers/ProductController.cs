using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Application.Interfaces;
using AtelieDosPontinhos.Application.DTOs; // Namespace para o CreateProductDto

namespace AtelieDosPontinhos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // 🔍 GET: /api/Product/search?term=...
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> Search([FromQuery] string term)
        {
            try
            {
                // Busca todos os produtos usando a sua camada de serviço existente
                var products = await _productService.GetAllAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal error searching products: {ex.Message}");
            }
        }

        // 🟢 POST: /api/Product
        // ROTA DEFINITIVA: Recebe o DTO e salva o produto usando a arquitetura do seu projeto
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateProductDto newProductDto)
        {
            if (newProductDto == null)
            {
                return BadRequest("Product payload cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // 🔥 Passa o DTO que o seu método CreateAsync exige (conforme image_291df8.png)
                await _productService.CreateAsync(newProductDto);

                // Retorna o status 201 Created apontando para a rota de busca
                return CreatedAtAction(nameof(Search), new { term = newProductDto.Name }, newProductDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving product to database: {ex.Message}");
            }
        }
    }
}