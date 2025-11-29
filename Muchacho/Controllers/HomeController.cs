using Microsoft.AspNetCore.Mvc;
using Muchacho.Models;
using System.Diagnostics;

namespace Muchacho.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Si el usuario está autenticado, mostrar Dashboard
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return View("Dashboard");
            }

            // Si no está autenticado, mostrar la página de inicio con login
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}