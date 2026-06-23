using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtelieDosPontinhos.UI.Controllers
{
    public class DashboardController : Controller
    {
        // 👤 Usuário logado (cliente ou admin)
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // 🔐 Somente Admin
        [Authorize(Roles = "Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }
    }
}