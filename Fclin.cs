using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sisvta.Infrastructure.Persistence.Contexts; // <- Namespace necesario para el DbContext
using Sisvta.UseCases;

namespace Sisvta
{
    public partial class Fclin : Form
    {
        private Flin? _flin;
        private readonly GestionarLineasArticulosUseCase _useCase;
        private readonly ApplicationDbContext _context; // <- Campo para el contexto

        // Constructor con inyección tanto del Caso de Uso como del DbContext
        public Fclin(GestionarLineasArticulosUseCase useCase, ApplicationDbContext context)
        {
            InitializeComponent();
            _useCase = useCase;
            _context = context; // <- Asignación del contexto recibido

            dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete;

            // Forzamos la carga de datos de manera inmediata al abrir el formulario
            _ = CargarGridAsync();
        }

        private async void Flin_Load(object sender, EventArgs e)
        {
            // Opcional si ya se llama en el constructor
        }

        public async Task CargarGridAsync()
        {
            try
            {
                var lista = await _useCase.ObtenerTodosAsync();
                var listaResultado = lista.ToList();

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = listaResultado;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las líneas de artículos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.Columns["IdLineasArticulos"] != null)
            {
                dataGridView1.Columns["IdLineasArticulos"].HeaderText = "ID Línea";
                dataGridView1.Columns["IdLineasArticulos"].Width = 50;
            }
            if (dataGridView1.Columns["Nombre"] != null)
            {
                dataGridView1.Columns["Nombre"].HeaderText = "Nombre Línea";
                dataGridView1.Columns["Nombre"].Width = 200;
            }
            if (dataGridView1.Columns["NombreGrupo"] != null)
            {
                dataGridView1.Columns["NombreGrupo"].HeaderText = "Grupo Línea";
                dataGridView1.Columns["NombreGrupo"].Width = 150;
            }
            if (dataGridView1.Columns["Utilidad"] != null)
            {
                dataGridView1.Columns["Utilidad"].HeaderText = "Utilidad";
                dataGridView1.Columns["Utilidad"].DefaultCellStyle.Format = "0.00 '%'";
                dataGridView1.Columns["Utilidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dataGridView1.Columns["PrcComision"] != null)
            {
                dataGridView1.Columns["PrcComision"].HeaderText = "% Comisión";
                dataGridView1.Columns["PrcComision"].DefaultCellStyle.Format = "0.00 '%'";
                dataGridView1.Columns["PrcComision"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["PrcComision"].Width = 50;
            }
            if (dataGridView1.Columns["DateRecord"] != null)
            {
                dataGridView1.Columns["DateRecord"].HeaderText = "Fecha Registro";
                dataGridView1.Columns["DateRecord"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
            }
        }

        private void AsegurarFlinVivo()
        {
            if (_flin == null || _flin.IsDisposed)
            {
                // <- Pasamos tanto el UseCase como el DbContext a Flin
                _flin = new Flin(_useCase, _context);

                // Suscribimos la recarga automática del grid cuando Flin guarde cambios
                _flin.LineaModificada += async (s, e) => await CargarGridAsync();

                if (this.MdiParent != null)
                {
                    _flin.MdiParent = this.MdiParent;
                }
            }
            else if (_flin.MdiParent == null && this.MdiParent != null)
            {
                _flin.MdiParent = this.MdiParent;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 1. Validar fila válida
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView1.Rows.Count)
            {
                return;
            }

            try
            {
                var fila = dataGridView1.Rows[e.RowIndex];

                // 2. Extraer los valores de las celdas de la fila seleccionada
                int IdSeleccionado = fila.Cells["IdLineasArticulos"].Value != null ? Convert.ToInt32(fila.Cells["IdLineasArticulos"].Value) : 0;
                string nombreSeleccionado = fila.Cells["Nombre"].Value?.ToString() ?? string.Empty;
                string nombreGrupoSeleccionado = fila.Cells["NombreGrupo"].Value?.ToString() ?? string.Empty;
                string idGrupoSeleccionado = fila.Cells["GrupoLinea"].Value?.ToString() ?? string.Empty;
                string comisionSeleccionada = fila.Cells["PrcComision"].Value?.ToString() ?? string.Empty;

                // 3. Asegurarnos de que _flin esté vivo y configurado como MDI
                AsegurarFlinVivo();

                // 4. Enviamos los datos recopilados al formulario Flin
                _flin.CargarDatos(IdSeleccionado, nombreSeleccionado, nombreGrupoSeleccionado, idGrupoSeleccionado, comisionSeleccionada);

                // Si estaba minimizado, lo restauramos
                if (_flin.WindowState == FormWindowState.Minimized)
                {
                    _flin.WindowState = FormWindowState.Normal;
                }

                _flin.Show();
                _flin.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al seleccionar el registro: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btarticulo_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btGrupos_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void nuevobt_Click(object sender, EventArgs e)
        {
            // Aseguramos que el formulario esté instanciado antes de usarlo
            AsegurarFlinVivo();

            // Limpiamos los campos para garantizar que sea un registro completamente nuevo
            _flin.CargarNombre(string.Empty);

            _flin.Show();
            _flin.BringToFront();
        }

        private void btregresar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}