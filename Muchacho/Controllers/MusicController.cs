using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muchacho.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Muchacho.Controllers
{
    [Authorize] // Requiere que el usuario esté logueado
    public class MusicController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MusicController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Music/Index
        public async Task<IActionResult> Index()
        {
            var discos = await _context.Discos
                .FromSqlRaw("SELECT * FROM discos ORDER BY id")
                .ToListAsync();
            return View(discos);
        }

        // GET: Music/Add
        public async Task<IActionResult> Add()
        {
            // Cargar artistas para el dropdown
            var artistas = await _context.Artistas
                .FromSqlRaw("SELECT * FROM artistas ORDER BY name")
                .ToListAsync();
            ViewBag.Artistas = artistas;
            return View();
        }

        // POST: Music/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(string artistName, string name, int? year, string coverURL, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(artistName))
            {
                ViewBag.Error = "El nombre del álbum y artista son requeridos";
                return View();
            }

            try
            {
                // Buscar si el artista ya existe
                var artista = await _context.Artistas
                    .FromSqlRaw("SELECT * FROM artistas WHERE LOWER(name) = LOWER({0})", artistName)
                    .FirstOrDefaultAsync();

                int artistId;

                if (artista == null)
                {
                    // Crear nuevo artista si no existe
                    await _context.Database.ExecuteSqlRawAsync(
                        "INSERT INTO artistas (name) VALUES ({0})", artistName);

                    // Obtener el ID del artista recién creado
                    artista = await _context.Artistas
                        .FromSqlRaw("SELECT * FROM artistas WHERE LOWER(name) = LOWER({0})", artistName)
                        .FirstOrDefaultAsync();
                    artistId = artista.Id;
                }
                else
                {
                    artistId = artista.Id;
                }

                // Insertar el disco
                await _context.Database.ExecuteSqlRawAsync(
                    "INSERT INTO discos (artist_id, name, year, coverurl, price) VALUES ({0}, {1}, {2}, {3}, {4})",
                    artistId, name, year, coverURL ?? "", price);

                TempData["Success"] = "Álbum agregado exitosamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al agregar el álbum: {ex.Message}";
                return View();
            }
        }

        // GET: Music/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var disco = await _context.Discos
                .FromSqlRaw("SELECT * FROM discos WHERE id = {0}", id)
                .FirstOrDefaultAsync();

            if (disco == null)
            {
                return NotFound();
            }

            var artistas = await _context.Artistas
                .FromSqlRaw("SELECT * FROM artistas ORDER BY name")
                .ToListAsync();
            ViewBag.Artistas = artistas;

            return View(disco);
        }

        // POST: Music/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int artistId, string name, int? year, string coverURL, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Error = "El nombre del álbum es requerido";
                var artistas = await _context.Artistas
                    .FromSqlRaw("SELECT * FROM artistas ORDER BY name")
                    .ToListAsync();
                ViewBag.Artistas = artistas;

                // IMPORTANTE: Devolver el modelo con los datos actuales
                var discoActual = await _context.Discos
                    .FromSqlRaw("SELECT * FROM discos WHERE id = {0}", id)
                    .FirstOrDefaultAsync();

                return View(discoActual);
            }

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE discos SET artist_id = {0}, name = {1}, year = {2}, coverurl = {3}, price = {4} WHERE id = {5}",
                    artistId, name, year, coverURL ?? "", price, id);

                TempData["Success"] = "Álbum actualizado exitosamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al actualizar el álbum: {ex.Message}";
                var artistas = await _context.Artistas
                    .FromSqlRaw("SELECT * FROM artistas ORDER BY name")
                    .ToListAsync();
                ViewBag.Artistas = artistas;

                // Devolver el modelo con los datos actuales
                var discoActual = await _context.Discos
                    .FromSqlRaw("SELECT * FROM discos WHERE id = {0}", id)
                    .FirstOrDefaultAsync();

                return View(discoActual);
            }
        }

        // POST: Music/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Primero verificar si hay ventas asociadas
                var ventasExistentes = await _context.Ventas
                    .FromSqlRaw("SELECT * FROM ventas WHERE iddisco = {0}", id)
                    .ToListAsync();

                if (ventasExistentes.Any())
                {
                    TempData["Error"] = "No se puede eliminar el álbum porque tiene ventas asociadas";
                    return RedirectToAction("Index");
                }

                await _context.Database.ExecuteSqlRawAsync(
                    "DELETE FROM discos WHERE id = {0}", id);

                TempData["Success"] = "Álbum eliminado exitosamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar el álbum: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        // GET: Music/Sale/5
        public async Task<IActionResult> Sale(int id)
        {
            var disco = await _context.Discos
                .FromSqlRaw("SELECT * FROM discos WHERE id = {0}", id)
                .FirstOrDefaultAsync();

            if (disco == null)
            {
                return NotFound();
            }

            // Obtener el nombre del artista
            var artista = await _context.Artistas
                .FromSqlRaw("SELECT * FROM artistas WHERE id = {0}", disco.ArtistId)
                .FirstOrDefaultAsync();

            ViewBag.ArtistName = artista?.Name ?? "Artista desconocido";
            return View(disco);
        }

        // POST: Music/Sale/5
        [HttpPost]
        [ActionName("Sale")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalePost(int id)
        {
            try
            {
                var disco = await _context.Discos
                    .FromSqlRaw("SELECT * FROM discos WHERE id = {0}", id)
                    .FirstOrDefaultAsync();

                if (disco == null)
                {
                    return NotFound();
                }

                // Obtener el ID del usuario logueado
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var usuario = await _context.Usuarios
                    .FromSqlRaw("SELECT * FROM usuarios WHERE email = {0}", userEmail)
                    .FirstOrDefaultAsync();

                if (usuario == null)
                {
                    return Unauthorized();
                }

                // Registrar el pedido primero
                await _context.Database.ExecuteSqlRawAsync(
                    "INSERT INTO usuarios_pedidos (idusuario, fecha) VALUES ({0}, {1})",
                    usuario.Id, DateTime.UtcNow);

                // Registrar la venta
                await _context.Database.ExecuteSqlRawAsync(
                    "INSERT INTO ventas (iddisco, idusuario) VALUES ({0}, {1})",
                    id, usuario.Id);

                TempData["Success"] = $"Venta registrada: {disco.Name} - ${disco.Price}";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al registrar la venta: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        // GET: Music/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var disco = await _context.Discos
                .FromSqlRaw("SELECT * FROM discos WHERE id = {0}", id)
                .FirstOrDefaultAsync();

            if (disco == null)
            {
                return NotFound();
            }

            // Obtener el artista
            var artista = await _context.Artistas
                .FromSqlRaw("SELECT * FROM artistas WHERE id = {0}", disco.ArtistId)
                .FirstOrDefaultAsync();

            ViewBag.ArtistName = artista?.Name ?? "Artista desconocido";
            return View(disco);
        }
    }
}