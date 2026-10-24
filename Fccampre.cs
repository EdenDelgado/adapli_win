using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Fccampre : Form
    {
        private readonly ApplicationDbContext _context;

        public Fccampre(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));

            // Suscripción de eventos
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.CellMouseDoubleClick += dataGridView1_CellMouseDoubleClick;
            Filtrarbt.Click += Filtrarbt_Click;

            // Carga inicial
            _ = CargarMaestroDoctoscpAsync();
        }

        private async void Fccampre_Load(object sender, EventArgs e)
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
                    SELECT id_servicio,clave,s_nombre,preciocompra ,p.precioventa,l.listaprecio                      
                    FROM servicios as s left join preciosarticulo as p on s.id_servicio=p.articulo
                    left join listaprecio as l on p.listaprecio=l.id_listaprecio;";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

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
                MessageBox.Show($"Error al cargar las listas de precios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var rowView = dataGridView1.Rows[e.RowIndex].DataBoundItem as DataRowView;
                if (rowView != null)
                {
                    // Lógica para abrir detalle al hacer doble clic
                }
            }
        }

        private async void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // Lógica al cambiar de selección
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