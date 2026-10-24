using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisvta
{
    public partial class Frepinv : Form
    {
        private readonly ApplicationDbContext _context;

        public Frepinv(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));

            // Suscripción a eventos
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.CellMouseDoubleClick += dataGridView1_CellMouseDoubleClick;
            Filtrarbt.Click += Filtrarbt_Click;

            // FORZAMOS LA CARGA AQUÍ MISMO PARA EVITAR DEPENDER DEL EVENTO LOAD
            _ = CargarMaestroDoctoscpAsync();
        }

        private async void Frepinv_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
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
                        i.articulo AS Idarticulo, 
                        i.entradas AS Entradas,
                        i.salidas AS Salidas, 
                        i.saldo AS Saldo,
                        i.almacen AS Almacen,
                        s.s_nombre AS Nombre,
                        a.nombre AS NomAlmacen
                    FROM inventarioactual AS i 
                    LEFT JOIN servicios AS s ON i.articulo = s.id_servicio
                    LEFT JOIN almacen AS a ON i.almacen = a.id_almacen";

                // Opcional: Si deseas filtrar solo cuando se especifique un almacén
                string nombreAlmacen = txnomalmacen?.Text?.Trim() ?? string.Empty;
                bool filtrarPorAlmacen = !string.IsNullOrEmpty(nombreAlmacen);

                if (filtrarPorAlmacen)
                {
                    sqlQuery += " WHERE a.nombre = @NomAlmacen";
                }

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                if (filtrarPorAlmacen)
                {
                    command.Parameters.Add("@NomAlmacen", SqlDbType.VarChar).Value = nombreAlmacen;
                }

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
                MessageBox.Show($"Error al cargar el reporte de inventario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    // Lógica adicional para doble clic si se requiere consultar el detalle del artículo
                }
            }
        }

        private async void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // Lógica al cambiar de selección si aplica
        }

        private void regresabt_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void Filtrarbt_Click(object sender, EventArgs e)
        {
            await CargarMaestroDoctoscpAsync();
        }
    }
}