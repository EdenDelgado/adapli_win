using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Fsal : Form
    {
        private readonly ApplicationDbContext _context;
        private readonly int _idSalidaFiltro;

        // 1. Constructor estándar (por si se abre desde el menú principal)
        public Fsal(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idSalidaFiltro = 0;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            _ = CargarMaestroDoctoscpAsync();
        }

        // 2. Constructor con parámetro (para cuando se invoca haciendo doble clic desde Fcsal)
        public Fsal(ApplicationDbContext context, int idSalida)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idSalidaFiltro = idSalida;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            _ = CargarMaestroDoctoscpAsync();
        }

        private async void Fccom_Load(object sender, EventArgs e)
        {
            await CargarMaestroDoctoscpAsync();
        }

        public async Task CargarMaestroDoctoscpAsync(string filtroProveedor = null)
        {
            try
            {
                if (_context == null)
                {
                    MessageBox.Show("El contexto de base de datos es nulo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Si se pasó un ID específico por constructor, filtramos la consulta maestro para mostrar solo ese registro
                string sqlQuery = @"
                    SELECT 
                        idsalida AS Idsalida, 
                        folio AS Folio, 
                        fecha AS Fecha,
                        s.concepto AS Concepto, 
                        c.concepto AS NomConcepto,
                        s.cliente AS Cliente, cl.nombre AS NomCliente,
                        almacen AS IdAlmacen,a.nombre AS NomAlmacen,
                        aquien AS Aquien, observaciones AS Observacion, 
                        proyecto AS Proyecto, idunidad AS Idunidad

                    FROM salidainv as s left join concepto as c on s.concepto = c.id_conceptosprov 
                    left join clientes as cl on s.cliente = cl.id_empros left join almacen as a on s.almacen = a.id_almacen";

                if (_idSalidaFiltro > 0)
                {
                    sqlQuery += " WHERE idsalida = @IdSalidaFiltro";
                }

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                if (_idSalidaFiltro > 0)
                {
                    command.Parameters.AddWithValue("@IdSalidaFiltro", _idSalidaFiltro);
                }

                using var reader = await command.ExecuteReaderAsync();
                var dataTable = new System.Data.DataTable();
                dataTable.Load(reader);

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dataTable;
                dataGridView1.Refresh();

                if (dataGridView1.Rows.Count > 0)
                {
                    await CargarDetalleDoctoscpAsync();
                    txfolio.Text = dataGridView1.CurrentRow.Cells["Folio"].Value?.ToString() ?? string.Empty;
                    txfecha.Text = dataGridView1.CurrentRow.Cells["Fecha"].Value?.ToString() ?? string.Empty;
                    txconcept.Text = dataGridView1.CurrentRow.Cells["NomConcepto"].Value?.ToString() ?? string.Empty;
                    txalmacen.Text = dataGridView1.CurrentRow.Cells["NomAlmacen"].Value?.ToString() ?? string.Empty;
                    // txtrabajo.Text = dataGridView1.CurrentRow.Cells["Trabajo"].Value?.ToString() ?? string.Empty;
                    txcliente.Text = dataGridView1.CurrentRow.Cells["NomCliente"].Value?.ToString() ?? string.Empty;
                    txaquien.Text = dataGridView1.CurrentRow.Cells["Aquien"].Value?.ToString() ?? string.Empty;
                    txobservacion.Text = dataGridView1.CurrentRow.Cells["Observacion"].Value?.ToString() ?? string.Empty;
                    txproyecto.Text = dataGridView1.CurrentRow.Cells["Proyecto"].Value?.ToString() ?? string.Empty;
                    txunidad.Text = dataGridView1.CurrentRow.Cells["Idunidad"].Value?.ToString() ?? string.Empty;
                    //txdoc_origen.Text = dataGridView1.CurrentRow.Cells["Doc_Origen"].Value?.ToString() ?? string.Empty;
                }
                else
                {
                    dataGridView2.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el maestro salidainv: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarDetalleDoctoscpAsync()
        {
            try
            {
                if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Cells["idsalida"].Value == null)
                {
                    dataGridView2.DataSource = null;
                    return;
                }

                int idDoctoscp = Convert.ToInt32(dataGridView1.CurrentRow.Cells["idsalida"].Value);

                string sqlQuery = @"
                    SELECT 
                        salida AS Salida, 
                        idsalidadet AS Idsalidadet, qty as cantidad,
                        producto AS Producto, 
                        s.clave AS ClaveProducto,
                        s.s_nombre AS NombreProducto,
                        precio AS Precio, 
                        (qty * precio) AS Importe
                    FROM salidadetinv left join servicios s on salidadetinv.producto = s.id_servicio
                    WHERE salida = @Idsalida";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;
                command.Parameters.AddWithValue("@Idsalida", idDoctoscp);

                using var reader = await command.ExecuteReaderAsync();
                var dataTable = new System.Data.DataTable();
                dataTable.Load(reader);

                dataGridView2.AutoGenerateColumns = true;
                dataGridView2.RowHeadersWidth = 20; // Default is usually 43
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = dataTable;
                // Define una altura de 40 píxeles para todas las filas
                dataGridView2.RowTemplate.Height = 40;
                // 1. Ocultar las líneas divisorias (tanto horizontales como verticales)
                dataGridView2.CellBorderStyle = DataGridViewCellBorderStyle.None;

                // 2. Personalizar el color de fondo del encabezado a Azul y su texto en Blanco
                dataGridView2.EnableHeadersVisualStyles = false; // Necesario para que permita cambiar colores predeterminados de Windows
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(0, 122, 204); // Azul moderno
                dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                dataGridView2.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
                dataGridView2.Columns["NombreProducto"].Width = 260; // Cambia el encabezado de la columna
                dataGridView2.Columns["Salida"].Visible = false; // Oculta la columna Salida
                dataGridView2.Columns["Idsalidadet"].Visible = false; // Oculta la columna Idsalidadet
                dataGridView2.Columns["producto"].Visible = false; // Oculta la columna Producto
                dataGridView2.Columns["Precio"].DefaultCellStyle.Format = "C2"; // Formato de moneda
                dataGridView2.Columns["Importe"].DefaultCellStyle.Format = "C2"; // Formato de moneda
                dataGridView2.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el detalle salidadetinv: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            await CargarDetalleDoctoscpAsync();
        }

        private void btregresar_Click(object sender, EventArgs e)
        {
         
        }

        private void regresabt_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}