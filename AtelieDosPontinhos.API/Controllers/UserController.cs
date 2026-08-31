using AtelieDosPontinhos.Application.DTOs;
using AtelieDosPontinhos.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
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
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAll()
        {
            var users = await _usuariosService.GetAllAsync();
            return Ok(users);
        }

        // 2. BUSCAR USUÁRIO POR ID
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetById(string id)
        {
            var user = await _usuariosService.GetByIdAsync(id);
            if (user == null) return NotFound(new { message = "Usuário não encontrado." });

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioDto>> Create([FromBody] CreateUsuarioDto dto)
        {
            var (success, usuario, error) = await _usuariosService.CreateAsync(dto);
            if (!success) return BadRequest(new { message = error });

            return CreatedAtAction(nameof(GetById), new { id = usuario!.Id }, usuario);
        }

        // 3. EDITAR USUÁRIO (ATUALIZAR E-MAIL)
        [HttpPut("{id}")]
        public async Task<ActionResult<UsuarioDto>> Update(string id, [FromBody] UpdateUsuarioDto dto)
        {
            var (success, usuario, error) = await _usuariosService.UpdateAsync(id, dto);
            if (!success) return BadRequest(new { message = error });

            return Ok(usuario);
        }

        // 4. REMOVER USUÁRIO
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var (success, error) = await _usuariosService.DeleteAsync(id);
            if (!success) return BadRequest(new { message = error });

            return NoContent();
        }

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