using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Ftac : Form
    {
        private readonly ApplicationDbContext _context;
        private readonly int _idtransaccionFiltro;

        // 1. Constructor estándar (por si se abre desde el menú principal)
        public Ftac(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idtransaccionFiltro = 0;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            _ = CargarMaestroDoctoscpAsync();
        }

        // 2. Constructor con parámetro (para cuando se invoca haciendo doble clic desde el catálogo)
        public Ftac(ApplicationDbContext context, int idTransaccion)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idtransaccionFiltro = idTransaccion;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            _ = CargarMaestroDoctoscpAsync();
        }

        private async void Fccom_Load(object sender, EventArgs e)
        {
            await CargarMaestroDoctoscpAsync();
        }

        public async Task CargarMaestroDoctoscpAsync(string filtrotransaccion = null)
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
                        id_transactions AS Idtransaccion, 
                        whentry AS Moventrada,
                        qty AS Cantidad,
                        fechamov AS Fecha,
                        locationfrom AS Locationfrom,
                        locationto AS Locationto
                    FROM transacciones";

                if (_idtransaccionFiltro > 0)
                {
                    sqlQuery += " WHERE id_transactions = @Idtransaccion";
                }

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                if (_idtransaccionFiltro > 0)
                {
                    command.Parameters.AddWithValue("@Idtransaccion", _idtransaccionFiltro);
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
                    txtransaccion.Text = dataGridView1.CurrentRow.Cells["Idtransaccion"].Value?.ToString() ?? string.Empty;
                    txfecha.Text = dataGridView1.CurrentRow.Cells["Fecha"].Value?.ToString() ?? string.Empty;
                    txmoventrada.Text = dataGridView1.CurrentRow.Cells["Moventrada"].Value?.ToString() ?? string.Empty;
                    txalmacenorigen.Text = dataGridView1.CurrentRow.Cells["Locationfrom"].Value?.ToString() ?? string.Empty;
                    txalmacendestino.Text = dataGridView1.CurrentRow.Cells["Locationto"].Value?.ToString() ?? string.Empty;
                    txcantidad.Text = dataGridView1.CurrentRow.Cells["Cantidad"].Value?.ToString() ?? string.Empty;
                }
                else
                {
                    dataGridView2.DataSource = null;
                    dataGridView3.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el maestro transacciones: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarDetalleDoctoscpAsync()
        {
            try
            {
                if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Cells["Idtransaccion"].Value == null ||
                    dataGridView1.CurrentRow.Cells["Idtransaccion"].Value == DBNull.Value)
                {
                    dataGridView2.DataSource = null;
                    dataGridView3.DataSource = null;
                    return;
                }

                int idTransaccionActual = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Idtransaccion"].Value);
                int idMoventrada = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Moventrada"].Value);

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                // ==========================================
                // 1. CARGAR DATAGRIDVIEW2 (Detalle de Inventario)
                // ==========================================
                string sqlQuery1 = @"
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
                    WHERE d.id_invdet = @IdMoventrada";

                using (var command1 = connection.CreateCommand())
                {
                    command1.CommandText = sqlQuery1;
                    command1.Parameters.AddWithValue("@IdMoventrada", idMoventrada);

                    using var reader1 = await command1.ExecuteReaderAsync();
                    var dataTable1 = new System.Data.DataTable();
                    dataTable1.Load(reader1);

                    dataGridView2.AutoGenerateColumns = true;
                    dataGridView2.RowHeadersWidth = 20;
                    dataGridView2.DataSource = null;
                    dataGridView2.DataSource = dataTable1;

                    dataGridView2.RowTemplate.Height = 40;
                    dataGridView2.CellBorderStyle = DataGridViewCellBorderStyle.None;

                    dataGridView2.EnableHeadersVisualStyles = false;
                    dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
                    dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                    dataGridView2.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);

                    if (dataGridView2.Columns.Contains("NombreProducto"))
                        dataGridView2.Columns["NombreProducto"].Width = 260;

                    if (dataGridView2.Columns.Contains("Entrada"))
                        dataGridView2.Columns["Entrada"].Visible = false;

                    if (dataGridView2.Columns.Contains("Entradadet"))
                        dataGridView2.Columns["Entradadet"].Visible = false;

                    if (dataGridView2.Columns.Contains("Articulo"))
                        dataGridView2.Columns["Articulo"].Visible = false;

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

                // ==========================================
                // 2. CARGAR DATAGRIDVIEW3 (Detalle de Salidas asociadas a la Transacción)
                // ==========================================
                string sqlQuery2 = @"
                    SELECT   
                        d.salida AS Salida,   
                        idsalidadet AS Salidadet,   
                        d.qty AS Cantidad,
                        producto AS Articulo,   
                        s.clave AS ClaveProducto,
                        s.s_nombre AS NombreProducto,
                        precio AS Precio,   
                        (d.qty * precio) AS Importe
                    FROM transsalidadet as t
                    INNER JOIN salidadetinv as d ON t.salidadet = d.idsalidadet
                    LEFT JOIN servicios s ON d.producto = s.id_servicio
                    WHERE t.transaccion = @Idtransaccion";

                using (var command2 = connection.CreateCommand())
                {
                    command2.CommandText = sqlQuery2;
                    command2.Parameters.AddWithValue("@Idtransaccion", idTransaccionActual);

                    using var reader3 = await command2.ExecuteReaderAsync();
                    var dataTable3 = new System.Data.DataTable();
                    dataTable3.Load(reader3);

                    dataGridView3.AutoGenerateColumns = true;
                    dataGridView3.RowHeadersWidth = 20;
                    dataGridView3.DataSource = null;
                    dataGridView3.DataSource = dataTable3;

                    dataGridView3.RowTemplate.Height = 40;
                    dataGridView3.CellBorderStyle = DataGridViewCellBorderStyle.None;

                    dataGridView3.EnableHeadersVisualStyles = false;
                    dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
                    dataGridView3.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                    dataGridView3.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);

                    if (dataGridView3.Columns.Contains("NombreProducto"))
                        dataGridView3.Columns["NombreProducto"].Width = 260;

                    if (dataGridView3.Columns.Contains("Salida"))
                        dataGridView3.Columns["Salida"].Visible = false;

                    if (dataGridView3.Columns.Contains("Salidadet"))
                        dataGridView3.Columns["Salidadet"].Visible = false;

                    if (dataGridView3.Columns.Contains("Articulo"))
                        dataGridView3.Columns["Articulo"].Visible = false;

                    if (dataGridView3.Columns.Contains("Precio"))
                    {
                        dataGridView3.Columns["Precio"].DefaultCellStyle.Format = "C2";
                        dataGridView3.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView3.Columns["Precio"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }

                    if (dataGridView3.Columns.Contains("Importe"))
                    {
                        dataGridView3.Columns["Importe"].DefaultCellStyle.Format = "C2";
                        dataGridView3.Columns["Importe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView3.Columns["Importe"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }

                    dataGridView3.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el detalle de transacciones: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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