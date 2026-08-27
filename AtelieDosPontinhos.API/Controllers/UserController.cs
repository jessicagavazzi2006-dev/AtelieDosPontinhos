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
        // Adicione o tipo genérico aqui:
        private readonly UserManager<IdentityUser> _userManager;

        // Adicione também no construtor do controlador:
        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // 1. LISTAR TODOS OS USUÁRIOS
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            // Retorna apenas os dados necessários em formato simplificado
            var userList = users.Select(u => new
            {
                id = u.Id,
                email = u.Email,
                userName = u.UserName
            }).ToList();

            return Ok(userList);
        }

        // 2. BUSCAR USUÁRIO POR ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound(new { message = "Usuário não encontrado." });

            return Ok(new { id = user.Id, email = user.Email, userName = user.UserName });
        }

        // 3. EDITAR USUÁRIO (ATUALIZAR E-MAIL)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] IdentityUser updatedUser)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound(new { message = "Usuário não encontrado." });

            user.Email = updatedUser.Email;
            user.UserName = updatedUser.Email; // Mantém o UserName igual ao Email

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Erro ao atualizar usuário.", errors = result.Errors });
            }

            return NoContent();
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
            var perfis = await _userManager.GetPerfisAsync();
            return Ok(perfis);
        }
    }
}