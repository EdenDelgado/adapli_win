using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Entities;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Fddev : Form
    {
        private readonly ApplicationDbContext _context;
        private readonly string _movInv; // Variable para almacenar el ID del movimiento/inventario recibido

        // Constructor que recibe el ApplicationDbContext y el identificador del movimiento
        public Fddev(ApplicationDbContext context, string movinv)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _movInv = movinv;

            // Forzamos la carga inicial aquí para no depender exclusivamente del Load
            _ = InicializarDatosAsync();
        }

        private async Task InicializarDatosAsync()
        {
            await CargarServiciosAsync();
        }

        private async void Fdent_Load(object sender, EventArgs e)
        {
            await CargarServiciosAsync();
        }

        // =========================================================================
        // MÉTODO PARA CARGAR SERVICIOS EN 'clavetxt'
        // =========================================================================
        private async Task CargarServiciosAsync()
        {
            try
            {
                if (_context == null) return;

                var listaServicios = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
                    _context.Set<Sisvta.Domain.Entities.Servicio>()
                        .Select(s => new { id_servicio = s.IdServicio, clave = s.Clave })
                );

                if (listaServicios != null && listaServicios.Count > 0)
                {
                    clavetxt.DataSource = null;
                    clavetxt.DisplayMember = "clave";
                    clavetxt.ValueMember = "id_servicio";
                    clavetxt.DataSource = listaServicios;
                    clavetxt.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los servicios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================================
        // EVENTO PARA INSERTAR EL DETALLE EN LA BASE DE DATOS
        // =========================================================================
        private async void btaceptar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validaciones básicas antes de insertar
                if (clavetxt.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione un artículo o clave válida.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    clavetxt.Focus();
                    return;
                }

                if (!decimal.TryParse(cantidadtxt.Text, out decimal cantidad) || cantidad <= 0)
                {
                    MessageBox.Show("Ingrese una cantidad válida.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cantidadtxt.Focus();
                    return;
                }

                if (!decimal.TryParse(preciotxt.Text, out decimal precio) || precio < 0)
                {
                    MessageBox.Show("Ingrese un precio unitario válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    preciotxt.Focus();
                    return;
                }

                int idArticulo = Convert.ToInt32(clavetxt.SelectedValue);

                // 2. Consulta SQL de inserción adaptada a tus tablas de inventario_detalles
                string sqlInsert = @"
                    INSERT INTO inventario_detalles (movinventario, articulo, cantidad, preciou) 
                    VALUES (@movinventario, @articulo, @cantidad, @preciou)";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlInsert;

                // Añadimos los parámetros de forma segura para evitar SQL Injection
                command.Parameters.Add("@movinventario", SqlDbType.Int).Value = Convert.ToInt32(_movInv);
                command.Parameters.Add("@articulo", SqlDbType.Int).Value = idArticulo;
                command.Parameters.Add("@cantidad", SqlDbType.Decimal).Value = cantidad;
                command.Parameters.Add("@preciou", SqlDbType.Float).Value = (double)precio;

                await command.ExecuteNonQueryAsync();

                MessageBox.Show("Artículo agregado correctamente al detalle.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Indicamos que todo salió bien y cerramos el formulario para refrescar el grid padre
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el detalle: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clavetxt_Leave(object sender, EventArgs e)

        {
            string selectsql = @"Select nombre from servicios where clavetxt.txt=servicios.clave";
            //nombretxt.Text = Servicio. nombre;
        }

        // Opción recomendada: Usar el evento SelectedIndexChanged o SelectionChangeCommitted 
        // para que se actualice automáticamente al elegir un elemento del ComboBox.
        private async void clavetxt_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (clavetxt.SelectedValue != null && int.TryParse(clavetxt.SelectedValue.ToString(), out int idServicio))
                {
                    // Consultamos el nombre del servicio usando Entity Framework Core de forma rápida
                    var servicio = await _context.Set<Sisvta.Domain.Entities.Servicio>()
                        .AsNoTracking()
                        .Where(s => s.IdServicio == idServicio)
                        .Select(s => new { s.Nombre, s.PrecioCompra }) // Si también quieres el precio por defecto
                        .FirstOrDefaultAsync();

                    if (servicio != null)
                    {
                        nombretxt.Text = servicio.Nombre ?? string.Empty;

                        // Opcional: Si deseas autocompletar el precio unitario con el precio de compra del servicio
                        // if (servicio.PrecioCompra.HasValue && string.IsNullOrEmpty(preciotxt.Text))
                        // {
                        //     preciotxt.Text = servicio.PrecioCompra.Value.ToString("N2");
                        // }
                    }
                }
                else
                {
                    nombretxt.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el nombre del servicio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void preciotxt_Leave(object sender, EventArgs e)
        {
           
            importetxt.Text = (Convert.ToDouble(cantidadtxt.Text) * Convert.ToDouble(preciotxt.Text)).ToString("N2");
        }
    }
}