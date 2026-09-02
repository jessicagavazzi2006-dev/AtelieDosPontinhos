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

            // 1. Tenta extrair o UserId com fallback seguro para Claims
            var userId = _userManager.GetUserId(User)
                         ?? User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirst("sub")?.Value;

            System.Diagnostics.Debug.WriteLine($"🔐 CreateOrder: UserId extraído = '{userId}'");

            if (string.IsNullOrEmpty(userId))
            {
                System.Diagnostics.Debug.WriteLine("❌ CreateOrder: UserId vazio - retornando Unauthorized");
                return Unauthorized("Usuário não autenticado no contexto da API.");
            }

            if (dto == null || dto.Items == null || !dto.Items.Any())
            {
                System.Diagnostics.Debug.WriteLine("❌ CreateOrder: DTO ou lista de itens veio nula/vazia");
                return BadRequest("O pedido deve conter pelo menos um item.");
            }

            System.Diagnostics.Debug.WriteLine($"📦 CreateOrder: Recebido DTO com {dto.Items.Count} itens");

            // 2. Instancia a entidade Pedido garantindo a inicialização da lista de Itens
            var pedido = new Pedido
            {
                UserId = userId,
                DataPedido = DateTime.UtcNow,
                ValorTotal = dto.Items.Sum(i => i.PrecoUnitario * i.Quantidade),
                Status = "Pendente",
                MetodoPagamento = dto.MetodoPagamento ?? string.Empty,
                CEP = dto.CEP ?? string.Empty,
                Cidade = dto.Cidade ?? string.Empty,
                Estado = dto.Estado ?? string.Empty,
                Numero = dto.Numero ?? string.Empty,
                Complemento = dto.Complemento ?? string.Empty,
                Itens = new List<PedidoItem>() // Evita NullReferenceException
            };

            foreach (var it in dto.Items)
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

            // 3. Grava no banco de dados
            await _context.SaveChangesAsync();
            System.Diagnostics.Debug.WriteLine($"✅ CreateOrder: Pedido gravado no banco com sucesso! ID={pedido.Id}");

            // Retorna resposta limpa sem disparar erro de ciclo de JSON
            return Ok(new { success = true, pedidoId = pedido.Id, message = "Pedido criado com sucesso!" });
        }

        // Lista pedidos do usuário atual
        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> MyOrders()
        {
            var userId = _userManager.GetUserId(User)
                         ?? User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirst("sub")?.Value;

            System.Diagnostics.Debug.WriteLine($"🔍 MyOrders: Buscando pedidos para UserId='{userId}'");

            var pedidos = await _context.Pedidos
                .Where(p => p.UserId == userId)
                .Include(p => p.Itens)
                .AsNoTracking()
                .ToListAsync();

            System.Diagnostics.Debug.WriteLine($"📊 MyOrders: Encontrados {pedidos.Count} pedidos");

            return Ok(pedidos);
        }

        // Admin: lista todos os pedidos com dados do comprador
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllOrders()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Itens)
                .AsNoTracking()
                .ToListAsync();

            // Mapeia o Email do Usuário a partir da tabela do Identity
            var userIds = pedidos.Select(p => p.UserId).Distinct().ToList();
            var usuarios = await _userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.Email);

            var resultado = pedidos.Select(p => new
            {
                p.Id,
                p.UserId,
                EmailUsuario = usuarios.ContainsKey(p.UserId) ? usuarios[p.UserId] : "Usuário Não Encontrado",
                p.DataPedido,
                p.ValorTotal,
                p.Status,
                p.MetodoPagamento,
                p.CEP,
                p.Cidade,
                p.Estado,
                p.Numero,
                p.Complemento,
                Itens = p.Itens.Select(i => new
                {
                    i.Id,
                    i.ProductId,
                    i.Quantidade,
                    i.PrecoUnitario
                })
            });

            return Ok(resultado);
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