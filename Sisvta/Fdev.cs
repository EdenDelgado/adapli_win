using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Fdev : Form
    {
        private readonly ApplicationDbContext _context;
        private readonly int _idEntradaFiltro;

        // 1. Constructor estándar (por si se abre desde el menú principal)
        public Fdev(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idEntradaFiltro = 0;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            _ = CargarMaestroDoctoscpAsync();
        }

        // 2. Constructor con parámetro (para cuando se invoca haciendo doble clic desde el catálogo)
        public Fdev(ApplicationDbContext context, int idEntrada)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idEntradaFiltro = idEntrada;

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

                string sqlQuery = @"
                 
                    SELECT 
                        d.id_Devolucion AS Iddevolucion, 
                        d.foliodevolucion AS Folio,d.id_VentaDcc AS VentaDcc, 
                        d.fecha AS Fecha,
                      d.id_Entrada AS Entrada, 
                        i.id_inventario as Idinventario,
                        i.usercreator as usuario,
                        a.nombre as NomAlmacen,
                       d.id_NotaCredito as NotaCredito                        
                    FROM devolucion as d left join inventario as i on d.id_Entrada=i.id_inventario
                        left join almacen as a on i.almacen=a.id_almacen ";                   

                // CORREGIDO: Se evalúa sobre d.id_factura para asegurar que funcione al filtrar por ID específico
                if (_idEntradaFiltro > 0)
                {
                    sqlQuery += " WHERE d.id_Devolucion = @IdEntradaFiltro";
                }

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                if (_idEntradaFiltro > 0)
                {
                    command.Parameters.AddWithValue("@IdEntradaFiltro", _idEntradaFiltro);
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
                    txfolio.Text = dataGridView1.CurrentRow.Cells["Folio"].Value?.ToString() ?? string.Empty;
                    txfecha.Text = dataGridView1.CurrentRow.Cells["Fecha"].Value?.ToString() ?? string.Empty;
                    txusuario.Text = dataGridView1.CurrentRow.Cells["usuario"].Value?.ToString() ?? string.Empty;
                    txmovinv.Text = dataGridView1.CurrentRow.Cells["IdInventario"].Value?.ToString() ?? string.Empty;
                    txalmacen.Text = dataGridView1.CurrentRow.Cells["NomAlmacen"].Value?.ToString() ?? string.Empty;
                   // txproveedor.Text = dataGridView1.CurrentRow.Cells["NomProveedor"].Value?.ToString() ?? string.Empty;
                    //txobservacion.Text = dataGridView1.CurrentRow.Cells["Observacion"].Value?.ToString() ?? string.Empty;
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
                if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Cells["IdInventario"].Value == null ||
                    dataGridView1.CurrentRow.Cells["IdInventario"].Value == DBNull.Value)
                {
                    dataGridView2.DataSource = null;
                    return;
                }

                int idInventario = Convert.ToInt32(dataGridView1.CurrentRow.Cells["IdInventario"].Value);

                string sqlQuery = @"
                    SELECT 
                        movinventario AS Entrada, 
                        id_invdet AS Entradadet, 
                        cantidad AS Cantidad,
                        articulo AS Articulo, 
                        s.clave AS ClaveProducto,
                        s.s_nombre AS NombreProducto,
                        preciou AS Precio, 
                        (cantidad * preciou) AS Importe
                    FROM inventario_detalles as d 
                    LEFT JOIN servicios s ON d.articulo = s.id_servicio
                    WHERE movinventario = @idInventario";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;
                command.Parameters.AddWithValue("@IdInventario", idInventario);

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

              //  if (dataGridView2.Columns.Contains("Entradadet"))
                //    dataGridView2.Columns["Entradadet"].Visible = false;

                if (dataGridView2.Columns.Contains("Articulo"))
                    dataGridView2.Columns["Articulo"].Visible = false;

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