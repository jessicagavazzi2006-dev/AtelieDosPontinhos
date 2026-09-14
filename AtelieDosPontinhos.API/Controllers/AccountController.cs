using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AtelieDosPontinhos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AtelieDosPontinhosDbContext _context;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AtelieDosPontinhosDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { Message = "E-mail e senha são obrigatórios." });
            }

            // O UserName permanece sendo o Email (sem afetar as regras padrão do Identity)
            // O Nome Completo é gravado na propriedade personalizada
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                Nome = request.Nome ?? string.Empty
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                var roleName = string.IsNullOrEmpty(request.Role) ? "Cliente" : request.Role;

                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                await _userManager.AddToRoleAsync(user, roleName);

                try
                {
                    int.TryParse(request.Numero, out int numeroConvertido);

                    var novoEndereco = new Endereco
                    {
                        UserId = user.Id,
                        CEP = request.CEP ?? "",
                        Numero = numeroConvertido,
                        Estado = request.Estado ?? "",
                        Cidade = request.Cidade ?? "",
                        Referencia = request.Complemento ?? ""
                    };

                    _context.Add(novoEndereco);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Erro ao gravar endereço: {ex.Message}");
                }

                return Ok(new { Succeeded = true, Message = "Usuário cadastrado com sucesso!" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Fluxo original padrão por Email/UserName mantido 100% intacto
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                var roles = await _userManager.GetRolesAsync(user);

                return Ok(new { Succeeded = true, Email = request.Email, Roles = roles });
            }

            return Unauthorized(new { Succeeded = false, Message = "Usuário ou senha inválidos" });
        }

        [HttpGet("user-data")]
        public async Task<IActionResult> GetUserData([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email)) return BadRequest();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound(new { Message = "Usuário não localizado." });

            var endereco = await _context.Enderecos.FirstOrDefaultAsync(e => e.UserId == user.Id);
            var pagamento = await _context.Pagamentos.FirstOrDefaultAsync(p => p.UserId == user.Id);

            return Ok(new
            {
                nome = user.Nome, // Retorna a propriedade customizada
                email = user.Email,
                cep = endereco?.CEP ?? "",
                cidade = endereco?.Cidade ?? "",
                estado = endereco?.Estado ?? "",
                numero = endereco != null ? endereco.Numero.ToString() : "",
                referencial = endereco?.Referencia ?? "",
                metodo = pagamento != null ? ((int)pagamento.Metodo).ToString() : "1",
                titular = pagamento != null ? "ANA S SILVA" : "",
                cartao = pagamento != null ? "4532 •••• •••• 4321" : ""
            });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string TipoPagamento { get; set; } = string.Empty;
        public string NomeNoCartao { get; set; } = string.Empty;
        public string NumeroCartaoMascarado { get; set; } = string.Empty;
    }
}