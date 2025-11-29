using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Muchacho.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Artista> Artistas { get; set; }
        public DbSet<Disco> Discos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }


    }
}
