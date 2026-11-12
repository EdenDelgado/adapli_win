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
    public partial class Frepexi : Form
    {
        private readonly ApplicationDbContext _context;
        private bool _isInitialized = false;

        public Frepexi(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));

            // Suscripción a eventos
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.CellMouseDoubleClick += dataGridView1_CellMouseDoubleClick;

            // Inicialización unificada en el evento Shown para evitar conflictos de concurrencia
            this.Shown += Frepexi_Shown;
        }

        private async void Frepexi_Shown(object sender, EventArgs e)
        {
            try
            {
                // 1. Primero cargamos los almacenes para poblar el ComboBox
                await CargarAlmacenesAsync();

                // 2. Después cargamos las existencias en el Grid
                await CargarMaestroDoctoscpAsync();

                _isInitialized = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar el reporte de existencias: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Frepexi_Load(object sender, EventArgs e)
        {
            // Vinculado correctamente al ciclo de vida del formulario
        }

        public async Task CargarMaestroDoctoscpAsync()
        {
            try
            {
                if (_context == null)
                {
                    MessageBox.Show("El contexto de base de datos es nulo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Consulta SQL adaptada para existencias actuales con filtro opcional de almacén
                string sqlQuery = @"   
                    SELECT 
                        s.id_servicio AS Idservicio, 
                        s.clave AS Clave,
                        s.s_nombre AS Nombre,
                        i.almacen AS IdAlmacen, 
                        a.nombre AS NomAlmacen, i.saldo AS ExistenciaActual,
                        s.preciocompra AS PrecioCompra, 0 as Ultimoprecio
                    FROM servicios s 
                    LEFT JOIN inventarioactual AS i ON s.id_servicio = i.articulo  
                    LEFT JOIN almacen AS a ON i.almacen = a.id_almacen
                    WHERE i.saldo > 0 
                      AND (@Almacen = 0 OR i.almacen = @Almacen)";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                // Manejo seguro del almacén seleccionado (si es -1 o nulo, enviamos 0 para mostrar todos)
                int idAlmacenFiltro = 0;
                if (txalmacen?.SelectedValue != null && int.TryParse(txalmacen.SelectedValue.ToString(), out int valAlmacen))
                {
                    idAlmacenFiltro = valAlmacen;
                }
                command.Parameters.Add("@Almacen", SqlDbType.Int).Value = idAlmacenFiltro;

                using var reader = await command.ExecuteReaderAsync();
                var dataTable = new DataTable();
                dataTable.Load(reader);

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dataTable;

                // Ocultamos únicamente las columnas internas que existen en el SELECT
                if (dataGridView1.Columns["IdAlmacen"] != null)
                    dataGridView1.Columns["IdAlmacen"].Visible = false;
                dataGridView1.Columns["IdServicio"].Visible = false;
                dataGridView1.Columns["ExistenciaActual"].DefaultCellStyle.Format = "N3";
                dataGridView1.Columns["ExistenciaActual"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["PrecioCompra"].DefaultCellStyle.Format = "N2";
                dataGridView1.Columns["PrecioCompra"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["Ultimoprecio"].DefaultCellStyle.Format = "N2";
                dataGridView1.Columns["Ultimoprecio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["Nombre"].Width = 200;

                // AGREGAR COLUMNA DE MOVIMIENTOS CON SÍMBOLO GRÁFICO (📊)
                // =========================================================================
                if (!dataGridView1.Columns.Contains("ColMov"))
                {
                    var colMov = new DataGridViewButtonColumn
                    {
                        Name = "ColMov",
                        HeaderText = "Movs",
                        Text = "📊",
                        UseColumnTextForButtonValue = true,
                        Width = 55,
                        FlatStyle = FlatStyle.Flat
                    };
                    dataGridView1.Columns.Add(colMov);
                }

                if (dataGridView1.Columns["ColMov"] != null)
                {
                    dataGridView1.Columns["ColMov"].DisplayIndex = dataGridView1.Columns.Count - 1;
                }

                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el reporte de existencias: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento que se dispara al hacer doble clic en una fila del DataGridView
        private async void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var rowView = dataGridView1.Rows[e.RowIndex].DataBoundItem as DataRowView;

                if (rowView != null)
                {
                    // Si necesitas abrir un detalle por servicio, asegúrate de pasar el ID correcto
                    int idServicio = Convert.ToInt32(rowView["Idservicio"]);

                    // Ejemplo si quisieras abrir movimientos de ese artículo específico:
                    var fcmovForm = new Fcmov(_context, idServicio.ToString());
                    if (fcmovForm.MdiParent == null && this.MdiParent != null)
                    {
                        fcmovForm.MdiParent = this.MdiParent;
                    }
                    fcmovForm.Show();
                    fcmovForm.BringToFront();
                }
            }
        }

        private async void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (!_isInitialized) return;
        }

        private void regresabt_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void Filtrarbt_Click(object sender, EventArgs e)
        {
            await CargarMaestroDoctoscpAsync();
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
                    txalmacen.SelectedIndex = -1; // Sin selección inicial por defecto
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los almacenes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}