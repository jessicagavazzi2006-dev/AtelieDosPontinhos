using AtelieDosPontinhos.Infrastructure.Context;
using AtelieDosPontinhos.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AtelieDosPontinhos.API.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly AtelieDosPontinhosDbContext _context;

        public CartController(AtelieDosPontinhosDbContext context)
        {
            _context = context;
        }

        // 🛒 ADICIONAR AO CARRINHO
        [HttpPost("add")]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var item = new CartItem
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity
            };

            _context.CartItems.Add(item);
            await _context.SaveChangesAsync();

            return Ok("Produto adicionado ao carrinho");
        }

        // 🛒 VER CARRINHO
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = await _context.CartItems
                .Include(x => x.Product)
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return Ok(cart);
        }

        // 🗑 REMOVER ITEM
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var item = await _context.CartItems.FindAsync(id);

            if (item == null)
                return NotFound();

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();

            return Ok("Item removido");
        }
    }
}