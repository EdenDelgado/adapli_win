using Microsoft.EntityFrameworkCore;
using Sisvta;
using Sisvta.Domain.Entities;
using Sisvta.Infrastructure.Persistence.Contexts;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Sisvta
{
    public partial class Fcalm : Form
    {
        private readonly ApplicationDbContext _context;

        // Constructor con Inyección de Dependencias
        public Fcalm(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private void Fcalm_Load(object sender, EventArgs e)
        {
            CargarDatosAlmacen();
        }

        private void CargarDatosAlmacen()
        {
            try
            {
                // Realizamos un Join entre Almacen y Estado para mostrar el nombre del estado
                var listaAlmacenes = _context.Almacenes
                    .AsNoTracking()
                    .GroupJoin(
                        _context.Set<Estado>(),
                        almacen => (int)almacen.Estado,
                        estado => estado.IdEstado,
                        (almacen, estados) => new { Almacen = almacen, Estados = estados }
                    )
                    .SelectMany(
                        x => x.Estados.DefaultIfEmpty(),
                        (x, estado) => new
                        {
                            x.Almacen.IdAlmacen,
                            x.Almacen.Nombre,
                            x.Almacen.Calle,
                            x.Almacen.Ciudad,
                            // Mostramos el nombre del estado si existe, de lo contrario un texto por defecto
                            NombreEstado = estado != null ? estado.Nombre : "Sin Estado",
                            EstadoIdOriginal = x.Almacen.Estado, // Guardamos el ID por si se necesita internamente
                            x.Almacen.Telefono,
                            x.Almacen.Leyendatickt,
                            x.Almacen.Default
                        }
                    )
                    .ToList();

                dataGridView1.DataSource = listaAlmacenes;
                FormatearGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los almacenes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatearGrid()
        {
            if (dataGridView1.Columns["IdAlmacen"] != null)
                dataGridView1.Columns["IdAlmacen"].HeaderText = "ID";
            if (dataGridView1.Columns["Nombre"] != null)
                dataGridView1.Columns["Nombre"].HeaderText = "Nombre";
            if (dataGridView1.Columns["Calle"] != null)
                dataGridView1.Columns["Calle"].HeaderText = "Calle";
            if (dataGridView1.Columns["Ciudad"] != null)
                dataGridView1.Columns["Ciudad"].HeaderText = "Ciudad";
            if (dataGridView1.Columns["NombreEstado"] != null)
                dataGridView1.Columns["NombreEstado"].HeaderText = "Estado";

            // Ocultamos la columna del ID original del estado si no es necesaria a la vista
            if (dataGridView1.Columns["EstadoIdOriginal"] != null)
                dataGridView1.Columns["EstadoIdOriginal"].Visible = false;

            if (dataGridView1.Columns["Telefono"] != null)
                dataGridView1.Columns["Telefono"].HeaderText = "Teléfono";
            if (dataGridView1.Columns["Leyendatickt"] != null)
                dataGridView1.Columns["Leyendatickt"].HeaderText = "Leyenda Ticket";
            if (dataGridView1.Columns["Default"] != null)
                dataGridView1.Columns["Default"].HeaderText = "Predeterminado";
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarDatosAlmacen();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Validar que se haya hecho clic sobre una fila válida (y no en la cabecera)
            if (e.RowIndex >= 0)
            {
                var row = dataGridView1.Rows[e.RowIndex];

                if (row.Cells["IdAlmacen"].Value != null)
                {
                    int idalmacen = Convert.ToInt32(row.Cells["IdAlmacen"].Value);

                    // Buscamos directamente el almacén en la base de datos para pasarlo al formulario de edición con seguridad
                    var almacenSeleccionado = _context.Almacenes.Find(idalmacen);

                    if (almacenSeleccionado != null)
                    {
                        var formFalm = new Falm(_context, idalmacen);
                        formFalm.nombretx.Text = almacenSeleccionado.Nombre;
                        formFalm.calletx.Text = almacenSeleccionado.Calle;
                        formFalm.ciudadtx.Text = almacenSeleccionado.Ciudad;

                        // Asignamos el valor del estado para que el ComboBox en Falm lo seleccione correctamente
                        if (almacenSeleccionado.Estado > 0)
                            formFalm.estadotx.SelectedValue = Convert.ToInt32(almacenSeleccionado.Estado);

                        // Asignamos el valor del país para que el ComboBox en Falm lo seleccione correctamente
                        if (almacenSeleccionado.Pais > 0)
                            formFalm.paistx.SelectedValue = Convert.ToInt32(almacenSeleccionado.Pais);

                        formFalm.cptx.Text = almacenSeleccionado.cp;
                        formFalm.telefonotx.Text = almacenSeleccionado.Telefono;
                        formFalm.leyendatx.Text = almacenSeleccionado.Leyendatickt?.ToString();
                        formFalm.almdefaultx.Text = almacenSeleccionado.Default.ToString();

                        if (formFalm.ShowDialog() == DialogResult.OK)
                        {
                            // Recargar la tabla si se guardaron cambios
                            CargarDatosAlmacen();
                        }
                    }
                }
            }
        }

        private void btregresar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnuevo_Click(object sender, EventArgs e)
        {
            try
            {
                // Abrir el formulario Falm pasándole el contexto (id por defecto es 0 para un registro nuevo)
                using (var formFalm = new Falm(_context))
                {
                    if (formFalm.ShowDialog() == DialogResult.OK)
                    {
                        // Recargar el grid si se guardó correctamente el nuevo almacén
                        CargarDatosAlmacen();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el formulario de nuevo almacén: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}