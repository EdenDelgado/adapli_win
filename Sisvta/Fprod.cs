using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;
using Sisvta.Infrastructure.Repositories;
using Sisvta.UseCases;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisvta
{
    public partial class Fprod : Form
    {
        private readonly GestionarProductosUseCase _useCase;

        private readonly ApplicationDbContext _context; // Añadimos el contexto para poder instanciar repositorios hijos
        private Fprodd _finv2;
        private Fcgru _fcgru;

        private Fclin _fclin;

        public Fprod(GestionarProductosUseCase useCase, ApplicationDbContext context)
        {
            InitializeComponent();
            _useCase = useCase;
            _context = context;

            // Inicializamos Finv2 inyectándole el mismo caso de uso
            _finv2 = new Fprodd(_useCase, _context);
            _finv2.ProductoModificado += async (s, e) => await CargarGridAsync();

            // Corregido: Usamos el nombre exacto 'RepositorioGrupoLinea' y le pasamos el _context
            _fcgru = new Fcgru(new GestionarGruposLineasUseCase(new GrupoLineaRepository(_context)));

            dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete;
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


        private async Task CargarGridAsync()
        {
            try
            {
                // En lugar de ObtenerTodosAsync() (que trae la entidad pura sin el JOIN de grupo),
                // usamos ObtenerFiltradosDtoAsync sin restricciones para obtener el DTO completo con el Grupo.
                var lista = await _useCase.ObtenerFiltradosDtoAsync(null, null, string.Empty);

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = lista.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al recargar la tabla: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void Fprod_Load(object sender, EventArgs e)
        {
            await CargarGridAsync();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.Columns["Pcompra"] != null)
            {
                dataGridView1.Columns["Pcompra"].HeaderText = "pcompra";
                dataGridView1.Columns["Pcompra"].Width = 60;
                dataGridView1.Columns["Pcompra"].DefaultCellStyle.Format = "N2";
                dataGridView1.Columns["Pcompra"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["Pventa"].DefaultCellStyle.Format = "N2";
                dataGridView1.Columns["Pventa"].Width = 60;
                dataGridView1.Columns["Pventa"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            dataGridView1.Columns["Nombre"].Width = 220;
            dataGridView1.Columns["Status"].Width = 40;
            dataGridView1.Columns["nomlinea"].Width = 140; dataGridView1.Columns["nomgrupo"].Width = 140;
            if (dataGridView1.Columns["Linea"] != null)
            {
                dataGridView1.Columns["Linea"].Visible = false;
            }
            if (dataGridView1.Columns["Grupo"] != null)
            {
                dataGridView1.Columns["Grupo"].Visible = false;
            }
            dataGridView1.Columns["descripcion"].Visible = false; dataGridView1.Columns["umedida"].Visible = false;
            dataGridView1.Columns["moneda"].Visible = false; dataGridView1.Columns["inventariable"].Visible = false;
            dataGridView1.Columns["maximo"].Visible = false; dataGridView1.Columns["minimo"].Visible = false; dataGridView1.Columns["reorden"].Visible = false;
            //dataGridView1.Columns["Clave"].Visible = false;
            if (!dataGridView1.Columns.Contains("ColMov"))
            {
                var colMov = new DataGridViewTextBoxColumn
                {
                    Name = "ColMov",
                    HeaderText = "mov",
                    ReadOnly = true,

                    //UseColumnTextForButtonValue = true, // Esto hace que dibuje el texto/icono en cada botón de la celda

                    Width = 50
                };
                dataGridView1.Columns.Add(colMov);
            }

            // --- 3. AGREGAR COLUMNA 'Borrar' CON ICONO DE BOTE DE BASURA (SI NO EXISTE YA) ---
            if (!dataGridView1.Columns.Contains("ColBorrar"))
            {
                var colBorrar = new DataGridViewButtonColumn
                {
                    Name = "ColBorrar",
                    HeaderText = "Borrar",
                    Text = "🗑️", // Puedes usar un emoji de bote de basura o texto alternativo
                    UseColumnTextForButtonValue = true, // Esto hace que dibuje el texto/icono en cada botón de la celda
                    Width = 55
                };
                dataGridView1.Columns.Add(colBorrar);
            }
            // Forzar que ColMov y ColBorrar se coloquen siempre al final del grid
            if (dataGridView1.Columns["ColMov"] != null)
            {
                dataGridView1.Columns["ColMov"].DisplayIndex = dataGridView1.Columns.Count - 2;
            }
            if (dataGridView1.Columns["ColBorrar"] != null)
            {
                dataGridView1.Columns["ColBorrar"].DisplayIndex = dataGridView1.Columns.Count - 1;
            }
        }

        // Método para abrir Finv2 y registrar un nuevo producto
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            AsegurarFinv2Vivo();
            if (_finv2.MdiParent == null && this.MdiParent != null)
            {
                _finv2.MdiParent = this.MdiParent;
            }
            // Limpiamos todos los campos para asegurar un registro completamente nuevo
            _finv2.clavetxt.Clear();
            _finv2.nombretxt.Clear();
            _finv2.descripciontxt.Clear();
            _finv2.inventariabletxt.Clear();
            _finv2.prodservtxt.Visible = false;
            _finv2.claveproservtxt.Clear();
            _finv2.statustxt.Clear();
            _finv2.preciotxt.Text = "0.00";
            _finv2.lineatxt.Clear();
            _finv2.monedatx.Clear();
            _finv2.precioptx.Text = "0.00";
            _finv2.ultimopreciotxt.Text = "0.00";
            _finv2.umedidatxt.Text = "";
            _finv2.almacentx.Text = "";
            _finv2.existx.Clear();
            _finv2.maximotx.Clear();
            _finv2.minimotx.Clear();
            _finv2.reordentx.Clear();
            _finv2.impuestotx.Clear();
            _finv2.tasatx.Clear();

            //_finv2.almacentx.Clear();
            // Habilitamos la clave para escritura
            _finv2.clavetxt.Enabled = true;

            _finv2.Show();
            _finv2.BringToFront();
        }

        // Al hacer doble clic en el grid, cargamos los datos en Finv2 para editarlos/verlos


        // Al hacer doble clic en el grid, cargamos los datos en Finv2 para editarlos/verlos




        // Botón de filtro actualizado para evaluar Línea, Status y el Prefijo de Clave (nomclatxt)


        private async void btfiltro_Click(object sender, EventArgs e)
        {
            try
            {
                string lineaFiltro = lineatxt.Text.Trim();
                int? statusFiltro = int.TryParse(statustxt.Text.Trim(), out int s) ? s : (int?)null;
                string clavePrefix = nomclatxt.Text.Trim();

                // Agregamos la lectura del grupo (asumiendo que tu textbox se llama grupotxt, 
                // si tiene otro nombre, reemplázalo aquí)
                string grupoFiltro = grupotxt.Text.Trim();

                // Pasamos el grupoFiltro al método del caso de uso
                var resultados = await _useCase.ObtenerFiltradosDtoAsync(lineaFiltro, statusFiltro, clavePrefix, grupoFiltro);

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = resultados.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar los productos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }





        private void btGrupos_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validamos que el contexto exista
                if (_context == null)
                {
                    MessageBox.Show("El contexto de la base de datos no está inicializado en Fprod.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2. Si el formulario no existe o ya fue cerrado (Disposed), lo recreamos igual que en el constructor
                if (_fcgru == null || _fcgru.IsDisposed)
                {
                    _fcgru = new Fcgru(new GestionarGruposLineasUseCase(new GrupoLineaRepository(_context)));
                }
                if (_fcgru.MdiParent == null && this.MdiParent != null)
                {
                    _fcgru.MdiParent = this.MdiParent;
                }
                // 3. Mostramos el formulario
                _fcgru.Show();
                _fcgru.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el formulario de grupos: {ex.Message}", "Excepción", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // private Fclin _fclin;

        private void btlinea_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Verificamos que el contexto exista para evitar el NullReferenceException
                if (_context == null)
                {
                    MessageBox.Show("El contexto de la base de datos no está inicializado en Fprod.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2. Si el formulario no existe o ya fue cerrado (Disposed), lo creamos de nuevo
                if (_fclin == null || _fclin.IsDisposed)
                {
                    var repositorio = new LineaArticuloRepository(_context);
                    var useCase = new GestionarLineasArticulosUseCase(repositorio);
                    _fclin = new Fclin(useCase, _context);
                }
                if (_fclin.MdiParent == null)  // && this.MdiParent != null)
                {
                    _fclin.MdiParent = this.MdiParent;
                }
                // 3. Mostramos el formulario
                _fclin.Show();
                _fclin.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el formulario de líneas: {ex.Message}", "Excepción", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {

        }




        private async void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Validar que se hizo clic en una fila válida (excluyendo encabezados)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                string claveSeleccionada = fila.Cells["Prodserv"].Value?.ToString();

                if (string.IsNullOrEmpty(claveSeleccionada)) return;

                // Obtenemos el nombre de la columna actual de forma segura
                string nombreColumna = dataGridView1.Columns[e.ColumnIndex].Name;

                // 1. Si hace doble clic en ColMov (Columna 9 o por nombre) -> Abre Fcmov
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
                // 2. Si hace doble clic en ColBorrar (Columna 10 o por nombre) -> Lógica de borrado
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
                            // await _useCase.EliminarAsync(claveSeleccionada);
                            // await CargarGridAsync(); // Recargamos el grid después de borrar
                            MessageBox.Show($"Producto '{claveSeleccionada}' borrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error al borrar el producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                // 3. Por defecto (Cualquier otra celda o columna 0) -> Abre _finv2 (Detalle / Edición)
                else
                {
                    dataGridView1.CurrentCell = fila.Cells[0];

                    AsegurarFinv2Vivo();

                    _finv2.clavetxt.Text = fila.Cells["Clave"].Value?.ToString();
                    _finv2.nombretxt.Text = fila.Cells["Nombre"].Value?.ToString();
                    _finv2.descripciontxt.Text = fila.Cells["Descripcion"].Value?.ToString();
                    _finv2.umedidatxt.Text = fila.Cells["umedida"].Value?.ToString();
                    _finv2.inventariabletxt.Text = fila.Cells["inventariable"].Value?.ToString();
                    _finv2.prodservtxt.Visible = true;
                    _finv2.prodservtxt.Text = fila.Cells["Prodserv"].Value?.ToString();
                    _finv2.claveproservtxt.Text = fila.Cells["ClaveProserv"].Value?.ToString();
                    _finv2.statustxt.Text = fila.Cells["Status"].Value?.ToString();
                    _finv2.maximotx.Text = fila.Cells["Maximo"].Value?.ToString();
                    _finv2.minimotx.Text = fila.Cells["Minimo"].Value?.ToString();
                    _finv2.reordentx.Text = fila.Cells["Reorden"].Value?.ToString();

                    if (fila.Cells["Pcompra"].Value != null && double.TryParse(fila.Cells["Pcompra"].Value.ToString(), out double preciocompra))
                    {
                        _finv2.preciotxt.Text = preciocompra.ToString("N2");
                    }
                    else
                    {
                        _finv2.preciotxt.Text = "0.00";
                    }

                    if (fila.Cells["Pventa"].Value != null && double.TryParse(fila.Cells["Pventa"].Value.ToString(), out double precioventa))
                    {
                        _finv2.precioptx.Text = precioventa.ToString("N2");
                    }
                    else
                    {
                        _finv2.precioptx.Text = "0.00";
                    }

                    if (_finv2.MdiParent == null && this.MdiParent != null)
                    {
                        _finv2.MdiParent = this.MdiParent;
                    }

                    _finv2.lineatxt.Text = fila.Cells["Linea"].Value?.ToString();
                    _finv2.monedatx.Text = fila.Cells["moneda"].Value?.ToString();

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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btregresar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}