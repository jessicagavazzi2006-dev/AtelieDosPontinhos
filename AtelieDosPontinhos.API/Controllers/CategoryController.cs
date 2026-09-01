using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AtelieDosPontinhos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly AtelieDosPontinhosDbContext _context;

        public CategoryController(AtelieDosPontinhosDbContext context)
        {
            _context = context;
        }

        // Listar todas as categorias
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _context.Categories
                .Include(c => c.Products)
                .AsNoTracking()
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    ProductCount = c.Products != null ? c.Products.Count : 0,
                    c.ImageLocal
                })
                .ToListAsync();

            return Ok(categories);
        }

        // Buscar por id (inclui produtos associados)
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    ProductCount = c.Products != null ? c.Products.Count : 0,
                    c.ImageLocal
                })
                .FirstOrDefaultAsync();

            if (category == null) return NotFound(new { message = "Categoria não encontrada." });

            return Ok(category);
        }

        // Busca por termo no nome
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? term = null)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Ok(new List<object>());
            var results = await _context.Categories
                .Include(c => c.Products)
                .Where(c => c.Name.Contains(term))
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    ProductCount = c.Products != null ? c.Products.Count : 0,
                    c.ImageLocal
                })
                .AsNoTracking()
                .ToListAsync();

            return Ok(results);
        }

        // Criar categoria (Admin)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            if (category == null || string.IsNullOrWhiteSpace(category.Name))
                return BadRequest(new { message = "Dados inválidos." });

            // Evita duplicidade pelo nome (case-insensitive)
            var exists = await _context.Categories
                .AnyAsync(c => c.Name.ToLower() == category.Name.Trim().ToLower());

            if (exists)
                return BadRequest(new { message = "Já existe uma categoria com esse nome." });

            category.Name = category.Name.Trim();
            category.ImageLocal ??= string.Empty;

            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { message = "Erro ao salvar a categoria.", detalhes = inner });
            }
        }

        // Atualizar categoria (Admin)
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Category updated)
        {
            if (updated == null || id != updated.Id)
                return BadRequest(new { message = "Dados inválidos." });

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound(new { message = "Categoria não encontrada." });

            // Checagem de nome duplicado em outra categoria
            var nameConflict = await _context.Categories
                .AnyAsync(c => c.Id != id && c.Name.ToLower() == updated.Name.Trim().ToLower());

            if (nameConflict)
                return BadRequest(new { message = "Outra categoria já usa esse nome." });

            category.Name = updated.Name?.Trim() ?? category.Name;
            category.ImageLocal = updated.ImageLocal ?? category.ImageLocal;

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Categories.AnyAsync(c => c.Id == id))
                    return NotFound(new { message = "Categoria não encontrada." });
                throw;
            }

            return NoContent();
        }

        // Excluir categoria (Admin)
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return NotFound(new { message = "Categoria não encontrada." });

            // Opcional: impedir exclusão se houver produtos associados
            if (category.Products != null && category.Products.Any())
            {
                return BadRequest(new { message = "Não é possível excluir uma categoria que possui produtos associados." });
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

