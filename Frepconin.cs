using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Frepconin : Form
    {
        private readonly ApplicationDbContext _context;

        public Frepconin(ApplicationDbContext context)
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
                        d.id_factura AS Idfactura, 
                        d.folio AS Folio,i.id_inventario AS IdInventario, 
                        d.fechafactura AS Fecha,
                      i.concepto AS Concepto,c.concepto as Nomconcepto,
                        d.proveedor AS Proveedor, i.proveedor as iprov, p.nombre as Nomproveedor,
                        i.descripcion AS Descripcion,
                        d.almacen AS IdAlmacen, a.nombre as nomalmacen
                    FROM doctoscc as d left join inventario as i on d.id_factura = i.id_factura
                    left join almacen as a on d.almacen=a.id_almacen
                    left join concepto as c on i.concepto=c.id_conceptosprov
                    left join proveedores as p on d.proveedor=p.id_proveedor
                    WHERE d.fechafactura >= @desde AND d.fechafactura <= @hasta";

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
                dataGridView1.Columns["Idfactura"].Visible = false;
                dataGridView1.Columns["IdInventario"].Visible = false; dataGridView1.Columns["Concepto"].Visible = false;
                dataGridView1.Columns["iprov"].Visible = false;
                dataGridView1.Columns["proveedor"].Visible = false; dataGridView1.Columns["idalmacen"].Visible = false;
              

                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las entradas de almacén: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento que se dispara al hacer doble clic en una fila del DataGridView
        private async void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Validar que se haya hecho clic sobre una fila válida (y no en la cabecera)
            if (e.RowIndex >= 0)
            {
                // Como el DataSource es un DataTable, cada fila se mapea como DataRowView
                var rowView = dataGridView1.Rows[e.RowIndex].DataBoundItem as DataRowView;

                if (rowView != null)
                {
                    // Obtenemos el ID de la entrada (o los campos que necesites)
                    int idfactura = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Idfactura"].Value);

                    // Instanciamos el formulario Fent pasándole el ID (Asegúrate de que Fent tenga un constructor que reciba este int)
                    var formFent = new Fent(_context,idfactura);

                    if (formFent.ShowDialog() == DialogResult.OK)
                    {
                        // Recargamos el grid si se realizaron cambios o guardados
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
    }
}