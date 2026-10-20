namespace SmartBusinessPro.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }
        public string Nombre { get; set; } = string.Empty;

        // Relación 1:N
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}