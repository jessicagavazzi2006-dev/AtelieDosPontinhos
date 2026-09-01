using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtelieDosPontinhos.Infrastructure.Context;
using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Application.DTOs;

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
            System.Diagnostics.Debug.WriteLine("📍 CreateOrder: Iniciando criação de pedido...");

            var userId = _userManager.GetUserId(User);
            System.Diagnostics.Debug.WriteLine($"🔐 CreateOrder: UserId extraído = '{userId}'");

            if (string.IsNullOrEmpty(userId))
            {
                System.Diagnostics.Debug.WriteLine("❌ CreateOrder: UserId vazio - retornando Unauthorized");
                return Unauthorized();
            }

            System.Diagnostics.Debug.WriteLine($"📦 CreateOrder: Recebido DTO com {dto.Items?.Count ?? 0} itens, ValorTotal={dto.ValorTotal}");

            var pedido = new Pedido
            {
                UserId = userId,
                DataPedido = DateTime.UtcNow,
                ValorTotal = dto.Items!= null ? dto.Items.Sum(i => i.PrecoUnitario * i.Quantidade) : 0,
                Status = "Pendente",
                MetodoPagamento = dto.MetodoPagamento ?? string.Empty,
                CEP = dto.CEP ?? string.Empty,
                Cidade = dto.Cidade ?? string.Empty,
                Estado = dto.Estado ?? string.Empty,
                Numero = dto.Numero ?? string.Empty,
                Complemento = dto.Complemento ?? string.Empty
            };

            System.Diagnostics.Debug.WriteLine($"✏️ CreateOrder: Entidade Pedido criada - Id={pedido.Id}, ValorTotal={pedido.ValorTotal}");

            foreach (var it in dto.Items!)
            {
                System.Diagnostics.Debug.WriteLine($"  ➕ Item: ProdutoId={it.ProdutoId}, Quantidade={it.Quantidade}, Preço={it.PrecoUnitario}");
                var item = new PedidoItem
                {
                    ProductId = it.ProdutoId,
                    Quantidade = it.Quantidade,
                    PrecoUnitario = it.PrecoUnitario
                };
                pedido.Itens.Add(item);
            }

            _context.Pedidos.Add(pedido);
            System.Diagnostics.Debug.WriteLine($"📌 CreateOrder: Pedido adicionado ao DbContext");

            await _context.SaveChangesAsync();
            System.Diagnostics.Debug.WriteLine($"✅ CreateOrder: Pedido gravado no banco com sucesso! ID={pedido.Id}");

            return CreatedAtAction(nameof(GetById), new { id = pedido.Id }, pedido);
        }

        // Lista pedidos do usuário atual
        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> MyOrders()
        {
            var userId = _userManager.GetUserId(User);
            System.Diagnostics.Debug.WriteLine($"🔍 MyOrders: Buscando pedidos para UserId='{userId}'");

            var pedidos = await _context.Pedidos
                .Where(p => p.UserId == userId)
                .Include(p => p.Itens)
                .ToListAsync();

            System.Diagnostics.Debug.WriteLine($"📊 MyOrders: Encontrados {pedidos.Count} pedidos");
            foreach (var p in pedidos)
            {
                System.Diagnostics.Debug.WriteLine($"  - Pedido ID={p.Id}, Status={p.Status}, Total={p.ValorTotal}, Itens={p.Itens.Count}");
            }

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
}
