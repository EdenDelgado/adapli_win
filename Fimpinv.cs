using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisvta
{
    public partial class Fimpinv : Form
    {
        private readonly ApplicationDbContext _context;
        private readonly int _idEntradaFiltro;

        // 1. Constructor estándar (por si se abre desde el menú principal)
        public Fimpinv(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idEntradaFiltro = 0;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            _ = CargarMaestroDoctoscpAsync();
        }

        // 2. Constructor con parámetro (para cuando se invoca haciendo doble clic desde el catálogo)
        public Fimpinv(ApplicationDbContext context, int idEntrada)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idEntradaFiltro = idEntrada;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            _ = CargarMaestroDoctoscpAsync();
        }

        private async void Fccom_Load(object sender, EventArgs e)
        {
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

                string sqlQuery = @"    SELECT 
                        id_invfisico AS Idinvfisico, 
                        fecha AS Fecha,
                        usuario AS Usuario, u.nombre as Nombre,u.idAlmacen as Almacen,a.nombre as Nomalmacen
                    FROM invfisico as i left join usuarios as u on i.usuario=u.id_usuario
                        left join almacen as a on u.idAlmacen=a.id_almacen";

                if (_idEntradaFiltro > 0)
                {
                    sqlQuery += " WHERE id_invfisico = @IdEntradaFiltro";
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
                    txfolio.Text = dataGridView1.CurrentRow.Cells["Idinvfisico"].Value?.ToString() ?? string.Empty;
                    txfecha.Text = dataGridView1.CurrentRow.Cells["Fecha"].Value?.ToString() ?? string.Empty;
                    txnomalmacen.Text = dataGridView1.CurrentRow.Cells["Nomalmacen"].Value?.ToString() ?? string.Empty;
                    txusuario.Text = dataGridView1.CurrentRow.Cells["Nombre"].Value?.ToString() ?? string.Empty;
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
                if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Cells["Idinvfisico"].Value == null ||
                    dataGridView1.CurrentRow.Cells["Idinvfisico"].Value == DBNull.Value)
                {
                    dataGridView2.DataSource = null;
                    return;
                }

                int idInventario = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Idinvfisico"].Value);

                string sqlQuery = @"
                    SELECT 
                        invfisico AS Invfisico, 
                        id_invfisdet AS Ifinvfisdet, 
                        cantidad AS Cantidad,
                        articulo AS Articulo, 
                        s.clave AS ClaveProducto,
                        s.s_nombre AS NombreProducto                       
                    FROM invfisdet as d 
                    LEFT JOIN servicios s ON d.articulo = s.id_servicio
                    WHERE Invfisico = @idInventario";

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

                if (dataGridView2.Columns.Contains("Articulo"))
                    dataGridView2.Columns["Articulo"].Visible = false;

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

        private async void ImportaInventario_Click(object sender, EventArgs e)
        {
            string rutaArchivo = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"D:\user\pedro\downloads";
                openFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    rutaArchivo = openFileDialog.FileName;
                    string nombreArchivo = Path.GetFileName(rutaArchivo);

                    txtNombreArchivo.Text = nombreArchivo;
                    txtNombreArchivo.Tag = rutaArchivo;
                }
                else
                {
                    return;
                }
            }

            List<InventarioDto> listaInventario = ParsearArchivo(rutaArchivo);

            if (listaInventario.Count == 0)
            {
                MessageBox.Show("No se encontraron registros válidos para insertar en el archivo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = _context.Database.GetConnectionString();

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                try
                {
                    await conexion.OpenAsync();

                    using (SqlTransaction transaccion = conexion.BeginTransaction())
                    {
                        // 1. Crear el registro maestro en invfisico y obtener el id_invfisico generado
                        string queryMaestro = "INSERT INTO invfisico (usuario, fecha) VALUES (@usuario, GETDATE()); SELECT SCOPE_IDENTITY();";
                        int idInvFisicoGenerado = 0;

                        using (SqlCommand comandoMaestro = new SqlCommand(queryMaestro, conexion, transaccion))
                        {
                            comandoMaestro.Parameters.AddWithValue("@usuario", 185);
                            object resultado = await comandoMaestro.ExecuteScalarAsync();
                            idInvFisicoGenerado = Convert.ToInt32(resultado);
                        }

                        // 2. Insertar los detalles en invfisdet asociados al id_invfisico obtenido
                        string queryDetalle = "INSERT INTO invfisdet (invfisico, articulo, cantidad) VALUES (@invfisico, @articulo, @cantidad)";

                        using (SqlCommand comandoDetalle = new SqlCommand(queryDetalle, conexion, transaccion))
                        {
                            comandoDetalle.Parameters.Add("@invfisico", SqlDbType.Float);
                            comandoDetalle.Parameters.Add("@articulo", SqlDbType.VarChar);
                            comandoDetalle.Parameters.Add("@cantidad", SqlDbType.Float);

                            foreach (var item in listaInventario)
                            {
                                comandoDetalle.Parameters["@invfisico"].Value = idInvFisicoGenerado;
                                comandoDetalle.Parameters["@articulo"].Value = item.Clave;
                                comandoDetalle.Parameters["@cantidad"].Value = item.Cantidad;

                                await comandoDetalle.ExecuteNonQueryAsync();
                            }
                        }

                        transaccion.Commit();
                        MessageBox.Show($"Se creó el inventario físico #{idInvFisicoGenerado} y se importaron {listaInventario.Count} registros con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Recargar el grid
                        await CargarMaestroDoctoscpAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error en la base de datos al importar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private List<InventarioDto> ParsearArchivo(string ruta)
        {
            List<InventarioDto> lista = new List<InventarioDto>();
            try
            {
                if (!File.Exists(ruta)) return lista;

                string[] lineas = File.ReadAllLines(ruta);

                foreach (string linea in lineas)
                {
                    if (string.IsNullOrWhiteSpace(linea)) continue;

                    string[] datos = linea.Split(',');

                    if (datos.Length >= 2)
                    {
                        lista.Add(new InventarioDto
                        {
                            Clave = datos[0].Trim(),
                            Cantidad = float.TryParse(datos[1].Trim(), out float cant) ? cant : 0
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return lista;
        }
    }

    public class InventarioDto
    {
        public string Clave { get; set; } = string.Empty;
        public float Cantidad { get; set; }
    }
}