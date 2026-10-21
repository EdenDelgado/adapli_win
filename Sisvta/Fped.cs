using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Sisvta.Infrastructure.Repositories;
using Sisvta.UseCases;

namespace Sisvta;

public partial class Fped : Form
{
    private readonly GestionarPedidosUseCase _gestionarPedidosUseCase;
    private readonly IServiceProvider _serviceProvider;

    // Inyectamos tanto el caso de uso como el proveedor de servicios
    public Fped(GestionarPedidosUseCase gestionarPedidosUseCase, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _gestionarPedidosUseCase = gestionarPedidosUseCase;
        _serviceProvider = serviceProvider;
    }

    private async Task CargarDatosPedidoAsync(int idPedido)
    {
        try
        {
            var (cabecera, nomCliente, detalle) = await _gestionarPedidosUseCase.CargarCompletoAsync(idPedido);
            if (cabecera == null)
            {
                MessageBox.Show("No se encontró ningún pedido con ese ID.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txcliente.Clear();
                txnomcli.Clear();
                dataGridView1.DataSource = null;
                return;
            }

            txcliente.Text = cabecera.Cliente.ToString();
            dtfecha.Value = cabecera.FechaFactura;
            lbndet.Text = cabecera.Ndet;
            txnomcli.Text = nomCliente;

            var lista = detalle.ToList();
            dataGridView1.DataSource = lista;

            if (dataGridView1.Columns["Importe"] != null)
                dataGridView1.Columns["Importe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            if (dataGridView1.Columns["Precio"] != null)
                dataGridView1.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error de lectura: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void txpedido_Leave(object sender, EventArgs e)
    {
        if (int.TryParse(txpedido.Text.Trim(), out int idPedido))
            await CargarDatosPedidoAsync(idPedido);
    }

    private async void guardar_Click(object sender, EventArgs e)
    {
        if (int.TryParse(txpedido.Text, out int idPedido))
        {
            bool exito = await _gestionarPedidosUseCase.GuardarCabeceraAsync(idPedido, txcliente.Text, dtfecha.Value);
            if (exito) MessageBox.Show("Pedido guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show("Ingrese un número de pedido válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void addproducto_Click(object sender, EventArgs e)
    {
        if (!int.TryParse(txpedido.Text.Trim(), out int idPedido))
        {
            MessageBox.Show("Ingrese un número de pedido válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Le pasamos el _serviceProvider a Fpedet para que pueda abrir Fcprod mediante DI
        using var fpedet = new Fpedet(_serviceProvider);
        if (fpedet.ShowDialog() == DialogResult.OK)
        {
            int itemActual = int.TryParse(lbndet.Text.Trim(), out int parsed) ? parsed : 0;
            bool ok = await _gestionarPedidosUseCase.AgregarItemAsync(idPedido, itemActual + 1, fpedet.CodigoBarra, fpedet.Cantidad, fpedet.Precio);
            if (ok)
            {
                MessageBox.Show("Producto agregado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CargarDatosPedidoAsync(idPedido);
            }
        }
    }

    private async void dataGridView1_DoubleClick(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.Index < 0) return;
        int itemSel = Convert.ToInt32(dataGridView1.CurrentRow.Cells["IdDetalleFactura"]?.Value ?? 0);
        // Lógica de edición pendiente de implementar con _gestionarPedidosUseCase
    }

    private async void btimportatx_Click(object sender, EventArgs e)
    {
        string ruta = @"D:\users\pedro\Downloads\ggf.txt";
        if (!File.Exists(ruta))
        {
            MessageBox.Show($"Archivo no encontrado: {ruta}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Cursor = Cursors.WaitCursor;
        try
        {
            int filas = await _gestionarPedidosUseCase.ImportarTxtAsync(ruta);
            MessageBox.Show(filas > 0 ? "Importación exitosa desde archivo de texto." : "Sin registros nuevos.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (int.TryParse(txpedido.Text.Trim(), out int idActual)) await CargarDatosPedidoAsync(idActual);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error de importación: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

 
    private async void tximporta_Click(object sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            int importados = await _gestionarPedidosUseCase.ImportarTodosAsync();
            MessageBox.Show(importados > 0 ? $"Se importaron {importados} tickets pendientes correctamente." : "No hay tickets pendientes por importar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Si hay un pedido escrito en pantalla, recargamos por si acaso afectó a la vista actual
            if (int.TryParse(txpedido.Text.Trim(), out int idActual))
                await CargarDatosPedidoAsync(idActual);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al importar tickets: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }
}