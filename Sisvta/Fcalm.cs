using Microsoft.EntityFrameworkCore;
using sisvta;
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
                var listaAlmacenes = _context.Almacenes
                    .AsNoTracking()
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
            if (dataGridView1.Columns["Estado"] != null)
                dataGridView1.Columns["Estado"].HeaderText = "Estado";
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
                // Obtener el objeto de la fila seleccionada (mapeado desde Entity Framework)
                var almacenSeleccionado = dataGridView1.Rows[e.RowIndex].DataBoundItem as Almacen;

                if (almacenSeleccionado != null)
                {
                    // Abrir el formulario Falm pasándole el ID seleccionado
                    var idalmacen = almacenSeleccionado.IdAlmacen;
                    var formFalm = new Falm(_context, idalmacen);
                    formFalm.nombretx.Text = almacenSeleccionado.Nombre;
                    formFalm.calletx.Text = almacenSeleccionado.Calle;
                    formFalm.ciudadtx.Text = almacenSeleccionado.Ciudad;
                    formFalm.estadotx.Text = almacenSeleccionado.Estado.ToString();
                    formFalm.paistx.Text = almacenSeleccionado.Pais.ToString();
                    formFalm.cptx.Text = almacenSeleccionado.cp;
                    formFalm.telefonotx.Text = almacenSeleccionado.Telefono;
                    formFalm.leyendatx.Text = almacenSeleccionado.Leyendatickt.ToString();
                    formFalm.almdefaultx.Text = almacenSeleccionado.Default.ToString();

                    if (formFalm.ShowDialog() == DialogResult.OK)
                    {
                        // Recargar la tabla si se guardaron cambios


                        CargarDatosAlmacen();
                    }
                }
            }
        }

        private void btregresar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}