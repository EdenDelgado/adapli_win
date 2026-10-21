namespace Sisvta
{
    partial class Fimpinv
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fimpinv));
            splitContainer1 = new SplitContainer();
            ImportaInventario = new Button();
            panel3 = new Panel();
            regresabt = new Button();
            button2 = new Button();
            btGuardar = new Button();
            label8 = new Label();
            pictureBox1 = new PictureBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            txnomalmacen = new TextBox();
            txusuario = new TextBox();
            txfecha = new TextBox();
            txfolio = new TextBox();
            dataGridView1 = new DataGridView();
            dataGridView2 = new DataGridView();
            txtNombreArchivo = new TextBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
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
            splitContainer1.Panel1.Controls.Add(txtNombreArchivo);
            splitContainer1.Panel1.Controls.Add(ImportaInventario);
            splitContainer1.Panel1.Controls.Add(panel3);
            splitContainer1.Panel1.Controls.Add(label4);
            splitContainer1.Panel1.Controls.Add(label3);
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(txnomalmacen);
            splitContainer1.Panel1.Controls.Add(txusuario);
            splitContainer1.Panel1.Controls.Add(txfecha);
            splitContainer1.Panel1.Controls.Add(txfolio);
            splitContainer1.Panel1.Controls.Add(dataGridView1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dataGridView2);
            splitContainer1.Size = new Size(827, 522);
            splitContainer1.SplitterDistance = 177;
            splitContainer1.TabIndex = 0;
            // 
            // ImportaInventario
            // 
            ImportaInventario.Location = new Point(646, 137);
            ImportaInventario.Name = "ImportaInventario";
            ImportaInventario.Size = new Size(112, 27);
            ImportaInventario.TabIndex = 23;
            ImportaInventario.Text = "ImportaInventario";
            ImportaInventario.UseVisualStyleBackColor = true;
            ImportaInventario.Click += ImportaInventario_Click;
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.ControlLight;
            panel3.Controls.Add(regresabt);
            panel3.Controls.Add(button2);
            panel3.Controls.Add(btGuardar);
            panel3.Controls.Add(label8);
            panel3.Controls.Add(pictureBox1);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(827, 84);
            panel3.TabIndex = 22;
            // 
            // regresabt
            // 
            regresabt.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            regresabt.BackColor = Color.MediumAquamarine;
            regresabt.Image = (Image)resources.GetObject("regresabt.Image");
            regresabt.Location = new Point(713, 4);
            regresabt.Name = "regresabt";
            regresabt.Size = new Size(80, 80);
            regresabt.TabIndex = 6;
            regresabt.Text = "Regresar";
            regresabt.TextImageRelation = TextImageRelation.ImageAboveText;
            regresabt.UseVisualStyleBackColor = false;
            regresabt.Click += regresabt_Click;
            // 
            // button2
            // 
            button2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button2.BackColor = Color.MediumAquamarine;
            button2.Image = (Image)resources.GetObject("button2.Image");
            button2.Location = new Point(627, 3);
            button2.Name = "button2";
            button2.Size = new Size(80, 80);
            button2.TabIndex = 4;
            button2.Text = "Nuevo";
            button2.TextImageRelation = TextImageRelation.ImageAboveText;
            button2.UseVisualStyleBackColor = false;
            // 
            // btGuardar
            // 
            btGuardar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btGuardar.BackColor = Color.MediumAquamarine;
            btGuardar.Image = (Image)resources.GetObject("btGuardar.Image");
            btGuardar.Location = new Point(541, 4);
            btGuardar.Name = "btGuardar";
            btGuardar.Size = new Size(80, 80);
            btGuardar.TabIndex = 2;
            btGuardar.Text = "Guardar";
            btGuardar.TextImageRelation = TextImageRelation.ImageAboveText;
            btGuardar.UseVisualStyleBackColor = false;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(123, 12);
            label8.Name = "label8";
            label8.Size = new Size(252, 32);
            label8.TabIndex = 1;
            label8.Text = "IMPORTA INVENTARIO";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(50, 50);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(568, 87);
            label4.Name = "label4";
            label4.Size = new Size(54, 15);
            label4.TabIndex = 15;
            label4.Text = "Almacen";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(412, 87);
            label3.Name = "label3";
            label3.Size = new Size(47, 15);
            label3.TabIndex = 14;
            label3.Text = "Usuario";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(297, 87);
            label2.Name = "label2";
            label2.Size = new Size(38, 15);
            label2.TabIndex = 13;
            label2.Text = "Fecha";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(172, 86);
            label1.Name = "label1";
            label1.Size = new Size(63, 15);
            label1.TabIndex = 12;
            label1.Text = "IdInvFisico";
            // 
            // txnomalmacen
            // 
            txnomalmacen.Location = new Point(568, 105);
            txnomalmacen.Name = "txnomalmacen";
            txnomalmacen.Size = new Size(157, 23);
            txnomalmacen.TabIndex = 5;
            // 
            // txusuario
            // 
            txusuario.Location = new Point(410, 105);
            txusuario.Name = "txusuario";
            txusuario.Size = new Size(152, 23);
            txusuario.TabIndex = 4;
            // 
            // txfecha
            // 
            txfecha.Location = new Point(252, 105);
            txfecha.Name = "txfecha";
            txfecha.Size = new Size(128, 23);
            txfecha.TabIndex = 2;
            // 
            // txfolio
            // 
            txfolio.Location = new Point(139, 105);
            txfolio.Name = "txfolio";
            txfolio.Size = new Size(102, 23);
            txfolio.TabIndex = 1;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(0, 89);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(52, 39);
            dataGridView1.TabIndex = 0;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(0, 0);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.Size = new Size(827, 341);
            dataGridView2.TabIndex = 0;
            // 
            // txtNombreArchivo
            // 
            txtNombreArchivo.Location = new Point(488, 141);
            txtNombreArchivo.Name = "txtNombreArchivo";
            txtNombreArchivo.Size = new Size(152, 23);
            txtNombreArchivo.TabIndex = 24;
            // 
            // Fimpinv
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(827, 522);
            Controls.Add(splitContainer1);
            Name = "Fimpinv";
            Text = "Fimpinv  IMPORTA INVENTARIO";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private DataGridView dataGridView1;
        private DataGridView dataGridView2;
        private TextBox txnomalmacen;
        private TextBox txusuario;
        private TextBox txfecha;
        private TextBox txfolio;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Panel panel3;
        private Button regresabt;
        private Button button2;
        private Button btGuardar;
        private Label label8;
        private PictureBox pictureBox1;
        private Button ImportaInventario;
        private TextBox txtNombreArchivo;
    }
}