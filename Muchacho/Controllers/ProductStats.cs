
using Microsoft.AspNetCore.Mvc;
using Muchacho.Models;

namespace Muchacho.Controllers
{
    public class ProductStats : Controller
    {
        public IActionResult Stats()
        {
            // Obtener todos los álbumes ordenados por año (más nuevo a más viejo)
            var albumsOrdenados = Music.Musics.OrderByDescending(m => m.Year).ToList();

            // Calcular estadísticas
            var stats = new
            {
                TotalAlbums = Music.Musics.Count,
                PrecioPromedio = Music.Musics.Any() ? Music.Musics.Average(m => m.Price) : 0,
                PrecioTotal = Music.Musics.Sum(m => m.Price),
                AlbumsPorPeriodo = CalcularAlbumsPorPeriodo()
            };

            ViewBag.Albums = albumsOrdenados;
            ViewBag.TotalAlbums = stats.TotalAlbums;
            ViewBag.PrecioPromedio = stats.PrecioPromedio;
            ViewBag.PrecioTotal = stats.PrecioTotal;
            ViewBag.AlbumsPorPeriodo = stats.AlbumsPorPeriodo;

            return View();
        }

        private Dictionary<string, int> CalcularAlbumsPorPeriodo()
        {
            var albumsPorPeriodo = new Dictionary<string, int>();

            // Obtener el año más antiguo y más reciente
            int yearMin = Music.Musics.Min(m => m.Year);
            int yearMax = Music.Musics.Max(m => m.Year);

            // Crear periodos de 5 años
            for (int year = yearMin; year <= yearMax; year += 5)
            {
                int yearFin = year + 4;
                string periodo = $"{year}-{yearFin}";

                int count = Music.Musics.Count(m => m.Year >= year && m.Year <= yearFin);

                if (count > 0)
                {
                    albumsPorPeriodo.Add(periodo, count);
                }
            }

            return albumsPorPeriodo;
        }
    }
}