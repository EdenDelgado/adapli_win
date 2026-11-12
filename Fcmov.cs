using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Entities;
using Sisvta.Infrastructure.Persistence.Contexts;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisvta
{
    public partial class Fcmov : Form
    {
        private readonly ApplicationDbContext _context;
        private readonly string _idServicioInicial;
        private bool _isInitialized = false;

        public Fcmov(ApplicationDbContext context, string idServicioInicial = null)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idServicioInicial = idServicioInicial;

            // Unificamos la carga exclusivamente en el evento Shown para evitar duplicidad
            this.Shown += Fcmov_Shown;
        }

        private async void Fcmov_Shown(object sender, EventArgs e)
        {
            try
            {
                // 1. Cargamos primero los almacenes para el ComboBox
                await CargarAlmacenesAsync();

                // 2. Cargamos el maestro de servicios en el Grid superior
                await CargarMaestromovtosAsync();

                // 3. Posicionamos el cursor en el servicio inicial si fue provisto
                if (dataGridView1.Rows.Count > 0)
                {
                    bool encontrado = false;

                    if (!string.IsNullOrEmpty(_idServicioInicial))
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            string idServ = row.Cells["Idservicio"].Value?.ToString();
                            string clave = row.Cells["Clave"].Value?.ToString();

                            if (idServ == _idServicioInicial || clave == _idServicioInicial)
                            {
                                dataGridView1.CurrentCell = row.Cells[0];
                                encontrado = true;
                                break;
                            }
                        }
                    }

                    if (!encontrado)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
                    }

                    if (dataGridView1.CurrentRow != null)
                    {
                        clavetx.Text = dataGridView1.CurrentRow.Cells["Clave"].Value?.ToString();
                        nombretx.Text = dataGridView1.CurrentRow.Cells["Nombre"].Value?.ToString();
                    }
                }

                // Activamos el indicador para permitir las consultas del detalle
                _isInitialized = true;

                // 4. Cargamos los movimientos correspondientes al servicio seleccionado
                await CargarmovtosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error crítico al inicializar movimientos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Eliminamos Fcmov_Load para evitar doble ejecución con Fcmov_Shown

        public async Task CargarMaestromovtosAsync()
        {
            try
            {
                if (_context == null) return;

                string sqlQuery = @"
                    SELECT 
                        id_servicio AS Idservicio, 
                        s_nombre AS Nombre, 
                        descripcion AS Descripcion, 
                        clave AS Clave
                    FROM servicios 
                    ORDER BY id_servicio";

                string connectionString = _context.Database.GetConnectionString();

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                using var reader = await command.ExecuteReaderAsync();
                var dataTable = new DataTable();
                dataTable.Load(reader);

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = dataTable;
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el maestro servicios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarmovtosAsync()
        {
            try
            {
                if (!_isInitialized) return;

                if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Cells["Idservicio"].Value == null)
                {
                    dataGridView2.DataSource = null;
                    totaltx.Text = "Entradas: 0.00  |  Salidas: 0.00  |  Importe: 0.00";
                    return;
                }

                // Actualizamos las cajas de texto con la fila seleccionada actualmente
                clavetx.Text = dataGridView1.CurrentRow.Cells["Clave"].Value?.ToString();
                nombretx.Text = dataGridView1.CurrentRow.Cells["Nombre"].Value?.ToString();

                var idmovtos = dataGridView1.CurrentRow.Cells["Idservicio"].Value;

                string sqlQuery = @"
                    SELECT
                        d.articulo AS Producto, 
                        d.daterecord AS fecha, 
                        I.almacen AS Almacen, a.nombre AS AlmacenNombre,
                        d.movinventario AS Referencia, I.folio AS Folio,
                        d.Cantidad AS entrada, 
                        0 AS salunid,
                        d.preciou AS costo,
                        (d.preciou * d.Cantidad) AS Importe
                    FROM inventario_detalles AS d
                    INNER JOIN INVENTARIO AS I ON d.movinventario = I.id_inventario
                    LEFT JOIN almacen AS a ON I.almacen = a.id_almacen
                    WHERE d.articulo = @Idservicio 
                      AND d.daterecord >= @DesdeFecha 
                      AND d.daterecord <= @HastaFecha 
                      AND (I.almacen = @almacen OR @almacen = '') 
                      AND ( @tipmov = 'Entradas' OR @tipmov = 'Todos' )

                    UNION ALL

                    SELECT
                        d.producto AS Producto, 
                        d.daterecord AS fecha, 
                        I.almacen AS Almacen, a.nombre AS AlmacenNombre,
                        d.salida AS Referencia, I.folio AS Folio,
                        0 AS entrada,
                        d.qty AS salunid, 
                        d.precio AS costo,
                        (d.precio * (-d.qty)) AS Importe
                    FROM salidadetinv AS d
                    INNER JOIN salidainv AS I ON d.salida = I.idsalida
                    LEFT JOIN almacen AS a ON I.almacen = a.id_almacen
                    WHERE d.producto = @Idservicio 
                      AND d.daterecord >= @DesdeFecha 
                      AND d.daterecord <= @HastaFecha 
                      AND (I.almacen = @almacen OR @almacen = 0) 
                      AND (@tipmov != 'Entradas' OR @tipmov = 'Todos')
                    ORDER BY fecha;";

                string connectionString = _context.Database.GetConnectionString();
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;
                command.Parameters.AddWithValue("@Idservicio", idmovtos);

                // Validación por si los DateTimePickers no tienen valor asignado aún
                command.Parameters.AddWithValue("@DesdeFecha", desdefecha?.Value.Date ?? DateTime.Today);
                command.Parameters.AddWithValue("@HastaFecha", hastafecha?.Value.Date ?? DateTime.Today);

                // Manejo seguro del ComboBox de almacén
                object almacenVal = txalmacen?.SelectedValue ?? "";
                command.Parameters.AddWithValue("@Almacen", almacenVal == null || string.IsNullOrEmpty(almacenVal.ToString()) ? "" : almacenVal);

                command.Parameters.AddWithValue("@tipmov", string.IsNullOrEmpty(Movtotx?.Text) ? "Todos" : Movtotx.Text);

                using var reader = await command.ExecuteReaderAsync();
                var dataTable = new DataTable();
                dataTable.Load(reader);

                dataGridView2.AutoGenerateColumns = true;
                dataGridView2.DataSource = dataTable;

                if (dataGridView2.Columns["Producto"] != null) dataGridView2.Columns["Producto"].Visible = false;
                if (dataGridView2.Columns["Almacen"] != null) dataGridView2.Columns["Almacen"].Visible = false;
                if (dataGridView2.Columns["Referencia"] != null) dataGridView2.Columns["Referencia"].Visible = false;
                dataGridView2.Refresh();

                if (dataGridView2.Columns["Importe"] != null)
                {
                    dataGridView2.Columns["Importe"].DefaultCellStyle.Format = "N2";
                    dataGridView2.Columns["Importe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (dataGridView2.Columns["costo"] != null)
                {
                    dataGridView2.Columns["costo"].DefaultCellStyle.Format = "N2";
                    dataGridView2.Columns["costo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                dataGridView2.Columns["entrada"].DefaultCellStyle.Format = "N3";
                dataGridView2.Columns["entrada"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; 
                dataGridView2.Columns["salunid"].DefaultCellStyle.Format = "N3";
                dataGridView2.Columns["salunid"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                ActualizarTotalCosto();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en SQL de movimientos: {ex.Message}", "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (_isInitialized)
            {
                await CargarmovtosAsync();
            }
        }

        private void ActualizarTotalCosto()
        {
            decimal totalImporte = 0;
            decimal totalEntrada = 0;
            decimal totalSalunid = 0;

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.IsNewRow) continue;

                try
                {
                    if (row.Cells["Importe"].Value != null && row.Cells["Importe"].Value != DBNull.Value)
                        totalImporte += Convert.ToDecimal(row.Cells["Importe"].Value);

                    if (row.Cells["entrada"].Value != null && row.Cells["entrada"].Value != DBNull.Value)
                        totalEntrada += Convert.ToDecimal(row.Cells["entrada"].Value);

                    if (row.Cells["salunid"].Value != null && row.Cells["salunid"].Value != DBNull.Value)
                        totalSalunid += Convert.ToDecimal(row.Cells["salunid"].Value);
                }
                catch
                {
                    continue;
                }
            }

            totaltx.Text = $"Entradas: {totalEntrada:N2}  |  Salidas: {totalSalunid:N2}  |  Importe: {totalImporte:N2}";
        }

        private void btregresar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void Ejecuta_Click(object sender, EventArgs e)
        {
            await CargarmovtosAsync();
        }

        private async Task CargarAlmacenesAsync()
        {
            try
            {
                if (_context == null || txalmacen == null) return;

                var listaAlmacenes = await _context.Set<Sisvta.Domain.Entities.Almacen>()
                    .AsNoTracking()
                    .Select(a => new { id_almacen = a.IdAlmacen, nombre = a.Nombre })
                    .ToListAsync();

                if (listaAlmacenes != null && listaAlmacenes.Count > 0)
                {
                    txalmacen.DataSource = null;
                    txalmacen.DisplayMember = "nombre";
                    txalmacen.ValueMember = "id_almacen";
                    txalmacen.DataSource = listaAlmacenes;
                    txalmacen.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los almacenes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btregresar_Click_1(object sender, EventArgs e)
        {
            Close();
        }
    }
}