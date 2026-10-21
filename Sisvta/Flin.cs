using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;
using Sisvta.UseCases; // Asegúrate de tener este using
using System;
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
        }

        // Constructor con inyección del Caso de Uso (el que usaremos desde Fclin)
        public Flin(GestionarLineasArticulosUseCase useCase)
        {
            InitializeComponent();
            _useCase = useCase;
        }

        // Método para recibir los datos al hacer doble clic en el grid
        public void CargarDatos(int id, string nombre, string nombreGrupo, string grupoLineaTexto, string prcComision)
        {
            this.idLinea = id;                 // Almacena el ID para saber que es edición
            nombretxt.Text = nombre;
            grupotxt.Text = grupoLineaTexto;
            nombregrupotxt.Text = nombreGrupo;
            comisiontxt.Text = prcComision;
        }

        public void CargarNombre(string nombre)
        {
            this.idLinea = 0; // Si solo cargamos nombre desde cero, asumimos nuevo registro
            nombretxt.Text = nombre;
            // Opcional: limpiar los demás campos si es un registro totalmente nuevo
            grupotxt.Clear();
            nombregrupotxt.Clear();
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

                // Parsear valores numéricos de forma segura
                decimal.TryParse(comisiontxt.Text.Trim(), out decimal comision);
                string grupoLinea = grupotxt.Text.Trim(); // Ajusta según el nombre de tu control o propiedad del DTO

                if (this.idLinea == 0)
                {
                    // === NUEVO REGISTRO (INSERT) ===
                    // Nota: Ajusta los parámetros según la firma exacta que tenga tu método de creación en el UseCase
                    await _useCase.CrearAsync(nombre, grupoLinea, comision, idempresa);

                    MessageBox.Show("Línea de artículo registrada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // === ACTUALIZACIÓN (UPDATE) usando el Id como llave ===
                    // Nota: Ajusta los parámetros según la firma exacta que tenga tu método de actualización en el UseCase
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
                    grupotxt.Text = selector.IdGrupoSeleccionado;
                    nombregrupotxt.Text = selector.NombreGrupoSeleccionado;
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