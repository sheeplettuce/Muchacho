using Microsoft.AspNetCore.Mvc;

namespace Muchacho.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult About()
        {
            return View();
        }
    }
}
