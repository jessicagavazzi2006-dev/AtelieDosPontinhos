/*using AtelieDosPontinhos.Infrastructure.Context;
using AtelieDosPontinhos.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AtelieDosPontinhos.API.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize] // 🔐 tudo exige usuário logado
    public class CartController : ControllerBase
    {
        private readonly AtelieDosPontinhosDbContext _context;

        public CartController(AtelieDosPontinhosDbContext context)
        {
            _context = context;
        }

        // 🧠 pega usuário atual de forma segura
        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // 🛒 ADICIONAR AO CARRINHO
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromQuery] int productId, [FromQuery] int quantity = 1)
        {
            var userId = GetUserId();

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não autenticado");

            var item = await _context.CartItems
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);

            // 🔥 se já existe, só soma quantidade
            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                item = new CartItem
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity
                };

                _context.CartItems.Add(item);
            }

            await _context.SaveChangesAsync();

            return Ok("Produto adicionado ao carrinho");
        }

        // 🛒 VER CARRINHO
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = GetUserId();

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não autenticado");

            var cart = await _context.CartItems
                .Include(x => x.Product)
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return Ok(cart);
        }

        // 🧮 CONTAGEM DO CARRINHO (ícone 🛒)
        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var userId = GetUserId();

            if (string.IsNullOrEmpty(userId))
                return Ok(0);

            var count = await _context.CartItems
                .Where(x => x.UserId == userId)
                .SumAsync(x => x.Quantity);

            return Ok(count);
        }

        // 🗑 REMOVER ITEM
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var userId = GetUserId();

            var item = await _context.CartItems
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (item == null)
                return NotFound();

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();

            return Ok("Item removido");
        }
    }
}*/