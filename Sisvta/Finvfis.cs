using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Finvfis : Form
    {
        private readonly ApplicationDbContext _context;
        private readonly int _idEntradaFiltro;

        // 1. Constructor estándar (por si se abre desde el menú principal)
        public Finvfis(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idEntradaFiltro = 0;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            _ = CargarMaestroDoctoscpAsync();
        }

        // 2. Constructor con parámetro (para cuando se invoca haciendo doble clic desde el catálogo)
        public Finvfis(ApplicationDbContext context, int idEntrada)
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

                string sqlQuery = @"    SELECT 
                        id_invfisico AS Idinvfisico, 
                        fecha AS Fecha,
                        usuario AS Usuario, u.nombre as Nombre,u.idAlmacen as Almacen,a.nombre as Nomalmacen
                    FROM invfisico as i left join usuarios as u on i.usuario=u.id_usuario
                        left join almacen as a on u.idAlmacen=a.id_almacen";

                // CORREGIDO: Se evalúa sobre d.id_factura para asegurar que funcione al filtrar por ID específico
                if (_idEntradaFiltro > 0)
                {
                    sqlQuery += " WHERE id_invfisico = @IdEntradaFiltro";
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
                    txfolio.Text = dataGridView1.CurrentRow.Cells["Idinvfisico"].Value?.ToString() ?? string.Empty;
                    txfecha.Text = dataGridView1.CurrentRow.Cells["Fecha"].Value?.ToString() ?? string.Empty;
                    txnomalmacen.Text = dataGridView1.CurrentRow.Cells["Nomalmacen"].Value?.ToString() ?? string.Empty;
                    //txmovinv.Text = dataGridView1.CurrentRow.Cells["IdInventario"].Value?.ToString() ?? string.Empty;
                    txusuario.Text = dataGridView1.CurrentRow.Cells["Nombre"].Value?.ToString() ?? string.Empty;
                   // txproveedor.Text = dataGridView1.CurrentRow.Cells["NomProveedor"].Value?.ToString() ?? string.Empty;
                   // txobservacion.Text = dataGridView1.CurrentRow.Cells["Observacion"].Value?.ToString() ?? string.Empty;
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
                if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Cells["Idinvfisico"].Value == null ||
                    dataGridView1.CurrentRow.Cells["Idinvfisico"].Value == DBNull.Value)
                {
                    dataGridView2.DataSource = null;
                    return;
                }

                int idInventario = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Idinvfisico"].Value);

                string sqlQuery = @"
                    SELECT 
                        invfisico AS Invfisico, 
                        id_invfisdet AS Ifinvfisdet, 
                        cantidad AS Cantidad,
                        articulo AS Articulo, 
                        s.clave AS ClaveProducto,
                        s.s_nombre AS NombreProducto                       
                    FROM invfisdet as d 
                    LEFT JOIN servicios s ON d.articulo = s.id_servicio
                    WHERE Invfisico = @idInventario";

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