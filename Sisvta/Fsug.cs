using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using sisvta;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Fsug : Form
    {
        private readonly ApplicationDbContext _context;
        private readonly int _idEntradaFiltro;

        // 1. Constructor estándar (por si se abre desde el menú principal)
        public Fsug(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idEntradaFiltro = 0;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            _ = InicializarFormularioAsync();
        }

        // 2. Constructor con parámetro (para cuando se invoca haciendo doble clic desde el catálogo)
        public Fsug(ApplicationDbContext context, int idEntrada)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idEntradaFiltro = idEntrada;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            _ = InicializarFormularioAsync();
        }

        private async Task InicializarFormularioAsync()
        {
            await CargarAlmacenesAsync();
            await CargarProveedoresAsync();
            await CargarConceptosAsync(); // <-- Cargamos el ComboBox de conceptos
            await CargarMaestroDoctoscpAsync();
        }

        private async void Fccom_Load(object sender, EventArgs e)
        {
            await CargarAlmacenesAsync();
            await CargarProveedoresAsync();
            await CargarConceptosAsync(); // <-- Aseguramos la carga también en el Load
            await CargarMaestroDoctoscpAsync();
        }

        // Método para cargar Almacenes en 'txalmacen'
        private async Task CargarAlmacenesAsync()
        {
            try
            {
                if (_context == null) return;

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

        // Método para cargar Proveedores en 'txproveedor'
        private async Task CargarProveedoresAsync()
        {
            try
            {
                if (_context == null) return;

                var listaProveedores = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
                    _context.Set<Sisvta.Domain.Entities.Proveedores>()
                        .Select(p => new { id_proveedor = p.Id, nombre = p.Nombre })
                );

                if (listaProveedores != null && listaProveedores.Count > 0)
                {
                    txproveedor.DataSource = null;
                    txproveedor.DisplayMember = "nombre";
                    txproveedor.ValueMember = "id_proveedor";
                    txproveedor.DataSource = listaProveedores;
                    txproveedor.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los proveedores: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================================
        // NUEVO MÉTODO PARA CARGAR CONCEPTOS EN 'conceptotxt'
        // =========================================================================
        private async Task CargarConceptosAsync()
        {
            try
            {
                if (_context == null) return;

                // Verificamos si el control conceptotxt existe en el formulario
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

        public async Task CargarMaestroDoctoscpAsync(string filtroProveedor = null)
        {
            try
            {
                if (_context == null)
                {
                    MessageBox.Show("El contexto de base de datos es nulo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string sqlQuery = @"
                    SELECT 
                        i.id_factura AS Idfactura, 
                        i.folio AS Folio, 
                        i.fecha AS Fecha,
                        i.concepto AS Concepto, 
                        c.concepto AS NomConcepto,
                        d.proveedor AS Proveedor, p.nombre AS NomProveedor,
                        i.almacen AS IdAlmacen, a.nombre AS NomAlmacen,
                        i.id_inventario AS IdInventario,
                        i.observaciones AS Observacion,
                        d.proveedor AS IdProveedor 
                    FROM inventario as i 
                    left join doctoscc as d on i.id_factura=d.id_factura
                    LEFT JOIN concepto as c ON i.concepto = c.id_conceptosprov 
                    LEFT JOIN proveedores as p ON d.proveedor = p.id_proveedor 
                    LEFT JOIN almacen as a ON i.almacen = a.id_almacen";

                if (_idEntradaFiltro > 0)
                {
                    sqlQuery += " WHERE i.id_inventario = @IdEntradaFiltro";
                }

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                if (_idEntradaFiltro > 0)
                {
                    command.Parameters.AddWithValue("@IdEntradaFiltro", _idEntradaFiltro);
                }

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
                    txfecha.Text = dataGridView1.CurrentRow.Cells["Fecha"].Value?.ToString() ?? string.Empty;
                    txmovinv.Text = dataGridView1.CurrentRow.Cells["IdInventario"].Value?.ToString() ?? string.Empty;

                    // Asignar selección del ComboBox de Concepto
                    if (dataGridView1.CurrentRow.Cells["Concepto"].Value != null &&
                        int.TryParse(dataGridView1.CurrentRow.Cells["Concepto"].Value.ToString(), out int idCon))
                    {
                        conceptotxt.SelectedValue = idCon;
                    }

                    // Asignar selección del ComboBox de Almacén
                    if (dataGridView1.CurrentRow.Cells["IdAlmacen"].Value != null &&
                        int.TryParse(dataGridView1.CurrentRow.Cells["IdAlmacen"].Value.ToString(), out int idAlm))
                    {
                        txalmacen.SelectedValue = idAlm;
                    }

                    // Asignar selección del ComboBox de Proveedor
                    if (dataGridView1.CurrentRow.Cells["IdProveedor"].Value != null &&
                        int.TryParse(dataGridView1.CurrentRow.Cells["IdProveedor"].Value.ToString(), out int idProv))
                    {
                        txproveedor.SelectedValue = idProv;
                    }

                    txobservacion.Text = dataGridView1.CurrentRow.Cells["Observacion"].Value?.ToString() ?? string.Empty;
                }
                else
                {
                    dataGridView2.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el maestro inventario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarDetalleDoctoscpAsync()
        {
            try
            {
                if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Cells["IdInventario"].Value == null ||
                    dataGridView1.CurrentRow.Cells["IdInventario"].Value == DBNull.Value)
                {
                    dataGridView2.DataSource = null;
                    return;
                }

                int idInventario = Convert.ToInt32(dataGridView1.CurrentRow.Cells["IdInventario"].Value);

                string sqlQuery = @"
                    SELECT 
                        movinventario AS Entrada, 
                        id_invdet AS Entradadet, 
                        cantidad AS Cantidad,
                        articulo AS Articulo, 
                        s.clave AS ClaveProducto,
                        s.s_nombre AS NombreProducto,
                        preciou AS Precio, 
                        (cantidad * preciou) AS Importe
                    FROM inventario_detalles as d 
                    LEFT JOIN servicios s ON d.articulo = s.id_servicio
                    WHERE movinventario = @idInventario";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;
                command.Parameters.AddWithValue("@IdInventario", idInventario);

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

                if (dataGridView2.Columns.Contains("Entrada"))
                    dataGridView2.Columns["Entrada"].Visible = false;

                if (dataGridView2.Columns.Contains("Articulo"))
                    dataGridView2.Columns["Articulo"].Visible = false;

                if (dataGridView2.Columns.Contains("Precio"))
                {
                    dataGridView2.Columns["Precio"].DefaultCellStyle.Format = "C2";
                    dataGridView2.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView2.Columns["Precio"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dataGridView2.Columns.Contains("Importe"))
                {
                    dataGridView2.Columns["Importe"].DefaultCellStyle.Format = "C2";
                    dataGridView2.Columns["Importe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView2.Columns["Importe"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                dataGridView2.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el detalle de inventario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            await CargarDetalleDoctoscpAsync();

            if (dataGridView1.CurrentRow != null)
            {
                // Actualizar Concepto
                if (dataGridView1.CurrentRow.Cells["Concepto"].Value != null &&
                    int.TryParse(dataGridView1.CurrentRow.Cells["Concepto"].Value.ToString(), out int idCon))
                {
                    conceptotxt.SelectedValue = idCon;
                }

                // Actualizar Almacén
                if (dataGridView1.CurrentRow.Cells["IdAlmacen"].Value != null &&
                    int.TryParse(dataGridView1.CurrentRow.Cells["IdAlmacen"].Value.ToString(), out int idAlm))
                {
                    txalmacen.SelectedValue = idAlm;
                }

                // Actualizar Proveedor
                if (dataGridView1.CurrentRow.Cells["IdProveedor"].Value != null &&
                    int.TryParse(dataGridView1.CurrentRow.Cells["IdProveedor"].Value.ToString(), out int idProv))
                {
                    txproveedor.SelectedValue = idProv;
                }
            }
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
                    dataGridView1.CurrentRow.Cells["IdInventario"].Value == null ||
                    dataGridView1.CurrentRow.Cells["IdInventario"].Value == DBNull.Value)
                {
                    MessageBox.Show("Seleccione una entrada de inventario válida en el catálogo maestro antes de agregar detalles.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string idMovimiento = dataGridView1.CurrentRow.Cells["IdInventario"].Value.ToString();

                if (string.IsNullOrEmpty(idMovimiento))
                {
                    MessageBox.Show("El identificador del movimiento de inventario no es válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var formDetalle = new Fdent(_context, idMovimiento))
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
                    dataGridView2.CurrentRow.Cells["Entradadet"].Value == null ||
                    dataGridView2.CurrentRow.Cells["Entradadet"].Value == DBNull.Value)
                {
                    MessageBox.Show("Seleccione un renglón del detalle de inventario que desee eliminar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(dataGridView2.CurrentRow.Cells["Entradadet"].Value.ToString(), out int idDetalle))
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

                string sqlDelete = "DELETE FROM inventario_detalles WHERE id_invdet = @IdInvDet";

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

        private async void btGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validaciones básicas
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

                DateTime fecha = txfecha.Value;


                int idConcepto = Convert.ToInt32(conceptotxt.SelectedValue);
                int idAlmacen = Convert.ToInt32(txalmacen.SelectedValue);
                string observaciones = txobservacion.Text.Trim();
                string folio = txfolio.Text.Trim();

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                if ((_idEntradaFiltro == 0) || (_idEntradaFiltro == 999999))
                {
                    // ==========================================
                    // NUEVO REGISTRO (INSERT) - Incluyendo concepto
                    // ==========================================
                    string sqlInsert = @"
                        INSERT INTO inventario (folio, fecha, concepto, almacen, observaciones) 
                        VALUES (@folio, @fecha, @concepto, @almacen, @observaciones);
                        SELECT SCOPE_IDENTITY();";

                    using var command = connection.CreateCommand();
                    command.CommandText = sqlInsert;
                    command.Parameters.Add("@folio", SqlDbType.NVarChar).Value = folio;
                    command.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                    command.Parameters.Add("@concepto", SqlDbType.Int).Value = idConcepto;
                    command.Parameters.Add("@almacen", SqlDbType.Int).Value = idAlmacen;
                    command.Parameters.Add("@observaciones", SqlDbType.NVarChar).Value = observaciones;

                    var nuevoIdObj = await command.ExecuteScalarAsync();

                    if (nuevoIdObj != null && int.TryParse(nuevoIdObj.ToString(), out int nuevoId))
                    {
                        MessageBox.Show("Entrada de inventario guardada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    // ==========================================
                    // ACTUALIZACIÓN (UPDATE) - Incluyendo concepto
                    // ==========================================
                    string sqlUpdate = @"
                        UPDATE inventario 
                        SET folio = @folio, 
                            fecha = @fecha, 
                            concepto = @concepto,
                            almacen = @almacen, 
                            observaciones = @observaciones 
                        WHERE id_inventario = @idInventario";

                    using var command = connection.CreateCommand();
                    command.CommandText = sqlUpdate;
                    command.Parameters.Add("@folio", SqlDbType.NVarChar).Value = folio;
                    command.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                    command.Parameters.Add("@concepto", SqlDbType.Int).Value = idConcepto;
                    command.Parameters.Add("@almacen", SqlDbType.Int).Value = idAlmacen;
                    command.Parameters.Add("@observaciones", SqlDbType.NVarChar).Value = observaciones;
                    command.Parameters.Add("@idInventario", SqlDbType.Int).Value = _idEntradaFiltro;

                    int filasAfectadas = await command.ExecuteNonQueryAsync();

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Entrada de inventario actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show($"Error al guardar el registro maestro: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}