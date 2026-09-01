using AtelieDosPontinhos.Domain;
using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Infrastructure.Context;
using AtelieDosPontinhos.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AtelieDosPontinhos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AtelieDosPontinhosDbContext _context;

        public ProductController(AtelieDosPontinhosDbContext context)
        {
            _context = context;
        }

        // 1. LISTAR TODOS OS PRODUTOS DO BANCO (projetando CategoryName)
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.CoverImageUrl,
                    p.Price,
                    p.Stock,
                    p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.Name : string.Empty,
                    p.IsFeatured
                })
                .ToListAsync();

            return Ok(products);
        }

        // 2. BUSCAR PRODUTOS POR TEXTO 
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string? term = null)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return Ok(new List<object>());
            }

            var filteredProducts = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Name.Contains(term) || p.Description.Contains(term))
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.CoverImageUrl,
                    p.Price,
                    p.Stock,
                    p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.Name : string.Empty,
                    p.IsFeatured
                })
                .ToListAsync();

            return Ok(filteredProducts);
        }


        // 3. BUSCAR POR ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.CoverImageUrl,
                    p.Price,
                    p.Stock,
                    p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.Name : string.Empty,
                    p.IsFeatured
                })
                .FirstOrDefaultAsync();

            if (product == null) return NotFound(new { message = "Produto não encontrado." });
            return Ok(product);
        }

        // 4. CRIAR NOVO PRODUTO NO BANCO
        // 4. CRIAR NOVO PRODUTO NO BANCO (COM TRATAMENTO DE CATEGORIA)
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (product == null) return BadRequest(new { message = "Dados inválidos." });

            try
            {
                // Limpa o objeto Category completo se ele vier preenchido para evitar que o Entity Framework tente duplicar a categoria
                product.Category = null;

                // 🌟 CHECAGEM DEFENSIVA: Se a categoria enviada for zero ou não existir no banco, colocamos uma válida automaticamente
                var categoriaExiste = await _context.Categories.AnyAsync(c => c.Id == product.CategoryId);

                if (product.CategoryId == 0 || !categoriaExiste)
                {
                    // Busca a primeira categoria cadastrada no seu banco (ex: "Banho") para usar como padrão
                    var primeiraCategoria = await _context.Categories.FirstOrDefaultAsync();

                    if (primeiraCategoria != null)
                    {
                        product.CategoryId = primeiraCategoria.Id;
                    }
                    else
                    {
                        return BadRequest(new { message = "Erro: Nenhuma categoria cadastrada no banco de dados para associar ao produto." });
                    }
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                System.Diagnostics.Debug.WriteLine($"ERRO DE INSERÇÃO NO BANCO: {innerMessage}");
                return BadRequest(new { message = "Erro nas regras do banco de dados.", detalhes = innerMessage });
            }
        }


        // 5. ATUALIZAR PRODUTO NO BANCO
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] AtelieDosPontinhos.Application.DTOs.UpdateProductDto dto)
        {
            if (dto == null) return BadRequest(new { message = "Dados inválidos." });

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound(new { message = "Produto não encontrado." });

            // Atualiza campos permitidos
            product.Name = dto.Name ?? product.Name;
            product.Description = dto.Description ?? product.Description;
            product.CoverImageUrl = dto.CoverImageUrl ?? product.CoverImageUrl;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.IsFeatured = dto.IsFeatured;

            // Valida e atualiza categoria
            if (dto.CategoryId != product.CategoryId)
            {
                var existe = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);
                if (!existe) return BadRequest(new { message = "Categoria inválida." });
                product.CategoryId = dto.CategoryId;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Products.AnyAsync(p => p.Id == id)) return NotFound();
                throw;
            }

            // Retorna o produto atualizado na forma esperada pelo cliente
            var updated = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.CoverImageUrl,
                    p.Price,
                    p.Stock,
                    p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.Name : string.Empty,
                    p.IsFeatured
                })
                .FirstOrDefaultAsync();

            return Ok(updated);
        }

        // 6. EXCLUIR PRODUTO DO BANCO
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound(new { message = "Produto não encontrado." });

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}