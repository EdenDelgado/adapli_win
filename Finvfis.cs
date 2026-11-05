using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Entities;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Finvfis : Form
    {
        private readonly ApplicationDbContext _context;
        private readonly int _idinvfisFiltro;

        // 1. Constructor estándar (por si se abre desde el menú principal)
        public Finvfis(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idinvfisFiltro = 0;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

            // Carga inicial segura y secuencial desde el constructor
            _ = InicializarFormularioAsync();
        }

        // 2. Constructor con parámetro (para cuando se invoca haciendo doble clic o nuevo)
        public Finvfis(ApplicationDbContext context, int idEntrada)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idinvfisFiltro = idEntrada;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

            // Carga inicial segura y secuencial desde el constructor
            _ = InicializarFormularioAsync();
        }

        private async Task InicializarFormularioAsync()
        {
            // Primero cargamos los catálogos obligatoriamente para que los ComboBox tengan elementos
            await CargarAlmacenesAsync();
            await CargarUsuariosAsync();

            // Después cargamos el maestro e intentamos asignar los valores seleccionados
            await CargarMaestroDoctoscpAsync();
        }

        private async void Finvfis_Load(object sender, EventArgs e)
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

                // Si el ID es 0 (Nuevo Registro), limpiamos y salimos sin mostrar datos antiguos
                if (_idinvfisFiltro == 0)
                {
                    dataGridView1.DataSource = null;
                    dataGridView2.DataSource = null;
                    txfolio.Clear();
                    txfecha.Value = DateTime.Now;
                    txalmacen.SelectedIndex = -1;
                    txusuario.SelectedIndex = -1;
                    return;
                }

                string sqlQuery = @"    
                    SELECT 
                        id_invfisico AS Idinvfisico, 
                        fecha AS Fecha,
                        usuario AS Usuario, 
                        u.nombre as Nombre, 
                        u.idAlmacen as Almacen, 
                        a.nombre as Nomalmacen
                    FROM invfisico as i 
                    LEFT JOIN usuarios as u ON i.usuario = u.id_usuario
                    LEFT JOIN almacen as a ON u.idAlmacen = a.id_almacen
                    WHERE id_invfisico = @IdinvfisFiltro";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;
                command.Parameters.AddWithValue("@IdinvfisFiltro", _idinvfisFiltro);

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

                    txfolio.Text = dataGridView1.CurrentRow.Cells["Idinvfisico"].Value?.ToString() ?? string.Empty;

                    if (dataGridView1.CurrentRow.Cells["Fecha"].Value != DBNull.Value &&
                        DateTime.TryParse(dataGridView1.CurrentRow.Cells["Fecha"].Value?.ToString(), out DateTime fechaReg))
                    {
                        txfecha.Value = fechaReg;
                    }

                    // Asignación correcta a los ComboBox usando SelectedValue
                    if (dataGridView1.CurrentRow.Cells["Almacen"].Value != DBNull.Value)
                    {
                        txalmacen.SelectedValue = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Almacen"].Value);
                    }

                    if (dataGridView1.CurrentRow.Cells["Usuario"].Value != DBNull.Value)
                    {
                        txusuario.SelectedValue = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Usuario"].Value);
                    }
                }
                else
                {
                    dataGridView2.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el maestro invfisico: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarDetalleDoctoscpAsync()
        {
            try
            {
                if (_idinvfisFiltro == 0 || dataGridView1.CurrentRow == null ||
                    dataGridView1.CurrentRow.Cells["Idinvfisico"].Value == null ||
                    dataGridView1.CurrentRow.Cells["Idinvfisico"].Value == DBNull.Value)
                {
                    dataGridView2.DataSource = null;
                    return;
                }

                int idInvfisico = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Idinvfisico"].Value);

                string sqlQuery = @"
                    SELECT 
                        invfisico AS Invfisico, 
                        id_invfisdet AS Idinvfisdet, 
                        articulo AS Articulo, 
                        s.clave AS ClaveProducto,
                        s.s_nombre AS NombreProducto,
                        cantidad,
                        cantidad_real                        
                    FROM invfisdet as d 
                    LEFT JOIN servicios s ON d.articulo = s.id_servicio
                    WHERE Invfisico = @IdInvfisico";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;
                command.Parameters.AddWithValue("@IdInvfisico", idInvfisico);

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

                if (dataGridView2.Columns.Contains("Articulo"))
                    dataGridView2.Columns["Articulo"].Visible = false;

                if (dataGridView2.Columns.Contains("Invfisico"))
                    dataGridView2.Columns["Invfisico"].Visible = false;

                if (dataGridView2.Columns.Contains("Idinvfisdet"))
                    dataGridView2.Columns["Idinvfisdet"].Visible = false;

                if (dataGridView2.Columns.Contains("cantidad_real"))
                {
                    dataGridView2.Columns["cantidad_real"].DefaultCellStyle.Format = "N3";
                    dataGridView2.Columns["cantidad_real"].Width = 120;
                    dataGridView2.Columns["cantidad_real"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dataGridView2.Columns.Contains("cantidad"))
                {
                    dataGridView2.Columns["cantidad"].DefaultCellStyle.Format = "N3";
                    dataGridView2.Columns["cantidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                dataGridView2.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el detalle de invfisico: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private async Task CargarUsuariosAsync()
        {
            try
            {
                if (_context == null) return;
                if (txusuario == null) return;

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
                if (dataGridView1.CurrentRow == null ||
                    dataGridView1.CurrentRow.Cells["IdInvfisico"].Value == null ||
                    dataGridView1.CurrentRow.Cells["IdInvfisico"].Value == DBNull.Value)
                {
                    MessageBox.Show("Seleccione una entrada de invfisico válida en el catálogo maestro antes de agregar detalles.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string idMovimiento = dataGridView1.CurrentRow.Cells["IdInvfisico"].Value.ToString();

                if (string.IsNullOrEmpty(idMovimiento))
                {
                    MessageBox.Show("El identificador del movimiento de invfisico no es válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var fdinvfis = new Fdinvfis(_context, idMovimiento))
                {
                    if (fdinvfis.ShowDialog() == DialogResult.OK)
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
                    dataGridView2.CurrentRow.Cells["Idinvfisdet"].Value == null ||
                    dataGridView2.CurrentRow.Cells["Idinvfisdet"].Value == DBNull.Value)
                {
                    MessageBox.Show("Seleccione un renglón del detalle de invfisico que desee eliminar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(dataGridView2.CurrentRow.Cells["Idinvfisdet"].Value.ToString(), out int idDetalle))
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

                string sqlDelete = "DELETE FROM invfisdet WHERE id_invfisdet = @IdInvfisdet";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlDelete;
                command.Parameters.Add("@IdInvfisdet", SqlDbType.Int).Value = idDetalle;
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
                if (txalmacen.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione un almacén válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txalmacen.Focus();
                    return;
                }

                if (txusuario.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione un usuario válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txusuario.Focus();
                    return;
                }

                DateTime fecha = txfecha.Value;
                int idUsuario = Convert.ToInt32(txusuario.SelectedValue);

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                if (_idinvfisFiltro == 0)
                {
                    // ==========================================
                    // NUEVO REGISTRO (INSERT)
                    // ==========================================
                    string sqlInsert = @"
                        INSERT INTO invfisico (fecha, usuario) 
                        VALUES (@fecha, @usuario);
                        SELECT SCOPE_IDENTITY();";

                    using var command = connection.CreateCommand();
                    command.CommandText = sqlInsert;
                    command.Parameters.Add("@usuario", SqlDbType.Int).Value = idUsuario;
                    command.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;

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
                    // ACTUALIZACIÓN (UPDATE) - CORREGIDO EL PARÁMETRO @idInvfisico
                    // ==========================================
                    string sqlUpdate = @"
                        UPDATE invfisico 
                        SET  
                            fecha = @fecha, 
                            usuario = @usuario
                        WHERE id_invfisico = @idInvfisico";

                    using var command = connection.CreateCommand();
                    command.CommandText = sqlUpdate;
                    command.Parameters.Add("@usuario", SqlDbType.Int).Value = idUsuario;
                    command.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                    command.Parameters.Add("@idInvfisico", SqlDbType.Int).Value = _idinvfisFiltro; // Corregido de idInvfisco a idInvfisico

                    int filasAfectadas = await command.ExecuteNonQueryAsync();

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Entrada de invfis actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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