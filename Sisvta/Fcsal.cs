using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Fcsal : Form
    {
        private readonly ApplicationDbContext _context;

        public Fcsal(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));

            // Suscripción a eventos
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.CellMouseDoubleClick += dataGridView1_CellMouseDoubleClick;

            // FORZAMOS LA CARGA AQUÍ MISMO PARA EVITAR DEPENDER DEL EVENTO LOAD
            _ = CargarMaestroDoctoscpAsync();
        }

        private async void Fcsal_Load(object sender, EventArgs e)
        {
            await CargarMaestroDoctoscpAsync();
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
                        s.concepto AS Concepto, c.concepto as Nomconcepto,
                        s.cliente AS Cliente, cl.nombre as Nomcliente, 
                        s.almacen AS IdAlmacen, a.nombre as Nomalmacen, s.aquien, s.observaciones, s.proyecto
                    FROM salidainv s 
                    LEFT JOIN concepto as c ON s.concepto = c.id_conceptosprov
                    LEFT JOIN Clientes as cl ON s.cliente = cl.id_empros
                    LEFT JOIN almacen as a ON s.almacen = a.id_almacen
                    WHERE fecha >= @desde AND fecha <= @hasta";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                DateTime fechaDesde = desdefechatx.Value.Date;
                DateTime fechaHasta = hastafechatx.Value.Date.AddDays(1).AddSeconds(-1);

                command.Parameters.Add("@desde", SqlDbType.DateTime).Value = fechaDesde;
                command.Parameters.Add("@hasta", SqlDbType.DateTime).Value = fechaHasta;

                using var reader = await command.ExecuteReaderAsync();
                var dataTable = new System.Data.DataTable();
                dataTable.Load(reader);

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dataTable;
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las salidas de almacén: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento que se dispara al hacer doble clic en una fila del DataGridView (Modo Edición)
        private async void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var rowView = dataGridView1.Rows[e.RowIndex].DataBoundItem as DataRowView;

                if (rowView != null)
                {
                    int idSalida = Convert.ToInt32(rowView["Idsalida"]);

                    // Instanciamos Fsal en modo edición pasándole el ID existente
                    var formFsal = new Fsal(_context, idSalida);

                    if (formFsal.ShowDialog() == DialogResult.OK)
                    {
                        await CargarMaestroDoctoscpAsync();
                    }
                }
            }
        }

        // Evento que se dispara al cambiar de fila en el DataGridView1 (Maestro)
        private async void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // await CargarDetalleDoctoscpAsync();
        }

        private void regresabt_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void Filtrarbt_Click(object sender, EventArgs e)
        {
            await CargarMaestroDoctoscpAsync();
        }

        private async void nuevobt_Click(object sender, EventArgs e)
        {
            try
            {
                // Instanciamos Fsal sin parámetros (o pasando 0) para indicar que es un NUEVO registro
                // Gracias a tu constructor estándar Fsal(ApplicationDbContext context), esto abrirá la forma limpia.
                using (var formFsal = new Fsal(_context))
                {
                    if (formFsal.ShowDialog() == DialogResult.OK)
                    {
                        // Recargamos el grid para mostrar la nueva salida de almacén guardada
                        await CargarMaestroDoctoscpAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el formulario de nueva salida: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}