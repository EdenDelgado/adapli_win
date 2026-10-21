namespace Sisvta
{
    partial class Fprov
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
            rfctxt = new TextBox();
            btrfc = new Label();
            btBaja = new Button();
            btactualiza = new Button();
            domiciliotxt = new TextBox();
            nombretxt = new TextBox();
            label2 = new Label();
            label1 = new Label();
            Clave = new Label();
            proveedortxt = new TextBox(); // <--- Actualizado a proveedortxt
            btguardar = new Button();
            statusStrip1 = new StatusStrip();
            panel2 = new Panel();
            dataGridView1 = new DataGridView();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(rfctxt);
            panel1.Controls.Add(btrfc);
            panel1.Controls.Add(btBaja);
            panel1.Controls.Add(btactualiza);
            panel1.Controls.Add(domiciliotxt);
            panel1.Controls.Add(nombretxt);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(Clave);
            panel1.Controls.Add(proveedortxt); // <--- Actualizado a proveedortxt
            panel1.Controls.Add(btguardar);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(466, 167);
            panel1.TabIndex = 11;
            panel1.Paint += panel1_Paint;
            // 
            // rfctxt
            // 
            rfctxt.Location = new Point(97, 104);
            rfctxt.Name = "rfctxt";
            rfctxt.Size = new Size(324, 23);
            rfctxt.TabIndex = 20;
            // 
            // btrfc
            // 
            btrfc.AutoSize = true;
            btrfc.Location = new Point(19, 109);
            btrfc.Name = "btrfc";
            btrfc.Size = new Size(24, 15);
            btrfc.TabIndex = 19;
            btrfc.Text = "Rfc";
            // 
            // btBaja
            // 
            btBaja.Location = new Point(318, 131);
            btBaja.Name = "btBaja";
            btBaja.Size = new Size(75, 23);
            btBaja.TabIndex = 18;
            btBaja.Text = "Baja";
            btBaja.UseVisualStyleBackColor = true;
            btBaja.Click += btBaja_Click;
            // 
            // btactualiza
            // 
            btactualiza.Location = new Point(146, 131);
            btactualiza.Name = "btactualiza";
            btactualiza.Size = new Size(75, 23);
            btactualiza.TabIndex = 17;
            btactualiza.Text = "Actualizar";
            btactualiza.UseVisualStyleBackColor = true;
            btactualiza.Click += btactualiza_Click;
            // 
            // domiciliotxt
            // 
            domiciliotxt.Location = new Point(97, 75);
            domiciliotxt.Name = "domiciliotxt";
            domiciliotxt.Size = new Size(324, 23);
            domiciliotxt.TabIndex = 16;
            // 
            // nombretxt
            // 
            nombretxt.Location = new Point(97, 45);
            nombretxt.Name = "nombretxt";
            nombretxt.Size = new Size(324, 23);
            nombretxt.TabIndex = 15;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(19, 80);
            label2.Name = "label2";
            label2.Size = new Size(58, 15);
            label2.TabIndex = 14;
            label2.Text = "Domicilio";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(19, 48);
            label1.Name = "label1";
            label1.Size = new Size(51, 15);
            label1.TabIndex = 13;
            label1.Text = "Nombre";
            // 
            // Clave
            // 
            Clave.AutoSize = true;
            Clave.Location = new Point(21, 16);
            Clave.Name = "Clave";
            Clave.Size = new Size(61, 15);
            Clave.TabIndex = 12;
            Clave.Text = "Proveedor";
            // 
            // proveedortxt
            // 
            proveedortxt.Location = new Point(97, 13);
            proveedortxt.Name = "proveedortxt";
            proveedortxt.Size = new Size(124, 23);
            proveedortxt.TabIndex = 11;
            proveedortxt.Leave += proveedortxt_Leave; // <--- Evento Leave apuntando al método correcto
            // 
            // btguardar
            // 
            btguardar.Location = new Point(227, 131);
            btguardar.Name = "btguardar";
            btguardar.Size = new Size(75, 23);
            btguardar.TabIndex = 10;
            btguardar.Text = "Guardar";
            btguardar.UseVisualStyleBackColor = true;
            btguardar.Click += btguardar_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Location = new Point(0, 463);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(466, 22);
            statusStrip1.TabIndex = 12;
            statusStrip1.Text = "statusStrip1";
            // 
            // panel2
            // 
            panel2.Controls.Add(dataGridView1);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 160);
            panel2.Name = "panel2";
            panel2.Size = new Size(466, 303);
            panel2.TabIndex = 13;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(466, 303);
            dataGridView1.TabIndex = 11;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            dataGridView1.Click += dataGridView1_Click;
            dataGridView1.DoubleClick += dataGridView1_Click;
            // 
            // Fprov
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(466, 485);
            Controls.Add(panel2);
            Controls.Add(statusStrip1);
            Controls.Add(panel1);
            Name = "Fprov";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fprov Captura de Proveedores";
            Load += Fcli_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Button btBaja;
        private Button btactualiza;
        private TextBox domiciliotxt;
        private TextBox nombretxt;
        private Label label2;
        private Label label1;
        private Label Clave;
        private TextBox proveedortxt; // <--- Declarado como proveedortxt
        private Button btguardar;
        private StatusStrip statusStrip1;
        private Panel panel2;
        private DataGridView dataGridView1;
        private TextBox rfctxt;
        private Label btrfc;
    }
}