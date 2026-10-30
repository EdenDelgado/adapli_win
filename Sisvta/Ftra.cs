using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Entities;
using Sisvta.Infrastructure.Persistence.Contexts;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisvta
{
    public partial class Ftra : Form
    {
        private readonly ApplicationDbContext _context;
        private readonly int _idTraspasoFiltro;

        // 1. Constructor estándar (por si se abre desde el menú principal o nuevo)
        public Ftra(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idTraspasoFiltro = 0;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            _ = InicializarFormularioAsync();
        }

        // 2. Constructor con parámetro (para cuando se invoca haciendo doble clic desde el catálogo)
        public Ftra(ApplicationDbContext context, int idTraspaso)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idTraspasoFiltro = idTraspaso;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            _ = InicializarFormularioAsync();
        }

        private async Task InicializarFormularioAsync()
        {
            await CargarAlmacenesoriAsync();
            await CargarAlmacenesdesAsync();
            await CargarConceptosAsync();
            await CargarMaestroDoctoscpAsync();
        }

        private async void Ftra_Load(object sender, EventArgs e)
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
                if (_idTraspasoFiltro == 0)
                {
                    dataGridView1.DataSource = null;
                    dataGridView2.DataSource = null;
                    txtraspaso.Clear();
                    txfolio.Clear();
                    txcomentarios.Clear();
                    txentrada.Clear();
                    txfecha.Value = DateTime.Now;
                    conceptotxt.SelectedIndex = -1;
                    txalmacenorigen.SelectedIndex = -1;
                    txalmacendestino.SelectedIndex = -1;
                    return;
                }

                string sqlQuery = @"
                    SELECT 
                        t.id_traspaso AS Idtraspaso, 
                        t.almacen_origen AS AlmacenOrigen, 
                        t.almacen_destino AS AlmacenDestino,
                        t.fecha AS Fecha,
                        t.concepto AS Concepto, 
                        c.concepto AS NomConcepto,
                        o.nombre AS NomAlmacenOrigen,
                        d.nombre AS NomAlmacenDestino,
                        t.Entrada, t.folio, t.comentarios
                    FROM traspaso as t 
                    LEFT JOIN concepto as c ON t.concepto = c.id_conceptosprov 
                    LEFT JOIN almacen as o ON t.almacen_origen = o.id_almacen
                    LEFT JOIN almacen as d ON t.almacen_destino = d.id_almacen
                    WHERE t.id_traspaso = @IdTraspasoFiltro";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;
                command.Parameters.AddWithValue("@IdTraspasoFiltro", _idTraspasoFiltro);

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

                    txtraspaso.Text = dataGridView1.CurrentRow.Cells["IdTraspaso"].Value?.ToString() ?? string.Empty;

                    if (dataGridView1.CurrentRow.Cells["Fecha"].Value != DBNull.Value &&
                        DateTime.TryParse(dataGridView1.CurrentRow.Cells["Fecha"].Value?.ToString(), out DateTime fechaReg))
                    {
                        txfecha.Value = fechaReg;
                    }

                    if (dataGridView1.CurrentRow.Cells["Concepto"].Value != DBNull.Value)
                    {
                        conceptotxt.SelectedValue = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Concepto"].Value);
                    }

                    txentrada.Text = dataGridView1.CurrentRow.Cells["Entrada"].Value?.ToString() ?? string.Empty;

                    if (dataGridView1.CurrentRow.Cells["AlmacenOrigen"].Value != DBNull.Value)
                    {
                        txalmacenorigen.SelectedValue = Convert.ToInt32(dataGridView1.CurrentRow.Cells["AlmacenOrigen"].Value);
                    }

                    if (dataGridView1.CurrentRow.Cells["AlmacenDestino"].Value != DBNull.Value)
                    {
                        txalmacendestino.SelectedValue = Convert.ToInt32(dataGridView1.CurrentRow.Cells["AlmacenDestino"].Value);
                    }

                    txfolio.Text = dataGridView1.CurrentRow.Cells["Folio"].Value?.ToString() ?? string.Empty;
                    txcomentarios.Text = dataGridView1.CurrentRow.Cells["Comentarios"].Value?.ToString() ?? string.Empty;
                }
                else
                {
                    dataGridView2.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el maestro de traspasos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarDetalleDoctoscpAsync()
        {
            try
            {
                if (_idTraspasoFiltro == 0 || dataGridView1.CurrentRow == null ||
                    dataGridView1.CurrentRow.Cells["IdTraspaso"].Value == null ||
                    dataGridView1.CurrentRow.Cells["IdTraspaso"].Value == DBNull.Value)
                {
                    dataGridView2.DataSource = null;
                    return;
                }

                int idTraspaso = Convert.ToInt32(dataGridView1.CurrentRow.Cells["IdTraspaso"].Value);

                string sqlQuery = @"
                    SELECT 
                        traspaso AS traspaso, 
                        idtraspasodet AS Traspasodet, 
                        qty AS Cantidad,
                        d.producto AS Producto, 
                        s.clave AS Clave,
                        s.s_nombre AS Nombre,
                        preciou AS Precio, 
                        (qty * preciou) AS Importe
                    FROM traspaso_det as d 
                    LEFT JOIN servicios s ON d.producto = s.id_servicio
                    WHERE traspaso = @idTraspaso";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;
                command.Parameters.AddWithValue("@idTraspaso", idTraspaso);

                using var reader = await command.ExecuteReaderAsync();
                var dataTable = new System.Data.DataTable();
                dataTable.Load(reader);

                dataGridView2.AutoGenerateColumns = true;
                dataGridView2.RowHeadersWidth = 20;
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = dataTable;

                if (dataGridView2.Columns.Contains("traspaso"))
                    dataGridView2.Columns["traspaso"].Visible = false;

                dataGridView2.RowTemplate.Height = 40;
                dataGridView2.CellBorderStyle = DataGridViewCellBorderStyle.None;

                dataGridView2.EnableHeadersVisualStyles = false;
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
                dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                dataGridView2.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);

                if (dataGridView2.Columns.Contains("Nombre"))
                    dataGridView2.Columns["Nombre"].Width = 260;

                if (dataGridView2.Columns.Contains("Producto"))
                    dataGridView2.Columns["Producto"].Visible = false;

                if (dataGridView2.Columns.Contains("Traspasodet"))
                    dataGridView2.Columns["Traspasodet"].Visible = false;

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
                MessageBox.Show($"Error al cargar el detalle del traspaso: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private async Task CargarConceptosAsync()
        {
            try
            {
                if (_context == null || conceptotxt == null) return;

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

        private async Task CargarAlmacenesoriAsync()
        {
            try
            {
                if (_context == null || txalmacenorigen == null) return;

                var listaAlmacenes = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
                    _context.Set<Sisvta.Domain.Entities.Almacen>()
                        .Select(a => new { id_almacen = a.IdAlmacen, nombre = a.Nombre })
                );

                if (listaAlmacenes != null && listaAlmacenes.Count > 0)
                {
                    txalmacenorigen.DataSource = null;
                    txalmacenorigen.DisplayMember = "nombre";
                    txalmacenorigen.ValueMember = "id_almacen";
                    txalmacenorigen.DataSource = listaAlmacenes;
                    txalmacenorigen.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los almacenes de origen: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarAlmacenesdesAsync()
        {
            try
            {
                if (_context == null || txalmacendestino == null) return;

                var listaAlmacenes = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
                    _context.Set<Sisvta.Domain.Entities.Almacen>()
                        .Select(a => new { id_almacen = a.IdAlmacen, nombre = a.Nombre })
                );

                if (listaAlmacenes != null && listaAlmacenes.Count > 0)
                {
                    txalmacendestino.DataSource = null;
                    txalmacendestino.DisplayMember = "nombre";
                    txalmacendestino.ValueMember = "id_almacen";
                    txalmacendestino.DataSource = listaAlmacenes;
                    txalmacendestino.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los almacenes de destino: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btagregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_idTraspasoFiltro == 0)
                {
                    MessageBox.Show("Debe guardar el registro maestro del traspaso antes de agregar detalles.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dataGridView1.CurrentRow == null ||
                    dataGridView1.CurrentRow.Cells["IdTraspaso"].Value == null ||
                    dataGridView1.CurrentRow.Cells["IdTraspaso"].Value == DBNull.Value)
                {
                    MessageBox.Show("Seleccione un traspaso de inventario válido en el catálogo maestro antes de agregar detalles.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string idMovimiento = dataGridView1.CurrentRow.Cells["IdTraspaso"].Value.ToString();

                using (var fdtra = new Fdtra(_context, idMovimiento))
                {
                    if (fdtra.ShowDialog() == DialogResult.OK)
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
                    dataGridView2.CurrentRow.Cells["Traspasodet"].Value == null ||
                    dataGridView2.CurrentRow.Cells["Traspasodet"].Value == DBNull.Value)
                {
                    MessageBox.Show("Seleccione un renglón del detalle de inventario que desee eliminar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string idTraspaso = dataGridView1.CurrentRow.Cells["IdTraspaso"].Value.ToString();

                if (!int.TryParse(dataGridView2.CurrentRow.Cells["Traspasodet"].Value.ToString(), out int idDetalle))
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

                string sqlDelete = "DELETE FROM traspaso_det WHERE idtraspasodet = @IdTraDet AND traspaso = @IdTraspaso";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlDelete;
                command.Parameters.Add("@IdTraDet", SqlDbType.Int).Value = idDetalle;
                command.Parameters.Add("@IdTraspaso", SqlDbType.Int).Value = Convert.ToInt32(idTraspaso);

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

                if (conceptotxt.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione un concepto válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    conceptotxt.Focus();
                    return;
                }

                if (txalmacenorigen.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione un almacén de origen válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txalmacenorigen.Focus();
                    return;
                }

                if (txalmacendestino.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione un almacén de destino válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txalmacendestino.Focus();
                    return;
                }

                int idAlmacenorigen = Convert.ToInt32(txalmacenorigen.SelectedValue);
                int idAlmacendestino = Convert.ToInt32(txalmacendestino.SelectedValue);
                int idConcepto = Convert.ToInt32(conceptotxt.SelectedValue);
                int idEntrada = int.TryParse(txentrada.Text, out var ent) ? ent : 0;
                DateTime fecha = txfecha.Value;
                string comentarios = txcomentarios.Text.Trim();
                string folio = txfolio.Text.Trim();

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                if (_idTraspasoFiltro == 0)
                {
                    string sqlInsert = @"
                        INSERT INTO traspaso (almacen_origen, almacen_destino, concepto, comentarios, entrada, fecha, folio) 
                        VALUES (@almacenorigen, @almacendestino, @concepto, @comentarios, @entrada, @fecha, @folio);
                        SELECT SCOPE_IDENTITY();";

                    using var command = connection.CreateCommand();
                    command.CommandText = sqlInsert;
                    command.Parameters.Add("@almacenorigen", SqlDbType.Int).Value = idAlmacenorigen;
                    command.Parameters.Add("@almacendestino", SqlDbType.Int).Value = idAlmacendestino;
                    command.Parameters.Add("@concepto", SqlDbType.Int).Value = idConcepto;
                    command.Parameters.Add("@comentarios", SqlDbType.NVarChar).Value = comentarios;
                    command.Parameters.Add("@entrada", SqlDbType.Int).Value = idEntrada;
                    command.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                    command.Parameters.Add("@folio", SqlDbType.NVarChar).Value = folio;

                    var nuevoIdObj = await command.ExecuteScalarAsync();

                    if (nuevoIdObj != null && int.TryParse(nuevoIdObj.ToString(), out int nuevoId))
                    {
                        MessageBox.Show("Traspaso guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    string sqlUpdate = @"
                        UPDATE traspaso 
                        SET almacen_origen = @almacen_origen, 
                            almacen_destino = @almacendestino, 
                            concepto = @concepto,
                            comentarios = @comentarios, 
                            fecha = @fecha, 
                            folio = @folio 
                        WHERE id_traspaso = @idTraspaso";

                    using var command = connection.CreateCommand();
                    command.CommandText = sqlUpdate;
                    command.Parameters.Add("@almacen_origen", SqlDbType.Int).Value = idAlmacenorigen;
                    command.Parameters.Add("@almacendestino", SqlDbType.Int).Value = idAlmacendestino;
                    command.Parameters.Add("@concepto", SqlDbType.Int).Value = idConcepto;
                    command.Parameters.Add("@comentarios", SqlDbType.NVarChar).Value = comentarios;
                    command.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                    command.Parameters.Add("@folio", SqlDbType.NVarChar).Value = folio;
                    command.Parameters.Add("@idTraspaso", SqlDbType.Int).Value = _idTraspasoFiltro;

                    int filasAfectadas = await command.ExecuteNonQueryAsync();

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Traspaso actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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