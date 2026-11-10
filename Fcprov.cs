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
        private readonly IServiceProvider _serviceProvider;
        private readonly BindingSource _bindingSource = new();
        private bool _isCargando = false; // Candado anti-concurrencia

        public string ProveedorIdSeleccionado { get; private set; } = string.Empty;

        public Fcprov(IConsultarProveedoresUseCase consultarUseCase, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _useCase = consultarUseCase ?? throw new ArgumentNullException(nameof(consultarUseCase));
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
            if (_isCargando) return;

            try
            {
                _isCargando = true;

                var proveedores = await _useCase.ObtenerTodosAsync();
                var lista = proveedores.ToList();

                if (!lista.Any())
                {
                    // Opcional: limpiar el grid si no hay registros en lugar de mostrar alerta en cada recarga
                    dgvProveedores.DataSource = null;
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

        private IServiceScope? _fprovScope; // Scope a nivel de clase para aislar el DbContext de Fprov

        private void AsegurarfprovVivo()
        {
            if (_fprov == null || _fprov.IsDisposed)
            {
                _fprovScope?.Dispose();
                _fprovScope = null;

                // 1. Creamos un nuevo scope independiente
                _fprovScope = _serviceProvider.CreateScope();

                // 2. Resolvemos una instancia completamente nueva de Fprov
                _fprov = _fprovScope.ServiceProvider.GetRequiredService<Fprov>();

                // 3. NOS SUSCRIBIMOS AL EVENTO DE ACTUALIZACIÓN
                // Cada vez que Fprov guarde, actualice o elimine, recargará este catálogo automáticamente.
                _fprov.ProveedorModificado += async () =>
                {
                    await CargarProveedoresAsync();
                };

                // 4. Limpiamos el scope y dejamos la referencia en null cuando el usuario cierre la ventana
                _fprov.FormClosed += (s, e) =>
                {
                    _fprovScope?.Dispose();
                    _fprovScope = null;
                    _fprov = null;
                };

                // 5. Vinculamos el MdiParent correctamente
                var formPrincipal = this.MdiParent ?? this.TopLevelControl as Form;
                if (formPrincipal != null && formPrincipal.IsMdiContainer)
                {
                    _fprov.MdiParent = formPrincipal;
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
            if (rowIndex < 0 || rowIndex >= dgvProveedores.Rows.Count) return;

            try
            {
                var row = dgvProveedores.Rows[rowIndex];

                // Verificamos de forma segura si la celda "Id" existe y tiene un valor válido
                if (row.Cells["Id"]?.Value != null && int.TryParse(row.Cells["Id"].Value.ToString(), out int proveedorId))
                {
                    ProveedorIdSeleccionado = proveedorId.ToString();

                    AsegurarfprovVivo();

                    if (_fprov != null && !_fprov.IsDisposed)
                    {
                        if (_fprov.WindowState == FormWindowState.Minimized)
                        {
                            _fprov.WindowState = FormWindowState.Normal;
                        }

                        _fprov.EstablecerIdPendiente(proveedorId);
                        _fprov.Show();
                        _fprov.BringToFront();
                        _fprov.Focus();

                        dgvProveedores.Refresh();
                    }
                }
                else
                {
                    MessageBox.Show("No se pudo obtener el ID del proveedor seleccionado en la fila.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el formulario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void regresabt_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}