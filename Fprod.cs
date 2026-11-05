using Microsoft.EntityFrameworkCore;
using Sisvta;
using Sisvta.Domain.Entities;
using Sisvta.Infrastructure.Persistence.Contexts;
using Sisvta.Infrastructure.Repositories;
using Sisvta.UseCases;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisvta
{
    public partial class Fprod : Form
    {
        private readonly GestionarProductosUseCase _useCase;
        private readonly ApplicationDbContext _context;
        private Fprodd _finv2;
        private Fcgru _fcgru;
        private Fclin _fclin;

        public Fprod(GestionarProductosUseCase useCase, ApplicationDbContext context)
        {
            InitializeComponent();
            _useCase = useCase;
            _context = context ?? throw new ArgumentNullException(nameof(context));

            // Inicializamos Finv2 inyectándole el mismo caso de uso
            _finv2 = new Fprodd(_useCase, _context);
            _finv2.ProductoModificado += async (s, e) => await CargarGridAsync();

            _fcgru = new Fcgru(new GestionarGruposLineasUseCase(new GrupoLineaRepository(_context)));

            dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete;

            // Vinculamos el evento Load para la carga inicial secuencial y segura
            this.Load += Fprod_Load_Secuencial;
        }

        private void Fprod_Load_Secuencial(object sender, EventArgs e)
        {
            try
            {
                // 1. Cargamos el ComboBox de estatus de forma independiente con ADO.NET
                CargarStatusServicioSincrono();

                // 2. Cargamos los datos del grid de forma segura
                CargarGridSincrono();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar la ventana: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================================
        // CARGA AISLADA CON ADO.NET PARA EVITAR CONFLICTOS CON EL DbContext
        // =========================================================================
        private void CargarStatusServicioSincrono()
        {
            try
            {
                if (_context == null) return;

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                connection.Open();

                string sqlQuery = "SELECT id_status_servicios AS IdStatusServicios, status AS Status FROM status_servicios ORDER BY id_status_servicios";

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                using var reader = command.ExecuteReader();
                var dataTable = new DataTable();
                dataTable.Load(reader);

                if (dataTable.Rows.Count > 0)
                {
                    statustxt.DataSource = null;
                    statustxt.DisplayMember = "Status";          // Campo visible
                    statustxt.ValueMember = "IdStatusServicios"; // Valor interno ID
                    statustxt.DataSource = dataTable;
                    statustxt.SelectedIndex = -1;                // Sin selección por defecto
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el catálogo de status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarGridSincrono()
        {
            try
            {
                var task = Task.Run(async () => await _useCase.ObtenerFiltradosDtoAsync(null, null, string.Empty));
                var lista = task.Result;

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = lista.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al recargar la tabla: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarGridAsync()
        {
            try
            {
                var lista = await _useCase.ObtenerFiltradosDtoAsync(null, null, string.Empty);

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = lista.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al recargar la tabla: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AsegurarFinv2Vivo()
        {
            if (_finv2 == null || _finv2.IsDisposed)
            {
                _finv2 = new Fprodd(_useCase, _context);
                _finv2.ProductoModificado += async (s, e) => await CargarGridAsync();
                if (this.MdiParent != null)
                {
                    _finv2.MdiParent = this.MdiParent;
                }
            }
        }

        private void Fprod_Load(object sender, EventArgs e)
        {
            // Evento reservado del diseñador
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.Columns["Pcompra"] != null)
            {
                dataGridView1.Columns["Pcompra"].HeaderText = "pcompra";
                dataGridView1.Columns["Pcompra"].Width = 60;
                dataGridView1.Columns["Pcompra"].DefaultCellStyle.Format = "N2";
                dataGridView1.Columns["Pcompra"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            dataGridView1.Columns["Nombre"].Width = 220;
            dataGridView1.Columns["Status"].Width = 40;
            dataGridView1.Columns["nomlinea"].Width = 140;
            dataGridView1.Columns["nomgrupo"].Width = 140;

            if (dataGridView1.Columns["Linea"] != null)
            {
                dataGridView1.Columns["Linea"].Visible = false;
            }
            if (dataGridView1.Columns["Grupo"] != null)
            {
                dataGridView1.Columns["Grupo"].Visible = false;
            }
            dataGridView1.Columns["descripcion"].Visible = false;
            dataGridView1.Columns["status"].Visible = false;
            dataGridView1.Columns["umedida"].Visible = false;
            dataGridView1.Columns["maximo"].Visible = false;
            dataGridView1.Columns["minimo"].Visible = false;
            dataGridView1.Columns["reorden"].Visible = false; dataGridView1.Columns["claveproserv"].Visible = false;



            // 1. Agregar la columna de Movimientos con un icono/emoji representativo
            if (!dataGridView1.Columns.Contains("ColMov"))
            {
                var colMov = new DataGridViewButtonColumn
                {
                    Name = "ColMov",
                    HeaderText = "Movs",
                    Text = "📊", // Icono o emoji para representar movimientos/reportes
                    UseColumnTextForButtonValue = true,
                    Width = 55,
                    FlatStyle = FlatStyle.Flat
                };
                dataGridView1.Columns.Add(colMov);
            }

            // Asegurar que quede colocado siempre antes de la columna de borrar (al final del grid)
            if (dataGridView1.Columns["ColMov"] != null)
            {
                dataGridView1.Columns["ColMov"].DisplayIndex = dataGridView1.Columns.Count - 2;
            }

            if (!dataGridView1.Columns.Contains("ColBorrar"))
            {
                var colBorrar = new DataGridViewButtonColumn
                {
                    Name = "ColBorrar",
                    HeaderText = "Borrar",
                    Text = "🗑️",
                    UseColumnTextForButtonValue = true,
                    Width = 55
                };
                dataGridView1.Columns.Add(colBorrar);
            }

            if (dataGridView1.Columns["ColMov"] != null)
            {
                dataGridView1.Columns["ColMov"].DisplayIndex = dataGridView1.Columns.Count - 2;
            }
            if (dataGridView1.Columns["ColBorrar"] != null)
            {
                dataGridView1.Columns["ColBorrar"].DisplayIndex = dataGridView1.Columns.Count - 1;
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            AsegurarFinv2Vivo();
            if (_finv2.MdiParent == null && this.MdiParent != null)
            {
                _finv2.MdiParent = this.MdiParent;
            }

            _finv2.clavetxt.Clear();
            _finv2.nombretxt.Clear();
            _finv2.descripciontxt.Clear();
            _finv2.inventariabletxt.Clear();
            _finv2.prodservtxt.Visible = false;
            _finv2.claveproservtxt.Clear();
            _finv2.preciotxt.Text = "0.00";
            _finv2.precioptx.Text = "0.00";
            _finv2.ultimopreciotxt.Text = "0.00";
            _finv2.umedidatxt.Text = "";
            _finv2.almacentx.Text = "";
            _finv2.existx.Clear();
            _finv2.maximotx.Clear();
            _finv2.minimotx.Clear();
            _finv2.reordentx.Clear();
            _finv2.tasatx.Clear();
            _finv2.clavetxt.Enabled = true;

            _finv2.Show();
            _finv2.BringToFront();
        }

        private async void btfiltro_Click(object sender, EventArgs e)
        {
            try
            {
                string lineaFiltro = lineatxt.Text.Trim();

                int? statusFiltro = null;
                if (statustxt is ComboBox combo && combo.SelectedValue != null)
                {
                    if (int.TryParse(combo.SelectedValue.ToString(), out int valStatus))
                    {
                        statusFiltro = valStatus;
                    }
                }
                else if (int.TryParse(statustxt.Text.Trim(), out int s))
                {
                    statusFiltro = s;
                }

                string clavePrefix = nomclatxt.Text.Trim();
                string grupoFiltro = grupotxt.Text.Trim();

                var resultados = await _useCase.ObtenerFiltradosDtoAsync(lineaFiltro, statusFiltro, clavePrefix, grupoFiltro);

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = resultados.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar los productos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void btGrupos_Click(object sender, EventArgs e)
        {
            try
            {
                if (_context == null)
                {
                    MessageBox.Show("El contexto de la base de datos no está inicializado en Fprod.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (_fcgru == null || _fcgru.IsDisposed)
                {
                    _fcgru = new Fcgru(new GestionarGruposLineasUseCase(new GrupoLineaRepository(_context)));
                }
                if (_fcgru.MdiParent == null && this.MdiParent != null)
                {
                    _fcgru.MdiParent = this.MdiParent;
                }
                _fcgru.Show();
                _fcgru.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el formulario de grupos: {ex.Message}", "Excepción", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btlinea_Click(object sender, EventArgs e)
        {
            try
            {
                if (_context == null)
                {
                    MessageBox.Show("El contexto de la base de datos no está inicializado en Fprod.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (_fclin == null || _fclin.IsDisposed)
                {
                    var repositorio = new LineaArticuloRepository(_context);
                    var useCase = new GestionarLineasArticulosUseCase(repositorio);
                    _fclin = new Fclin(useCase, _context);
                }
                if (_fclin.MdiParent == null)
                {
                    _fclin.MdiParent = this.MdiParent;
                }
                _fclin.Show();
                _fclin.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el formulario de líneas: {ex.Message}", "Excepción", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e) { }

        private async void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                string claveSeleccionada = fila.Cells["Prodserv"].Value?.ToString();

                if (string.IsNullOrEmpty(claveSeleccionada)) return;

                string nombreColumna = dataGridView1.Columns[e.ColumnIndex].Name;

                if (e.ColumnIndex == 13 || nombreColumna == "ColMov")
                {
                    var fcmovForm = new Fcmov(_context, claveSeleccionada);
                    if (fcmovForm.MdiParent == null && this.MdiParent != null)
                    {
                        fcmovForm.MdiParent = this.MdiParent;
                    }
                    fcmovForm.Show();
                    fcmovForm.BringToFront();
                }
                else if (e.ColumnIndex == 14 || nombreColumna == "ColBorrar")
                {
                    var confirmResult = MessageBox.Show($"¿Está seguro de que desea borrar el producto '{claveSeleccionada}'?",
                                                       "Confirmar Borrado",
                                                       MessageBoxButtons.YesNo,
                                                       MessageBoxIcon.Warning);
                    if (confirmResult == DialogResult.Yes)
                    {
                        try
                        {
                            MessageBox.Show($"Producto '{claveSeleccionada}' borrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error al borrar el producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    dataGridView1.CurrentCell = fila.Cells[0];

                    AsegurarFinv2Vivo();

                    _finv2.clavetxt.Text = fila.Cells["Clave"].Value?.ToString();
                    _finv2.nombretxt.Text = fila.Cells["Nombre"].Value?.ToString();
                    _finv2.descripciontxt.Text = fila.Cells["Descripcion"].Value?.ToString();
                    _finv2.umedidatxt.Text = fila.Cells["Nommedida"].Value?.ToString();
                    _finv2.inventariabletxt.Text = fila.Cells["inventariable"].Value?.ToString();
                    _finv2.prodservtxt.Visible = true;
                    _finv2.prodservtxt.Text = fila.Cells["Prodserv"].Value?.ToString();
                    _finv2.claveproservtxt.Text = fila.Cells["ClaveProserv"].Value?.ToString();
                    _finv2.statustxt.Text = fila.Cells["Nomstatus"].Value?.ToString();
                    _finv2.maximotx.Text = fila.Cells["Maximo"].Value?.ToString();
                    _finv2.minimotx.Text = fila.Cells["Minimo"].Value?.ToString();
                    _finv2.reordentx.Text = fila.Cells["Reorden"].Value?.ToString();
                    _finv2.lineatxt.Text = fila.Cells["Nomlinea"].Value?.ToString();

                    if (fila.Cells["Pcompra"].Value != null && double.TryParse(fila.Cells["Pcompra"].Value.ToString(), out double preciocompra))
                    {
                        _finv2.preciotxt.Text = preciocompra.ToString("N2");
                    }
                    else
                    {
                        _finv2.preciotxt.Text = "0.00";
                    }

                    if (_finv2.MdiParent == null && this.MdiParent != null)
                    {
                        _finv2.MdiParent = this.MdiParent;
                    }

                    _finv2.Show();
                    _finv2.BringToFront();

                    await _finv2.CargarDetalleServiciosAsync(claveSeleccionada);
                }
            }
        }

        private void nuevotx_Click(object sender, EventArgs e)
        {
            AsegurarFinv2Vivo();
            _finv2.Show();
            _finv2.BringToFront();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) { }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e) { }

        private void btregresar_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}