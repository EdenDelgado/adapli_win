using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

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
            _context = context;
            _idServicioInicial = idServicioInicial;
            //prodservtx.Text = idServicioInicial;
            // ¡Probamos ejecutando la carga principal directamente aquí para forzarla!
            this.Shown += Fcmov_Shown;
        }

        private async void Fcmov_Shown(object sender, EventArgs e)
        {
            try
            {
               // MessageBox.Show("Entrando al evento Shown de Fcmov", "Debug");

                await CargarMaestromovtosAsync();

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
                    clavetx.Text = dataGridView1.CurrentRow.Cells["Clave"].Value?.ToString();
                    nombretx.Text = dataGridView1.CurrentRow.Cells["Nombre"].Value?.ToString();
                    if (!encontrado)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
                       
                    }
                }

                _isInitialized = true;
                await CargarmovtosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error crítico en Shown: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void Fcmov_Load(object sender, EventArgs e)
        {
            MessageBox.Show($"Fcmov initialized with idServicioInicial: [{_idServicioInicial ?? "null"}]", "Initialization Debug");

            await CargarMaestromovtosAsync();

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
            }

            _isInitialized = true;
            await CargarmovtosAsync();
        }

        public async Task CargarMaestromovtosAsync()
        {
            try
            {
                if (_context == null)
                {
                    // MessageBox.Show("El contexto de base de datos es nulo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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

                // Verificación de éxito en maestro
                // MessageBox.Show($"Total de servicios cargados: {dataTable.Rows.Count}", "Éxito Maestro");
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

                // Capturamos el valor actual de la celda
                var idmovtos = dataGridView1.CurrentRow.Cells["Idservicio"].Value;

                // --- LÍNEA DE DEPURACIÓN CRUCIAL ---
                // MessageBox.Show($"Valor capturado: [{idmovtos}] | Tipo: {idmovtos?.GetType().Name}", "Depuración Parámetro");

                string sqlQuery = @"
                    SELECT
                        d.articulo AS Producto, 
                        d.daterecord AS fecha, 
                        I.almacen AS Almacen,
                        d.movinventario AS Referencia, 
                        d.Cantidad AS entrada, 
                        0 AS salunid,
                        d.preciou AS costo,
                        (d.preciou * d.Cantidad) AS Importe
                    FROM inventario_detalles AS d
                    INNER JOIN INVENTARIO AS I ON d.movinventario = I.id_inventario
                    WHERE d.articulo = @Idservicio and d.daterecord>= @DesdeFecha and d.daterecord<= @HastaFecha 
                        and (I.almacen =@almacen or @almacen='') and ( @tipmov='Entradas' or @tipmov='Todos')

UNION ALL

                    SELECT
                        d.articulo AS Producto, 
                        d.daterecord AS fecha, 
                        I.almacen AS Almacen,
                        d.movinventario AS Referencia, 
                        0 AS entrada,
                        d.cantidad AS salunid, 
                        d.preciou AS costo,
                        (d.preciou * (-d.cantidad)) AS Importe
                    FROM inventario_detalles AS d
                    INNER JOIN transacciones AS t ON d.id_invdet = t.whentry
                    INNER JOIN INVENTARIO AS I ON d.movinventario = I.id_inventario
                    WHERE d.articulo = @Idservicio AND (t.locationto = 2 OR t.locationto = 3)and d.daterecord>= @DesdeFecha and d.daterecord<= @HastaFecha 
                        and (I.almacen =@almacen or @almacen='') and (@tipmov!='Entradas' or @tipmov='Todos')
                    ORDER BY fecha;";

                string connectionString = _context.Database.GetConnectionString();
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;
                command.Parameters.AddWithValue("@Idservicio", idmovtos);
                // <-- AQUÍ DEBES AGREGAR EL PARÁMETRO @DesdeFecha -->
                    command.Parameters.AddWithValue("@DesdeFecha", desdefecha.Value.Date);
                // <-- AQUÍ DEBES AGREGAR EL PARÁMETRO @HastaFecha -->
                command.Parameters.AddWithValue("@HastaFecha", hastafecha.Value.Date);
                command.Parameters.AddWithValue("@Almacen", almacentx.Text);
                command.Parameters.AddWithValue("@tipmov", Movtotx.Text);
                using var reader = await command.ExecuteReaderAsync();
                var dataTable = new DataTable();
                dataTable.Load(reader);

                dataGridView2.AutoGenerateColumns = true;
                dataGridView2.DataSource = dataTable;
                dataGridView2.Refresh();

                if (dataGridView2.Columns["Importe"] != null)
                {
                    dataGridView2.Columns["Importe"].DefaultCellStyle.Format = "N2";
                }
                if (dataGridView2.Columns["costo"] != null)
                {
                    dataGridView2.Columns["costo"].DefaultCellStyle.Format = "N2";
                }

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
    }
}