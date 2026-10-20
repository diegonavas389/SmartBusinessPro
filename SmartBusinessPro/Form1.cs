using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SmartBusinessPro.Data;

namespace SmartBusinessPro
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            btnProbarConexion.Click += btnProbarConexion_Click;
            btnCargar.Click += btnCargar_Click;
        }

        // Verifica la comunicación con el servidor y la base de datos
        private void btnProbarConexion_Click(object? sender, EventArgs e)
        {
            try
            {
                using var db = new SmartBusinessContext();

                if (db.Database.CanConnect())
                {
                    lblEstado.Text = "✔ Conexión exitosa a SmartBusinessPro";
                    lblEstado.ForeColor = Color.Green;
                }
                else
                {
                    lblEstado.Text = "✘ No se pudo conectar (servidor o base de datos incorrectos)";
                    lblEstado.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        // Recupera los productos y los muestra en el DataGridView
        private void btnCargar_Click(object? sender, EventArgs e)
        {
            try
            {
                using var db = new SmartBusinessContext();

                var datos = db.Productos
                    .AsNoTracking()
                    .Include(p => p.Categoria)
                    .OrderBy(p => p.Nombre)
                    .Select(p => new
                    {
                        Producto = p.Nombre,
                        Precio = p.Precio,
                        Existencia = p.Existencia,
                        Categoria = p.Categoria!.Nombre
                    })
                    .ToList();

                dgvProductos.DataSource = datos;
                dgvProductos.Columns["Precio"].DefaultCellStyle.Format = "C2";

                lblEstado.Text = $"✔ {datos.Count} productos recuperados";
                lblEstado.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void MostrarError(Exception ex)
        {
            string mensaje = ex switch
            {
                SqlException { Number: 53 or 40 or -1 or 2 } =>
                    "No se encontró el servidor SQL. Verifique el nombre de la instancia y que el servicio esté activo.",
                SqlException { Number: 4060 } =>
                    "La base de datos indicada no existe o el usuario no tiene acceso a ella.",
                SqlException { Number: 18456 } =>
                    "Inicio de sesión fallido: usuario o contraseña incorrectos.",
                SqlException { Number: -2 } =>
                    "Se agotó el tiempo de espera de la conexión.",
                SqlException sql =>
                    $"Error de SQL Server ({sql.Number}): {sql.Message}",
                InvalidOperationException =>
                    $"Error de configuración: {ex.Message}",
                _ => $"Error inesperado: {ex.Message}"
            };

            lblEstado.Text = "✘ " + mensaje;
            lblEstado.ForeColor = Color.Red;
            MessageBox.Show(mensaje, "Error de conexión",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}