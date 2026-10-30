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
    public partial class Fcampre : Form
    {
        private readonly ApplicationDbContext _context;
        private readonly int _idServicio;
        private readonly string _clave; private readonly string _nombre;

        private readonly int _listaPrecio; private readonly string _nomlistaPrecio;
        private readonly double _precioVenta;

        // Constructor para edición que recibe el Contexto, ID del servicio, Nombre de la lista y Precio de venta
        public Fcampre(ApplicationDbContext context, int idServicio, string clave, string nombre, int listaPrecio, string NomlistaPrecio, double precioVenta)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idServicio = idServicio;
            _clave = clave;
            _nombre = nombre;
            _listaPrecio = listaPrecio; _nomlistaPrecio = NomlistaPrecio;
            _precioVenta = precioVenta;

            // Forzamos la carga inicial aquí para no depender exclusivamente del Load
            _ = InicializarDatosAsync();
        }

        // Constructor vacío por si se requiere instanciar sin parámetros en algún otro lado
        public Fcampre(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idServicio = 0;
            _clave = string.Empty; _nombre = string.Empty;
            _listaPrecio = 0; _nomlistaPrecio = string.Empty;
            _precioVenta = 0.0;

            _ = InicializarDatosAsync();
        }

        private async Task InicializarDatosAsync()
        {
            await CargarServiciosAsync();

            // Si estamos editando, asignamos los valores a los controles
            if (_idServicio > 0)
            {
                clavetxt.SelectedValue = _idServicio;
                nombretxt.Text = _nombre;
                listapreciotxt.Text = _nomlistaPrecio;
                preciotxt.Text = _precioVenta.ToString("N2");
            }
        }

        private async void Fcampre_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
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
        // EVENTO PARA ACTUALIZAR EL PRECIO EN LA BASE DE DATOS
        // =========================================================================
        private async void btaceptar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validaciones básicas antes de actualizar
                if (clavetxt.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione un artículo o clave válida.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    clavetxt.Focus();
                    return;
                }

                if (!decimal.TryParse(preciotxt.Text, out decimal precio) || precio < 0)
                {
                    MessageBox.Show("Ingrese un precio unitario válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    preciotxt.Focus();
                    return;
                }

                int idArticulo = Convert.ToInt32(clavetxt.SelectedValue);

                // 2. Consulta SQL de ACTUALIZACIÓN adaptada a preciosarticulo
                string sqlUpdate = @"
                    UPDATE preciosarticulo 
                    SET precioventa = @precioventa 
                    WHERE articulo = @articulo and listaprecio=@listaprecio;";

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = sqlUpdate;

                // Añadimos los parámetros de forma segura para evitar SQL Injection
                command.Parameters.Add("@precioventa", SqlDbType.Float).Value = (double)precio;
                command.Parameters.Add("@articulo", SqlDbType.Int).Value = idArticulo;
                command.Parameters.Add("@listaprecio", SqlDbType.Int).Value = _listaPrecio;

                await command.ExecuteNonQueryAsync();

                MessageBox.Show("Precio de venta actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Indicamos que todo salió bien y cerramos el formulario para refrescar el grid padre
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el precio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void preciotxt_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(preciotxt.Text, out decimal precio))
            {
                preciotxt.Text = precio.ToString("N2");
            }
        }


    }
}