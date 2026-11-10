using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using Sisvta.Domain.Entities;
using Sisvta.UseCases;

namespace Sisvta
{
    public partial class Fcli : Form
    {
        private readonly GestionarClientesUseCase _useCase;
        private int? _idPendienteCarga = null;

        // Evento para notificar al catálogo Fccli que debe refrescar la consulta SQL
        public event Action? ClienteModificado;

        public Fcli(GestionarClientesUseCase useCase)
        {
            InitializeComponent();
            _useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));

            this.Shown += Fcli_Shown;
        }

        public void EstablecerIdPendiente(int clienteId)
        {
            _idPendienteCarga = clienteId;
        }

        private async void Fcli_Shown(object? sender, EventArgs e)
        {
            if (DesignMode) return;

            // Si recibió un ID desde el catálogo, cargamos solo ese cliente y ocultamos el grid general
            if (_idPendienteCarga.HasValue && _idPendienteCarga.Value > 0)
            {
                int idToLoad = _idPendienteCarga.Value;
                _idPendienteCarga = null;
                await CargarClientePorIdAsync(idToLoad);
            }
            else
            {
                // Si se abrió de forma independiente, mostramos y cargamos el grid completo
                if (dataGridView1 != null)
                {
                    dataGridView1.Visible = true;
                }
                await CargarGridAsync();
                LimpiarControles();
            }
        }

        public async Task CargarClientePorIdAsync(int clienteId)
        {
            try
            {
                clientetxt.Text = clienteId.ToString();

                var cliente = await _useCase.ObtenerPorIdAsync(clienteId);
                if (cliente != null)
                {
                    nombretxt.Text = cliente.Nombre;
                    domiciliotxt.Text = cliente.Domicilio;
                    rfctxt.Text = cliente.Rfc;
                    telefono1txt.Text = cliente.Telefono1;
                    curptxt.Text = cliente.curp;

                    btguardar.Visible = false;
                    btactualiza.Visible = true;
                    btBaja.Visible = true;
                }
                else
                {
                    MessageBox.Show("No se encontró el registro del cliente seleccionado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarControles();
                }

                // Ocultamos el grid ya que solo estamos editando un registro individual recibido desde el catálogo
                if (dataGridView1 != null)
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el cliente seleccionado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarGridAsync()
        {
            try
            {
                var lista = await _useCase.ObtenerTodosAsync();
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = lista.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al recargar la tabla: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];

                clientetxt.Text = fila.Cells["Id"].Value?.ToString();
                nombretxt.Text = fila.Cells["Nombre"].Value?.ToString();
                domiciliotxt.Text = fila.Cells["Domicilio"].Value?.ToString();
                rfctxt.Text = fila.Cells["Rfc"].Value?.ToString();
                telefono1txt.Text = fila.Cells["Telefono1"].Value?.ToString();
                curptxt.Text = fila.Cells["curp"].Value?.ToString();

                btguardar.Visible = false;
                btactualiza.Visible = true;
                btBaja.Visible = true;
            }
        }

        private async void btguardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(clientetxt.Text) ||
                string.IsNullOrWhiteSpace(nombretxt.Text) ||
                string.IsNullOrWhiteSpace(domiciliotxt.Text) ||
                string.IsNullOrWhiteSpace(rfctxt.Text))
            {
                MessageBox.Show("Por favor, completa todos los campos requeridos.", "Campos requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var cliente = new Cliente
                {
                    Id = int.Parse(clientetxt.Text.Trim()),
                    Nombre = nombretxt.Text.Trim(),
                    Domicilio = domiciliotxt.Text.Trim(),
                    Rfc = rfctxt.Text.Trim(),
                    Telefono1 = telefono1txt.Text.Trim(),
                    curp = curptxt.Text.Trim()
                };

                bool exito = await _useCase.RegistrarAsync(cliente);

                if (exito)
                {
                    MessageBox.Show("Cliente registrado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClienteModificado?.Invoke();
                    LimpiarControles();
                    await CargarGridAsync();
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                MessageBox.Show("El ID especificado ya existe en la base de datos.", "Error de Clave", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btactualiza_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(clientetxt.Text) ||
                string.IsNullOrWhiteSpace(nombretxt.Text) ||
                string.IsNullOrWhiteSpace(domiciliotxt.Text) ||
                string.IsNullOrWhiteSpace(rfctxt.Text))
            {
                MessageBox.Show("Por favor, ingresa el ID y los campos que deseas actualizar.", "Campos requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var cliente = new Cliente
                {
                    Id = int.Parse(clientetxt.Text.Trim()),
                    Nombre = nombretxt.Text.Trim(),
                    Domicilio = domiciliotxt.Text.Trim(),
                    Rfc = rfctxt.Text.Trim(),
                    Telefono1 = telefono1txt.Text.Trim(),
                    curp = curptxt.Text.Trim()
                };

                bool exito = await _useCase.ActualizarAsync(cliente);

                if (exito)
                {
                    MessageBox.Show("Cliente actualizado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClienteModificado?.Invoke();
                    await CargarGridAsync();
                    Close(); // Cierra automáticamente tras actualizar con éxito
                }
                else
                {
                    MessageBox.Show("No se encontró ningún cliente con ese ID.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btBaja_Click(object sender, EventArgs e)
        {
            string id = clientetxt.Text.Trim();
            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("Por favor, ingresa el ID del cliente que deseas eliminar.", "Campo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                $"¿Estás seguro de que deseas eliminar el cliente con ID {id}?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (respuesta != DialogResult.Yes) return;

            try
            {
                bool exito = await _useCase.EliminarAsync(int.Parse(id));
                if (exito)
                {
                    MessageBox.Show("Cliente eliminado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClienteModificado?.Invoke();
                    LimpiarControles();
                    await CargarGridAsync();
                    Close(); // Cierra automáticamente tras eliminar con éxito
                }
                else
                {
                    MessageBox.Show("No se encontró ningún cliente con ese ID.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                MessageBox.Show("No se puede eliminar el cliente porque está siendo utilizado en otras tablas (ej. ventas).", "Restricción de integridad", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void clientetxt_Leave(object sender, EventArgs e)
        {
            string id = clientetxt.Text.Trim();
            if (string.IsNullOrWhiteSpace(id)) return;

            try
            {
                var cliente = await _useCase.ObtenerPorIdAsync(int.Parse(id));
                if (cliente != null)
                {
                    nombretxt.Text = cliente.Nombre;
                    domiciliotxt.Text = cliente.Domicilio;
                    rfctxt.Text = cliente.Rfc;
                    telefono1txt.Text = cliente.Telefono1;
                    curptxt.Text = cliente.curp;

                    btguardar.Visible = false;
                    btactualiza.Visible = true;
                    btBaja.Visible = true;
                }
                else
                {
                    MessageBox.Show("No existe un cliente con ese ID.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarControles();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de consulta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarControles()
        {
            clientetxt.Clear();
            nombretxt.Clear();
            domiciliotxt.Clear();
            rfctxt.Clear();
            telefono1txt.Clear();
            curptxt.Clear();
            btguardar.Visible = true;
            btactualiza.Visible = false;
            btBaja.Visible = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void dataGridView1_Click(object sender, EventArgs e) { }
    }
}