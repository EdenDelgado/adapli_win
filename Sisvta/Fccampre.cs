using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Entities;
using Sisvta.Infrastructure.Persistence.Contexts;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisvta
{
    public partial class Fccampre : Form
    {
        private readonly ApplicationDbContext _context;

        public Fccampre(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));

            // Suscripción de eventos
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.CellMouseDoubleClick += dataGridView1_CellMouseDoubleClick;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick; // <-- Evento para el botón de editar
            Filtrarbt.Click += Filtrarbt_Click;

            // Carga inicial segura y secuencial desde el constructor
            _ = InicializarFormularioAsync();
        }

        private async Task InicializarFormularioAsync()
        {
            await CargarAlmacenesAsync();
            await CargarLineasArticuloAsync();
            await CargarMaestroDoctoscpAsync();
        }

        private async void Fccampre_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
        }

        public async Task CargarMaestroDoctoscpAsync()
        {
            try
            {
                if (_context == null)
                {
                    MessageBox.Show("El contexto de base de datos es nulo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Aseguramos incluir tanto el ID numérico de la lista como el nombre si se requiere
                string sqlQuery = @"
                    SELECT id_servicio, clave, s_nombre as Nombre, s.lineaarticulo, la.nombre as Nomlinea,
                    preciocompra, p.precioventa, p.listaprecio as listaprecio, l.listaprecio as Nomlistaprecio                     
                    FROM servicios as s 
                    LEFT JOIN preciosarticulo as p ON s.id_servicio = p.articulo
                    LEFT JOIN listaprecio as l ON p.listaprecio = l.id_listaprecio
                    LEFT JOIN lineaarticulo as la ON s.lineaarticulo = la.id_lineasarticulos
                    WHERE (@linea = 0 OR s.lineaarticulo = @linea) AND (@lista = 0 OR p.listaprecio = @lista);";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                // Extracción segura de los valores seleccionados en los ComboBox
                int lineaId = (lineatxt.SelectedValue != null && int.TryParse(lineatxt.SelectedValue.ToString(), out int l)) ? l : 0;
                int listaId = (txlista.SelectedValue != null && int.TryParse(txlista.SelectedValue.ToString(), out int a)) ? a : 0;

                command.Parameters.Add("@linea", SqlDbType.Int).Value = lineaId;
                command.Parameters.Add("@lista", SqlDbType.Int).Value = listaId;

                using var reader = await command.ExecuteReaderAsync();
                var dataTable = new System.Data.DataTable();
                dataTable.Load(reader);

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dataTable;

                // =========================================================================
                // AGREGAR COLUMNA DE BOTÓN "EDITAR" (Si no existe previamente)
                // =========================================================================
                if (!dataGridView1.Columns.Contains("btnEditar"))
                {
                    var btnColumn = new DataGridViewButtonColumn
                    {
                        Name = "btnEditar",
                        HeaderText = "Acción",
                        Text = "✎",
                        UseColumnTextForButtonValue = true,
                        Width = 80
                    };
                    dataGridView1.Columns.Add(btnColumn);
                }

                // =========================================================================
                // CONFIGURACIÓN DE ANCHO, FORMATOS Y ALINEACIONES
                // =========================================================================
                if (dataGridView1.Columns.Contains("Nombre"))
                {
                    dataGridView1.Columns["Nombre"].Width = 200;
                }

                if (dataGridView1.Columns.Contains("lineaarticulo"))
                {
                    dataGridView1.Columns["lineaarticulo"].Visible = false;
                }

                // Ocultamos la columna técnica id_listaprecio si se prefiere no mostrar en bruto
                if (dataGridView1.Columns.Contains("listaprecio"))
                {
                   
                    dataGridView1.Columns["listaprecio"].Visible = false;
                }
                dataGridView1.Columns["id_servicio"].Visible = false;
                if (dataGridView1.Columns.Contains("preciocompra"))
                {
                    dataGridView1.Columns["preciocompra"].DefaultCellStyle.Format = "N2";
                    dataGridView1.Columns["preciocompra"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView1.Columns["preciocompra"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dataGridView1.Columns.Contains("precioventa"))
                {
                    dataGridView1.Columns["precioventa"].DefaultCellStyle.Format = "N2";
                    dataGridView1.Columns["precioventa"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView1.Columns["precioventa"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las listas de precios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método auxiliar centralizado para procesar la edición de un registro
        private async Task EditarRegistroSeleccionadoAsync(int rowIndex)
        {
            if (rowIndex >= 0)
            {
                var rowView = dataGridView1.Rows[rowIndex].DataBoundItem as DataRowView;
                if (rowView != null)
                {
                    // Obtenemos los valores de la fila seleccionada de forma robusta
                    int idServicio = rowView["id_servicio"] != DBNull.Value ? Convert.ToInt32(rowView["id_servicio"]) : 0;
                    string nombre = rowView["nombre"] != DBNull.Value ? rowView["Nombre"].ToString() : string.Empty;
                    string clave = rowView["clave"] != DBNull.Value ? rowView["clave"].ToString() : string.Empty;

                    // Si tienes un ID numérico de lista, úsalo; si prefieres el texto, asegúrate de pasarlo adecuadamente
                    int listaPrecio = rowView["listaprecio"] != DBNull.Value ? Convert.ToInt32(rowView["listaprecio"]) : 0;
                    string NomlistaPrecio = rowView["Nomlistaprecio"] != DBNull.Value ? rowView["Nomlistaprecio"].ToString() : string.Empty;

                    double precioventa = (rowView["precioventa"] != DBNull.Value) ? Convert.ToDouble(rowView["precioventa"]) : 0.0;

                    // Instanciamos el formulario pasando el contexto y los datos del registro
                    using (var formEdicion = new Fcampre(_context, idServicio, clave, nombre, listaPrecio,NomlistaPrecio, precioventa))
                    {
                        if (formEdicion.ShowDialog() == DialogResult.OK)
                        {
                            // Recargamos el grid si se guardaron los cambios correctamente
                            await CargarMaestroDoctoscpAsync();
                        }
                    }
                }
            }
        }

        // Evento que se dispara al hacer clic en el botón dentro del DataGridView
        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Validamos que se haya hecho clic sobre la columna del botón "btnEditar"
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "btnEditar")
            {
                await EditarRegistroSeleccionadoAsync(e.RowIndex);
            }
        }

        // Evento que se dispara al hacer doble clic en una fila del DataGridView
        private async void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            await EditarRegistroSeleccionadoAsync(e.RowIndex);
        }

        private async void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // Lógica al cambiar de selección
        }

        private void regresabt_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void Filtrarbt_Click(object sender, EventArgs e)
        {
            await CargarMaestroDoctoscpAsync();
        }

        private async Task CargarAlmacenesAsync()
        {
            try
            {
                if (_context == null) return;
                if (txlista == null) return;

                var listaAlmacenes = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
                    _context.Set<Sisvta.Domain.Entities.ListaPrecio>()
                        .Select(a => new { id_lista = a.IdListaPrecio, nombre = a.NombreListaPrecio })
                );

                if (listaAlmacenes != null && listaAlmacenes.Count > 0)
                {
                    txlista.DataSource = null;
                    txlista.DisplayMember = "nombre";
                    txlista.ValueMember = "id_lista";
                    txlista.DataSource = listaAlmacenes;
                    txlista.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las listas de precios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarLineasArticuloAsync()
        {
            try
            {
                if (_context == null) return;
                if (lineatxt == null) return;

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
    }
}