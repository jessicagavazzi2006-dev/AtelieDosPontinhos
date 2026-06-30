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

        // 1. LISTAR TODOS OS PRODUTOS DO BANCO
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // 2. BUSCAR PRODUTOS POR TEXTO (Corrigido para Name e Description)
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string? query = null)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Ok(await _context.Products.ToListAsync());
            }

            // 🛠️ CORRIGIDO: Alterado de Nome/Descricao para Name/Description
            var filteredProducts = await _context.Products
                .Where(p => p.Name.Contains(query) || p.Description.Contains(query))
                .ToListAsync();

            return Ok(filteredProducts);
        }

        // 3. BUSCAR POR ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound(new { message = "Produto não encontrado." });
            return Ok(product);
        }

        // 4. CRIAR NOVO PRODUTO NO BANCO
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (product == null) return BadRequest(new { message = "Dados inválidos." });

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        // 5. ATUALIZAR PRODUTO NO BANCO
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            if (id != updatedProduct.Id) return BadRequest(new { message = "ID incorreto." });

            _context.Entry(updatedProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(p => p.Id == id)) return NotFound();
                throw;
            }

            return NoContent();
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