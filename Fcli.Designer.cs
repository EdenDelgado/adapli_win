namespace Sisvta
{
    partial class Fcli
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
            label4 = new Label();
            label3 = new Label();
            curptxt = new TextBox();
            btBaja = new Button();
            btguardar = new Button();
            btactualiza = new Button();
            telefono1txt = new TextBox();
            rfctxt = new TextBox();
            btrfc = new Label();
            domiciliotxt = new TextBox();
            nombretxt = new TextBox();
            label2 = new Label();
            label1 = new Label();
            Clave = new Label();
            clientetxt = new TextBox();
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
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(curptxt);
            panel1.Controls.Add(btBaja);
            panel1.Controls.Add(btguardar);
            panel1.Controls.Add(btactualiza);
            panel1.Controls.Add(telefono1txt);
            panel1.Controls.Add(rfctxt);
            panel1.Controls.Add(btrfc);
            panel1.Controls.Add(domiciliotxt);
            panel1.Controls.Add(nombretxt);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(Clave);
            panel1.Controls.Add(clientetxt);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(466, 226);
            panel1.TabIndex = 11;
            panel1.Paint += panel1_Paint;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(21, 161);
            label4.Name = "label4";
            label4.Size = new Size(33, 15);
            label4.TabIndex = 26;
            label4.Text = "Curp";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(21, 139);
            label3.Name = "label3";
            label3.Size = new Size(58, 15);
            label3.TabIndex = 25;
            label3.Text = "Telefono1";
            // 
            // curptxt
            // 
            curptxt.Location = new Point(97, 158);
            curptxt.Name = "curptxt";
            curptxt.Size = new Size(324, 23);
            curptxt.TabIndex = 22;
            // 
            // btBaja
            // 
            btBaja.Location = new Point(282, 187);
            btBaja.Name = "btBaja";
            btBaja.Size = new Size(75, 23);
            btBaja.TabIndex = 24;
            btBaja.Text = "Baja";
            btBaja.UseVisualStyleBackColor = true;
            btBaja.Click += btBaja_Click;
            // 
            // btguardar
            // 
            btguardar.Location = new Point(191, 187);
            btguardar.Name = "btguardar";
            btguardar.Size = new Size(75, 23);
            btguardar.TabIndex = 22;
            btguardar.Text = "Guardar";
            btguardar.UseVisualStyleBackColor = true;
            btguardar.Click += btguardar_Click;
            // 
            // btactualiza
            // 
            btactualiza.Location = new Point(110, 187);
            btactualiza.Name = "btactualiza";
            btactualiza.Size = new Size(75, 23);
            btactualiza.TabIndex = 23;
            btactualiza.Text = "Actualizar";
            btactualiza.UseVisualStyleBackColor = true;
            btactualiza.Click += btactualiza_Click;
            // 
            // telefono1txt
            // 
            telefono1txt.Location = new Point(97, 131);
            telefono1txt.Name = "telefono1txt";
            telefono1txt.Size = new Size(324, 23);
            telefono1txt.TabIndex = 21;
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
            Clave.Size = new Size(44, 15);
            Clave.TabIndex = 12;
            Clave.Text = "Cliente";
            // 
            // clientetxt
            // 
            clientetxt.Location = new Point(97, 13);
            clientetxt.Name = "clientetxt";
            clientetxt.Size = new Size(124, 23);
            clientetxt.TabIndex = 11;
            clientetxt.Leave += clientetxt_Leave;
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
            panel2.Location = new Point(0, 224);
            panel2.Name = "panel2";
            panel2.Size = new Size(466, 239);
            panel2.TabIndex = 13;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(0, 105);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(454, 131);
            dataGridView1.TabIndex = 11;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            dataGridView1.Click += dataGridView1_Click;
            dataGridView1.DoubleClick += dataGridView1_Click;
            // 
            // Fcli
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(466, 485);
            Controls.Add(panel2);
            Controls.Add(statusStrip1);
            Controls.Add(panel1);
            Name = "Fcli";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fcli Captura de Clientes";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private TextBox domiciliotxt;
        private TextBox nombretxt;
        private Label label2;
        private Label label1;
        private Label Clave;
        private TextBox clientetxt;
        private StatusStrip statusStrip1;
        private Panel panel2;
        private DataGridView dataGridView1;
        private TextBox rfctxt;
        private Label btrfc;
        private TextBox telefono1txt;
        private TextBox curptxt;
        private Button btBaja;
        private Button btguardar;
        private Button btactualiza;
        private Label label4;
        private Label label3;
    }
}