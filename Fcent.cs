using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Fcent : Form
    {
        private readonly ApplicationDbContext _context;

        public Fcent(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));

            // Suscripción a eventos
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.CellClick += dataGridView1_CellClick; // Evento para manejar el clic en el botón de edición

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
                        i.id_factura AS Idfactura, 
                        i.folio AS Folio, 
                        i.id_inventario AS IdInventario, 
                        i.fecha AS Fecha,
                        i.concepto AS Concepto, c.concepto as Nomconcepto,
                        d.proveedor AS Proveedor, p.nombre as NomProveedor, 
                        i.descripcion AS Descripcion,
                        i.almacen AS IdAlmacen, a.nombre as NomAlmacen
                    FROM inventario as i 
                    LEFT JOIN doctoscc as d ON i.id_factura = d.id_factura
                    LEFT JOIN proveedores as p ON d.proveedor = p.id_proveedor
                    LEFT JOIN almacen as a ON i.almacen = a.id_almacen
                    LEFT JOIN concepto as c ON i.concepto = c.id_conceptosprov
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

                // Ocultar columnas técnicas
                if (dataGridView1.Columns["Concepto"] != null) dataGridView1.Columns["Concepto"].Visible = false;
                if (dataGridView1.Columns["Idalmacen"] != null) dataGridView1.Columns["Idalmacen"].Visible = false;
                if (dataGridView1.Columns["Proveedor"] != null) dataGridView1.Columns["Proveedor"].Visible = false;

                // AGREGAR O ASEGURAR LA COLUMNA DE EDICIÓN CON EL LÁPIZ AL FINAL
                AgregarColumnaEditar();

                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las entradas de almacén: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AgregarColumnaEditar()
        {
            // Evitamos duplicar la columna si ya existe al recargar el grid
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
            if (dataGridView1.Columns.Contains("ColEditar"))
            {
                dataGridView1.Columns["ColEditar"].DisplayIndex = dataGridView1.Columns.Count - 1;
            }
        }

        // Evento que se dispara al hacer clic en cualquier celda, incluyendo nuestra columna de edición del lápiz
        private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Validar que se hizo clic en la columna "ColEditar" y en una fila válida
            if (e.ColumnIndex == dataGridView1.Columns["ColEditar"]?.Index && e.RowIndex >= 0)
            {
                await AbrirEdicionFila(e.RowIndex);
            }
        }

        // Evento que se mantenía opcional para doble clic en la fila
        private async void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                await AbrirEdicionFila(e.RowIndex);
            }
        }

        private async Task AbrirEdicionFila(int rowIndex)
        {
            var rowView = dataGridView1.Rows[rowIndex].DataBoundItem as DataRowView;

            if (rowView != null)
            {
                var valorId = rowView["IdInventario"];

                if (valorId != null && valorId != DBNull.Value && int.TryParse(valorId.ToString(), out int idinventario))
                {
                    var formFent = new Fent(_context, idinventario);

                    if (formFent.ShowDialog() == DialogResult.OK)
                    {
                        await CargarMaestroDoctoscpAsync();
                    }
                }
                else
                {
                    MessageBox.Show("La fila seleccionada no contiene un ID de inventario válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            int idinventario = 999999;
            try
            {
                using (var fent = new Fent(_context, idinventario))
                {
                    if (fent.ShowDialog() == DialogResult.OK)
                    {
                        await CargarMaestroDoctoscpAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el formulario de nueva entrada: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}