<<<<<<< HEAD
﻿using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;
=======
﻿using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Entities;
using Sisvta.Infrastructure.Persistence.Contexts;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
>>>>>>> main

namespace Sisvta
{
    public partial class Fcsug : Form
    {
        private readonly ApplicationDbContext _context;

        public Fcsug(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));

            // Suscripción a eventos
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.CellMouseDoubleClick += dataGridView1_CellMouseDoubleClick;

<<<<<<< HEAD
            // FORZAMOS LA CARGA AQUÍ MISMO PARA EVITAR DEPENDER DEL EVENTO LOAD
            _ = CargarMaestroDoctoscpAsync();
        }

        private async void Fcsal_Load(object sender, EventArgs e)
        {
            await CargarMaestroDoctoscpAsync();
        }

=======
            // FORZAMOS LA CARGA DE FORMA SEGURA DESDE EL CONSTRUCTOR
            _ = InicializarFormularioAsync();
        }

        private async Task InicializarFormularioAsync()
        {
            await CargarAlmacenesAsync();
            await CargarLineasArticuloAsync();
            await CargarMaestroDoctoscpAsync();
        }

        private async void Fcsug_Load(object sender, EventArgs e)
        {
            // Mantenemos el Load de respaldo por si el ciclo de vida del formulario lo requiere
        }

>>>>>>> main
        public async Task CargarMaestroDoctoscpAsync()
        {
            try
            {
                if (_context == null)
                {
                    MessageBox.Show("El contexto de base de datos es nulo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string sqlQuery = @"
<<<<<<< HEAD
                    SELECT 
                        d.id_factura AS Idfactura, 
                        d.folio AS Folio,i.id_inventario AS IdInventario, 
                        d.fechafactura AS Fecha,
                      i.concepto AS Concepto,
                        d.proveedor AS Proveedor, i.proveedor as iprov, 
                        i.descripcion AS Descripcion,
                        d.almacen AS IdAlmacen
                    FROM doctoscc as d left join inventario as i on d.id_factura = i.id_factura
                    WHERE fechafactura >= @desde AND fechafactura <= @hasta";
=======
                   select s.id_servicio, s.clave, s.s_nombre as Nombre, s.lineaarticulo, l.nombre as Nomlinea, s.maximo, i.almacen, a.nombre as Nomalmacen, i.saldo,
                   (s.maximo - i.saldo) as sugerencia, s.preciocompra, s.preciocompra * i.saldo as costototal from servicios s 
                   left join inventarioactual i on s.id_servicio = i.articulo
                   left join lineaarticulo l on s.lineaarticulo = l.id_lineasarticulos
                   left join almacen a on i.almacen = a.id_almacen
                   WHERE (@linea = 0 OR s.lineaarticulo = @linea)
                     AND (@almacen = 0 OR i.almacen = @almacen)";
>>>>>>> main

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

<<<<<<< HEAD
                DateTime fechaDesde = desdefechatx.Value.Date;
                DateTime fechaHasta = hastafechatx.Value.Date.AddDays(1).AddSeconds(-1);

                command.Parameters.Add("@desde", SqlDbType.DateTime).Value = fechaDesde;
                command.Parameters.Add("@hasta", SqlDbType.DateTime).Value = fechaHasta;
=======
                // Extracción segura de los valores seleccionados en los ComboBox
                int lineaId = (lineatxt.SelectedValue != null && int.TryParse(lineatxt.SelectedValue.ToString(), out int l)) ? l : 0;
                int almacenId = (txalmacen.SelectedValue != null && int.TryParse(txalmacen.SelectedValue.ToString(), out int a)) ? a : 0;

                command.Parameters.Add("@linea", SqlDbType.Int).Value = lineaId;
                command.Parameters.Add("@almacen", SqlDbType.Int).Value = almacenId;
>>>>>>> main

                using var reader = await command.ExecuteReaderAsync();
                var dataTable = new System.Data.DataTable();
                dataTable.Load(reader);

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dataTable;
<<<<<<< HEAD
=======

                // =========================================================================
                // OCULTAR COLUMNAS ESPECÍFICAS
                // =========================================================================
                if (dataGridView1.Columns.Contains("id_servicio"))
                    dataGridView1.Columns["id_servicio"].Visible = false;

                if (dataGridView1.Columns.Contains("lineaarticulo"))
                    dataGridView1.Columns["lineaarticulo"].Visible = false;

                if (dataGridView1.Columns.Contains("almacen"))
                    dataGridView1.Columns["almacen"].Visible = false;

                // =========================================================================
                // DAR FORMATO "N3" Y ALINEAR A LA DERECHA
                // =========================================================================
                if (dataGridView1.Columns.Contains("saldo"))
                {
                    dataGridView1.Columns["Nomlinea"].Width = 200; dataGridView1.Columns["Nombre"].Width = 200;
                    dataGridView1.Columns["saldo"].DefaultCellStyle.Format = "N3";
                    dataGridView1.Columns["saldo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView1.Columns["saldo"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dataGridView1.Columns.Contains("sugerencia"))
                {
                    dataGridView1.Columns["sugerencia"].DefaultCellStyle.Format = "N3";
                    dataGridView1.Columns["sugerencia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView1.Columns["sugerencia"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dataGridView1.Columns.Contains("maximo"))
                {
                    dataGridView1.Columns["maximo"].DefaultCellStyle.Format = "N3";
                    dataGridView1.Columns["maximo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView1.Columns["maximo"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                // =========================================================================
                // DAR FORMATO "N2" Y ALINEAR A LA DERECHA PARA PRECIO DE COMPRA
                // =========================================================================
                if (dataGridView1.Columns.Contains("preciocompra"))
                {
                    dataGridView1.Columns["preciocompra"].DefaultCellStyle.Format = "N2";
                    dataGridView1.Columns["preciocompra"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView1.Columns["preciocompra"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                // =========================================================================
                // DAR FORMATO "N2" Y ALINEAR A LA DERECHA PARA COSTO TOTAL
                // =========================================================================
                if (dataGridView1.Columns.Contains("costototal"))
                {
                    dataGridView1.Columns["costototal"].DefaultCellStyle.Format = "N2";
                    dataGridView1.Columns["costototal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView1.Columns["costototal"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

>>>>>>> main
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las entradas de almacén: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento que se dispara al hacer doble clic en una fila del DataGridView
        private async void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
<<<<<<< HEAD
            // Validar que se haya hecho clic sobre una fila válida (y no en la cabecera)
            if (e.RowIndex >= 0)
            {
                // Como el DataSource es un DataTable, cada fila se mapea como DataRowView
=======
            if (e.RowIndex >= 0)
            {
>>>>>>> main
                var rowView = dataGridView1.Rows[e.RowIndex].DataBoundItem as DataRowView;

                if (rowView != null)
                {
<<<<<<< HEAD
                    // Obtenemos el ID de la entrada (o los campos que necesites)
                    int idfactura = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Idfactura"].Value);

                    // Instanciamos el formulario Fent pasándole el ID (Asegúrate de que Fent tenga un constructor que reciba este int)
                    var formFent = new Fent(_context,idfactura);

                    if (formFent.ShowDialog() == DialogResult.OK)
                    {
                        // Recargamos el grid si se realizaron cambios o guardados
=======
                    int idservicio = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id_servicio"].Value);

                    var fsug = new Fsug(_context, idservicio);

                    if (fsug.ShowDialog() == DialogResult.OK)
                    {
>>>>>>> main
                        await CargarMaestroDoctoscpAsync();
                    }
                }
            }
        }

<<<<<<< HEAD
        // Evento que se dispara al cambiar de fila en el DataGridView1 (Maestro)
=======
>>>>>>> main
        private async void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // await CargarDetalleDoctoscpAsync();
        }

        private void regresabt_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void Filtrarbt_Click(object sender, EventArgs e)
        {
            await CargarMaestroDoctoscpAsync();
        }
<<<<<<< HEAD
=======

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
>>>>>>> main
    }
}