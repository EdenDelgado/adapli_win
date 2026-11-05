using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Entities;
using Sisvta.Infrastructure.Persistence.Contexts;
using Sisvta.Infrastructure.Repositories;
using Sisvta.UseCases;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisvta
{
    public partial class Fprodd : Form
    {
        private readonly GestionarProductosUseCase _useCase;
        private readonly ApplicationDbContext _context; // Contexto para consultas directas
        public event EventHandler ProductoModificado;

        public int linea = 0; // Variable para almacenar el id de la línea seleccionada

        public Fprodd(GestionarProductosUseCase useCase, ApplicationDbContext context)
        {
            InitializeComponent();
            _useCase = useCase;
            _context = context;
            // Forzamos la carga inicial aquí para no depender exclusivamente del evento Load
            _ = InicializarDatosAsync();
        }

        private async Task InicializarDatosAsync()
        {
            await CargarUnidadesMedidaAsync();
            await CargarImpuestosAsync();
            await CargarListasPrecioAsync();
            await CargarMonedasAsync();
            await CargarStatusAsync();
            await CargarLineasArticuloAsync(); // <-- Agregamos la carga de líneas de artículos aquí
            await CargarDetalleServiciosAsync(clavetxt.Text.Trim());
        }

        // Evento Load del formulario corregido con await en ambas cargas
        private async void Fprodd_Load(object sender, EventArgs e)
        {
            await CargarUnidadesMedidaAsync();
            await CargarImpuestosAsync();
            await CargarListasPrecioAsync();
            await CargarMonedasAsync();
            await CargarStatusAsync();
            await CargarLineasArticuloAsync(); // <-- Aseguramos la carga también en el Load
            await CargarDetalleServiciosAsync(clavetxt.Text.Trim());
        }

        public async Task CargarDetalleServiciosAsync(string claveProducto = null)
        {
            try
            {
                if (_context == null) return;

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                // ==========================================
                // 1. PRIMER QUERY (Grid 1)
                // ==========================================
                string sqlQuery1 = @"
                    SELECT 
                        d.movinventario AS Idfactura, 
                        d.cantidad AS Cantidad, 
                        d.preciou AS PrecioU, 
                        d.daterecord AS FechaRegistro, 
                        d.status AS Status, 
                        d.articulo AS Articulo,
                        s.umedidacompra as Umedida,u.nombre as Nommedida,
                        s.s_nombre AS NombreServicio,
                        h.almacen AS Almacen,
                        a.nombre AS NombreAlmacen,
                        ia.saldo AS Existencia, l.nombre as Nomlinea 
                    FROM inventario_detalles d
                    LEFT JOIN servicios s ON d.articulo = s.id_servicio 
                    LEFT JOIN Inventario h ON d.movinventario = h.id_inventario 
                    LEFT JOIN Inventarioactual AS ia ON d.articulo = ia.articulo AND h.almacen = ia.almacen 
                    LEFT JOIN almacen AS a ON h.almacen = a.id_almacen
                    left join unidadmedida as u on s.umedidacompra=u.id_um
                    left join lineaarticulo as l on s.lineaarticulo=l.id_lineasarticulos
                    WHERE (d.cantidad * d.preciou) > 0 
                      AND (@ClaveProducto IS NULL OR @ClaveProducto = '' OR d.articulo = @ClaveProducto)";

                using var command1 = connection.CreateCommand();
                command1.CommandText = sqlQuery1;
                command1.Parameters.AddWithValue("@ClaveProducto", string.IsNullOrEmpty(claveProducto) ? (object)DBNull.Value : claveProducto);

                using var reader1 = await command1.ExecuteReaderAsync();
                var dataTable1 = new System.Data.DataTable();
                dataTable1.Load(reader1);

                if (dataTable1.Rows.Count > 0)
                {
                    // Si prefieres que el ComboBox muestre la línea seleccionada automáticamente al buscar el producto:
                    // lineatxt.Text = dataTable1.Rows[0]["Nomlinea"]?.ToString();

                    if (dataTable1.Rows[0]["Nomlinea"] != DBNull.Value)
                    {
                        // Opcional: si el ComboBox es editable, puedes asignar el texto o el ID si lo tienes mapeado
                    }

                    if (dataTable1.Rows[0]["Umedida"] != DBNull.Value &&
                        int.TryParse(dataTable1.Rows[0]["Umedida"].ToString(), out int idUmReg))
                    {
                        umedidatxt.SelectedValue = idUmReg;
                    }

                    existx.Text = dataTable1.Rows[0]["Existencia"]?.ToString() ?? "0.00";
                    var ultimaFila = dataTable1.Rows[dataTable1.Rows.Count - 1];

                    if (ultimaFila["PrecioU"] != DBNull.Value && decimal.TryParse(ultimaFila["PrecioU"].ToString(), out decimal ultimoPrecio))
                    {
                        ultimopreciotxt.Text = ultimoPrecio.ToString("N2");
                        almacentx.Text = ultimaFila["NombreAlmacen"]?.ToString() ?? string.Empty;
                    }
                    else
                    {
                        ultimopreciotxt.Text = "0.00";
                    }
                }
                else
                {
                    ultimopreciotxt.Text = "0.00";
                    existx.Text = "0.00";
                }

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dataTable1;
                dataGridView1.Refresh();

                // ==========================================
                // 2. SEGUNDO QUERY (DataGridView2)
                // ==========================================
                string sqlQuery2 = @"
                    SELECT 
                        a.nombre AS NombreAlmacen,
                        CAST(ia.saldo AS DECIMAL(18, 3)) AS Existencia
                    FROM Inventarioactual ia
                    LEFT JOIN almacen a ON ia.almacen = a.id_almacen
                    WHERE (@ClaveProducto IS NULL OR @ClaveProducto = '' OR ia.articulo = @ClaveProducto)";

                using var command2 = connection.CreateCommand();
                command2.CommandText = sqlQuery2;
                command2.Parameters.AddWithValue("@ClaveProducto", string.IsNullOrEmpty(claveProducto) ? (object)DBNull.Value : claveProducto);

                using var reader2 = await command2.ExecuteReaderAsync();
                var dataTable2 = new System.Data.DataTable();
                dataTable2.Load(reader2);

                dataGridView2.AutoGenerateColumns = true;
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = dataTable2;
                dataGridView2.RowHeadersWidth = 8;
                if (dataGridView2.Columns["Existencia"] != null)
                {
                    dataGridView2.Columns["Existencia"].DefaultCellStyle.Format = "N3";
                    dataGridView2.Columns["Existencia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                dataGridView2.Refresh();

                // ==========================================
                // 3. TERCER QUERY (DataGridView3)
                // ==========================================
                string sqlQuery3 = @"
                    SELECT
                        l.listaprecio,                        
                        p.precioventa as Precio,m.moneda as Moneda
                    FROM preciosarticulo as p
                    LEFT JOIN servicios s ON p.articulo = s.id_servicio
                    left join listaprecio as l on p.listaprecio=l.id_listaprecio
                    left join moneda as m on p.moneda=m.id_moneda
                    WHERE (@ClaveProducto IS NULL OR @ClaveProducto = '' OR p.articulo = @ClaveProducto)";

                using var command3 = connection.CreateCommand();
                command3.CommandText = sqlQuery3;
                command3.Parameters.AddWithValue("@ClaveProducto", string.IsNullOrEmpty(claveProducto) ? (object)DBNull.Value : claveProducto);

                using var reader3 = await command3.ExecuteReaderAsync();
                var dataTable3 = new System.Data.DataTable();
                dataTable3.Load(reader3);

                dataGridView3.AutoGenerateColumns = true;
                dataGridView3.DataSource = null;
                dataGridView3.DataSource = dataTable3;
                dataGridView3.RowHeadersWidth = 8;
                if (dataGridView3.Columns["Precio"] != null)
                {
                    dataGridView3.Columns["Precio"].DefaultCellStyle.Format = "N2";
                    dataGridView3.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                dataGridView3.Refresh();

                // ==========================================
                // 4. CUARTO QUERY (DataGridView4)
                // ==========================================
                string sqlQuery4 = @"
                    SELECT
                        i.impuesto as Nomimpuesto, i.porcentaje as Porcentaje
                    FROM impuestos_articulo as ia
                    LEFT JOIN servicios s ON ia.articulo = s.id_servicio
                    left join impuesto as i on ia.impuesto=i.id_impuesto
                    WHERE (@ClaveProducto IS NULL OR @ClaveProducto = '' OR ia.articulo = @ClaveProducto)";

                using var command4 = connection.CreateCommand();
                command4.CommandText = sqlQuery4;
                command4.Parameters.AddWithValue("@ClaveProducto", string.IsNullOrEmpty(claveProducto) ? (object)DBNull.Value : claveProducto);

                using var reader4 = await command4.ExecuteReaderAsync();
                var dataTable4 = new System.Data.DataTable();
                dataTable4.Load(reader4);

                dataGridView4.AutoGenerateColumns = true;
                dataGridView4.DataSource = null;
                dataGridView4.DataSource = dataTable4;
                dataGridView4.RowHeadersWidth = 8;
                dataGridView4.Columns["Porcentaje"].Width = 70;
                dataGridView4.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el detalle: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarUnidadesMedidaAsync()
        {
            try
            {
                if (_context == null) return;

                var listaUnidades = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
                    _context.Set<Sisvta.Domain.Entities.UnidadMedida>()
                        .Select(u => new { id_um = u.IdUm, nombre = u.Nombre })
                );

                if (listaUnidades != null && listaUnidades.Count > 0)
                {
                    umedidatxt.DataSource = null;
                    umedidatxt.DisplayMember = "nombre";
                    umedidatxt.ValueMember = "id_um";
                    umedidatxt.DataSource = listaUnidades;
                    umedidatxt.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar unidades de medida con EF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarImpuestosAsync()
        {
            try
            {
                if (_context == null) return;

                var listaImpuestos = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
                    _context.Set<Sisvta.Domain.Entities.Impuesto>()
                        .Select(i => new { id_impuesto = i.IdImpuesto, nombre = i.NombreImpuesto })
                );

                if (listaImpuestos != null && listaImpuestos.Count > 0)
                {
                    impuestotx.DataSource = null;
                    impuestotx.DisplayMember = "nombre";
                    impuestotx.ValueMember = "id_impuesto";
                    impuestotx.DataSource = listaImpuestos;
                    impuestotx.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los impuestos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarListasPrecioAsync()
        {
            try
            {
                if (_context == null) return;

                var listaPrecios = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
                    _context.Set<Sisvta.Domain.Entities.ListaPrecio>()
                        .Select(lp => new { id_listaprecio = lp.IdListaPrecio, nombre = lp.NombreListaPrecio })
                );

                if (listaPrecios != null && listaPrecios.Count > 0)
                {
                    listapreciotx.DataSource = null;
                    listapreciotx.DisplayMember = "nombre";
                    listapreciotx.ValueMember = "id_listaprecio";
                    listapreciotx.DataSource = listaPrecios;
                    listapreciotx.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las listas de precio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarMonedasAsync()
        {
            try
            {
                if (_context == null) return;

                var listaMonedas = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
                    _context.Set<Sisvta.Domain.Entities.Moneda>()
                        .Select(m => new { id_moneda = m.IdMoneda, moneda = m.SimboloMoneda })
                );

                if (listaMonedas != null && listaMonedas.Count > 0)
                {
                    monedatx.DataSource = null;
                    monedatx.DisplayMember = "moneda";
                    monedatx.ValueMember = "id_moneda";
                    monedatx.DataSource = listaMonedas;
                    monedatx.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las monedas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarStatusAsync()
        {
            try
            {
                if (_context == null) return;

                var listaStatus = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
                    _context.Set<Sisvta.Domain.Entities.StatusServicio>()
                        .Select(s => new { id_status = s.IdStatusServicios, nombre = s.Status })
                );

                if (listaStatus != null && listaStatus.Count > 0)
                {
                    statustxt.DataSource = null;
                    statustxt.DisplayMember = "nombre";
                    statustxt.ValueMember = "id_status";
                    statustxt.DataSource = listaStatus;
                    statustxt.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los estatus: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================================
        // NUEVO MÉTODO PARA CARGAR LÍNEAS DE ARTÍCULO EN 'lineatxt'
        // =========================================================================
        private async Task CargarLineasArticuloAsync()
        {
            try
            {
                if (_context == null) return;

                // Nota: Asegúrate de que tu entidad en el Domain se mapee a la tabla 'lineaarticulo'
                // y que sus propiedades coincidan (por ejemplo: IdLineaArticulos y Nombre).
                var listaLineas = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
                    _context.Set<Sisvta.Domain.Entities.LineaArticulo>()
                        .Select(l => new { id_linea = l.IdLineasArticulos, nombre = l.Nombre })
                );

                if (listaLineas != null && listaLineas.Count > 0)
                {
                    lineatxt.DataSource = null;
                    lineatxt.DisplayMember = "nombre";
                    lineatxt.ValueMember = "id_linea";
                    lineatxt.DataSource = listaLineas;
                    lineatxt.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las líneas de artículos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
            else
            {
                base.OnFormClosing(e);
            }
        }

        private void btregresar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lineatxt_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (_context == null)
                {
                    MessageBox.Show("El contexto de la base de datos no está inicializado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using var fclinl = new Fclinl(_context);

                if (fclinl.ShowDialog() == DialogResult.OK)
                {
                    lineatxt.Text = fclinl.NombreSeleccionado;
                    linea = int.Parse(fclinl.IdLineaSeleccionado.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el selector de líneas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btguardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(clavetxt.Text) ||
                string.IsNullOrWhiteSpace(nombretxt.Text) ||
                string.IsNullOrWhiteSpace(descripciontxt.Text))
            {
                MessageBox.Show("Por favor, completa todos los campos requeridos.", "Campos requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idUnidadMedida = 0;
                if (umedidatxt.SelectedValue != null && int.TryParse(umedidatxt.SelectedValue.ToString(), out int valUm))
                {
                    idUnidadMedida = valUm;
                }

                if (idUnidadMedida <= 0)
                {
                    MessageBox.Show("Por favor, selecciona una unidad de medida válida.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool inventariable = inventariabletxt.Text.Trim().ToLower() == "true" || inventariabletxt.Text.Trim() == "1";
                int? prodServ = int.TryParse(prodservtxt.Text.Trim(), out int ps) ? ps : (int?)(null);

                int status = 0;
                if (statustxt.SelectedValue != null && int.TryParse(statustxt.SelectedValue.ToString(), out int stVal))
                {
                    status = stVal;
                }

                // Si deseas capturar la línea seleccionada directamente del ComboBox en lugar de la variable global de doble clic:
                if (lineatxt.SelectedValue != null && int.TryParse(lineatxt.SelectedValue.ToString(), out int linVal))
                {
                    linea = linVal;
                }

                double precio = double.TryParse(preciotxt.Text.Trim(), out double pr) ? pr : 0.0;

                var producto = new Producto(
                    clavetxt.Text.Trim(),
                    nombretxt.Text.Trim(),
                    descripciontxt.Text.Trim(),
                    inventariable,
                    idUnidadMedida,
                    prodServ,
                    claveproservtxt.Text.Trim(),
                    linea,
                    status,
                    precio
                );

                bool exito = await _useCase.RegistrarAsync(producto);

                if (exito)
                {
                    MessageBox.Show("Producto guardado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ProductoModificado?.Invoke(this, EventArgs.Empty);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                string errorReal = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show($"Error detallado al guardar: {errorReal}", "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView4.Rows[e.RowIndex];
                impuestotx.Text = fila.Cells["Nomimpuesto"]?.Value?.ToString();
                tasatx.Text = fila.Cells["Porcentaje"]?.Value?.ToString();
            }
        }

        private async void btguardarpre_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(prodservtxt.Text) || !int.TryParse(prodservtxt.Text.Trim(), out int idArticulo))
            {
                MessageBox.Show("Debe especificar una clave numérica válida para el artículo en prodservtxt.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int? idListaPrecio = listapreciotx.SelectedValue != null && int.TryParse(listapreciotx.SelectedValue.ToString(), out int lp) ? lp : (int?)null;
                int? idMoneda = monedatx.SelectedValue != null && int.TryParse(monedatx.SelectedValue.ToString(), out int m) ? m : (int?)null;

                decimal precioVenta = decimal.TryParse(precioptx.Text.Trim(), out decimal pv) ? pv : 0.00m;

                if (idListaPrecio == null || idMoneda == null)
                {
                    MessageBox.Show("Por favor, seleccione una lista de precio y una moneda válidas.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_context == null) return;

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                string queryInsert = @"
                    INSERT INTO preciosarticulo (articulo, listaprecio, precioventa, moneda) 
                    VALUES (@Articulo, @ListaPrecio, @PrecioVenta, @Moneda)";

                using var command = connection.CreateCommand();
                command.CommandText = queryInsert;

                command.Parameters.AddWithValue("@Articulo", idArticulo);
                command.Parameters.AddWithValue("@ListaPrecio", idListaPrecio.Value);
                command.Parameters.AddWithValue("@PrecioVenta", precioVenta);
                command.Parameters.AddWithValue("@Moneda", idMoneda.Value);

                await command.ExecuteNonQueryAsync();

                MessageBox.Show("Precio de artículo guardado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                precioptx.Clear();
                await CargarDetalleServiciosAsync(prodservtxt.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el precio del artículo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btguardarimp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(prodservtxt.Text) || !int.TryParse(prodservtxt.Text.Trim(), out int idArticulo))
            {
                MessageBox.Show("Debe especificar una clave numérica válida para el artículo en prodservtxt.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int? idImpuesto = impuestotx.SelectedValue != null && int.TryParse(impuestotx.SelectedValue.ToString(), out int lp) ? lp : (int?)null;

                if (idImpuesto == null)
                {
                    MessageBox.Show("Por favor, seleccione un impuesto válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_context == null) return;

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                string queryInsert = @"
                    INSERT INTO impuestos_articulo (articulo, impuesto) 
                    VALUES (@Articulo, @Impuesto)";

                using var command = connection.CreateCommand();
                command.CommandText = queryInsert;

                command.Parameters.AddWithValue("@Articulo", idArticulo);
                command.Parameters.AddWithValue("@Impuesto", idImpuesto.Value);

                await command.ExecuteNonQueryAsync();

                MessageBox.Show("Impuesto de artículo guardado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CargarDetalleServiciosAsync(prodservtxt.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el impuesto del artículo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnborrapre_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(prodservtxt.Text) || !int.TryParse(prodservtxt.Text.Trim(), out int idArticulo))
            {
                MessageBox.Show("Debe especificar una clave numérica válida del artículo para poder borrar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? idListaPrecio = listapreciotx.SelectedValue != null && int.TryParse(listapreciotx.SelectedValue.ToString(), out int lp) ? lp : (int?)null;

            if (idListaPrecio == null)
            {
                MessageBox.Show("Por favor, seleccione la lista de precio que desea eliminar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("¿Está seguro de que desea eliminar este precio?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                if (_context == null) return;

                string connectionString = _context.Database.GetConnectionString();
                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                string sqlQueryDelete = @"DELETE FROM preciosarticulo WHERE articulo = @Articulo AND listaprecio = @ListaPrecio";

                using var command = connection.CreateCommand();
                command.CommandText = sqlQueryDelete;
                command.Parameters.AddWithValue("@Articulo", idArticulo);
                command.Parameters.AddWithValue("@ListaPrecio", idListaPrecio.Value);

                int filasAfectadas = await command.ExecuteNonQueryAsync();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Precio eliminado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await CargarDetalleServiciosAsync(prodservtxt.Text.Trim());
                }
                else
                {
                    MessageBox.Show("No se encontró ningún registro con esa combinación para eliminar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al borrar el precio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnborraimp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(prodservtxt.Text) || !int.TryParse(prodservtxt.Text.Trim(), out int idArticulo))
            {
                MessageBox.Show("Debe especificar una clave numérica válida del artículo para poder borrar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? idImpuesto = impuestotx.SelectedValue != null && int.TryParse(impuestotx.SelectedValue.ToString(), out int imp) ? imp : (int?)null;

            if (idImpuesto == null)
            {
                MessageBox.Show("Por favor, seleccione el impuesto que desea eliminar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("¿Está seguro de que desea eliminar este impuesto del artículo?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                if (_context == null) return;

                string connectionString = _context.Database.GetConnectionString();
                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                string sqlQueryDelete = @"DELETE FROM impuestos_articulo WHERE articulo = @Articulo AND impuesto = @Impuesto";

                using var command = connection.CreateCommand();
                command.CommandText = sqlQueryDelete;
                command.Parameters.AddWithValue("@Articulo", idArticulo);
                command.Parameters.AddWithValue("@Impuesto", idImpuesto.Value);

                int filasAfectadas = await command.ExecuteNonQueryAsync();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Impuesto eliminado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await CargarDetalleServiciosAsync(prodservtxt.Text.Trim());
                }
                else
                {
                    MessageBox.Show("No se encontró ningún registro con esa combinación para eliminar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al borrar el impuesto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView3.Rows[e.RowIndex];
                listapreciotx.Text = fila.Cells["listaprecio"]?.Value?.ToString();
                precioptx.Text = fila.Cells["Precio"]?.Value?.ToString();
                monedatx.Text = fila.Cells["Moneda"]?.Value?.ToString();
            }
        }
    }
}