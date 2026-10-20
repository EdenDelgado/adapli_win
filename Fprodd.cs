using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Entities;
using Sisvta.Infrastructure.Persistence.Contexts; // Asegúrate de tener este using
using Sisvta.Infrastructure.Repositories;
using Sisvta.UseCases;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisvta
{
    public partial class Fprodd : Form
    {
        private readonly GestionarProductosUseCase _useCase;
        private readonly ApplicationDbContext _context; // Contexto para consultas directas
        public event EventHandler ProductoModificado;

        public int linea = 0; // Variable para almacenar el id de la línea seleccionada

        // Actualizamos el constructor para recibir también el ApplicationDbContext
        public Fprodd(GestionarProductosUseCase useCase, ApplicationDbContext context)
        {
            InitializeComponent();
            _useCase = useCase;
            _context = context;
            // _ = CargarDetalleServiciosAsync();
        }

        // Evento Load del formulario para cargar los datos en el grid al abrir
        private async void Fprodd_Load(object sender, EventArgs e)
        {
            await CargarDetalleServiciosAsync(clavetxt.Text.Trim());
        }





        public async Task CargarDetalleServiciosAsync(string claveProducto = null)
        {
            try
            {
                if (_context == null) return;

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                // ==========================================
                // 1. PRIMER QUERY (El que ya tienes para Grid 1)
                // ==========================================
                string sqlQuery1 = @"
                    SELECT 
                        d.movinventario AS Idfactura, 
                        d.cantidad AS Cantidad, 
                        d.preciou AS PrecioU, 
                        d.daterecord AS FechaRegistro, 
                        d.status AS Status, 
                        d.articulo AS Articulo,
                        t.impuesto AS Impuesto, t.porcentaje AS tasa,
                        s.s_nombre AS NombreServicio,
                        h.almacen AS Almacen,
                        a.nombre AS NombreAlmacen,
                        ia.saldo AS Existencia 
                    FROM inventario_detalles d
                    LEFT JOIN servicios s ON d.articulo = s.id_servicio 
                    LEFT JOIN Inventario h ON d.movinventario = h.id_inventario 
                    LEFT JOIN Inventarioactual AS ia ON d.articulo = ia.articulo AND h.almacen = ia.almacen 
                    LEFT JOIN almacen AS a ON h.almacen = a.id_almacen
                    LEFT JOIN impuestos_articulo AS i ON d.articulo = i.articulo
                    LEFT JOIN impuesto AS t ON i.impuesto = t.id_impuesto
                    WHERE (d.cantidad * d.preciou) > 0 
                      AND (@ClaveProducto IS NULL OR @ClaveProducto = '' OR d.articulo = @ClaveProducto)";

                using var command1 = connection.CreateCommand();
                command1.CommandText = sqlQuery1;
                command1.Parameters.AddWithValue("@ClaveProducto", string.IsNullOrEmpty(claveProducto) ? (object)DBNull.Value : claveProducto);

                using var reader1 = await command1.ExecuteReaderAsync();
                var dataTable1 = new System.Data.DataTable();
                dataTable1.Load(reader1);

                if (dataTable1.Rows.Count > 0)
                {
                    existx.Text = dataTable1.Rows[0]["Existencia"]?.ToString() ?? "0.00";
                    var ultimaFila = dataTable1.Rows[dataTable1.Rows.Count - 1];

                    if (ultimaFila["PrecioU"] != DBNull.Value && decimal.TryParse(ultimaFila["PrecioU"].ToString(), out decimal ultimoPrecio))
                    {
                        ultimopreciotxt.Text = ultimoPrecio.ToString("N2");
                        almacentx.Text = ultimaFila["NombreAlmacen"]?.ToString() ?? string.Empty;
                        impuestotx.Text = ultimaFila["Impuesto"]?.ToString() ?? string.Empty;
                        tasatx.Text = ultimaFila["tasa"]?.ToString() ?? string.Empty;
                    }
                    else
                    {
                        ultimopreciotxt.Text = "0.00";
                    }
                }
                else
                {
                    ultimopreciotxt.Text = "0.00";
                    existx.Text = "0.00";
                }

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dataTable1;
                dataGridView1.Refresh();


                // ==========================================
                // 2. SEGUNDO QUERY (Para Almacenes y Existencias en DataGridView2)
                // ==========================================
                string sqlQuery2 = @"
                    SELECT /*
                        ia.almacen AS Almacen,*/
                        a.nombre AS NombreAlmacen,
                        CAST(ia.saldo AS DECIMAL(18, 3)) AS Existencia
                        
                    FROM Inventarioactual ia
                    LEFT JOIN almacen a ON ia.almacen = a.id_almacen
                    WHERE (@ClaveProducto IS NULL OR @ClaveProducto = '' OR ia.articulo = @ClaveProducto)";

                using var command2 = connection.CreateCommand();
                command2.CommandText = sqlQuery2;
                command2.Parameters.AddWithValue("@ClaveProducto", string.IsNullOrEmpty(claveProducto) ? (object)DBNull.Value : claveProducto);

                using var reader2 = await command2.ExecuteReaderAsync();
                var dataTable2 = new System.Data.DataTable();
                dataTable2.Load(reader2);

                // Asignamos al segundo grid (Asegúrate de tener un DataGridView llamado dataGridView2 en tu formulario)
                dataGridView2.AutoGenerateColumns = true;
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = dataTable2;
                if (dataGridView2.Columns["Existencia"] != null)
                {
                    dataGridView2.Columns["Existencia"].DefaultCellStyle.Format = "N3";
                    dataGridView2.Columns["Existencia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                dataGridView2.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el detalle: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void btnguardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(clavetxt.Text) ||
                string.IsNullOrWhiteSpace(nombretxt.Text) ||
                string.IsNullOrWhiteSpace(descripciontxt.Text))
            {
                MessageBox.Show("Por favor, completa todos los campos requeridos.", "Campos requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var producto = new Producto(
                    clavetxt.Text.Trim(),
                    nombretxt.Text.Trim(),
                    descripciontxt.Text.Trim(),
                    inventariabletxt.Text.Trim().ToLower() == "true" || inventariabletxt.Text.Trim() == "1",
                    umedidatxt.Text.Trim() == string.Empty ? 0 : int.Parse(umedidatxt.Text.Trim()),
                    int.Parse(prodservtxt.Text.Trim()),
                    //almacentx.Text.Trim(),
                    claveproservtxt.Text.Trim(),
                    int.Parse(lineatxt.Text.Trim()),
                    //int.Parse(grupotxt.Text.Trim()),
                    int.Parse(statustxt.Text.Trim()),
                    double.Parse(preciotxt.Text.Trim())
                );

                bool exito = await _useCase.RegistrarAsync(producto);

                if (exito)
                {
                    MessageBox.Show("Producto guardado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ProductoModificado?.Invoke(this, EventArgs.Empty);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
            else
            {
                base.OnFormClosing(e);
            }
        }

        private void btregresar_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void lineatxt_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (_context == null)
                {
                    MessageBox.Show("El contexto de la base de datos no está inicializado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Instanciamos el selector Fclinl pasándole únicamente el contexto (igual que Fcgrul)
                using var fclinl = new Fclinl(_context);

                if (fclinl.ShowDialog() == DialogResult.OK)
                {
                    // Asignamos el valor seleccionado al campo correspondiente
                    lineatxt.Text = fclinl.NombreSeleccionado;
                    linea= int.Parse(fclinl.IdLineaSeleccionado.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el selector de líneas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento que enlaza directamente con el botón del diseñador visual
        private async void btGuardar_Click(object sender, EventArgs e)
        {
            // Reutilizamos la lógica del método btnguardar_Click existente
            await EjecutarGuardadoAsync();
        }

        // Método privado que contiene toda la lógica de validación e inserción/actualización
      

        private async Task EjecutarGuardadoAsync()
        {
            if (string.IsNullOrWhiteSpace(clavetxt.Text) ||
                string.IsNullOrWhiteSpace(nombretxt.Text) ||
                string.IsNullOrWhiteSpace(descripciontxt.Text))
            {
                MessageBox.Show("Por favor, completa todos los campos requeridos.", "Campos requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Conversiones seguras para evitar errores con campos vacíos
                int uMedida = int.TryParse(umedidatxt.Text.Trim(), out int um) ? um : 0;
                //int? prodServ = int.TryParse(prodservtxt.Text.Trim(), out int ps) ? ps : 0;
                int? prodServ = int.TryParse(prodservtxt.Text.Trim(), out int ps) ? ps : (int?)(null);
                int status = int.TryParse(statustxt.Text.Trim(), out int st) ? st : 0;
                double precio = double.TryParse(preciotxt.Text.Trim(), out double pr) ? pr : 0.0;

                var producto = new Producto(
                    clavetxt.Text.Trim(),
                    nombretxt.Text.Trim(),
                    descripciontxt.Text.Trim(),
                    inventariabletxt.Text.Trim().ToLower() == "true" || inventariabletxt.Text.Trim() == "1",
                    uMedida,
                    prodServ,
                    claveproservtxt.Text.Trim(),
                    linea,
                    status,
                    precio
                );

                bool exito = await _useCase.RegistrarAsync(producto);

                if (exito)
                {
                    MessageBox.Show("Producto guardado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ProductoModificado?.Invoke(this, EventArgs.Empty);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                string errorReal = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show($"Error detallado al guardar: {errorReal}", "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}