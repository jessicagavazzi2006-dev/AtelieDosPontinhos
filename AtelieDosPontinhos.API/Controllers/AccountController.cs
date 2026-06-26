using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AtelieDosPontinhos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager; // 🌟 Injetado para gerenciar as permissões (Roles)

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // 📝 ASSINATURA CORRETA PARA APARECER NO SWAGGER
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request) // 🌟 Alterado para RegisterRequest para receber a Role da UI
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { Message = "E-mail e senha são obrigatórios." });
            }

            var user = new IdentityUser { UserName = request.Email, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                // Define a role padrão se nenhuma for enviada (ex: se vier nulo da UI, vira Cliente)
                var roleName = string.IsNullOrEmpty(request.Role) ? "Cliente" : request.Role;

                // Garante que a role (ex: "Cliente") existe no sistema
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                // Vincula o usuário recém-criado à Role correspondente
                await _userManager.AddToRoleAsync(user, roleName);

                return Ok(new { Succeeded = true, Message = "Usuário cadastrado com sucesso!" });
            }

            return BadRequest(result.Errors);
        }

        // 🔑 ROTA DE LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                var roles = await _userManager.GetRolesAsync(user);

                return Ok(new { Succeeded = true, Email = request.Email, Roles = roles });
            }

            return Unauthorized(new { Succeeded = false, Message = "Usuário ou senha inválidos" });
        }
    }

    // Modelos de requisição DTO específicos para a API
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // 🌟 Propriedade adicionada para casar com o envio da UI
    }
}