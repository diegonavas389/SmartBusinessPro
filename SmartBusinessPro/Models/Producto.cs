namespace SmartBusinessPro.Models
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Existencia { get; set; }

        // Clave foránea y propiedad de navegación
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
    }
}