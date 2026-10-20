using System.Configuration;
using Microsoft.EntityFrameworkCore;
using SmartBusinessPro.Models;

namespace SmartBusinessPro.Data
{
    /// <summary>
    /// Contexto centralizado: único punto de configuración de la conexión
    /// y de exposición de las entidades.
    /// </summary>
    public class SmartBusinessContext : DbContext
    {
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Lee la cadena desde App.config
                string? cadena = ConfigurationManager
                    .ConnectionStrings["SmartBusinessConnection"]?.ConnectionString;

                if (string.IsNullOrWhiteSpace(cadena))
                    throw new InvalidOperationException(
                        "No se encontró la cadena 'SmartBusinessConnection' en App.config.");

                optionsBuilder.UseSqlServer(cadena);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>().ToTable("Categorias");
            modelBuilder.Entity<Producto>().ToTable("Productos");

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId);
        }
    }
}