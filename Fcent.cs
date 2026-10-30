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
                        i.id_factura AS Idfactura, 
                        i.folio AS Folio,i.id_inventario AS IdInventario, 
                        i.fecha AS Fecha,
                      i.concepto AS Concepto,c.concepto as Nomconcepto,
                        d.proveedor AS Proveedor, p.nombre as NomProveedor, 
                        i.descripcion AS Descripcion,
                        i.almacen AS IdAlmacen,a.nombre as NomAlmacen
                    FROM inventario as i left join doctoscc as d on i.id_factura=d.id_factura
                    left join proveedores as p on d.proveedor=p.id_proveedor
                    left join almacen as a on i.almacen=a.id_almacen
                    left join concepto as c on i.concepto=c.id_conceptosprov
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
                MessageBox.Show($"Error al cargar las entradas de almacén: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento que se dispara al hacer doble clic en una fila del DataGridView


        private async void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Validar que se haya hecho clic sobre una fila válida (y no en la cabecera)
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                var rowView = dataGridView1.Rows[e.RowIndex].DataBoundItem as DataRowView;

                if (rowView != null)
                {
                    // Leemos el valor de forma segura utilizando la columna del DataRowView
                    var valorId = rowView["IdInventario"];

                    // Verificamos que no sea nulo antes de convertirlo a entero
                    if (valorId != null && valorId != DBNull.Value && int.TryParse(valorId.ToString(), out int idinventario))
                    {
                        // Instanciamos el formulario Fent pasándole el ID de forma segura
                        var formFent = new Fent(_context, idinventario);

                        if (formFent.ShowDialog() == DialogResult.OK)
                        {
                            // Recargamos el grid si se realizaron cambios o guardados
                            await CargarMaestroDoctoscpAsync();
                        }
                    }
                    else
                    {
                        MessageBox.Show("La fila seleccionada no contiene un ID de factura válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            int idinventario = 999999; // debe ser el siguiente numero del ultimo id_inventario en la tabla inventario
            try
            {
                // Instanciamos Fent usando el constructor estándar (sin ID), 
                // lo que preparará el formulario en blanco para una nueva entrada.
                using (var fent = new Fent(_context,idinventario))
                {
                    // Abrimos como cuadro de diálogo modal
                    if (fent.ShowDialog() == DialogResult.OK)
                    {
                        // Si el usuario guarda el nuevo registro, recargamos el catálogo maestro
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