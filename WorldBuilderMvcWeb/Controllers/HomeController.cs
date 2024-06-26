using Microsoft.AspNetCore.Mvc;

namespace WorldBuilderWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Redirect to Character/Index with a default ID or handle appropriately
            return RedirectToAction("Index", "Character", new { id = 1 });
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
