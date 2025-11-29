
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Muchacho.Models;
using System.Security.Claims;


namespace Muchacho.Controllers
{
    
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        //vista
        public IActionResult Login()
        {
            return View();
        }


        //login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Introduce email y contraseña";
                return View();
            }

            var user = await _context.Usuarios
                 .FromSqlRaw("SELECT * FROM usuarios WHERE email = {0}", email)
                 .FirstOrDefaultAsync();

            // AQUÍ ESTÁ EL FIX: Verificar que el usuario existe Y que la contraseña coincide
            if (user != null && user.Password == password)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name) // Mejor usar el nombre real
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Email o contraseña incorrectos";
            return View();
        }
        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string email, string password, string name)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Error = "Introduce email, contraseña y nombre";
                return View();
            }

            // Verificar si el usuario ya existe en la base de datos
            var existingUser = await _context.Usuarios
                .FromSqlRaw("SELECT * FROM usuarios WHERE email = {0}", email)
                .FirstOrDefaultAsync();

            if (existingUser != null)
            {
                ViewBag.Error = "Este email ya está registrado";
                return View();
            }

            // Insertar nuevo usuario en la base de datos
            await _context.Database.ExecuteSqlRawAsync(
                "INSERT INTO usuarios (email, password, name) VALUES ({0}, {1}, {2})",
                email, password, name);

            return RedirectToAction("Login");
        }
        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}


