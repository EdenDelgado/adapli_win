using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Entities;
using Sisvta.Infrastructure.Persistence.Contexts;
using System;
using System.Windows.Forms;

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

            if (_esEdicion)
            {
                CargarAlmacenPorId(_idAlmacen);
            }
        }

        private void CargarAlmacenPorId(int id)
        {
            try
            {
                _almacenActual = _context.Almacenes.Find(id);

                if (_almacenActual != null)
                {
                    // Asignamos los valores de la base de datos a tus controles (TextBox, CheckBox, etc.)
                    // Asegúrate de cambiar "txtNombre", "txtCalle", etc., por los nombres reales de tus controles en el diseñador.
                    nombretx.Text = _almacenActual.Nombre;
                    calletx.Text = _almacenActual.Calle;
                    ciudadtx.Text = _almacenActual.Ciudad;
                    estadotx.Text = _almacenActual.Estado.ToString();
                    paistx.Text = _almacenActual.Pais.ToString();
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

                if (_esEdicion)
                {
                    // --- MODO EDICIÓN ---
                    if (_almacenActual != null)
                    {
                        _almacenActual.Nombre = nombretx.Text.Trim();
                        _almacenActual.Calle = calletx.Text.Trim();
                        _almacenActual.Ciudad = ciudadtx.Text.Trim();
                        _almacenActual.Estado = short.TryParse(estadotx.Text, out short estadoVal) ? estadoVal : (short)0;

                        if (short.TryParse(paistx.Text, out short paisVal))
                            _almacenActual.Pais = paisVal;

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
                        
                        Estado = short.TryParse(estadotx.Text, out short estadoVal) ? estadoVal : (short)0,
                        Pais = short.TryParse(paistx.Text, out short paisVal) ? paisVal : (short)0,
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
    }
}