namespace Sisvta
{
    partial class Fcmov
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fcmov));
            splitContainer1 = new SplitContainer();
            btregresar = new Button();
            txalmacen = new ComboBox();
            Movtotx = new ComboBox();
            label5 = new Label();
            label4 = new Label();
            nombretx = new TextBox();
            clavetx = new TextBox();
            label3 = new Label();
            hastafecha = new DateTimePicker();
            Ejecuta = new Button();
            label2 = new Label();
            desdefecha = new DateTimePicker();
            dataGridView1 = new DataGridView();
            totaltx = new Label();
            label1 = new Label();
            dataGridView2 = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(btregresar);
            splitContainer1.Panel1.Controls.Add(txalmacen);
            splitContainer1.Panel1.Controls.Add(Movtotx);
            splitContainer1.Panel1.Controls.Add(label5);
            splitContainer1.Panel1.Controls.Add(label4);
            splitContainer1.Panel1.Controls.Add(nombretx);
            splitContainer1.Panel1.Controls.Add(clavetx);
            splitContainer1.Panel1.Controls.Add(label3);
            splitContainer1.Panel1.Controls.Add(hastafecha);
            splitContainer1.Panel1.Controls.Add(Ejecuta);
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(desdefecha);
            splitContainer1.Panel1.Controls.Add(dataGridView1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(totaltx);
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Panel2.Controls.Add(dataGridView2);
            splitContainer1.Size = new Size(856, 443);
            splitContainer1.SplitterDistance = 90;
            splitContainer1.TabIndex = 0;
            // 
            // btregresar
            // 
            btregresar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btregresar.BackColor = Color.MediumAquamarine;
            btregresar.Image = (Image)resources.GetObject("btregresar.Image");
            btregresar.Location = new Point(764, 3);
            btregresar.Name = "btregresar";
            btregresar.Size = new Size(80, 80);
            btregresar.TabIndex = 13;
            btregresar.Text = "Regresar";
            btregresar.TextImageRelation = TextImageRelation.ImageAboveText;
            btregresar.UseVisualStyleBackColor = false;
            btregresar.Click += btregresar_Click_1;
            // 
            // txalmacen
            // 
            txalmacen.FormattingEnabled = true;
            txalmacen.Items.AddRange(new object[] { "1", "2", "3", "1012", "1013", "1015" });
            txalmacen.Location = new Point(329, 33);
            txalmacen.Name = "txalmacen";
            txalmacen.Size = new Size(105, 23);
            txalmacen.TabIndex = 12;
            // 
            // Movtotx
            // 
            Movtotx.FormattingEnabled = true;
            Movtotx.Items.AddRange(new object[] { "Todos", "Entradas", "Salidas", "Traspasos" });
            Movtotx.Location = new Point(555, 31);
            Movtotx.Name = "Movtotx";
            Movtotx.Size = new Size(105, 23);
            Movtotx.TabIndex = 11;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(456, 36);
            label5.Name = "label5";
            label5.Size = new Size(77, 15);
            label5.TabIndex = 10;
            label5.Text = "Movimientos";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(209, 36);
            label4.Name = "label4";
            label4.Size = new Size(54, 15);
            label4.TabIndex = 9;
            label4.Text = "Almacen";
            // 
            // nombretx
            // 
            nombretx.Location = new Point(126, 3);
            nombretx.Name = "nombretx";
            nombretx.Size = new Size(423, 23);
            nombretx.TabIndex = 8;
            // 
            // clavetx
            // 
            clavetx.Location = new Point(33, 3);
            clavetx.Name = "clavetx";
            clavetx.Size = new Size(66, 23);
            clavetx.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(456, 60);
            label3.Name = "label3";
            label3.Size = new Size(92, 15);
            label3.TabIndex = 5;
            label3.Text = "Hasta que fecha";
            // 
            // hastafecha
            // 
            hastafecha.Format = DateTimePickerFormat.Short;
            hastafecha.Location = new Point(565, 57);
            hastafecha.Name = "hastafecha";
            hastafecha.Size = new Size(95, 23);
            hastafecha.TabIndex = 4;
            // 
            // Ejecuta
            // 
            Ejecuta.BackColor = Color.MediumAquamarine;
            Ejecuta.Image = (Image)resources.GetObject("Ejecuta.Image");
            Ejecuta.Location = new Point(681, 12);
            Ejecuta.Name = "Ejecuta";
            Ejecuta.Size = new Size(71, 63);
            Ejecuta.TabIndex = 3;
            Ejecuta.Text = "Filtra";
            Ejecuta.TextAlign = ContentAlignment.BottomCenter;
            Ejecuta.UseVisualStyleBackColor = false;
            Ejecuta.Click += Ejecuta_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(207, 60);
            label2.Name = "label2";
            label2.Size = new Size(94, 15);
            label2.TabIndex = 2;
            label2.Text = "Desde que fecha";
            // 
            // desdefecha
            // 
            desdefecha.Format = DateTimePickerFormat.Short;
            desdefecha.Location = new Point(329, 59);
            desdefecha.Name = "desdefecha";
            desdefecha.Size = new Size(107, 23);
            desdefecha.TabIndex = 1;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Left;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(10, 90);
            dataGridView1.TabIndex = 0;
            dataGridView1.Visible = false;
            // 
            // totaltx
            // 
            totaltx.AutoSize = true;
            totaltx.Location = new Point(422, 330);
            totaltx.Name = "totaltx";
            totaltx.Size = new Size(28, 15);
            totaltx.TabIndex = 3;
            totaltx.Text = "0.00";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(384, 330);
            label1.Name = "label1";
            label1.Size = new Size(32, 15);
            label1.TabIndex = 2;
            label1.Text = "Total";
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Dock = DockStyle.Top;
            dataGridView2.Location = new Point(0, 0);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.Size = new Size(856, 309);
            dataGridView2.TabIndex = 0;
            // 
            // Fcmov
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(856, 443);
            Controls.Add(splitContainer1);
            Name = "Fcmov";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fcmov  MOVTOS ALMACEN";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private DataGridView dataGridView1;
        private DataGridView dataGridView2;
        private Label totaltx;
        private Label label1;
        private Label label2;
        private DateTimePicker desdefecha;
        private Button Ejecuta;
        private Label label3;
        private DateTimePicker hastafecha;
        private TextBox nombretx;
        private TextBox clavetx;
        private Label label5;
        private Label label4;
        private ComboBox txalmacen;
        private ComboBox Movtotx;
        private Button btregresar;
    }
}