using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AtelieDosPontinhos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requer autenticação por padrão
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AtelieDosPontinhosDbContext _context;

        public UserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AtelieDosPontinhosDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // 1. LISTAR TODOS OS USUÁRIOS
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var result = new List<object>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                result.Add(new
                {
                    id = u.Id,
                    nome = u.Nome,
                    email = u.Email,
                    userName = u.UserName,
                    roles = roles
                });
            }

            return Ok(result);
        }

        // 2. BUSCAR USUÁRIO POR ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound(new { message = "Usuário não encontrado." });
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new { id = user.Id, nome = user.Nome, email = user.Email, userName = user.UserName, roles = roles });
        }

        // 3. CRIAR USUÁRIO (Admin)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] AtelieDosPontinhos.Application.DTOs.CreateUsuarioDto dto)
        {
            if (dto == null) return BadRequest(new { message = "Dados inválidos." });

            if (dto.Password != dto.ConfirmPassword)
                return BadRequest(new { message = "As senhas não coincidem." });

            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing != null) return BadRequest(new { message = "E-mail já cadastrado." });

            var user = new ApplicationUser { UserName = dto.UserName, Email = dto.Email, Nome = dto.UserName };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new { message = $"Erro ao criar usuário: {errors}" });
            }

            var roleToAdd = string.IsNullOrWhiteSpace(dto.Role) ? "Usuario" : dto.Role;
            if (await _roleManager.RoleExistsAsync(roleToAdd))
            {
                await _userManager.AddToRoleAsync(user, roleToAdd);
            }

            var roles = await _userManager.GetRolesAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, new { id = user.Id, nome = user.Nome, email = user.Email, userName = user.UserName, roles = roles });
        }

        // 4. EDITAR USUÁRIO (ATUALIZAR E-MAIL, SENHA, ROLE) - ADMIN
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] AtelieDosPontinhos.Application.DTOs.UpdateUsuarioDto dto)
        {
            if (dto == null) return BadRequest(new { message = "Dados inválidos." });

            if (!string.IsNullOrWhiteSpace(dto.Password) && dto.Password != dto.ConfirmPassword)
                return BadRequest(new { message = "As senhas não coincidem." });

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound(new { message = "Usuário não encontrado." });

            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing != null && existing.Id != user.Id) return BadRequest(new { message = "E-mail já cadastrado por outro usuário." });

            user.UserName = dto.UserName;
            user.Email = dto.Email;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                return BadRequest(new { message = $"Erro ao atualizar usuário: {errors}" });
            }

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passResult = await _userManager.ResetPasswordAsync(user, token, dto.Password);
                if (!passResult.Succeeded)
                {
                    var errors = string.Join(", ", passResult.Errors.Select(e => e.Description));
                    return BadRequest(new { message = $"Erro ao atualizar senha: {errors}" });
                }
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!string.IsNullOrWhiteSpace(dto.Role) && !currentRoles.Contains(dto.Role))
            {
                if (await _roleManager.RoleExistsAsync(dto.Role))
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    await _userManager.AddToRoleAsync(user, dto.Role);
                }
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new { id = user.Id, nome = user.Nome, email = user.Email, userName = user.UserName, roles = roles });
        }

        // 5. OBTER O PERFIL DO PRÓPRIO USUÁRIO LOGADO
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                var userEmailClaim = User.FindFirst(ClaimTypes.Email)?.Value
                                   ?? User.Identity?.Name;

                if (!string.IsNullOrEmpty(userEmailClaim))
                {
                    var userByEmail = await _userManager.FindByEmailAsync(userEmailClaim);
                    if (userByEmail != null) userId = userByEmail.Id;
                }
            }

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Usuário não autenticado." });

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound(new { message = "Usuário não encontrado." });

            var endereco = await _context.Enderecos.FirstOrDefaultAsync(e => e.UserId == userId);
            var pagamento = await _context.Pagamentos.FirstOrDefaultAsync(p => p.UserId == userId);

            return Ok(new
            {
                nome = string.IsNullOrWhiteSpace(user.Nome) ? user.UserName : user.Nome,
                email = user.Email,
                telefone = user.PhoneNumber,
                cep = endereco?.CEP,
                cidade = endereco?.Cidade,
                estado = endereco?.Estado,
                numero = endereco?.Numero.ToString(),
                referencia = endereco?.Referencia,
                metodo = pagamento?.Metodo.ToString(),
                // Retorna os dados persistidos em vez de strings vazias
                titular = pagamento?.NomeCartao ?? "",
                cartao = pagamento?.NumeroCartao ?? ""
            });
        }

        // 6. ATUALIZAR O PERFIL DO PRÓPRIO USUÁRIO LOGADO
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] AtelieDosPontinhos.Application.DTOs.UpdateProfileDto dto)
        {
            if (dto == null) return BadRequest(new { message = "Dados inválidos." });

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                var userEmailClaim = User.FindFirst(ClaimTypes.Email)?.Value
                                   ?? User.Identity?.Name;

                if (!string.IsNullOrEmpty(userEmailClaim))
                {
                    var userByEmail = await _userManager.FindByEmailAsync(userEmailClaim);
                    if (userByEmail != null) userId = userByEmail.Id;
                }
            }

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Usuário não autenticado." });

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound(new { message = "Usuário não encontrado." });

            // 1. Atualiza dados do Usuário
            if (!string.IsNullOrWhiteSpace(dto.Nome)) user.Nome = dto.Nome;
            if (!string.IsNullOrWhiteSpace(dto.Telefone)) user.PhoneNumber = dto.Telefone;
            if (!string.IsNullOrWhiteSpace(dto.Email)) user.Email = dto.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new { message = $"Erro ao atualizar perfil: {errors}" });
            }

            // 2. Atualiza Endereço
            var endereco = await _context.Enderecos.FirstOrDefaultAsync(e => e.UserId == userId);
            if (endereco == null)
            {
                endereco = new Endereco { UserId = userId };
                _context.Enderecos.Add(endereco);
            }

            endereco.CEP = dto.Cep ?? string.Empty;
            endereco.Cidade = dto.Cidade ?? string.Empty;
            endereco.Estado = dto.Estado ?? string.Empty;
            endereco.Referencia = dto.Referencial ?? string.Empty;

            if (int.TryParse(dto.Numero, out int numParsed))
            {
                endereco.Numero = numParsed;
            }

            // 3. Atualiza Pagamento (Grava o Método, Nome impresso e Número do cartão)
            var pagamento = await _context.Pagamentos.FirstOrDefaultAsync(p => p.UserId == userId);
            if (pagamento == null)
            {
                pagamento = new Pagamento { UserId = userId };
                _context.Pagamentos.Add(pagamento);
            }

            if (Enum.TryParse<AtelieDosPontinhos.Domain.Enums.PaymentMethod>(dto.Metodo, true, out var metodoParsed))
            {
                pagamento.Metodo = metodoParsed;
            }

            pagamento.NomeCartao = dto.Titular ?? string.Empty;
            pagamento.NumeroCartao = dto.Cartao ?? string.Empty;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Perfil atualizado com sucesso!" });
        }

        // 7. REMOVER USUÁRIO
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound(new { message = "Usuário não encontrado." });

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Erro ao remover usuário." });
            }

            return NoContent();
        }

        [HttpGet("perfis")]
        public async Task<ActionResult<IEnumerable<string>>> GetPerfis()
        {
            var perfis = await _roleManager.Roles
                .Where(r => r.Name != null)
                .Select(r => r.Name!)
                .ToListAsync();

            return Ok(perfis);
        }
    }
}