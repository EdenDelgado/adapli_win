using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Ftra : Form
    {
        private readonly ApplicationDbContext _context;
        private readonly int _idTraspasoFiltro;

        // 1. Constructor estándar (por si se abre desde el menú principal)
        public Ftra(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idTraspasoFiltro = 0;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            _ = CargarMaestroDoctoscpAsync();
        }

        // 2. Constructor con parámetro (para cuando se invoca haciendo doble clic desde el catálogo)
        public Ftra(ApplicationDbContext context, int idTraspaso)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idTraspasoFiltro = idTraspaso;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            _ = CargarMaestroDoctoscpAsync();
        }

        private async void Ftra_Load(object sender, EventArgs e)
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

                string sqlQuery = @"
                    SELECT 
                        t.id_traspaso AS Idtraspaso, 
                        t.almacen_origen AS AlmacenOrigen, 
                        t.almacen_destino AS AlmacenDestino,
                        t.fecha AS Fecha,
                        t.concepto AS Concepto, 
                        c.concepto AS NomConcepto,
                  
                        d.nombre AS NomAlmacenDestino,
                        o.nombre AS NomAlmacenOrigen,
                        t.comentarios as Comentarios
                      
                    FROM traspaso as t 
                  
                    LEFT JOIN concepto as c ON t.concepto = c.id_conceptosprov 
                  left JOIN almacen as o ON t.almacen_origen = o.id_almacen
                    LEFT JOIN almacen as d ON t.almacen_destino = d.id_almacen";

                // CORREGIDO: Se evalúa sobre d.id_traspaso para asegurar que funcione al filtrar por ID específico
                if (_idTraspasoFiltro > 0)
                {
                    sqlQuery += " WHERE t.id_traspaso = @IdTraspasoFiltro";
                }

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                if (_idTraspasoFiltro > 0)
                {
                    command.Parameters.AddWithValue("@IdTraspasoFiltro", _idTraspasoFiltro  );
                }

                using var reader = await command.ExecuteReaderAsync();
                var dataTable = new System.Data.DataTable();
                dataTable.Load(reader);

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dataTable;
                dataGridView1.Refresh();

                if (dataGridView1.Rows.Count > 0 && dataGridView1.CurrentRow != null)
                {
                    await CargarDetalleDoctoscpAsync();
                    txtraspaso.Text = dataGridView1.CurrentRow.Cells["IdTraspaso"].Value?.ToString() ?? string.Empty;
                    txfecha.Text = dataGridView1.CurrentRow.Cells["Fecha"].Value?.ToString() ?? string.Empty;
                    txconcept.Text = dataGridView1.CurrentRow.Cells["NomConcepto"].Value?.ToString() ?? string.Empty;
                   // txmovinv.Text = dataGridView1.CurrentRow.Cells["IdInventario"].Value?.ToString() ?? string.Empty;
                    txalmacenorigen.Text = dataGridView1.CurrentRow.Cells["NomAlmacenOrigen"].Value?.ToString() ?? string.Empty;
                    txalmacendestino.Text = dataGridView1.CurrentRow.Cells["NomAlmacenDestino"].Value?.ToString() ?? string.Empty;
                   // txproveedor.Text = dataGridView1.CurrentRow.Cells["NomProveedor"].Value?.ToString() ?? string.Empty;
                    txobservacion.Text = dataGridView1.CurrentRow.Cells["Comentarios"].Value?.ToString() ?? string.Empty;
                }
                else
                {
                    dataGridView2.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el maestro inventario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarDetalleDoctoscpAsync()
        {
            try
            {
                if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Cells["IdTraspaso"].Value == null ||
                    dataGridView1.CurrentRow.Cells["IdTraspaso"].Value == DBNull.Value)
                {
                    dataGridView2.DataSource = null;
                    return;
                }

                int idInventario = Convert.ToInt32(dataGridView1.CurrentRow.Cells["IdTraspaso"].Value);

                string sqlQuery = @"
                    SELECT 
                        traspaso AS traspaso, 
                        idtraspasodet  AS Traspasodet, 
                        qty AS Cantidad,
                        producto AS Producto, 
                        s.clave AS ClaveProducto,
                        s.s_nombre AS NombreProducto,
                        preciou AS Precio, 
                        (qty * preciou) AS Importe
                    FROM traspaso_det as d 
                    LEFT JOIN servicios s ON d.producto = s.id_servicio
                    WHERE traspaso = @idTraspaso";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;
                command.Parameters.AddWithValue("@IdTraspaso", idInventario);

                using var reader = await command.ExecuteReaderAsync();
                var dataTable = new System.Data.DataTable();
                dataTable.Load(reader);

                dataGridView2.AutoGenerateColumns = true;
                dataGridView2.RowHeadersWidth = 20;
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = dataTable;

                dataGridView2.RowTemplate.Height = 40;
                dataGridView2.CellBorderStyle = DataGridViewCellBorderStyle.None;

                dataGridView2.EnableHeadersVisualStyles = false;
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
                dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                dataGridView2.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);

                dataGridView2.Columns["NombreProducto"].Width = 260;

                // Ocultar columnas técnicas o de control
                // Nota: Asegúrate de que la columna oculta en el detalle coincida con un campo devuelto en el SELECT (en este caso usamos Entrada)
                if (dataGridView2.Columns.Contains("Entrada"))
                    dataGridView2.Columns["Entrada"].Visible = false;

                if (dataGridView2.Columns.Contains("Entradadet"))
                    dataGridView2.Columns["Entradadet"].Visible = false;

                if (dataGridView2.Columns.Contains("Producto"))
                    dataGridView2.Columns["Producto"].Visible = false;

                // Formatos y alineaciones monetarias
                if (dataGridView2.Columns.Contains("Precio"))
                {
                    dataGridView2.Columns["Precio"].DefaultCellStyle.Format = "C2";
                    dataGridView2.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView2.Columns["Precio"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dataGridView2.Columns.Contains("Importe"))
                {
                    dataGridView2.Columns["Importe"].DefaultCellStyle.Format = "C2";
                    dataGridView2.Columns["Importe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView2.Columns["Importe"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                dataGridView2.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el detalle de inventario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            await CargarDetalleDoctoscpAsync();
        }

        private void regresabt_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}