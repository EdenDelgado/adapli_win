using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sisvta.UseCases;

namespace Sisvta
{
    public partial class Fcgru : Form
    {
        private Fgru? _fgru;
        private readonly GestionarGruposLineasUseCase _useCase;

        public Fcgru(GestionarGruposLineasUseCase useCase)
        {
            InitializeComponent();
            _useCase = useCase;

            dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete;
        }

        private async void Fcgru_Load(object sender, EventArgs e)
        {
            await CargarGridAsync();
        }


        public async Task CargarGridAsync()
        {
            try
            {
                var lista = await _useCase.ObtenerTodosAsync();
                var listaResultado = lista.ToList();

                // MENSAJE DE DIAGNÓSTICO TEMPORAL:
                // MessageBox.Show($"Registros encontrados en Grupos: {listaResultado.Count}", "Diagnóstico", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = listaResultado;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error detallado al cargar grupos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Opcional: Personalizar nombres de columnas o formatos si lo deseas
            if (dataGridView1.Columns["Grupo"] != null)
            {
                dataGridView1.Columns["Grupo"].HeaderText = "Grupo";
                dataGridView1.Columns["Grupo"].Width = 50; // Ancho ajustado para ~6 caracteres (puedes probar entre 60 y 70 píxeles)
                dataGridView1.Columns["Grupo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Opcional: centrado
                dataGridView1.Columns["Nombre"].Width = 140;
            }
            if (dataGridView1.Columns["DateRecord"] != null)
            {
                dataGridView1.Columns["DateRecord"].HeaderText = "Fecha Registro";
                dataGridView1.Columns["DateRecord"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {

        }






        private void AsegurarFgruVivo()
        {
            if (_fgru == null || _fgru.IsDisposed)
            {
                _fgru = new Fgru(_useCase);
                _fgru.Owner = this;
                if (this.MdiParent != null)
                {
                    _fgru.MdiParent = this.MdiParent;
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 1. Validar fila válida
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView1.Rows.Count)
            {
                return;
            }

            try
            {
                // 2. Obtener el texto del campo de la celda
                var cellValue = dataGridView1.Rows[e.RowIndex].Cells["nombre"].Value;
                string nombreSeleccionado = cellValue?.ToString() ?? string.Empty;

                // 3. Asegurarnos de que _fgru esté vivo
                AsegurarFgruVivo();

                // 4. Enviar el valor y mostrar el formulario
                _fgru.CargarNombre(nombreSeleccionado);
                _fgru.Show();
                _fgru.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al seleccionar el registro: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void nuevobt_Click(object sender, EventArgs e)
        {
            // Aseguramos que el formulario esté instanciado antes de usarlo
            AsegurarFgruVivo();

            // Opcional: Si deseas limpiar o preparar el formulario para un registro totalmente nuevo, puedes llamar a un método de limpieza aquí si lo tienes en Fgru.

            _fgru.Show();
            _fgru.BringToFront();
        }

        private void btlinea_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btarticulo_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void regresabt_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}