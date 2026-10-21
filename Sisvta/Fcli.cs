using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using Sisvta.Domain.Entities;
using Sisvta.UseCases;

namespace Sisvta;

public partial class Fcli : Form
{
    private readonly GestionarClientesUseCase _useCase;

    public Fcli(GestionarClientesUseCase useCase)
    {
        InitializeComponent();
        _useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
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

    private async void Fcli_Load(object sender, EventArgs e)
    {
        await CargarGridAsync();
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
            MessageBox.Show("Por favor, completa todos los campos.", "Campos requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var cliente = new Cliente(
                int.Parse(clientetxt.Text.Trim()),
                nombretxt.Text.Trim(),
                domiciliotxt.Text.Trim(),
                rfctxt.Text.Trim()
                
            );

            bool exito = await _useCase.RegistrarAsync(cliente);

            if (exito)
            {
                MessageBox.Show("Cliente registrado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            var cliente = new Cliente(
                int.Parse(clientetxt.Text.Trim()),
                nombretxt.Text.Trim(),
                domiciliotxt.Text.Trim(),
                rfctxt.Text.Trim()
                
            );

            bool exito = await _useCase.ActualizarAsync(cliente);

            if (exito)
            {
                MessageBox.Show("Cliente actualizado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CargarGridAsync();
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
                LimpiarControles();
                await CargarGridAsync();
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
                btguardar.Visible = false;
                btactualiza.Visible = true;
                btBaja.Visible = true;
            }
            else
            {
                MessageBox.Show("No existe un cliente con ese ID.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        clientetxt.Clear();
        nombretxt.Clear();
        domiciliotxt.Clear();
        rfctxt.Clear();
        btguardar.Visible = true;
        btactualiza.Visible = false;
        btBaja.Visible = false;
    }

    private void panel1_Paint(object sender, PaintEventArgs e) { }
    private void dataGridView1_Click(object sender, EventArgs e) { }
}