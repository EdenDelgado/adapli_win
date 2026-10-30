using Sisvta.UseCases; // Ajusta según tu namespace de casos de uso
using System;
using System.Windows.Forms;
using Sisvta.UseCases.DTO;
namespace Sisvta
{
    public partial class Fgru : Form
    {
        private readonly GestionarGruposLineasUseCase _useCase;

        // El constructor recibe el caso de uso inyectado desde Fcgru
        public Fgru(GestionarGruposLineasUseCase useCase)
        {
            InitializeComponent();
            _useCase = useCase;
        }

        // Método público para recibir el nombre seleccionado desde el grid de Fcgru
        public void CargarNombre(string nombre)
        {
            nombretxt.Text = nombre;
        }

        private void Fgru_Load(object sender, EventArgs e)
        {
        }



        private async void btguardar_Click(object sender, EventArgs e)
        {
            string nombreGrupo = nombretxt.Text.Trim();

            // 1. Validar que la caja de texto no esté vacía
            if (string.IsNullOrEmpty(nombreGrupo))
            {
                MessageBox.Show("El nombre del grupo no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nombretxt.Focus();
                return;
            }

            try
            {
                // 2. Crear el DTO con el valor del TextBox
                var nuevoGrupo = new GrupoLineaDto
                {
                    Nombre = nombreGrupo
                };

                // 3. Llamar exactamente a 'GuardarAsync' (reemplaza cualquier otro nombre anterior)
                await _useCase.GuardarAsync(nuevoGrupo);

                MessageBox.Show("Registro guardado exitosamente en la base de datos.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 4. Cerrar el formulario indicando éxito
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al intentar guardar el registro: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btregresar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}