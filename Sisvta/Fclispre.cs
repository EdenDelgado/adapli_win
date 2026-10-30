using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Entities;
using Sisvta.Infrastructure.Persistence.Contexts;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisvta
{
    public partial class Fclispre : Form
    {
        private readonly ApplicationDbContext _context;

        public Fclispre(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));

            // Suscripción a eventos
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.CellMouseDoubleClick += dataGridView1_CellMouseDoubleClick;
            dataGridView1.CellClick += dataGridView1_CellClick; // <-- Evento para capturar el clic en la columna de edición
            dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete; // <-- Para asegurar la columna de imagen

            // FORZAMOS LA CARGA AQUÍ MISMO PARA EVITAR DEPENDER DEL EVENTO LOAD
            _ = CargarMaestroDoctoscpAsync();
        }

        private async void Fcsal_Load(object sender, EventArgs e)
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
                    SELECT id_listaprecio, listaprecio, condicion, porcentaje, pdv, id_empresa as empresa                       
                    FROM listaprecio;";

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

        // Método auxiliar para abrir el formulario de edición de manera centralizada
        private async void AbrirEdicionFila(DataGridViewRow fila)
        {
            try
            {
                string nombreColumnaId = "id_listaprecio";

                if (dataGridView1.Columns.Contains(nombreColumnaId) &&
                    fila.Cells[nombreColumnaId].Value != DBNull.Value)
                {
                    int idListaPrecio = Convert.ToInt32(fila.Cells[nombreColumnaId].Value);

                    string listaprecio = fila.Cells["listaprecio"].Value?.ToString() ?? string.Empty;
                    string condicion = fila.Cells["condicion"].Value?.ToString() ?? string.Empty;
                    string porcentaje = fila.Cells["porcentaje"].Value?.ToString() ?? string.Empty;
                    string pdv = fila.Cells["pdv"].Value?.ToString() ?? string.Empty;
                    string empresa = fila.Cells["empresa"].Value?.ToString() ?? string.Empty;

                    using (var formDetalle = new Flispre(_context, listaprecio, condicion, porcentaje, pdv, empresa, idListaPrecio))
                    {
                        if (formDetalle.ShowDialog() == DialogResult.OK)
                        {
                            await CargarMaestroDoctoscpAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el detalle de la lista de precios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento que se dispara al hacer doble clic en una fila
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.CurrentRow != null)
            {
                AbrirEdicionFila(dataGridView1.CurrentRow);
            }
        }

        // Evento para capturar el clic sobre la columna de imagen/icono de edición
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificamos que se haya hecho clic en la última columna (la de edición) y en una fila válida
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns.Count - 1)
            {
                DataGridViewRow filaSeleccionada = dataGridView1.Rows[e.RowIndex];
                AbrirEdicionFila(filaSeleccionada);
            }
        }

        // Agregamos la columna de imagen automáticamente cada vez que el grid enlaza sus datos
 
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            string nombreColumnaBtn = "ColEditar";

            // Si la columna ya existe, evitamos duplicarla
            if (!dataGridView1.Columns.Contains(nombreColumnaBtn))
            {
                // Cambiamos a ButtonColumn para usar texto/emojis sin necesidad de imágenes
                var btnColumna = new DataGridViewButtonColumn();
                btnColumna.Name = nombreColumnaBtn;
                btnColumna.HeaderText = "Editar";
                btnColumna.Text = "✏️";
                btnColumna.UseColumnTextForButtonValue = true; // Muestra el texto/emoji en cada renglón

                dataGridView1.Columns.Add(btnColumna);
            }

            // Ajustar el ancho de la columna de edición
            if (dataGridView1.Columns.Contains(nombreColumnaBtn))
            {
                dataGridView1.Columns[nombreColumnaBtn].Width = 60;
                dataGridView1.Columns[nombreColumnaBtn].DisplayIndex = dataGridView1.Columns.Count - 1;
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

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private async void nuevobt_Click(object sender, EventArgs e)
        {
            using (var formDetalle = new Flispre(_context))
            {
                if (formDetalle.ShowDialog() == DialogResult.OK)
                {
                    await CargarMaestroDoctoscpAsync();
                }
            }
        }
    }
}