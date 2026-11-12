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
                        i.factura AS Idfactura, 
                        i.folio AS Folio, i.id_inventario AS IdInventario, 
                        i.fecha AS Fecha,
                        i.concepto AS Concepto, c.concepto as Nomconcepto,
                        i.proveedor AS Proveedor, 
                        i.descripcion AS Descripcion,
                        i.almacen AS IdAlmacen, a.nombre as nomalmacen, sum(ia.entradas) as Entradas,
                        sum(ia.entradas*id.preciou) as TotalEntradas, sum(ia.salidas*id.preciou) as TotalSalidas, sum(id.preciou*ia.saldo) as Saldo
                    FROM inventario as i 
                    left join inventario_detalles id on i.id_inventario=id.movinventario
                    left join almacen as a on i.almacen=a.id_almacen
                    left join concepto as c on i.concepto=c.id_conceptosprov
                    left join proveedores as p on i.proveedor=p.id_proveedor
                    left join inventarioactual as ia on id.articulo=ia.articulo and i.almacen=ia.almacen
                    WHERE i.fecha >= @desde AND i.fecha <= @hasta
                    group by i.factura, i.folio, i.id_inventario, i.fecha, i.concepto, c.concepto, i.proveedor, p.nombre, i.descripcion, i.almacen, a.nombre, ia.entradas, ia.salidas";

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

                // Ocultar columnas internas
                if (dataGridView1.Columns["Idfactura"] != null) dataGridView1.Columns["Idfactura"].Visible = false;
                if (dataGridView1.Columns["IdInventario"] != null) dataGridView1.Columns["IdInventario"].Visible = false;
                if (dataGridView1.Columns["Concepto"] != null) dataGridView1.Columns["Concepto"].Visible = false;
                if (dataGridView1.Columns["proveedor"] != null) dataGridView1.Columns["proveedor"].Visible = false;
                if (dataGridView1.Columns["idalmacen"] != null) dataGridView1.Columns["idalmacen"].Visible = false;

                // Ajustes de formato y ancho
                if (dataGridView1.Columns["Folio"] != null) dataGridView1.Columns["Folio"].Width = 40;

                if (dataGridView1.Columns["TotalEntradas"] != null)
                {
                    dataGridView1.Columns["TotalEntradas"].DefaultCellStyle.Format = "N2";
                    dataGridView1.Columns["TotalEntradas"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (dataGridView1.Columns["Totalsalidas"] != null)
                {
                    dataGridView1.Columns["Totalsalidas"].DefaultCellStyle.Format = "N2";
                    dataGridView1.Columns["Totalsalidas"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (dataGridView1.Columns["saldo"] != null)
                {
                    dataGridView1.Columns["saldo"].DefaultCellStyle.Format = "N2";
                    dataGridView1.Columns["saldo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                // =========================================================================
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

        // Evento que se dispara al hacer doble clic en una fila o columna del DataGridView
        private async void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                string nombreColumna = dataGridView1.Columns[e.ColumnIndex].Name;

                // Validamos si se hizo doble clic sobre la columna gráfica de movimientos
                if (nombreColumna == "ColMov")
                {
                    var idInventarioVal = fila.Cells["IdInventario"].Value;
                    if (idInventarioVal != null && idInventarioVal != DBNull.Value)
                    {
                        string idInventarioStr = idInventarioVal.ToString();

                        // Ejemplo: Abrir formulario Fcmov pasando el ID o la referencia correspondiente
                        var fcmov = new Fcmov(_context, idInventarioStr);
                        if (fcmov.MdiParent == null && this.MdiParent != null)
                        {
                            fcmov.MdiParent = this.MdiParent;
                        }
                        fcmov.Show();
                        fcmov.BringToFront();
                    }
                    return;
                }

                // Lógica habitual de apertura de la entrada de almacén (Fent)
                var rowView = fila.DataBoundItem as DataRowView;
                if (rowView != null)
                {
                    var Id = rowView["IdInventario"];

                    if (Id != null && Id != DBNull.Value && int.TryParse(Id.ToString(), out int idinventario))
                    {
                        var fent = new Fent(_context, idinventario);
                        fent.btagregar.Visible = false;
                        fent.btGuardar.Visible = false;
                        fent.btborrar.Visible = false;

                        if (fent.ShowDialog() == DialogResult.OK)
                        {
                            await CargarMaestroDoctoscpAsync();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se pudo obtener el ID de la entrada seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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