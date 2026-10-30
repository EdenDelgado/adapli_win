using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Fctra : Form
    {
        private readonly ApplicationDbContext _context;

        public Fctra(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));

            // Suscripción a eventos
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.CellMouseDoubleClick += dataGridView1_CellMouseDoubleClick;

            // FORZAMOS LA CARGA AQUÍ MISMO PARA EVITAR DEPENDER DEL EVENTO LOAD
            _ = CargarMaestroDoctoscpAsync();
        }

        // CORREGIDO: El nombre del Load debe coincidir con el nombre de la clase (Fctra_Load)
        private async void Fctra_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
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
                        id_traspaso AS Idtraspaso, 
                        almacen_origen AS AlmacenOrigen, a.nombre as NomOrigen, 
                        almacen_destino AS AlmacenDestino, d.nombre as NomDestino,
                        fecha AS Fecha,
                        t.concepto AS Concepto, c.concepto as Nomconcepto,
                        entrada as Entrada                       
                    FROM traspaso t 
                    LEFT JOIN almacen a ON almacen_origen = a.id_almacen
                    LEFT JOIN almacen d ON almacen_destino = d.id_almacen
                    LEFT JOIN concepto c ON t.concepto = c.id_conceptosprov
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
                MessageBox.Show($"Error al cargar los traspasos de almacén: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    int idTraspaso = Convert.ToInt32(rowView["Idtraspaso"]);

                    using (var formFtra = new Ftra(_context, idTraspaso))
                    {
                        if (formFtra.ShowDialog() == DialogResult.OK)
                        {
                            await CargarMaestroDoctoscpAsync();
                        }
                    }
                }
            }
        }

        private async void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // Lógica para detalle si aplica
        }

        private void regresabt_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void Filtrarbt_Click(object sender, EventArgs e)
        {
            await CargarMaestroDoctoscpAsync();
        }

        // CORREGIDO: Implementación para dar de alta un nuevo traspaso pasando ID 0
        private async void nuevobt_Click(object sender, EventArgs e)
        {
            try
            {
                int idTraspasoNuevo = 0; // 0 indica nuevo registro

                using (var formFtra = new Ftra(_context, idTraspasoNuevo))
                {
                    if (formFtra.ShowDialog() == DialogResult.OK)
                    {
                        await CargarMaestroDoctoscpAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el formulario de nuevo traspaso: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}