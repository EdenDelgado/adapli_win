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
    public partial class Fccli : Form
    {
        private Fcli? _fcli;
        private readonly IConsultarClientesUseCase _useCase;
        private readonly IServiceProvider _serviceProvider; // Declarado correctamente
        private readonly BindingSource _bindingSource = new();
        private bool _isCargando = false; // Candado anti-concurrencia

        public string ClienteIdSeleccionado { get; private set; } = string.Empty;

        // Constructor con Inyección de Dependencias (recibe useCase y serviceProvider)
        public Fccli(IConsultarClientesUseCase useCase, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _useCase = useCase;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            this.Load += Fccli_Load;
            this.dgvClientes.CellDoubleClick += dgvClientes_CellDoubleClick;
            this.dgvClientes.CellMouseDoubleClick += dgvClientes_CellMouseDoubleClick;

            if (this.Controls.Find("txtBuscar", true).FirstOrDefault() is TextBox txt)
            {
                txt.TextChanged += txtBuscar_TextChanged;
            }
        }

        private async void Fccli_Load(object? sender, EventArgs e)
        {
            if (DesignMode) return;
            await CargarClientesAsync();
        }

        private async Task CargarClientesAsync()
        {
            if (_isCargando || _bindingSource.DataSource != null) return;

            try
            {
                _isCargando = true;

                var clientes = await _useCase.ObtenerTodosAsync();
                var lista = clientes.ToList();

                if (!lista.Any())
                {
                    MessageBox.Show("La consulta se ejecutó pero la tabla no contiene registros.", "Sin Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dgvClientes.DataSource = null;
                dgvClientes.AutoGenerateColumns = true;
                _bindingSource.DataSource = lista;
                dgvClientes.DataSource = _bindingSource;
                dgvClientes.Refresh();
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

        private void AsegurarFcliVivo(int clienteId)
        {
            if (_fcli == null || _fcli.IsDisposed)
            {
                // Resolvemos Fcli directamente a través de DI mediante un scope limpio
                var scope = _serviceProvider.CreateScope();
                _fcli = scope.ServiceProvider.GetRequiredService<Fcli>();

                // Liberamos el scope cuando se cierre el formulario secundario
                _fcli.FormClosed += (s, e) => scope.Dispose();

                // Obtenemos la ventana principal (Fmain) de forma segura como propietaria
                var formPrincipal = this.TopLevelControl as Form;
                if (formPrincipal != null)
                {
                    _fcli.Owner = formPrincipal;
                }
            }
        }

        private void dgvClientes_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            AbrirFormularioCliente(e.RowIndex);
        }

        private void dgvClientes_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            AbrirFormularioCliente(e.RowIndex);
        }

 
        private void AbrirFormularioCliente(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < dgvClientes.Rows.Count)
            {
                var row = dgvClientes.Rows[rowIndex];
                var item = row.DataBoundItem;

                if (item != null)
                {
                    int clienteId = 0;

                    // Extraemos la propiedad 'Id' dinámicamente sin importar si es el modelo Cliente o ClienteDto
                    var propId = item.GetType().GetProperty("Id");
                    if (propId != null)
                    {
                        var valorId = propId.GetValue(item);
                        if (valorId != null && int.TryParse(valorId.ToString(), out clienteId))
                        {
                            ClienteIdSeleccionado = clienteId.ToString();

                            // Aseguramos y mostramos el formulario Fcli mediante DI
                            AsegurarFcliVivo(clienteId);

                            if (_fcli != null)
                            {
                                if (_fcli.WindowState == FormWindowState.Minimized)
                                {
                                    _fcli.WindowState = FormWindowState.Normal;
                                }

                                _fcli.Show();
                                _fcli.BringToFront();
                                _fcli.Focus();
                            }
                        }
                    }
                }
            }
        }
    }
}