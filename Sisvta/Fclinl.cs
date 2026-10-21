using Microsoft.Data.SqlClient;
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
    public partial class Fclinl : Form
    {
        private readonly ApplicationDbContext _context;

        // Propiedades públicas para devolver el valor seleccionado al formulario que lo llamó
        public string IdLineaSeleccionado { get; private set; } = string.Empty;
        public string NombreSeleccionado { get; private set; } = string.Empty;
        public string idempresa { get; private set; } = string.Empty;

        public Fclinl(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context;

            dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete;
            this.Shown += Fcgrul_Shown;
        }



        private async void Fcgrul_Shown(object sender, EventArgs e)
        {
            await CargarGruposAsync();
        }

        private async Task CargarGruposAsync()
        {
            try
            {
                if (_context == null)
                {
                    MessageBox.Show("El contexto es nulo", "Error");
                    return;
                }

                string sqlQuery = @"
                    SELECT 
                        id_lineasarticulos AS Linea, 
                        nombre AS Nombre, 
                        status AS Status, 
                        id_empresa AS IdEmpresa
                    FROM dbo.lineaarticulo order by nombre";

                string connectionString = _context.Database.GetDbConnection().ConnectionString;
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                using var reader = await command.ExecuteReaderAsync();
                var dataTable = new DataTable();
                dataTable.Load(reader);

                // Forzamos la asignación al grid en el hilo principal de la UI
                this.Invoke((MethodInvoker)(() =>
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.AutoGenerateColumns = true;
                    dataGridView1.DataSource = dataTable;
                    dataGridView1.Refresh();
                    dataGridView1.Invalidate(); // Fuerza a redibujar el control visualmente
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las lineas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.Columns["Linea"] != null)
            {
                dataGridView1.Columns["Linea"].HeaderText = "Línea";
                dataGridView1.Columns["Linea"].Width = 80;
            }
            if (dataGridView1.Columns["Nombre"] != null)
            {
                dataGridView1.Columns["Nombre"].HeaderText = "Nombre de la Línea";
                dataGridView1.Columns["Nombre"].Width = 200;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView1.Rows.Count) return;

            var fila = dataGridView1.Rows[e.RowIndex];
            IdLineaSeleccionado = fila.Cells["Linea"].Value?.ToString() ?? string.Empty;
            NombreSeleccionado = fila.Cells["Nombre"].Value?.ToString() ?? string.Empty;
            idempresa = fila.Cells["IdEmpresa"].Value?.ToString() ?? string.Empty;
            //MessageBox.Show($"Grupo seleccionado: {IdGrupoSeleccionado} - {NombreGrupoSeleccionado}", "Selección", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btregresar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}