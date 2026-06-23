using AtelieDosPontinhos.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

                if (await _userManager.IsInRoleAsync(user, "Admin"))
                    return RedirectToAction("AdminPanel", "Dashboard");

                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError("", "Email ou senha inválidos");
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