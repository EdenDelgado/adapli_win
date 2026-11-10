using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Fcinvfis : Form
    {
        private readonly ApplicationDbContext _context;

        public Fcinvfis(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));

            // Suscripción a eventos
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.CellMouseDoubleClick += dataGridView1_CellMouseDoubleClick;

            // FORZAMOS LA CARGA AQUÍ MISMO PARA EVITAR DEPENDER DEL EVENTO LOAD
            _ = CargarMaestroDoctoscpAsync();
        }

        private async void Fcinvfis_Load(object sender, EventArgs e)
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
                        id_invfisico AS Idinvfisico, 
                        fecha AS Fecha,
                        usuario AS Usuario, u.nombre as Nombre, u.idAlmacen as Almacen, a.nombre as Nomalmacen
                    FROM invfisico as i 
                    LEFT JOIN usuarios as u ON i.usuario = u.id_usuario
                    LEFT JOIN almacen as a ON u.idAlmacen = a.id_almacen
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

                if (dataGridView1.Columns["Usuario"] != null) dataGridView1.Columns["Usuario"].Visible = false;
                if (dataGridView1.Columns["Almacen"] != null) dataGridView1.Columns["Almacen"].Visible = false;

                // Agregamos y posicionamos la columna al final
                AgregarColumnaEditar();

                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los inventarios físicos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AgregarColumnaEditar()
        {
            // Verificamos si ya existe para evitar duplicarla
            if (!dataGridView1.Columns.Contains("ColEditar"))
            {
                DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
                btnColumn.Name = "ColEditar";
                btnColumn.HeaderText = "Editar";
                btnColumn.Text = "✏️"; // Figura del lápiz
                btnColumn.UseColumnTextForButtonValue = true;
                btnColumn.Width = 60;
                btnColumn.FlatStyle = FlatStyle.Flat;
                dataGridView1.Columns.Add(btnColumn);
            }

            // Forzamos que la columna quede siempre al final, sin importar las columnas de SQL
            if (dataGridView1.Columns.Contains("ColEditar"))
            {
                dataGridView1.Columns["ColEditar"].DisplayIndex = dataGridView1.Columns.Count - 1;
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
                    int idinvfisico = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Idinvfisico"].Value);

                    var formFinvfis = new Finvfis(_context, idinvfisico);

                    if (formFinvfis.ShowDialog() == DialogResult.OK)
                    {
                        await CargarMaestroDoctoscpAsync();
                    }
                }
            }
        }

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
            int idinvfisico = 0; // 0 indica nuevo registro

            var formFinvfis = new Finvfis(_context, idinvfisico);

            if (formFinvfis.ShowDialog() == DialogResult.OK)
            {
                await CargarMaestroDoctoscpAsync();
            }
        }
    }
}