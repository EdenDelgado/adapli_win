using Microsoft.Extensions.DependencyInjection;
using Sisvta.Domain.Entities;
using Sisvta.Infrastructure.Repositories;
using Sisvta.UseCases;
using Sisvta.UseCases.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisvta
{
    public partial class Fcprov : Form
    {
        private Fprov? _fprov;
        private readonly IConsultarProveedoresUseCase _useCase;
        private readonly IServiceProvider _serviceProvider; // Declarado correctamente
        private readonly BindingSource _bindingSource = new();
        private bool _isCargando = false; // Candado anti-concurrencia

        public string ProveedorIdSeleccionado { get; private set; } = string.Empty;

        // Constructor con Inyección de Dependencias (recibe useCase y serviceProvider)
        public Fcprov(IConsultarProveedoresUseCase useCase, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _useCase = useCase;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            this.Load += Fcprov_Load;
            this.dgvProveedores.CellDoubleClick += dgvProveedors_CellDoubleClick;
            this.dgvProveedores.CellMouseDoubleClick += dgvProveedors_CellMouseDoubleClick;

            if (this.Controls.Find("txtBuscar", true).FirstOrDefault() is TextBox txt)
            {
                txt.TextChanged += txtBuscar_TextChanged;
            }
        }

        private async void Fcprov_Load(object? sender, EventArgs e)
        {
            if (DesignMode) return;
            await CargarProveedoresAsync();
        }

        private async Task CargarProveedoresAsync()
        {
            if (_isCargando || _bindingSource.DataSource != null) return;

            try
            {
                _isCargando = true;

                var Proveedors = await _useCase.ObtenerTodosAsync();
                var lista = Proveedors.ToList();

                if (!lista.Any())
                {
                    MessageBox.Show("La consulta se ejecutó pero la tabla no contiene registros.", "Sin Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dgvProveedores.DataSource = null;
                dgvProveedores.AutoGenerateColumns = true;
                _bindingSource.DataSource = lista;
                dgvProveedores.DataSource = _bindingSource;
                dgvProveedores.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al consultar la base de datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isCargando = false;
            }
        }

        private void txtBuscar_TextChanged(object? sender, EventArgs e)
        {
            if (sender is TextBox txt)
            {
                string filtro = txt.Text.Trim();

                if (string.IsNullOrWhiteSpace(filtro))
                {
                    _bindingSource.Filter = null;
                }
                else
                {
                    _bindingSource.Filter = string.Format("Nombre LIKE '%{0}%'", filtro);
                }
            }
        }

        private void AsegurarfprovVivo(int ProveedorId)
        {
            if (_fprov == null || _fprov.IsDisposed)
            {
                // Resolvemos fprov directamente a través de DI mediante un scope limpio
                var scope = _serviceProvider.CreateScope();
                _fprov = scope.ServiceProvider.GetRequiredService<Fprov>();

                // Liberamos el scope cuando se cierre el formulario secundario
                _fprov.FormClosed += (s, e) => scope.Dispose();

                // Obtenemos la ventana principal (Fmain) de forma segura como propietaria
                var formPrincipal = this.TopLevelControl as Form;
                if (formPrincipal != null)
                {
                    _fprov.Owner = formPrincipal;
                }
            }
        }

        private void dgvProveedors_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            AbrirFormularioProveedor(e.RowIndex);
        }

        private void dgvProveedors_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            AbrirFormularioProveedor(e.RowIndex);
        }

 
        private void AbrirFormularioProveedor(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < dgvProveedores.Rows.Count)
            {
                var row = dgvProveedores.Rows[rowIndex];
                var item = row.DataBoundItem;

                if (item != null)
                {
                    int ProveedorId = 0;

                    // Extraemos la propiedad 'Id' dinámicamente sin importar si es el modelo Proveedor o ProveedorDto
                    var propId = item.GetType().GetProperty("Id");
                    if (propId != null)
                    {
                        var valorId = propId.GetValue(item);
                        if (valorId != null && int.TryParse(valorId.ToString(), out ProveedorId))
                        {
                            ProveedorIdSeleccionado = ProveedorId.ToString();

                            // Aseguramos y mostramos el formulario fprov mediante DI
                            AsegurarfprovVivo(ProveedorId);

                            if (_fprov != null)
                            {
                                if (_fprov.WindowState == FormWindowState.Minimized)
                                {
                                    _fprov.WindowState = FormWindowState.Normal;
                                }

                                _fprov.Show();
                                _fprov.BringToFront();
                                _fprov.Focus();
                            }
                        }
                    }
                }
            }
        }
    }
}