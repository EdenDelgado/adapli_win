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
    public partial class Frepsal : Form
    {
        private readonly ApplicationDbContext _context;
        private bool _isInitialized = false;

        public Frepsal(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));

            // Suscripción a eventos
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.CellMouseDoubleClick += dataGridView1_CellMouseDoubleClick;

            // Unificamos la carga en el evento Shown para asegurar que los controles estén listos
            this.Shown += Frepsal_Shown;
        }

        private async void Frepsal_Shown(object sender, EventArgs e)
        {
            try
            {
                // 1. Cargamos el catálogo de almacenes primero
                await CargarAlmacenesAsync();
                await CargarConceptosAsync();

                // 2. Cargamos el maestro de salidas
                await CargarMaestroDoctoscpAsync();

                _isInitialized = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar el reporte de salidas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void Frepsal_Load(object sender, EventArgs e)
        {
            // Vinculado correctamente al nombre del formulario
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

                string sqlQuery = @"
                    SELECT 
                        idsalida AS Idsalida, 
                        folio AS Folio, 
                        fecha AS Fecha,
                        cl.nombre AS nomcliente,
                        a.nombre AS Nomalmacen,
                        c.concepto AS Nomconcepto
                    FROM salidainv s 
                    LEFT JOIN almacen a ON s.almacen = a.id_almacen
                    LEFT JOIN concepto AS c ON s.concepto = c.id_conceptosprov
                    LEFT JOIN clientes AS cl ON s.cliente = cl.id_empros
                    WHERE fecha >= @desde AND fecha <= @hasta
                      AND (@almacen = 0 OR s.almacen = @almacen)
                      AND (@concepto = 0 OR s.concepto = @concepto)";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                DateTime fechaDesde = desdefechatx?.Value.Date ?? DateTime.Today;
                DateTime fechaHasta = hastafechatx?.Value.Date.AddDays(1).AddSeconds(-1) ?? DateTime.Today.AddDays(1);

                command.Parameters.Add("@desde", SqlDbType.DateTime).Value = fechaDesde;
                command.Parameters.Add("@hasta", SqlDbType.DateTime).Value = fechaHasta;

                // Manejo del parámetro de almacén (si no hay selección, pasa 0 para traer todos)
                int idAlmacenFiltro = 0;
                if (txalmacen?.SelectedValue != null && int.TryParse(txalmacen.SelectedValue.ToString(), out int valAlmacen))
                {
                    idAlmacenFiltro = valAlmacen;
                }
                command.Parameters.Add("@almacen", SqlDbType.Int).Value = idAlmacenFiltro;

                // Manejo del parámetro de concepto (si no hay selección, pasa 0 para traer todos)
                int idConceptoFiltro = 0;
                if (txconcepto?.SelectedValue != null && int.TryParse(txconcepto.SelectedValue.ToString(), out int valConcepto))
                {
                    idConceptoFiltro = valConcepto;
                }
                command.Parameters.Add("@concepto", SqlDbType.Int).Value = idConceptoFiltro;

                using var reader = await command.ExecuteReaderAsync();
                var dataTable = new DataTable();
                dataTable.Load(reader);

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dataTable;
                dataGridView1.Columns["NomConcepto"].Width = 200; dataGridView1.Columns["NomCliente"].Width = 200;
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
                MessageBox.Show($"Error al cargar las salidas de almacén: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    int idSalida = Convert.ToInt32(rowView["idsalida"]);

                    var formFsal = new Fsal(_context, idSalida);

                    if (formFsal.ShowDialog() == DialogResult.OK)
                    {
                        await CargarMaestroDoctoscpAsync();
                    }
                }
            }
        }

        private async void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (!_isInitialized) return;
            // Lógica adicional para el detalle si lo requieres en el futuro
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
                    txalmacen.SelectedIndex = -1; // Sin selección inicial (muestra todo)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los almacenes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarConceptosAsync()
        {
            try
            {
                if (_context == null) return;
                if (txconcepto == null) return;

                var listaConceptos = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
                    _context.Set<Sisvta.Domain.Entities.Concepto>()
                        .Select(c => new { id_concepto = c.IdConceptosProv, descripcion = c.DescripcionConcepto })
                );

                if (listaConceptos != null && listaConceptos.Count > 0)
                {
                    txconcepto.DataSource = null;
                    txconcepto.DisplayMember = "descripcion";
                    txconcepto.ValueMember = "id_concepto";
                    txconcepto.DataSource = listaConceptos;
                    txconcepto.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los conceptos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}