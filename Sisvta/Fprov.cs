using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sisvta.Domain.Entities;
using Sisvta.UseCases;

namespace Sisvta
{
    public partial class Fprov : Form
    {
        private readonly IConsultarProveedoresUseCase _consultarUseCase;
        private readonly GestionarProveedoresUseCase _gestionarUseCase;

        public Fprov(IConsultarProveedoresUseCase consultarUseCase, GestionarProveedoresUseCase gestionarUseCase)
        {
            InitializeComponent();
            _consultarUseCase = consultarUseCase ?? throw new ArgumentNullException(nameof(consultarUseCase));
            _gestionarUseCase = gestionarUseCase ?? throw new ArgumentNullException(nameof(gestionarUseCase));

            this.Load += Fcli_Load;
        }

        private async Task CargarGridAsync()
        {
            try
            {
                var lista = await _consultarUseCase.ObtenerTodosAsync();
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = lista.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al recargar la tabla: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void Fcli_Load(object? sender, EventArgs e)
        {
            if (DesignMode) return;
            await CargarGridAsync();
        }

        private void dataGridView1_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];

                proveedortxt.Text = fila.Cells["Id"].Value?.ToString();
                nombretxt.Text = fila.Cells["Nombre"].Value?.ToString();
                domiciliotxt.Text = fila.Cells["Direccion"].Value?.ToString();
                rfctxt.Text = fila.Cells["Rfc"].Value?.ToString();

                btguardar.Visible = false;
                btactualiza.Visible = true;
                btBaja.Visible = true;
            }
        }

        private async void btguardar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nombretxt.Text))
            {
                MessageBox.Show("Por favor, completa al menos el nombre del proveedor.", "Campos requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var proveedor = new Proveedores
                {
                    Nombre = nombretxt.Text.Trim(),
                    Direccion = domiciliotxt.Text.Trim(),
                    Rfc = rfctxt.Text.Trim()
                };

                await _gestionarUseCase.GuardarAsync(proveedor);

                MessageBox.Show("Proveedor registrado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarControles();
                await CargarGridAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btactualiza_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(proveedortxt.Text) || string.IsNullOrWhiteSpace(nombretxt.Text))
            {
                MessageBox.Show("Por favor, ingresa el ID y el nombre del proveedor.", "Campos requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var proveedor = new Proveedores
                {
                    Id = int.Parse(proveedortxt.Text.Trim()),
                    Nombre = nombretxt.Text.Trim(),
                    Direccion = domiciliotxt.Text.Trim(),
                    Rfc = rfctxt.Text.Trim()
                };

                await _gestionarUseCase.GuardarAsync(proveedor);

                MessageBox.Show("Proveedor actualizado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CargarGridAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btBaja_Click(object? sender, EventArgs e)
        {
            string id = proveedortxt.Text.Trim();
            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("Por favor, ingresa el ID del proveedor que deseas eliminar.", "Campo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                $"¿Estás seguro de que deseas eliminar el proveedor con ID {id}?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (respuesta != DialogResult.Yes) return;

            try
            {
                await _gestionarUseCase.EliminarAsync(int.Parse(id));
                MessageBox.Show("Proveedor eliminado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarControles();
                await CargarGridAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void proveedortxt_Leave(object? sender, EventArgs e)
        {
            string id = proveedortxt.Text.Trim();
            if (string.IsNullOrWhiteSpace(id)) return;

            try
            {
                var proveedor = await _consultarUseCase.ObtenerPorIdAsync(int.Parse(id));
                if (proveedor != null)
                {
                    nombretxt.Text = proveedor.Nombre;
                    domiciliotxt.Text = proveedor.Direccion;
                    rfctxt.Text = proveedor.Rfc;
                    btguardar.Visible = false;
                    btactualiza.Visible = true;
                    btBaja.Visible = true;
                }
                else
                {
                    MessageBox.Show("No existe un proveedor con ese ID.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    nombretxt.Clear();
                    domiciliotxt.Clear();
                    rfctxt.Clear();
                    btguardar.Visible = true;
                    btactualiza.Visible = false;
                    btBaja.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de consulta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarControles()
        {
            proveedortxt.Clear();
            nombretxt.Clear();
            domiciliotxt.Clear();
            rfctxt.Clear();
            btguardar.Visible = true;
            btactualiza.Visible = false;
            btBaja.Visible = false;
        }

        private void panel1_Paint(object? sender, PaintEventArgs e) { }
        private void dataGridView1_Click(object? sender, EventArgs e) { }
    }
}