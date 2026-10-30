using System;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Entities;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Fdev : Form
    {
        private readonly ApplicationDbContext _context;
        private readonly int _idEntradaFiltro;

        // 1. Constructor estándar (por si se abre desde el menú principal)
        public Fdev(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idEntradaFiltro = 0;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            _ = InicializarFormularioAsync();
        }

        // 2. Constructor con parámetro (para cuando se invoca haciendo doble clic desde el catálogo)
        public Fdev(ApplicationDbContext context, int idEntrada)
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
            await CargarConceptosAsync();
            await CargarUsuariosAsync();
            await CargarMaestroDoctoscpAsync();
        }

        private async void Fccom_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
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
                // SI ES UN NUEVO REGISTRO (ID = 0), LIMPIAMOS CONTROLES Y SALIMOS
                // =========================================================================
                if (_idEntradaFiltro == 0)
                {
                    dataGridView1.DataSource = null;
                    dataGridView2.DataSource = null;
                    txfolio.Clear();
                    txentrada.Clear();
                    txdoc_origen.Clear();
                    txnotacredito.Clear();
                    txobservacion.Clear();
                    txfecha.Value = DateTime.Now;
                    txusuario.SelectedIndex = -1;
                    txalmacen.SelectedIndex = -1;
                    conceptotxt.SelectedIndex = -1;
                    return;
                }

                string sqlQuery = @"
                    SELECT 
                        d.id_Devolucion AS Iddevolucion, 
                        d.foliodevolucion AS Folio, 
                        d.id_VentaDcc AS Docto_orig, 
                        d.fecha AS Fecha,
                        d.id_Entrada AS Entrada, 
                        i.id_inventario as Idinventario,
                        d.usercreator as usuario, 
                        u.nombre as Nomusuario,
                        a.nombre as NomAlmacen,
                        d.id_NotaCredito as NotaCredito,
                        i.concepto as Concepto, 
                        i.observaciones as Observaciones
                    FROM devolucion as d 
                    LEFT JOIN inventario as i ON d.id_Entrada = i.id_inventario
                    LEFT JOIN almacen as a ON i.almacen = a.id_almacen 
                    LEFT JOIN usuarios as u ON d.usercreator = u.id_usuario
                    LEFT JOIN concepto c ON i.concepto = c.id_conceptosprov
                    WHERE d.id_Devolucion = @IdEntradaFiltro";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;
                command.Parameters.AddWithValue("@IdEntradaFiltro", _idEntradaFiltro);

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

                    if (dataGridView1.CurrentRow.Cells["Fecha"].Value != DBNull.Value &&
                        DateTime.TryParse(dataGridView1.CurrentRow.Cells["Fecha"].Value?.ToString(), out DateTime fechaReg))
                    {
                        txfecha.Value = fechaReg;
                    }

                    if (dataGridView1.CurrentRow.Cells["usuario"].Value != DBNull.Value)
                    {
                        txusuario.SelectedValue = Convert.ToInt32(dataGridView1.CurrentRow.Cells["usuario"].Value);
                    }

                    txentrada.Text = dataGridView1.CurrentRow.Cells["IdInventario"].Value?.ToString() ?? string.Empty;
                    txalmacen.Text = dataGridView1.CurrentRow.Cells["NomAlmacen"].Value?.ToString() ?? string.Empty;
                    txdoc_origen.Text = dataGridView1.CurrentRow.Cells["Docto_orig"].Value?.ToString() ?? string.Empty;
                    txnotacredito.Text = dataGridView1.CurrentRow.Cells["NotaCredito"].Value?.ToString() ?? string.Empty;

                    if (dataGridView1.CurrentRow.Cells["Concepto"].Value != DBNull.Value)
                    {
                        conceptotxt.SelectedValue = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Concepto"].Value);
                    }

                    txobservacion.Text = dataGridView1.CurrentRow.Cells["Observaciones"].Value?.ToString() ?? string.Empty;
                }
                else
                {
                    dataGridView2.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el maestro de devoluciones: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarDetalleDoctoscpAsync()
        {
            try
            {
                if (_idEntradaFiltro == 0 || dataGridView1.CurrentRow == null ||
                    dataGridView1.CurrentRow.Cells["Idinventario"].Value == null ||
                    dataGridView1.CurrentRow.Cells["Idinventario"].Value == DBNull.Value)
                {
                    dataGridView2.DataSource = null;
                    return;
                }

                int idInventario = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Idinventario"].Value);

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

                if (dataGridView2.Columns.Contains("NombreProducto"))
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
        }

        private void regresabt_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async Task CargarUsuariosAsync()
        {
            try
            {
                if (_context == null || txusuario == null) return;

                var listaUsuarios = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
                    _context.Set<Sisvta.Domain.Entities.Usuario>()
                        .Select(a => new { id_usuario = a.IdUsuario, nombre = a.Nombre })
                );

                if (listaUsuarios != null && listaUsuarios.Count > 0)
                {
                    txusuario.DataSource = null;
                    txusuario.DisplayMember = "nombre";
                    txusuario.ValueMember = "id_usuario";
                    txusuario.DataSource = listaUsuarios;
                    txusuario.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los usuarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btagregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_idEntradaFiltro == 0)
                {
                    MessageBox.Show("Debe guardar el registro maestro de la devolución antes de agregar detalles.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dataGridView1.CurrentRow == null ||
                    dataGridView1.CurrentRow.Cells["Idinventario"].Value == null ||
                    dataGridView1.CurrentRow.Cells["Idinventario"].Value == DBNull.Value)
                {
                    MessageBox.Show("Seleccione una entrada válida en el catálogo maestro antes de agregar detalles.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string idMovimiento = dataGridView1.CurrentRow.Cells["Idinventario"].Value.ToString();

                using (var formDetalle = new Fddev(_context, idMovimiento))
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
                    MessageBox.Show("Seleccione un renglón del detalle que desee eliminar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(dataGridView2.CurrentRow.Cells["Entradadet"].Value.ToString(), out int idDetalle))
                {
                    MessageBox.Show("No se pudo obtener el identificador del detalle seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!int.TryParse(dataGridView2.CurrentRow.Cells["Entrada"].Value.ToString(), out int idEntrada))
                {
                    MessageBox.Show("No se pudo obtener el identificador del detalle seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var confirmacion = MessageBox.Show(
                    "¿Está seguro de eliminar el artículo seleccionado del detalle?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmacion != DialogResult.Yes) return;

                string sqlDelete = "DELETE FROM inventario_detalles WHERE id_invdet = @IdInvDet AND movinventario = @IdEntrada";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlDelete;
                command.Parameters.Add("@IdEntrada", SqlDbType.Int).Value = idEntrada;
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
                if (string.IsNullOrWhiteSpace(txfolio.Text))
                {
                    MessageBox.Show("El campo Folio es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txfolio.Focus();
                    return;
                }

                if (txusuario.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione un usuario válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txusuario.Focus();
                    return;
                }

                if (txalmacen.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione un almacén válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txalmacen.Focus();
                    return;
                }

                if (conceptotxt.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione un concepto válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    conceptotxt.Focus();
                    return;
                }

                DateTime fecha = txfecha.Value;
                int idUsuario = Convert.ToInt32(txusuario.SelectedValue);
                string folio = txfolio.Text.Trim();
                int almacen = Convert.ToInt32(txalmacen.SelectedValue);
                int concepto = Convert.ToInt32(conceptotxt.SelectedValue);

                int doc_origen = int.TryParse(txdoc_origen.Text.Trim(), out var dOrg) ? dOrg : 0;
                int nota_credito = int.TryParse(txnotacredito.Text.Trim(), out var nCred) ? nCred : 0;
                string observacion = txobservacion.Text.Trim();

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                if (_idEntradaFiltro == 0)
                {
                    // 1. Insertar el movimiento en inventario
                    string sqlInsertent = @"
                        INSERT INTO inventario (almacen, folio, fecha, concepto, observaciones, usercreator) 
                        VALUES (@almacen, @folio, @fecha, @concepto, @observaciones, @usuario);
                        SELECT SCOPE_IDENTITY();";

                    using var command0 = connection.CreateCommand();
                    command0.CommandText = sqlInsertent;
                    command0.Parameters.Add("@almacen", SqlDbType.Int).Value = almacen;
                    command0.Parameters.Add("@folio", SqlDbType.NVarChar).Value = folio;
                    command0.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                    command0.Parameters.Add("@concepto", SqlDbType.Int).Value = concepto;
                    command0.Parameters.Add("@observaciones", SqlDbType.NVarChar).Value = observacion;
                    command0.Parameters.Add("@usuario", SqlDbType.Int).Value = idUsuario;

                    var nuevoIdObj0 = await command0.ExecuteScalarAsync();

                    if (nuevoIdObj0 == null || !int.TryParse(nuevoIdObj0.ToString(), out int nuevoId0))
                    {
                        MessageBox.Show("No se pudo generar el registro en inventario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    txentrada.Text = nuevoId0.ToString();

                    // 2. Insertar la devolución vinculada al ID de inventario recién creado
                    string sqlInsert = @"
                        INSERT INTO devolucion (foliodevolucion, fecha, id_ventaDcc, id_NotaCredito, id_entrada, usercreator) 
                        VALUES (@folio, @fecha, @doc_orig, @notacredito, @entrada, @usuario);
                        SELECT SCOPE_IDENTITY();";

                    using var command = connection.CreateCommand();
                    command.CommandText = sqlInsert;
                    command.Parameters.Add("@entrada", SqlDbType.Int).Value = nuevoId0;
                    command.Parameters.Add("@folio", SqlDbType.NVarChar).Value = folio;
                    command.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                    command.Parameters.Add("@usuario", SqlDbType.Int).Value = idUsuario;
                    command.Parameters.Add("@doc_orig", SqlDbType.Int).Value = doc_origen;
                    command.Parameters.Add("@notacredito", SqlDbType.Int).Value = nota_credito;

                    var nuevoIdObj = await command.ExecuteScalarAsync();

                    if (nuevoIdObj != null && int.TryParse(nuevoIdObj.ToString(), out int nuevoId))
                    {
                        MessageBox.Show("Devolución guardada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    // 1. Actualizar la tabla devolucion
                    string sqlUpdateDev = @"
                        UPDATE devolucion 
                        SET foliodevolucion = @folio, 
                            fecha = @fecha, 
                            id_NotaCredito = @notacredito,
                            usercreator = @usuario 
                        WHERE id_Devolucion = @idDevolucion";

                    using var command = connection.CreateCommand();
                    command.CommandText = sqlUpdateDev;
                    command.Parameters.Add("@folio", SqlDbType.NVarChar).Value = folio;
                    command.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                    command.Parameters.Add("@usuario", SqlDbType.Int).Value = idUsuario;
                    command.Parameters.Add("@notacredito", SqlDbType.Int).Value = nota_credito;
                    command.Parameters.Add("@idDevolucion", SqlDbType.Int).Value = _idEntradaFiltro;

                    int filasAfectadas = await command.ExecuteNonQueryAsync();

                    // 2. Actualizar también el registro asociado en inventario si existe
                    if (!string.IsNullOrWhiteSpace(txentrada.Text) && int.TryParse(txentrada.Text, out int idInventario))
                    {
                        string sqlUpdateInv = @"
                            UPDATE inventario 
                            SET almacen = @almacen, 
                                folio = @folio, 
                                fecha = @fecha, 
                                concepto = @concepto, 
                                observaciones = @observaciones 
                            WHERE id_inventario = @idInventario";

                        using var commandInv = connection.CreateCommand();
                        commandInv.CommandText = sqlUpdateInv;
                        commandInv.Parameters.Add("@almacen", SqlDbType.Int).Value = almacen;
                        commandInv.Parameters.Add("@folio", SqlDbType.NVarChar).Value = folio;
                        commandInv.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                        commandInv.Parameters.Add("@concepto", SqlDbType.Int).Value = concepto;
                        commandInv.Parameters.Add("@observaciones", SqlDbType.NVarChar).Value = observacion;
                        commandInv.Parameters.Add("@idInventario", SqlDbType.Int).Value = idInventario;

                        await commandInv.ExecuteNonQueryAsync();
                    }

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Devolución actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}