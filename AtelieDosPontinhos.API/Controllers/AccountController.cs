using AtelieDosPontinhos.Domain.Entities;
using AtelieDosPontinhos.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtelieDosPontinhos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
       
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AtelieDosPontinhosDbContext _context;

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AtelieDosPontinhosDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager; // Corrigido a atribuição sem erro de digitação
            _context = context;
        }

        // ASSINATURA INTEGRADA PARA CADASTRO EXPRESS
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { Message = "E-mail e senha são obrigatórios." });
            }

            var user = new IdentityUser { UserName = request.Email, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                var roleName = string.IsNullOrEmpty(request.Role) ? "Cliente" : request.Role;

                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                await _userManager.AddToRoleAsync(user, roleName);

                // FLUXO DE GRAVAÇÃO DO CHECKOUT EXPRESS NO BANCO
                try
                {
                    int numeroConvertido = 0;
                    int.TryParse(request.Numero, out numeroConvertido);

                    var novoEndereco = new AtelieDosPontinhos.Domain.Entities.Endereco
                    {
                        CEP = request.CEP ?? "",
                        NUMERO = numeroConvertido,
                        Estado = request.Estado ?? "",
                        Cidade = request.Cidade ?? "",
                        Referencial = request.Complemento ?? ""
                    };

                    _context.Add(novoEndereco);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Erro ao gravar endereço express: {ex.Message}");
                }

                return Ok(new { Succeeded = true, Message = "Usuário cadastrado com sucesso!" });
            }

            return BadRequest(result.Errors);
        }

        // 🌟 NOVO: A UI vai chamar este método na API para carregar o endereço do cliente na tela
        [HttpGet("user-data")]
        public async Task<IActionResult> GetUserData([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email)) return BadRequest();

            // Busca na tabela Enderecos o primeiro registro cadastrado
            var endereco = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(_context.Set<AtelieDosPontinhos.Domain.Entities.Endereco>());


            if (endereco == null)
            {
                // Se o banco estiver zerado, devolve vazio para o site não quebrar
                return Ok(new { cep = "", cidade = "", estado = "", numero = "0", referencial = "" });
            }

            // Envia os dados estruturados em JSON para o site ler e preencher as caixinhas
            return Ok(new
            {
                cep = endereco.CEP ?? "",
                cidade = endereco.Cidade ?? "",
                estado = endereco.Estado ?? "",
                numero = endereco.NUMERO.ToString(),
                referencial = endereco.Referencial ?? ""
            });
        }


        // ROTA DE LOGIN
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

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string TipoPagamento { get; set; } = string.Empty;
        public string NomeNoCartao { get; set; } = string.Empty;
        public string NumeroCartaoMascarado { get; set; } = string.Empty;
    }
}
