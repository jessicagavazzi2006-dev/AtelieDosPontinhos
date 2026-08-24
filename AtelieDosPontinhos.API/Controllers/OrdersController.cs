using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtelieDosPontinhos.Infrastructure.Context;
using AtelieDosPontinhos.Domain.Entities;

namespace AtelieDosPontinhos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AtelieDosPontinhosDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(AtelieDosPontinhosDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Cliente cria pedido (checkout)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var pedido = new Pedido
            {
                UserId = userId,
                DataPedido = DateTime.UtcNow,
                ValorTotal = dto.Itens.Sum(i => i.PrecoUnitario * i.Quantidade),
                Status = "Pendente",
                MetodoPagamento = dto.MetodoPagamento ?? string.Empty,
                CEP = dto.CEP ?? string.Empty,
                Cidade = dto.Cidade ?? string.Empty,
                Estado = dto.Estado ?? string.Empty,
                Numero = dto.Numero ?? string.Empty,
                Complemento = dto.Complemento ?? string.Empty
            };

            foreach (var it in dto.Itens)
            {
                var item = new PedidoItem
                {
                    ProductId = it.ProductId,
                    Quantidade = it.Quantidade,
                    PrecoUnitario = it.PrecoUnitario
                };
                pedido.Itens.Add(item);
            }

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = pedido.Id }, pedido);
        }

        // Lista pedidos do usuário atual
        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> MyOrders()
        {
            var userId = _userManager.GetUserId(User);
            var pedidos = await _context.Pedidos
                .Where(p => p.UserId == userId)
                .Include(p => p.Itens)
                .ToListAsync();
            return Ok(pedidos);
        }

        // Admin: lista todos os pedidos
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllOrders()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Itens)
                .ToListAsync();
            return Ok(pedidos);
        }

        // Obter por id (somente dono ou admin)
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Itens)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && pedido.UserId != userId) return Forbid();

            return Ok(pedido);
        }

        // Admin: atualizar status do pedido (ex: Pago, Enviado, Concluído)
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusDto dto)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return NotFound();

            pedido.Status = dto.Status;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    // DTOs simples (pode mover para outro arquivo)
    public class CreateOrderDto
    {
        public string MetodoPagamento { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public List<CreateOrderItemDto> Itens { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }

    public class UpdateStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }
}