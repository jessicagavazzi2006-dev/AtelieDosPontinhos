using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AtelieDosPontinhos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requer autenticação por padrão
    public class UserController : ControllerBase
    {
        // Dependências: UserManager para usuários e RoleManager para perfis/roles
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // Injetar UserManager e RoleManager
        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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
            return Ok(new { id = user.Id, email = user.Email, userName = user.UserName, roles = roles });
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

            var user = new IdentityUser { UserName = dto.UserName, Email = dto.Email };
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
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, new { id = user.Id, email = user.Email, userName = user.UserName, roles = roles });
        }

        // 4. EDITAR USUÁRIO (ATUALIZAR E-MAIL, SENHA, ROLE)
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

            // Atualiza roles
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
            return Ok(new { id = user.Id, email = user.Email, userName = user.UserName, roles = roles });
        }

        // 4. REMOVER USUÁRIO
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

        /// <summary>
        /// Retorna a lista de perfis disponíveis.
        /// GET /api/usuarios/perfis
        /// </summary>
        [HttpGet("perfis")]
        public async Task<ActionResult<IEnumerable<string>>> GetPerfis()
        {
            // Recupera os nomes das roles cadastradas via RoleManager
            var perfis = await _roleManager.Roles
                .Where(r => r.Name != null)
                .Select(r => r.Name!)
                .ToListAsync();

            return Ok(perfis);
        }
    }
}