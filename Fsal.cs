using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Fsal : Form
    {
        private readonly ApplicationDbContext _context;
        private readonly int _idSalidaFiltro;

        // 1. Constructor estándar (por si se abre desde el menú principal)
        public Fsal(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idSalidaFiltro = 0;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            _ = InicializarFormularioAsync();
        }

        // 2. Constructor con parámetro (para cuando se invoca haciendo doble clic desde Fcsal)
        public Fsal(ApplicationDbContext context, int idSalida)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idSalidaFiltro = idSalida;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            _ = InicializarFormularioAsync();
        }

        private async Task InicializarFormularioAsync()
        {
            await CargarConceptosAsync();
            await CargarAlmacenesAsync();
            await CargarClientesAsync(); // <-- Cargamos el ComboBox de clientes
            await CargarMaestroDoctoscpAsync();
        }

        private async void Fccom_Load(object sender, EventArgs e)
        {
            await CargarConceptosAsync();
            await CargarAlmacenesAsync();
            await CargarClientesAsync(); // <-- Aseguramos la carga también en el Load
            await CargarMaestroDoctoscpAsync();
        }

        public async Task CargarMaestroDoctoscpAsync(string filtroProveedor = null)
        {
            try
            {
                if (_context == null)
                {
                    MessageBox.Show("El contexto de base de datos es nulo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // =========================================================================
                // SI ES UN NUEVO REGISTRO (_idSalidaFiltro == 0), NO CARGAMOS NADA DEL MAESTRO
                // =========================================================================
                if (_idSalidaFiltro == 0)
                {
                    LimpiarFormularioNuevo();
                    return; // Salimos para que no intente leer filas fantasma
                }

                // Si _idSalidaFiltro > 0, procedemos a cargar el registro específico para edición
                string sqlQuery = @"
                    SELECT 
                        idsalida AS Idsalida, 
                        folio AS Folio, 
                        fecha AS Fecha,
                        s.concepto AS IdConcepto, 
                        c.concepto AS NomConcepto,
                        s.cliente AS IdCliente, 
                        cl.nombre AS NomCliente,
                        s.almacen AS IdAlmacen, 
                        a.nombre AS NomAlmacen,
                        aquien AS Aquien, 
                        observaciones AS Observacion, 
                        proyecto AS Proyecto, 
                        idunidad AS Idunidad
                    FROM salidainv as s 
                    LEFT JOIN concepto as c ON s.concepto = c.id_conceptosprov 
                    LEFT JOIN clientes as cl ON s.cliente = cl.id_empros 
                    LEFT JOIN almacen as a ON s.almacen = a.id_almacen
                    WHERE idsalida = @IdSalidaFiltro";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;
                command.Parameters.AddWithValue("@IdSalidaFiltro", _idSalidaFiltro);

                using var reader = await command.ExecuteReaderAsync();
                var dataTable = new System.Data.DataTable();
                dataTable.Load(reader);

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dataTable;
                dataGridView1.Refresh();

                if (dataGridView1.Rows.Count > 0 && dataGridView1.CurrentRow != null)
                {
                    await CargarDetalleDoctoscpAsync();
                    txfolio.Text = dataGridView1.CurrentRow.Cells["Folio"].Value?.ToString() ?? string.Empty;

                    if (dataGridView1.CurrentRow.Cells["Fecha"].Value != null &&
                        DateTime.TryParse(dataGridView1.CurrentRow.Cells["Fecha"].Value.ToString(), out DateTime fechaVal))
                    {
                        txfecha.Value = fechaVal;
                    }

                    if (dataGridView1.CurrentRow.Cells["IdConcepto"].Value != null &&
                        int.TryParse(dataGridView1.CurrentRow.Cells["IdConcepto"].Value.ToString(), out int idCon))
                    {
                        conceptotxt.SelectedValue = idCon;
                    }

                    if (dataGridView1.CurrentRow.Cells["IdAlmacen"].Value != null &&
                        int.TryParse(dataGridView1.CurrentRow.Cells["IdAlmacen"].Value.ToString(), out int idAlm))
                    {
                        txalmacen.SelectedValue = idAlm;
                    }

                    if (dataGridView1.CurrentRow.Cells["IdCliente"].Value != null &&
                        int.TryParse(dataGridView1.CurrentRow.Cells["IdCliente"].Value.ToString(), out int idCli))
                    {
                        txcliente.SelectedValue = idCli;
                    }

                    txaquien.Text = dataGridView1.CurrentRow.Cells["Aquien"].Value?.ToString() ?? string.Empty;
                    txobservacion.Text = dataGridView1.CurrentRow.Cells["Observacion"].Value?.ToString() ?? string.Empty;
                    txproyecto.Text = dataGridView1.CurrentRow.Cells["Proyecto"].Value?.ToString() ?? string.Empty;
                }
                else
                {
                    dataGridView2.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el maestro salidainv: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormularioNuevo()
        {
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;

            txfolio.Clear();
            txfecha.Value = DateTime.Now;
            conceptotxt.SelectedIndex = -1;
            txalmacen.SelectedIndex = -1;
            txcliente.SelectedIndex = -1;
            txaquien.Clear();
            txobservacion.Clear();
            txproyecto.Clear();
            txfolio.Focus();
             
        }

        private async Task CargarDetalleDoctoscpAsync()
        {
            try
            {
                if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Cells["Idsalida"].Value == null ||
                    dataGridView1.CurrentRow.Cells["Idsalida"].Value == DBNull.Value)
                {
                    dataGridView2.DataSource = null;
                    return;
                }

                int idDoctoscp = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Idsalida"].Value);

                string sqlQuery = @"
                    SELECT 
                        salida AS Salida, 
                        idsalidadet AS Idsalidadet, 
                        qty AS Cantidad,
                        producto AS Producto, 
                        s.clave AS ClaveProducto,
                        s.s_nombre AS NombreProducto,
                        precio AS Precio, 
                        (qty * precio) AS Importe
                    FROM salidadetinv 
                    LEFT JOIN servicios s ON salidadetinv.producto = s.id_servicio
                    WHERE salida = @Idsalida";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;
                command.Parameters.AddWithValue("@Idsalida", idDoctoscp);

                using var reader = await command.ExecuteReaderAsync();
                var dataTable = new System.Data.DataTable();
                dataTable.Load(reader);

                dataGridView2.AutoGenerateColumns = true;
                dataGridView2.RowHeadersWidth = 20;
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = dataTable;

                dataGridView2.RowTemplate.Height = 40;
                dataGridView2.CellBorderStyle = DataGridViewCellBorderStyle.None;

                dataGridView2.EnableHeadersVisualStyles = false;
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
                dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                dataGridView2.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);

                dataGridView2.Columns["NombreProducto"].Width = 260;

                if (dataGridView2.Columns.Contains("Salida"))
                    dataGridView2.Columns["Salida"].Visible = false;

                if (dataGridView2.Columns.Contains("Idsalidadet"))
                    dataGridView2.Columns["Idsalidadet"].Visible = false;

                if (dataGridView2.Columns.Contains("Producto"))
                    dataGridView2.Columns["Producto"].Visible = false;

                if (dataGridView2.Columns.Contains("Precio"))
                {
                    dataGridView2.Columns["Precio"].DefaultCellStyle.Format = "C2";
                    dataGridView2.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dataGridView2.Columns.Contains("Importe"))
                {
                    dataGridView2.Columns["Importe"].DefaultCellStyle.Format = "C2";
                    dataGridView2.Columns["Importe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                dataGridView2.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el detalle salidadetinv: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            await CargarDetalleDoctoscpAsync();

            if (dataGridView1.CurrentRow != null)
            {
                // Sincronizar ComboBox de Concepto
                if (dataGridView1.CurrentRow.Cells["IdConcepto"].Value != null &&
                    int.TryParse(dataGridView1.CurrentRow.Cells["IdConcepto"].Value.ToString(), out int idCon))
                {
                    conceptotxt.SelectedValue = idCon;
                }

                // Sincronizar ComboBox de Almacén
                if (dataGridView1.CurrentRow.Cells["IdAlmacen"].Value != null &&
                    int.TryParse(dataGridView1.CurrentRow.Cells["IdAlmacen"].Value.ToString(), out int idAlm))
                {
                    txalmacen.SelectedValue = idAlm;
                }

                // Sincronizar ComboBox de Cliente
                if (dataGridView1.CurrentRow.Cells["IdCliente"].Value != null &&
                    int.TryParse(dataGridView1.CurrentRow.Cells["IdCliente"].Value.ToString(), out int idCli))
                {
                    txcliente.SelectedValue = idCli;
                }
            }
        }

        private void btregresar_Click(object sender, EventArgs e)
        {
        }

        private void regresabt_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void btagregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null ||
                    dataGridView1.CurrentRow.Cells["Idsalida"].Value == null ||
                    dataGridView1.CurrentRow.Cells["Idsalida"].Value == DBNull.Value)
                {
                    MessageBox.Show("Seleccione una salida de inventario válida en el catálogo maestro antes de agregar detalles.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string idMovimiento = dataGridView1.CurrentRow.Cells["Idsalida"].Value.ToString();

                if (string.IsNullOrEmpty(idMovimiento))
                {
                    MessageBox.Show("El identificador del movimiento de inventario no es válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var formDetalle = new Fdsal(_context, idMovimiento))
                {
                    if (formDetalle.ShowDialog() == DialogResult.OK)
                    {
                        await CargarDetalleDoctoscpAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el formulario de detalle: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btborrar1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.CurrentRow == null ||
                    dataGridView2.CurrentRow.Cells["Salida"].Value == null ||
                    dataGridView2.CurrentRow.Cells["Salida"].Value == DBNull.Value)
                {
                    MessageBox.Show("Seleccione un renglón del detalle de inventario que desee eliminar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(dataGridView2.CurrentRow.Cells["idsalidadet"].Value.ToString(), out int idDetalle))
                {
                    MessageBox.Show("No se pudo obtener el identificador del detalle seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var confirmacion = MessageBox.Show(
                    "¿Está seguro de eliminar el artículo seleccionado del detalle?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmacion != DialogResult.Yes)
                {
                    return;
                }

                string sqlDelete = "DELETE FROM salidadetinv WHERE idsalidadet = @IdInvDet";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlDelete;
                command.Parameters.Add("@IdInvDet", SqlDbType.Int).Value = idDetalle;

                int filasAfectadas = await command.ExecuteNonQueryAsync();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Renglón eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await CargarDetalleDoctoscpAsync();
                }
                else
                {
                    MessageBox.Show("No se encontró el registro a eliminar en la base de datos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al intentar eliminar el registro: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarConceptosAsync()
        {
            try
            {
                if (_context == null) return;
                if (conceptotxt == null) return;

                var listaConceptos = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
                    _context.Set<Sisvta.Domain.Entities.Concepto>()
                        .Select(c => new { id_concepto = c.IdConceptosProv, descripcion = c.DescripcionConcepto })
                );

                if (listaConceptos != null && listaConceptos.Count > 0)
                {
                    conceptotxt.DataSource = null;
                    conceptotxt.DisplayMember = "descripcion";
                    conceptotxt.ValueMember = "id_concepto";
                    conceptotxt.DataSource = listaConceptos;
                    conceptotxt.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los conceptos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarAlmacenesAsync()
        {
            try
            {
                if (_context == null) return;
                if (txalmacen == null) return;

                var listaAlmacenes = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
                    _context.Set<Sisvta.Domain.Entities.Almacen>()
                        .Select(a => new { id_almacen = a.IdAlmacen, nombre = a.Nombre })
                );

                if (listaAlmacenes != null && listaAlmacenes.Count > 0)
                {
                    txalmacen.DataSource = null;
                    txalmacen.DisplayMember = "nombre";
                    txalmacen.ValueMember = "id_almacen";
                    txalmacen.DataSource = listaAlmacenes;
                    txalmacen.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los almacenes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarClientesAsync()
        {
            try
            {
                if (_context == null) return;
                if (txcliente == null) return;

                var listaClientes = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
                    _context.Set<Sisvta.Domain.Entities.Cliente>()
                        .Select(cl => new { id_empros = cl.Id, nombre = cl.Nombre })
                );

                if (listaClientes != null && listaClientes.Count > 0)
                {
                    txcliente.DataSource = null;
                    txcliente.DisplayMember = "nombre";
                    txcliente.ValueMember = "id_empros";
                    txcliente.DataSource = listaClientes;
                    txcliente.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los clientes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txfolio.Text))
                {
                    MessageBox.Show("El campo Folio es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txfolio.Focus();
                    return;
                }

                if (conceptotxt.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione un concepto válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    conceptotxt.Focus();
                    return;
                }

                if (txalmacen.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione un almacén válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txalmacen.Focus();
                    return;
                }

                if (txcliente.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione un cliente válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txcliente.Focus();
                    return;
                }

                string folio = txfolio.Text.Trim();
                DateTime fecha = txfecha.Value;
                int idConcepto = Convert.ToInt32(conceptotxt.SelectedValue);
                int idAlmacen = Convert.ToInt32(txalmacen.SelectedValue);
                int idCliente = Convert.ToInt32(txcliente.SelectedValue);
                string observacion = txobservacion.Text.Trim();
                string proyecto = txproyecto.Text.Trim();
                string aquien = txaquien.Text.Trim();

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                if (_idSalidaFiltro == 0)
                {
                    string sqlInsert = @"
                        INSERT INTO salidainv (folio, fecha, concepto, almacen, cliente, observaciones, proyecto, aquien) 
                        VALUES (@folio, @fecha, @concepto, @almacen, @cliente, @observaciones, @proyecto, @aquien);
                        SELECT SCOPE_IDENTITY();";

                    using var command = connection.CreateCommand();
                    command.CommandText = sqlInsert;
                    command.Parameters.Add("@folio", SqlDbType.NVarChar).Value = folio;
                    command.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                    command.Parameters.Add("@concepto", SqlDbType.Int).Value = idConcepto;
                    command.Parameters.Add("@almacen", SqlDbType.Int).Value = idAlmacen;
                    command.Parameters.Add("@cliente", SqlDbType.Int).Value = idCliente;
                    command.Parameters.Add("@observaciones", SqlDbType.NVarChar).Value = (object)observacion ?? DBNull.Value;
                    command.Parameters.Add("@proyecto", SqlDbType.NVarChar).Value = (object)proyecto ?? DBNull.Value;
                    command.Parameters.Add("@aquien", SqlDbType.NVarChar).Value = (object)aquien ?? DBNull.Value;

                    var nuevoIdObj = await command.ExecuteScalarAsync();

                    if (nuevoIdObj != null && int.TryParse(nuevoIdObj.ToString(), out int nuevoId))
                    {
                        MessageBox.Show("Salida de inventario guardada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    string sqlUpdate = @"
                        UPDATE salidainv 
                        SET folio = @folio, 
                            fecha = @fecha, 
                            concepto = @concepto, 
                            almacen = @almacen, 
                            cliente = @cliente, 
                            observaciones = @observaciones, 
                            proyecto = @proyecto, 
                            aquien = @aquien 
                        WHERE idsalida = @idSalida";

                    using var command = connection.CreateCommand();
                    command.CommandText = sqlUpdate;
                    command.Parameters.Add("@folio", SqlDbType.NVarChar).Value = folio;
                    command.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                    command.Parameters.Add("@concepto", SqlDbType.Int).Value = idConcepto;
                    command.Parameters.Add("@almacen", SqlDbType.Int).Value = idAlmacen;
                    command.Parameters.Add("@cliente", SqlDbType.Int).Value = idCliente;
                    command.Parameters.Add("@observaciones", SqlDbType.NVarChar).Value = (object)observacion ?? DBNull.Value;
                    command.Parameters.Add("@proyecto", SqlDbType.NVarChar).Value = (object)proyecto ?? DBNull.Value;
                    command.Parameters.Add("@aquien", SqlDbType.NVarChar).Value = (object)aquien ?? DBNull.Value;
                    command.Parameters.Add("@idSalida", SqlDbType.Int).Value = _idSalidaFiltro;

                    int filasAfectadas = await command.ExecuteNonQueryAsync();

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Salida de inventario actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo actualizar el registro en la base de datos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el registro maestro de salida: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnuevo_Click(object sender, EventArgs e)
        {
            // Limpia los campos para la captura de un nuevo registro en salidainv
         
            LimpiarFormularioNuevo();
        }
    }
}