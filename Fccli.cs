using Microsoft.Extensions.DependencyInjection;
using Sisvta.Domain.Entities;
using Sisvta.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisvta
{
    public partial class Fccli : Form
    {
        private Fcli? _fcli;
        private IServiceScope? _fcliScope;
        private readonly IConsultarClientesUseCase _useCase;
        private readonly IServiceProvider _serviceProvider;
        private readonly BindingSource _bindingSource = new();
        private bool _isCargando = false;

        public string ClienteIdSeleccionado { get; private set; } = string.Empty;

        public Fccli(IConsultarClientesUseCase useCase, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            this.Load += Fccli_Load;
            this.dgvClientes.CellDoubleClick += dgvClientes_CellDoubleClick;
            this.dgvClientes.CellMouseDoubleClick += dgvClientes_CellMouseDoubleClick;

            // Blindaje contra errores de formato o valores nulos en el DataGridView
            this.dgvClientes.DataError += dgvClientes_DataError;

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
            if (_isCargando) return;

            try
            {
                _isCargando = true;

                var clientes = await _useCase.ObtenerTodosAsync();
                var lista = clientes?.Where(c => c != null).ToList() ?? new List<Cliente>();

                dgvClientes.DataSource = null;
                dgvClientes.AutoGenerateColumns = true;
                _bindingSource.DataSource = lista;
                dgvClientes.DataSource = _bindingSource;
                AgregarColumnaEditar();
                dgvClientes.Refresh();
            }
            catch (Exception ex)
            {
                // 👇 Pon un punto de interrupción (Breakpoint) aquí o lee el mensaje completo
                string mensajeCompleto = ex.ToString();
                MessageBox.Show($"Error detallado: {mensajeCompleto}", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isCargando = false;
            }
        }

        private void AgregarColumnaEditar()
        {
            // Verificamos si ya existe para evitar duplicarla
            if (!dgvClientes.Columns.Contains("ColEditar"))
            {
                DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
                btnColumn.Name = "ColEditar";
                btnColumn.HeaderText = "Editar";
                btnColumn.Text = "✏️"; // Figura del lápiz
                btnColumn.UseColumnTextForButtonValue = true;
                btnColumn.Width = 60;
                btnColumn.FlatStyle = FlatStyle.Flat;
                dgvClientes.Columns.Add(btnColumn);
            }

            // Forzamos que la columna quede siempre al final, sin importar las columnas de SQL
            if (dgvClientes.Columns.Contains("ColEditar"))
            {
                dgvClientes.Columns["ColEditar"].DisplayIndex = dgvClientes.Columns.Count - 1;
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

        private void AsegurarFcliVivo()
        {
            if (_fcli == null || _fcli.IsDisposed)
            {
                _fcliScope?.Dispose();
                _fcliScope = null;

                // 1. Creamos un scope independiente para aislar el DbContext de Fcli
                _fcliScope = _serviceProvider.CreateScope();

                // 2. Resolvemos la instancia de Fcli mediante el contenedor DI
                _fcli = _fcliScope.ServiceProvider.GetRequiredService<Fcli>();

                // 3. Nos suscribimos al evento para refrescar automáticamente la consulta SQL del grid
                _fcli.ClienteModificado += async () =>
                {
                    await CargarClientesAsync();
                };

                // 4. Limpiamos el scope y referencias al cerrar la ventana
                _fcli.FormClosed += (s, e) =>
                {
                    _fcliScope?.Dispose();
                    _fcliScope = null;
                    _fcli = null;
                };

                // 5. Configuramos el contenedor MdiParent si aplica
                var formPrincipal = this.MdiParent ?? this.TopLevelControl as Form;
                if (formPrincipal != null && formPrincipal.IsMdiContainer)
                {
                    _fcli.MdiParent = formPrincipal;
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
            if (rowIndex < 0 || rowIndex >= dgvClientes.Rows.Count) return;

            try
            {
                var row = dgvClientes.Rows[rowIndex];
                var item = row.DataBoundItem;

                if (item != null)
                {
                    int clienteId = 0;
                    var propId = item.GetType().GetProperty("Id");
                    if (propId != null)
                    {
                        var valorId = propId.GetValue(item);
                        if (valorId != null && int.TryParse(valorId.ToString(), out clienteId))
                        {
                            ClienteIdSeleccionado = clienteId.ToString();

                            // Aseguramos que la instancia y su scope estén listos
                            AsegurarFcliVivo();

                            if (_fcli != null && !_fcli.IsDisposed)
                            {
                                if (_fcli.WindowState == FormWindowState.Minimized)
                                {
                                    _fcli.WindowState = FormWindowState.Normal;
                                }

                                // Pasamos el ID para que Fcli cargue únicamente este registro
                                _fcli.EstablecerIdPendiente(clienteId);
                                _fcli.Show();
                                _fcli.BringToFront();
                                _fcli.Focus();

                                dgvClientes.Refresh();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el formulario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvClientes_DataError(object? sender, DataGridViewDataErrorEventArgs e)
        {
            // Evita que excepciones internas de renderizado por celdas nulas colapsen la interfaz
            e.ThrowException = false;
          
        }

        private void regresabt_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}