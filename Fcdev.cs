using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Fcdev : Form
    {
        private readonly ApplicationDbContext _context;

        public Fcdev(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));

            // Suscripción a eventos
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.CellMouseDoubleClick += dataGridView1_CellMouseDoubleClick;

            // FORZAMOS LA CARGA AQUÍ MISMO PARA EVITAR DEPENDER DEL EVENTO LOAD
            _ = CargarMaestroDoctoscpAsync();
        }

<<<<<<< HEAD
        private async void Fcsal_Load(object sender, EventArgs e)
        {
            await CargarMaestroDoctoscpAsync();
=======
        private async void Fcdev_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
>>>>>>> main
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
<<<<<<< HEAD
                        d.id_Devolucion AS Iddevolucion, 
                        d.foliodevolucion AS Folio,d.id_VentaDcc AS VentaDcc, 
                        d.fecha AS fechadev,
                        i.fecha as fechamov,
                      d.id_Entrada AS Entrada,                      
                       d.id_NotaCredito as NotaCredito,
                        d.usercreator as Usuario, u.nombre as nomusuario, d.idempresa,d.idsucursal,
                    i.observaciones as Observacion
                    FROM devolucion as d left join inventario as i on d.id_Entrada=i.id_inventario
                        left join usuarios as u on d.usercreator=u.id_usuario
=======
                        d.id_Devolucion AS Iddevolucion,  
                        d.foliodevolucion AS Folio, d.fecha AS fechadev, d.id_VentaDcc AS Docto_Origen, 
                        d.id_Entrada AS Entrada,                      
                        d.id_NotaCredito as NotaCredito, IdingresoREP as Ingreso,i.concepto,i.observaciones as Observaciones,
                        d.usercreator as Usuario, u.nombre as USUARIO_CREADOR
                    FROM devolucion as d 
                    LEFT JOIN inventario as i ON d.id_Entrada = i.id_inventario
                    LEFT JOIN usuarios as u ON d.usercreator = u.id_usuario
>>>>>>> main
                    WHERE d.fecha >= @desde AND d.fecha <= @hasta";

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
<<<<<<< HEAD
=======

                // =========================================================================
                // OCULTAR COLUMNA TÉCNICA "Usuario"
                // =========================================================================
                if (dataGridView1.Columns.Contains("Usuario"))
                {
                    dataGridView1.Columns["Usuario"].Visible = false;
                }

>>>>>>> main
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las devoluciones de almacén: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

<<<<<<< HEAD
        // Evento que se dispara al hacer doble clic en una fila del DataGridView
        private async void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Validar que se haya hecho clic sobre una fila válida (y no en la cabecera)
            if (e.RowIndex >= 0)
            {
                // Como el DataSource es un DataTable, cada fila se mapea como DataRowView
=======
        // Evento que se dispara al hacer doble clic en una fila del DataGridView (Editar)
        private async void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
>>>>>>> main
                var rowView = dataGridView1.Rows[e.RowIndex].DataBoundItem as DataRowView;

                if (rowView != null)
                {
<<<<<<< HEAD
                    // Obtenemos el ID de la entrada (o los campos que necesites)
                    int idfactura = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Iddevolucion"].Value);

                    // Instanciamos el formulario Fent pasándole el ID (Asegúrate de que Fent tenga un constructor que reciba este int)
                    var formFdev = new Fdev(_context,idfactura);

                    if (formFdev.ShowDialog() == DialogResult.OK)
                    {
                        // Recargamos el grid si se realizaron cambios o guardados
=======
                    int idfactura = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Iddevolucion"].Value);

                    var formFdev = new Fdev(_context, idfactura);

                    if (formFdev.ShowDialog() == DialogResult.OK)
                    {
>>>>>>> main
                        await CargarMaestroDoctoscpAsync();
                    }
                }
            }
        }

<<<<<<< HEAD
        // Evento que se dispara al cambiar de fila en el DataGridView1 (Maestro)
=======
>>>>>>> main
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
<<<<<<< HEAD
=======

        private async void nuevobt_Click(object sender, EventArgs e)
        {
            try
            {
                // Abrir el formulario Fdev pasando el contexto y 0 para indicar que es un registro nuevo
                using (var formFdev = new Fdev(_context))
                {
                    if (formFdev.ShowDialog() == DialogResult.OK)
                    {
                        // Recargar el grid si se guardó correctamente el nuevo registro
                        await CargarMaestroDoctoscpAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el formulario de nueva devolución: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
>>>>>>> main
    }
}