using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    // 🔑 LOGIN VIEW
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // 🔐 LOGIN POST REAL
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(
            email,
            password,
            false,
            false
        );

        if (result.Succeeded)
        {
            var user = User;

            if (User.IsInRole("Admin"))
                return RedirectToAction("AdminPanel", "Dashboard");

            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Login inválido";
        return View();
    }

    // 🚪 LOGOUT
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}