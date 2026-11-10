namespace Sisvta
{
    partial class Fvta
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            label1 = new Label();
            dtpHasta = new DateTimePicker();
            dtpDesde = new DateTimePicker();
            txtIdCliente = new TextBox();
            btnBuscar = new Button();
            dgvVentas = new DataGridView();
            panel2 = new Panel();
            lblTotalAcumulado = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVentas).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label1);
            panel1.Controls.Add(dtpHasta);
            panel1.Controls.Add(dtpDesde);
            panel1.Controls.Add(txtIdCliente);
            panel1.Controls.Add(btnBuscar);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(859, 59);
            panel1.TabIndex = 11;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 10);
            label1.Name = "label1";
            label1.Size = new Size(44, 15);
            label1.TabIndex = 21;
            label1.Text = "Cliente";
            // 
            // dtpHasta
            // 
            dtpHasta.Location = new Point(438, 4);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(211, 23);
            dtpHasta.TabIndex = 20;
            // 
            // dtpDesde
            // 
            dtpDesde.Location = new Point(175, 6);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(211, 23);
            dtpDesde.TabIndex = 19;
            // 
            // txtIdCliente
            // 
            txtIdCliente.Location = new Point(61, 4);
            txtIdCliente.Name = "txtIdCliente";
            txtIdCliente.Size = new Size(100, 23);
            txtIdCliente.TabIndex = 9;
            txtIdCliente.DoubleClick += txtIdCliente_DoubleClick;
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(699, 6);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(75, 23);
            btnBuscar.TabIndex = 8;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // dgvVentas
            // 
            dgvVentas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVentas.Dock = DockStyle.Fill;
            dgvVentas.Location = new Point(0, 59);
            dgvVentas.Name = "dgvVentas";
            dgvVentas.Size = new Size(859, 464);
            dgvVentas.TabIndex = 13;
            // 
            // panel2
            // 
            panel2.Controls.Add(lblTotalAcumulado);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 464);
            panel2.Name = "panel2";
            panel2.Size = new Size(859, 59);
            panel2.TabIndex = 18;
            // 
            // lblTotalAcumulado
            // 
            lblTotalAcumulado.AutoSize = true;
            lblTotalAcumulado.Location = new Point(527, 22);
            lblTotalAcumulado.Name = "lblTotalAcumulado";
            lblTotalAcumulado.Size = new Size(32, 15);
            lblTotalAcumulado.TabIndex = 0;
            lblTotalAcumulado.Text = "Total";
            // 
            // Fvta
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(859, 523);
            Controls.Add(panel2);
            Controls.Add(dgvVentas);
            Controls.Add(panel1);
            Name = "Fvta";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fvta";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVentas).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel panel1;
        private Button btnBuscar;
        private TextBox txtIdCliente;
        private DateTimePicker dtpHasta;
        private DateTimePicker dtpDesde;
        private DataGridView dgvVentas;
        private Panel panel2;
        private Label lblTotalAcumulado;
        private Label label1;
    }
}