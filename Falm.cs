using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Entities;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Falm : Form
    {
        private readonly ApplicationDbContext _context;
        private readonly int _idAlmacen;
        private readonly bool _esEdicion;
        private Almacen? _almacenActual;

        // Constructor vacío (por si el diseñador lo requiere)
        public Falm()
        {
            InitializeComponent();
        }

        // Constructor con DI y opcionalmente el ID para edición
        public Falm(ApplicationDbContext context, int idAlmacen = 0) : this()
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idAlmacen = idAlmacen;
            _esEdicion = _idAlmacen > 0;

            // 1. Cargamos los combos al inicializar
            CargarEstadosCombo();
            CargarPaisesCombo();

            if (_esEdicion)
            {
                CargarAlmacenPorId(_idAlmacen);
            }
            else
            {
                estadotx.SelectedIndex = -1;
                paistx.SelectedIndex = -1;
            }
        }

        private void Falm_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            CargarEstadosCombo();
            CargarPaisesCombo();
        }

        private void CargarEstadosCombo()
        {
            try
            {
                if (_context == null || estadotx == null) return;

                var listaEstados = _context.Set<Estado>()
                    .AsNoTracking()
                    .Select(e => new { id_estado = e.IdEstado, nombre = e.Nombre })
                    .ToList();

                if (listaEstados != null && listaEstados.Count > 0)
                {
                    estadotx.DataSource = null;
                    estadotx.DisplayMember = "nombre";
                    estadotx.ValueMember = "id_estado";
                    estadotx.DataSource = listaEstados;
                    estadotx.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los estados: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarPaisesCombo()
        {
            try
            {
                if (_context == null || paistx == null) return;

                var listaPaises = _context.Set<Pais>()
                    .AsNoTracking()
                    .Select(p => new { id_pais = p.IdPais, nombre = p.NombrePais })
                    .ToList();

                if (listaPaises != null && listaPaises.Count > 0)
                {
                    paistx.DataSource = null;
                    paistx.DisplayMember = "nombre";
                    paistx.ValueMember = "id_pais";
                    paistx.DataSource = listaPaises;
                    paistx.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los países: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarAlmacenPorId(int id)
        {
            try
            {
                _almacenActual = _context.Almacenes.Find(id);

                if (_almacenActual != null)
                {
                    nombretx.Text = _almacenActual.Nombre;
                    calletx.Text = _almacenActual.Calle;
                    ciudadtx.Text = _almacenActual.Ciudad;

                    // Asignamos el Estado seleccionado
                    if (_almacenActual.Estado > 0)
                    {
                        estadotx.SelectedValue = Convert.ToInt32(_almacenActual.Estado);
                    }
                    else
                    {
                        estadotx.SelectedIndex = -1;
                    }

                    // Asignamos el País seleccionado
                    if (_almacenActual.Pais > 0)
                    {
                        paistx.SelectedValue = Convert.ToInt32(_almacenActual.Pais);
                    }
                    else
                    {
                        paistx.SelectedIndex = -1;
                    }

                    telefonotx.Text = _almacenActual.Telefono;
                    leyendatx.Text = _almacenActual.Leyendatickt;
                    almdefaultx.Text = _almacenActual.Default.ToString();
                }
                else
                {
                    MessageBox.Show("No se encontró el almacén especificado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el almacén: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validaciones básicas antes de guardar
                if (string.IsNullOrWhiteSpace(nombretx.Text))
                {
                    MessageBox.Show("El nombre del almacén es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    nombretx.Focus();
                    return;
                }

                if (estadotx.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione un estado válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    estadotx.Focus();
                    return;
                }

                if (paistx.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione un país válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    paistx.Focus();
                    return;
                }

                short estadoSeleccionado = Convert.ToInt16(estadotx.SelectedValue);
                short paisSeleccionado = Convert.ToInt16(paistx.SelectedValue);

                if (_esEdicion)
                {
                    // --- MODO EDICIÓN ---
                    if (_almacenActual != null)
                    {
                        _almacenActual.Nombre = nombretx.Text.Trim();
                        _almacenActual.Calle = calletx.Text.Trim();
                        _almacenActual.Ciudad = ciudadtx.Text.Trim();
                        _almacenActual.Estado = estadoSeleccionado;
                        _almacenActual.Pais = paisSeleccionado;
                        _almacenActual.Telefono = telefonotx.Text.Trim();
                        _almacenActual.Leyendatickt = leyendatx.Text.Trim();
                        _almacenActual.Default = bool.Parse(almdefaultx.Text);

                        _context.Almacenes.Update(_almacenActual);
                    }
                }
                else
                {
                    // --- MODO NUEVO REGISTRO ---
                    var nuevoAlmacen = new Almacen
                    {
                        Nombre = nombretx.Text.Trim(),
                        Calle = calletx.Text.Trim(),
                        Ciudad = ciudadtx.Text.Trim(),
                        Estado = estadoSeleccionado,
                        Pais = paisSeleccionado,
                        cp = cptx.Text.Trim(),
                        Telefono = telefonotx.Text.Trim(),
                        Leyendatickt = leyendatx.Text.Trim(),
                        Default = bool.Parse(almdefaultx.Text)
                    };

                    _context.Almacenes.Add(nuevoAlmacen);
                }

                // 2. Guardar cambios en la Base de Datos
                _context.SaveChanges();

                MessageBox.Show("¡Almacén guardado correctamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 3. Indicamos que todo salió bien y cerramos el formulario para que Fcalm se actualice
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el almacén: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btregresar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}