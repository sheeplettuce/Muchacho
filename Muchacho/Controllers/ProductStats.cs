using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Muchacho.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Muchacho.Controllers
{
    public class ProductStatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductStatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Stats()
        {
            try
            {
                // Cargar todos los discos ordenados por año
                var discos = await _context.Discos
                    .OrderByDescending(d => d.Year)
                    .ToListAsync();

                if (!discos.Any())
                {
                    ViewBag.Error = "No hay álbumes registrados";
                    return View();
                }

                // Cargar artistas para resolver nombres
                var artistas = await _context.Artistas.ToListAsync();
                var artistaDict = artistas.ToDictionary(a => a.Id, a => a.Name);

                // Proyección para la vista (propiedades esperadas por la vista)
                var albums = discos
                    .Select(d => new
                    {
                        CoverUrl = d.CoverURL,
                        Album = d.Name,
                        Artist = artistaDict.TryGetValue(d.ArtistId, out var n) ? n : $"Artista ID: {d.ArtistId}",
                        Year = d.Year,
                        Price = d.Price
                    })
                    .ToList();

                // Calcular estadísticas básicas
                var stats = new
                {
                    TotalAlbums = discos.Count,
                    PrecioPromedio = discos.Average(d => d.Price),
                    PrecioTotal = discos.Sum(d => d.Price),
                    AlbumsPorPeriodo = CalcularAlbumsPorPeriodo(discos)
                };

                // --- Nuevo: total de dinero vendido por el usuario logueado ---
                decimal currentUserSalesAmount = 0m;
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                Usuario currentUsuario = null;
                if (!string.IsNullOrEmpty(userEmail))
                {
                    currentUsuario = await _context.Usuarios
                        .FirstOrDefaultAsync(u => u.Email == userEmail);
                }

                if (currentUsuario != null)
                {
                    currentUserSalesAmount = await (from v in _context.Ventas
                                                    join d in _context.Discos on v.IdDisco equals d.Id
                                                    where v.IdUsuario == currentUsuario.Id
                                                    select d.Price).SumAsync();
                }

                // --- Nuevo: ranking por monto total vendido (dinero) ---
                var ventasPorUsuario = await (from v in _context.Ventas
                                              join d in _context.Discos on v.IdDisco equals d.Id
                                              group d by v.IdUsuario into g
                                              select new
                                              {
                                                  UserId = g.Key,
                                                  TotalAmount = g.Sum(x => x.Price),
                                                  SalesCount = g.Count()
                                              })
                                             .OrderByDescending(x => x.TotalAmount)
                                             .ToListAsync();

                var userIds = ventasPorUsuario.Select(x => x.UserId).ToList();
                var usuarios = await _context.Usuarios
                    .Where(u => userIds.Contains(u.Id))
                    .ToListAsync();
                var usuarioDict = usuarios.ToDictionary(u => u.Id, u => u.Name);

                var ranking = ventasPorUsuario
                    .Select((x, index) => new
                    {
                        Rank = index + 1,
                        UserId = x.UserId,
                        UserName = usuarioDict.TryGetValue(x.UserId, out var nm) ? nm : $"Usuario {x.UserId}",
                        TotalAmount = x.TotalAmount,
                        SalesCount = x.SalesCount
                    })
                    .ToList();

                // Pasar datos a la vista
                ViewBag.Albums = albums;
                ViewBag.TotalAlbums = stats.TotalAlbums;
                ViewBag.PrecioPromedio = stats.PrecioPromedio;
                ViewBag.PrecioTotal = stats.PrecioTotal;
                ViewBag.AlbumsPorPeriodo = stats.AlbumsPorPeriodo;

                ViewBag.UserSalesAmount = currentUserSalesAmount;
                ViewBag.CurrentUsuarioName = currentUsuario?.Name ?? null;
                ViewBag.Ranking = ranking;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al generar estadísticas: {ex.Message}";
                return View();
            }
        }

        private Dictionary<string, int> CalcularAlbumsPorPeriodo(List<Disco> discos)
        {
            var resultado = new Dictionary<string, int>();

            // Evitar errores si algún disco tiene año null
            var years = discos
                .Where(d => d.Year.HasValue)
                .Select(d => d.Year.Value)
                .ToList();

            if (!years.Any())
                return resultado;

            int yearMin = years.Min();
            int yearMax = years.Max();

            // Crear periodos de 5 años
            for (int year = yearMin; year <= yearMax; year += 5)
            {
                int yearFin = year + 4;
                string periodo = $"{year}-{yearFin}";

                int count = discos.Count(d =>
                    d.Year.HasValue &&
                    d.Year.Value >= year &&
                    d.Year.Value <= yearFin
                );

                if (count > 0)
                    resultado.Add(periodo, count);
            }

            return resultado;
        }
    }
}
