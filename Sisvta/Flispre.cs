using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta
{
    public partial class Flispre : Form
    {
        private readonly ApplicationDbContext _context;
        private readonly int _idListaPrecio;
        private readonly bool _esEdicion;

        // Variables para almacenar los datos recibidos
        private string _listaprecio;
        private string _condicion;
        private string _porcentaje;
        private string _pdv;
        private string _empresa;

        // Constructor vacío (requerido por el diseñador visual de Windows Forms)
        public Flispre()
        {
            InitializeComponent();
        }

        // Constructor con DI y parámetros (colocando los opcionales al final para evitar errores de compilación)
        public Flispre(ApplicationDbContext context,
                       string listaprecio = "",
                       string condicion = "",
                       string porcentaje = "",
                       string pdv = "",
                       string empresa = "",
                       int idlispre = 0) : this()
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _idListaPrecio = idlispre;
            _esEdicion = _idListaPrecio > 0;

            _listaprecio = listaprecio;
            _condicion = condicion;
            _porcentaje = porcentaje;
            _pdv = pdv;
            _empresa = empresa;

            // Si es edición, cargamos los datos en los controles
            if (_esEdicion)
            {
                listapreciotx.Text = _listaprecio;
                condiciontx.Text = _condicion;
                porcentajetx.Text = _porcentaje;
                pdvtx.Text = _pdv;
                empresatx.Text = _empresa;
            }
        }

        private void Falm_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
        }

        private async void btGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validaciones básicas
                if (string.IsNullOrWhiteSpace(listapreciotx.Text))
                {
                    MessageBox.Show("El nombre de la lista de precios es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    listapreciotx.Focus();
                    return;
                }

                string listaprecioVal = listapreciotx.Text.Trim();
                string condicionVal = condiciontx.Text.Trim();
                string pdvVal = pdvtx.Text.Trim();

                // Conversión segura de numéricos
                decimal.TryParse(porcentajetx.Text.Trim(), out decimal porcentajeVal);
                int.TryParse(empresatx.Text.Trim(), out int empresaVal);

                string connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString)) return;

                using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                await connection.OpenAsync();

                if (!_esEdicion)
                {
                    // === NUEVO REGISTRO (INSERT) ===
                    string sqlInsert = @"
                        INSERT INTO listaprecio (listaprecio, condicion, porcentaje, pdv, id_empresa)
                        VALUES (@listaprecio, @condicion, @porcentaje, @pdv, @id_empresa);";

                    using var command = connection.CreateCommand();
                    command.CommandText = sqlInsert;
                    command.Parameters.Add("@listaprecio", SqlDbType.NVarChar).Value = listaprecioVal;
                    command.Parameters.Add("@condicion", SqlDbType.NVarChar).Value = condicionVal;
                    command.Parameters.Add("@porcentaje", SqlDbType.Decimal).Value = porcentajeVal;
                    command.Parameters.Add("@pdv", SqlDbType.NVarChar).Value = pdvVal;
                    command.Parameters.Add("@id_empresa", SqlDbType.Int).Value = empresaVal;

                    await command.ExecuteNonQueryAsync();

                    MessageBox.Show("Lista de precios guardada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // === ACTUALIZACIÓN (UPDATE) ===
                    string sqlUpdate = @"
                        UPDATE listaprecio 
                        SET listaprecio = @listaprecio,
                            condicion = @condicion,
                            porcentaje = @porcentaje,
                            pdv = @pdv,
                            id_empresa = @id_empresa
                        WHERE id_listaprecio = @id_listaprecio;";

                    using var command = connection.CreateCommand();
                    command.CommandText = sqlUpdate;
                    command.Parameters.Add("@listaprecio", SqlDbType.NVarChar).Value = listaprecioVal;
                    command.Parameters.Add("@condicion", SqlDbType.NVarChar).Value = condicionVal;
                    command.Parameters.Add("@porcentaje", SqlDbType.Decimal).Value = porcentajeVal;
                    command.Parameters.Add("@pdv", SqlDbType.NVarChar).Value = pdvVal;
                    command.Parameters.Add("@id_empresa", SqlDbType.Int).Value = empresaVal;
                    command.Parameters.Add("@id_listaprecio", SqlDbType.Int).Value = _idListaPrecio;

                    int filasAfectadas = await command.ExecuteNonQueryAsync();

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Lista de precios actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el registro a actualizar en la base de datos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Indicamos éxito y cerramos para que el catálogo padre (Fclispre) se recargue
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la lista de precios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btregresar_Click(object sender, EventArgs e)
        {
            //DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}