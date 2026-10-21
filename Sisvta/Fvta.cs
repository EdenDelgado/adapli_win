using Microsoft.Extensions.DependencyInjection;
using Sisvta.Domain.Entities;
using Sisvta.UseCases;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Sisvta
{
    public partial class Fvta : Form
    {
        private readonly IObtenerTotalVentasQuery _obtenerTotalVentasQuery;
        private readonly IServiceProvider _serviceProvider;

        // Constructor único con Inyección de Dependencias pura
        public Fvta(IObtenerTotalVentasQuery obtenerTotalVentasQuery, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _obtenerTotalVentasQuery = obtenerTotalVentasQuery ?? throw new ArgumentNullException(nameof(obtenerTotalVentasQuery));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        private void Fvta_Load(object sender, EventArgs e)
        {
            var hoy = DateTime.Now;
            dtpDesde.Value = new DateTime(hoy.Year, hoy.Month, 1);
            dtpHasta.Value = hoy;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaDesde = dtpDesde.Value;
                DateTime fechaHasta = dtpHasta.Value;

                int? idCliente = null;

                if (!string.IsNullOrWhiteSpace(txtIdCliente.Text))
                {
                    string textoId = txtIdCliente.Text.Trim();

                    if (int.TryParse(textoId, out int parsedId))
                    {
                        idCliente = parsedId;
                    }
                    else
                    {
                        MessageBox.Show("El código del cliente debe ser un número entero válido.", "Formato incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtIdCliente.Focus();
                        return;
                    }
                }

                var listaResultados = _obtenerTotalVentasQuery.Ejecutar(fechaDesde, fechaHasta, idCliente).ToList();

                dgvVentas.DataSource = new BindingList<VentaPorClienteFecha>(listaResultados);
                ConfigurarFormatoGrid();

                decimal totalAcumulado = listaResultados.Sum(x => x.TotalVendido);
                lblTotalAcumulado.Text = $"Total Acumulado: {totalAcumulado:N2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al consultar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFormatoGrid()
        {
            if (dgvVentas.Columns["Fecha"] != null)
            {
                dgvVentas.Columns["Fecha"].HeaderText = "Fecha";
                dgvVentas.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }

            if (dgvVentas.Columns["IdCliente"] != null)
            {
                dgvVentas.Columns["IdCliente"].HeaderText = "Cód. Cliente";
            }

            if (dgvVentas.Columns["NombreCliente"] != null)
            {
                dgvVentas.Columns["NombreCliente"].HeaderText = "Cliente";
                dgvVentas.Columns["NombreCliente"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dgvVentas.Columns["CantidadPedidos"] != null)
            {
                dgvVentas.Columns["CantidadPedidos"].HeaderText = "Nº Pedidos";
                dgvVentas.Columns["CantidadPedidos"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvVentas.Columns["TotalVendido"] != null)
            {
                dgvVentas.Columns["TotalVendido"].HeaderText = "Total Vendido";
                dgvVentas.Columns["TotalVendido"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvVentas.Columns["TotalVendido"].DefaultCellStyle.Format = "N2";
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtIdCliente.Clear();
            dgvVentas.DataSource = null;
            lblTotalAcumulado.Text = "Total Acumulado: $ 0.00";
        }

        private void txtIdCliente_DoubleClick(object sender, EventArgs e)
        {
            // Resolvemos Fccli mediante el contenedor DI usando un scope temporal
            using var scope = _serviceProvider.CreateScope();
            using var form = scope.ServiceProvider.GetRequiredService<Fccli>();

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtIdCliente.Text = form.ClienteIdSeleccionado;
            }
        }
    }
}