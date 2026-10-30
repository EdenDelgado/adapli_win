using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Entities;
using Sisvta.Infrastructure.Persistence.Contexts;
using Sisvta.UseCases; // Asegúrate de tener este using
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Sisvta
{
    public partial class Flin : Form
    {
        private readonly GestionarLineasArticulosUseCase _useCase;
        public int idLinea = 0; // 0 indica que es un registro nuevo
        public string idempresa = string.Empty; // Variable para almacenar el idempresa seleccionado

        // Evento para notificar al formulario padre (Fclin) que hubo cambios y recargue el grid
        public event EventHandler? LineaModificada;
        private readonly ApplicationDbContext _context; // Añade este campo si no lo tienes

        // Constructor vacío para el diseñador visual
        public Flin(GestionarLineasArticulosUseCase useCase, ApplicationDbContext context)
        {
            InitializeComponent();
            _useCase = useCase;
            _context = context;
            CargarGrupoLineaCombo();
        }

        // Constructor con inyección del Caso de Uso (el que usaremos desde Fclin)
        public Flin(GestionarLineasArticulosUseCase useCase)
        {
            InitializeComponent();
            _useCase = useCase;
            CargarGrupoLineaCombo();
        }

        private void Flin_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            CargarGrupoLineaCombo();
        }

        // Método para cargar los grupos de líneas en el ComboBox grupotxt
        private void CargarGrupoLineaCombo()
        {
            try
            {
                if (_context == null || grupotxt == null) return;

                var listaGrupos = _context.Set<GrupoLinea>()
                    .AsNoTracking()
                    .Select(g => new { id_grupo = g.Id, nombre = g.Nombre })
                    .ToList();

                if (listaGrupos != null && listaGrupos.Count > 0)
                {
                    grupotxt.DataSource = null;
                    grupotxt.DisplayMember = "nombre";
                    grupotxt.ValueMember = "id_grupo";
                    grupotxt.DataSource = listaGrupos;
                    grupotxt.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los grupos de líneas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para recibir los datos al hacer doble clic en el grid
        public void CargarDatos(int id, string nombre, string nombreGrupo, string grupoIdTexto, string prcComision)
        {
            this.idLinea = id;                 // Almacena el ID para saber que es edición
            nombretxt.Text = nombre;
            //nombregrupotxt.Text = nombreGrupo;
            comisiontxt.Text = prcComision;

            // Asignar el ID seleccionado en el ComboBox si es numérico válido
            if (int.TryParse(grupoIdTexto, out int idGrupoVal) && grupotxt != null)
            {
                grupotxt.SelectedValue = idGrupoVal;
            }
            else
            {
                grupotxt.SelectedIndex = -1;
            }
        }

        public void CargarNombre(string nombre)
        {
            this.idLinea = 0; // Si solo cargamos nombre desde cero, asumimos nuevo registro
            nombretxt.Text = nombre;
            grupotxt.SelectedIndex = -1;
            //nombregrupotxt.Clear();
            comisiontxt.Text = "0.00";
        }

        private void btregresar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void btGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_useCase == null)
                {
                    MessageBox.Show("El caso de uso no está inicializado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 1. Validaciones básicas de campos vacíos
                string nombre = nombretxt.Text.Trim();
                if (string.IsNullOrEmpty(nombre))
                {
                    MessageBox.Show("El nombre de la línea no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    nombretxt.Focus();
                    return;
                }

                if (grupotxt.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione un grupo de línea válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    grupotxt.Focus();
                    return;
                }

                // Parsear valores numéricos de forma segura
                decimal.TryParse(comisiontxt.Text.Trim(), out decimal comision);
                string grupoLinea = grupotxt.SelectedValue.ToString(); // Obtenemos el ID del ComboBox

                if (this.idLinea == 0)
                {
                    // === NUEVO REGISTRO (INSERT) ===
                    await _useCase.CrearAsync(nombre, grupoLinea, comision, idempresa);

                    MessageBox.Show("Línea de artículo registrada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // === ACTUALIZACIÓN (UPDATE) usando el Id como llave ===
                    await _useCase.ActualizarAsync(this.idLinea, nombre, grupoLinea, comision, idempresa);

                    MessageBox.Show("Línea de artículo actualizada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Notificamos al grid principal que se recargue
                LineaModificada?.Invoke(this, EventArgs.Empty);

                // Cerramos el formulario de edición/creación al guardar con éxito
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la información: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void nombregrupotxt_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                // VERIFICACIÓN: Si _context es nulo en Flin, lo advertimos de inmediato
                if (_context == null)
                {
                    MessageBox.Show("El contexto de base de datos no está inicializado en Flin.", "Error crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using var selector = new Fcgrul(_context);
                if (selector.ShowDialog() == DialogResult.OK)
                {
                    // Si el selector devuelve un ID, lo asignamos directamente al ComboBox si existe
                    if (int.TryParse(selector.IdGrupoSeleccionado, out int idGrupoSeleccionado))
                    {
                        grupotxt.SelectedValue = idGrupoSeleccionado;
                    }

                    //nombregrupotxt.Text = selector.NombreGrupoSeleccionado;
                    idempresa = selector.idempresa;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el selector: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}