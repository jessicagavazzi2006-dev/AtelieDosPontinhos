using AtelieDosPontinhos.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AtelieDosPontinhos.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // 🔓 LOGIN (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 🔐 LOGIN (POST)
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
                    return RedirectToAction("AdminPanel", "Dashboard");

                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError("", "Email ou senha inválidos");
            return View(model);
        }

        // 👥 1. REGISTRAR USUÁRIO (GET) - Abre a página do formulário
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // 💾 2. REGISTRAR USUÁRIO (POST) - Cria o usuário de verdade no banco do Identity
        [HttpPost]
        public async Task<IActionResult> Register(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Cria a instância do usuário utilizando o Email como Username (padrão do Identity)
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };

            // Cria o usuário com a senha criptografada de forma segura
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Opcional: Se quiser que todo usuário cadastrado por aqui vire Admin automaticamente:
                // await _userManager.AddToRoleAsync(user, "Admin");

                // Redireciona de volta para o Painel Administrativo com sucesso
                return RedirectToAction("AdminPanel", "Dashboard");
            }

            // Se o Identity rejeitar a senha (ex: falta de letra maiúscula ou número), mostra os erros na tela
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // 🚪 LOGOUT
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}