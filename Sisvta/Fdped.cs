using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sisvta
{
    public partial class Fdped : Form
    {
        private readonly ApplicationDbContext _context;

        public Fdped(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context;

            // Suscribimos el evento para que al cambiar de renglón en el grid maestro, se actualice el detalle
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            // FORZAMOS LA CARGA AQUÍ MISMO PARA EVITAR DEPENDER DEL EVENTO LOAD
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

                string sqlQuery = @"
                    SELECT 
                        id_factura AS Factura, 
                        folio AS Folio, 
                        fechaFactura AS Fecha, 
                        fechapago AS FechaPago, 
                        status AS Status, 
                        tipo AS Tipo, 
                        cliente AS Cliente, 
                        moneda AS Moneda, 
                        almacen AS Almacen
                    FROM doctoscc"; // Quitamos temporalmente el WHERE para probar si trae todo

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                using var reader = await command.ExecuteReaderAsync();
                var dataTable = new System.Data.DataTable();
                dataTable.Load(reader);

                // Depuración rápida: Cuántos registros trajo la consulta maestra
                // MessageBox.Show($"Registros encontrados en doctoscp: {dataTable.Rows.Count}");

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dataTable;
                dataGridView1.Refresh();

                if (dataGridView1.Rows.Count > 0)
                {
                    await CargarDetalleDoctoscpAsync();
                }
                else
                {
                    dataGridView2.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el maestro doctoscp: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 2. Carga el DataGridView2 con los detalles (DOCTOSCP_DET) del documento seleccionado en el DataGridView1
        /// </summary>
        private async Task CargarDetalleDoctoscpAsync()
        {
            try
            {
                if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Cells["Factura"].Value == null)
                {
                    dataGridView2.DataSource = null;
                    return;
                }

                int idDoctoscp = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Factura"].Value);

                string sqlQuery = @"
                    SELECT 
                        id_factura AS Idfactura, 
                        id_detallefatura AS DetalleFactura, 
                        cantidad AS Cantidad, 
                        idprodserv AS IdProdServ,s.s_nombre   AS Nombre, 
                        preciou AS PrecioU, 
                        (cantidad * preciou) AS Importe,tasaiva AS ImpIva
                    FROM doctoscc_det left join servicios s on doctoscc_det.idprodserv  = s.id_servicio
                    WHERE id_factura = @Factura";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;
                command.Parameters.AddWithValue("@Factura", idDoctoscp);

                using var reader = await command.ExecuteReaderAsync();
                var dataTable = new System.Data.DataTable();
                dataTable.Load(reader);

                dataGridView2.AutoGenerateColumns = true;
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = dataTable;
                dataGridView2.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el detalle doctoscp_det: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento que se dispara al cambiar de fila en el DataGridView1 (Maestro)
        private async void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            await CargarDetalleDoctoscpAsync();
        }

    }
}
