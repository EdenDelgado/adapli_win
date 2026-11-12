using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Frepcosv : Form
    {
        private readonly ApplicationDbContext _context;

        public Frepcosv(ApplicationDbContext context)
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
                       /* i.id_factura AS Idfactura, */
                        i.folio AS Folio,i.idsalida AS IdSalida, 
                        i.fecha AS Fecha, i.almacen AS IdAlmacen, a.nombre as nomalmacen,
                        i.cliente AS Cliente, cl.nombre as Nomcliente,
                      i.concepto AS Concepto,c.concepto as Nomconcepto,
                        sum(d.qty*d.precio) as Total
                       /* , i.descripcion AS Descripcion */
                       
                    FROM salidainv as i 
                    left join salidadetinv as d on i.idsalida=d.salida
                    left join almacen as a on i.almacen=a.id_almacen
                    left join concepto as c on i.concepto=c.id_conceptosprov
                    left join clientes as cl on i.cliente=cl.id_empros
                    
                    WHERE i.fecha >= @desde AND i.fecha <= @hasta
                    GROUP BY
                    i.idsalida,
                     i.folio, 
                    i.fecha, 
                     i.almacen, 
                     a.nombre, 
                     i.cliente, 
                     cl.nombre, 
                 i.concepto, 
                 c.concepto";
              

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
                //dataGridView1.Columns["Idfactura"].Visible = false;
                dataGridView1.Columns["IdSalida"].Visible = false; dataGridView1.Columns["Concepto"].Visible = false;
               // dataGridView1.Columns["iprov"].Visible = false;
                dataGridView1.Columns["cliente"].Visible = false; dataGridView1.Columns["idalmacen"].Visible = false;
                if (dataGridView1.Columns["Nombre"] != null)  dataGridView1.Columns["Nombre"].Width = 200;
            
                dataGridView1.Columns["Total"].DefaultCellStyle.Format = "N2";
                dataGridView1.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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
                    //int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["IdInventario"].Value);
                    var Id = rowView["IdSalida"];

                    // Instanciamos el formulario Fent pasándole el ID (Asegúrate de que Fent tenga un constructor que reciba este int)
                    if (Id != null && Id != DBNull.Value && int.TryParse(Id.ToString(), out int idinventario))
                    {
                        var fsal = new Fsal(_context, idinventario);
                        fsal.btGuardar.Visible = false; // Oculta el botón "Nuevo" en Fsal
                        fsal.btagregar.Visible = false; // Oculta el botón "Agregar" en Fsal
                        fsal.btborrar.Visible = false; // Oculta el botón "Borrar" en Fsal
                        if (fsal.ShowDialog() == DialogResult.OK)
                        {
                            // Recargamos el grid si se realizaron cambios o guardados
                            await CargarMaestroDoctoscpAsync();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se pudo obtener el ID de la entrada seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("No se pudo obtener la fila seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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