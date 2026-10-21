using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace Sisvta
{
    public partial class Fpedet : Form
    {
        private readonly IServiceProvider? _serviceProvider;

        // Constructor con soporte para Inyección de Dependencias y compatibilidad vacía
        public Fpedet(IServiceProvider? serviceProvider = null)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }

        public int ItemSeleccionado { get; private set; } = 0;

        public string CodigoBarra => txcodigo.Text.Trim();
        public string Nombre => txnombre.Text.Trim();
        public decimal Cantidad => decimal.TryParse(txcantidad.Text, out decimal c) ? c : 0;
        public decimal Precio => decimal.TryParse(txprecio.Text, out decimal p) ? p : 0;
        public decimal Importe => Cantidad * Precio;

        public void CargarDatos(int item, string codigo, string nombre, string descripcion, decimal cantidad, decimal precio)
        {
            ItemSeleccionado = item;
            txcodigo.Text = codigo;
            txnombre.Text = descripcion;
            txcantidad.Text = cantidad.ToString("0.00");
            txprecio.Text = precio.ToString("0.00");
        }

        private void txcodigo_DoubleClick(object sender, EventArgs e)
        {
            if (_serviceProvider == null)
            {
                MessageBox.Show("El contenedor de servicios no está disponible.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Creamos el scope y resolvemos Fcprod limpiamente mediante DI
            using var scope = _serviceProvider.CreateScope();
            var fcprod = scope.ServiceProvider.GetRequiredService<Fcprod>();

            if (fcprod.ShowDialog() == DialogResult.OK)
            {
                this.txcodigo.Text = fcprod.IdSeleccionado.ToString();
                this.txnombre.Text = fcprod.NombreSeleccionado;
                this.txcantidad.Focus();
            }
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txcodigo.Text))
            {
                MessageBox.Show("Por favor seleccione o ingrese un código de producto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txcodigo.Focus();
                return;
            }

            if (Cantidad <= 0)
            {
                MessageBox.Show("Ingrese una cantidad mayor a cero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txcantidad.Focus();
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}