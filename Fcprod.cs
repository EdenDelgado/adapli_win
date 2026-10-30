using Sisvta.Domain.Entities;
using Sisvta.UseCases;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sisvta;

public partial class Fcprod : Form
{
    private readonly IConsultarProductosUseCase _consultarProductosUseCase;
    private readonly BindingSource _bindingSource = new();
    private bool _isCargando = false; // <--- Candado para evitar que se ejecute dos veces a la vez

    public int IdSeleccionado { get; private set; } = 0;
    public string NombreSeleccionado { get; private set; } = string.Empty;
    public string DescripcionSeleccionada { get; private set; } = string.Empty;

    public Fcprod(IConsultarProductosUseCase consultarProductosUseCase)
    {
        InitializeComponent();
        _consultarProductosUseCase = consultarProductosUseCase ?? throw new ArgumentNullException(nameof(consultarProductosUseCase));

        this.Load += Fcprod_Load;
        this.dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;

        if (this.Controls.Find("txtBuscar", true).FirstOrDefault() is TextBox txt)
        {
            txt.TextChanged += txtBuscar_TextChanged;
        }
    }

    private async void Fcprod_Load(object? sender, EventArgs e)
    {
        if (DesignMode) return;
        await CargarProductosAsync();
    }

    private async Task CargarProductosAsync()
    {
        // Si ya está cargando o ya tiene datos, no permitimos otra consulta simultánea
        if (_isCargando || _bindingSource.DataSource != null) return;

        try
        {
            _isCargando = true; // Activamos el candado

            var listaProductos = await _consultarProductosUseCase.ObtenerTodosAsync();

            _bindingSource.DataSource = listaProductos.ToList();
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = _bindingSource;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al consultar productos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _isCargando = false; // Liberamos el candado pase lo que pase
        }
    }

    private void dataGridView1_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) SeleccionarProducto();
    }

    private void SeleccionarProducto()
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is Producto p)
        {
            IdSeleccionado = int.Parse(p.Clave);
            NombreSeleccionado = p.Nombre;
            DescripcionSeleccionada = p.Descripcion;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }

    private void button1_Click_1(object? sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }

    private void txtBuscar_TextChanged(object? sender, EventArgs e)
    {
        // El filtro de búsqueda solo actúa de manera local en memoria, sin tocar la BD
        if (sender is TextBox txt)
        {
            string filtro = txt.Text.Trim();

            if (string.IsNullOrWhiteSpace(filtro))
            {
                _bindingSource.Filter = null;
            }
            else
            {
                _bindingSource.Filter = string.Format("Nombre LIKE '%{0}%' OR Descripcion LIKE '%{0}%'", filtro);
            }
        }
    }
}