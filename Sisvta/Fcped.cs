using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sisvta
{
    public partial class Fcped : Form
    {
        private readonly IServiceProvider _serviceProvider = null!;

        // Actualizado a tu IP y instancia correcta
        private readonly string connString = "Server=192.168.1.8\\sqlexpress; Database=adaplirenelomeli; Integrated Security=True; Encrypt=False; TrustServerCertificate=True;";

        public Fcped()
        {
            InitializeComponent();
        }

        public Fcped(IServiceProvider serviceProvider) : this()
        {
            _serviceProvider = serviceProvider;
        }

        private void txpedido_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txpedido.Text)) return;

            string query = "SELECT * FROM dbo.doctoscc WHERE id_factura = @id;";

            try
            {
                using (SqlConnection conexion = new SqlConnection(connString))
                {
                    conexion.Open();

                    using (SqlCommand command = new SqlCommand(query, conexion))
                    {
                        command.Parameters.AddWithValue("@id", txpedido.Text.Trim());

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txcliente.Text = reader["cliente"]?.ToString() ?? string.Empty;
                                txfecha.Text = reader["fechafactura"]?.ToString() ?? string.Empty;

                                string clienteId = txcliente.Text;
                                reader.Close(); // Cerramos el primer reader antes de abrir otra consulta en la misma conexión

                                // Consultar cliente
                                string querycli = "SELECT * FROM dbo.clientes WHERE Id_empros = @clienteId;";
                                using (SqlCommand cmd = new SqlCommand(querycli, conexion))
                                {
                                    cmd.Parameters.AddWithValue("@clienteId", clienteId);
                                    using (SqlDataReader readercli = cmd.ExecuteReader())
                                    {
                                        if (readercli.Read())
                                        {
                                            txnomcli.Text = readercli["nombre"]?.ToString() ?? string.Empty;
                                        }
                                    }
                                }

                                // Consultar detalle y llenar el Grid
                                string querydet = @"SELECT d.id_detallefatura, d.idprodserv, Cantidad, s.s_nombre as nombre, 
                                                   FORMAT(d.preciou,'0.00') as precio, 
                                                   FORMAT(cantidad*d.preciou,'##,##0.00') as importe  
                                                   FROM dbo.doctoscc_det as d 
                                                   LEFT JOIN dbo.servicios as s ON d.idprodserv = s.id_servicio  
                                                   WHERE id_factura = @id;";

                                using (SqlDataAdapter adapter = new SqlDataAdapter(querydet, conexion))
                                {
                                    adapter.SelectCommand.Parameters.AddWithValue("@id", txpedido.Text.Trim());
                                    DataTable dataTable = new DataTable();
                                    adapter.Fill(dataTable);

                                    dataGridView1.DataSource = dataTable;

                                    // Solución CS8602 con operador ?.[...] o ?.
                                    if (dataGridView1.Columns["importe"] != null)
                                        dataGridView1.Columns["importe"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                                    if (dataGridView1.Columns["precio"] != null)
                                        dataGridView1.Columns["precio"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                }
                            }
                            else
                            {
                                MessageBox.Show("No hay ese Id");
                                txcliente.Clear();
                                txfecha.Clear();
                                txnomcli.Clear();
                                dataGridView1.DataSource = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión o SQL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txpedido_DoubleClick(object sender, EventArgs e)
        {
        }

        private void tximporta_Click(object sender, EventArgs e)
        {
        }
    }
}