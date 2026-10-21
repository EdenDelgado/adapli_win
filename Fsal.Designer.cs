namespace Sisvta
{
    partial class Fsal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fsal));
            splitContainer1 = new SplitContainer();
            panel3 = new Panel();
            regresabt = new Button();
            button2 = new Button();
            btGuardar = new Button();
            label8 = new Label();
            pictureBox1 = new PictureBox();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            txdoc_origen = new TextBox();
            txunidad = new TextBox();
            txproyecto = new TextBox();
            txobservacion = new TextBox();
            txaquien = new TextBox();
            txcliente = new TextBox();
            txtrabajo = new TextBox();
            txalmacen = new TextBox();
            txconcept = new TextBox();
            txfecha = new TextBox();
            txfolio = new TextBox();
            dataGridView1 = new DataGridView();
            dataGridView2 = new DataGridView();
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
            splitContainer1.Panel1.Controls.Add(panel3);
            splitContainer1.Panel1.Controls.Add(label7);
            splitContainer1.Panel1.Controls.Add(label6);
            splitContainer1.Panel1.Controls.Add(label5);
            splitContainer1.Panel1.Controls.Add(label4);
            splitContainer1.Panel1.Controls.Add(label3);
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(txdoc_origen);
            splitContainer1.Panel1.Controls.Add(txunidad);
            splitContainer1.Panel1.Controls.Add(txproyecto);
            splitContainer1.Panel1.Controls.Add(txobservacion);
            splitContainer1.Panel1.Controls.Add(txaquien);
            splitContainer1.Panel1.Controls.Add(txcliente);
            splitContainer1.Panel1.Controls.Add(txtrabajo);
            splitContainer1.Panel1.Controls.Add(txalmacen);
            splitContainer1.Panel1.Controls.Add(txconcept);
            splitContainer1.Panel1.Controls.Add(txfecha);
            splitContainer1.Panel1.Controls.Add(txfolio);
            splitContainer1.Panel1.Controls.Add(dataGridView1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dataGridView2);
            splitContainer1.Size = new Size(1034, 522);
            splitContainer1.SplitterDistance = 177;
            splitContainer1.TabIndex = 0;
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
            panel3.Size = new Size(1034, 84);
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
            label8.Size = new Size(265, 32);
            label8.TabIndex = 1;
            label8.Text = "SALIDAS DE ARTICULOS";
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
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(231, 130);
            label7.Name = "label7";
            label7.Size = new Size(45, 15);
            label7.TabIndex = 18;
            label7.Text = "Aquien";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(732, 87);
            label6.Name = "label6";
            label6.Size = new Size(44, 15);
            label6.TabIndex = 17;
            label6.Text = "Cliente";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(587, 87);
            label5.Name = "label5";
            label5.Size = new Size(45, 15);
            label5.TabIndex = 16;
            label5.Text = "Trabajo";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(506, 87);
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
            label3.Size = new Size(59, 15);
            label3.TabIndex = 14;
            label3.Text = "Concepto";
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
            label1.Size = new Size(33, 15);
            label1.TabIndex = 12;
            label1.Text = "Folio";
            // 
            // txdoc_origen
            // 
            txdoc_origen.Location = new Point(715, 148);
            txdoc_origen.Name = "txdoc_origen";
            txdoc_origen.Size = new Size(102, 23);
            txdoc_origen.TabIndex = 11;
            // 
            // txunidad
            // 
            txunidad.Location = new Point(575, 148);
            txunidad.Name = "txunidad";
            txunidad.Size = new Size(102, 23);
            txunidad.TabIndex = 10;
            // 
            // txproyecto
            // 
            txproyecto.Location = new Point(454, 148);
            txproyecto.Name = "txproyecto";
            txproyecto.Size = new Size(102, 23);
            txproyecto.TabIndex = 9;
            // 
            // txobservacion
            // 
            txobservacion.Location = new Point(321, 148);
            txobservacion.Name = "txobservacion";
            txobservacion.Size = new Size(102, 23);
            txobservacion.TabIndex = 8;
            // 
            // txaquien
            // 
            txaquien.Location = new Point(191, 148);
            txaquien.Name = "txaquien";
            txaquien.Size = new Size(102, 23);
            txaquien.TabIndex = 7;
            // 
            // txcliente
            // 
            txcliente.Location = new Point(695, 105);
            txcliente.Name = "txcliente";
            txcliente.Size = new Size(308, 23);
            txcliente.TabIndex = 6;
            // 
            // txtrabajo
            // 
            txtrabajo.Location = new Point(587, 105);
            txtrabajo.Name = "txtrabajo";
            txtrabajo.Size = new Size(102, 23);
            txtrabajo.TabIndex = 5;
            // 
            // txalmacen
            // 
            txalmacen.Location = new Point(491, 105);
            txalmacen.Name = "txalmacen";
            txalmacen.Size = new Size(90, 23);
            txalmacen.TabIndex = 4;
            // 
            // txconcept
            // 
            txconcept.Location = new Point(360, 105);
            txconcept.Name = "txconcept";
            txconcept.Size = new Size(125, 23);
            txconcept.TabIndex = 3;
            // 
            // txfecha
            // 
            txfecha.Location = new Point(252, 105);
            txfecha.Name = "txfecha";
            txfecha.Size = new Size(102, 23);
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
            dataGridView1.Size = new Size(114, 80);
            dataGridView1.TabIndex = 0;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(0, 0);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.Size = new Size(1034, 341);
            dataGridView2.TabIndex = 0;
            // 
            // Fsal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1034, 522);
            Controls.Add(splitContainer1);
            Name = "Fsal";
            Text = "Fsal  SALIDAS ALMACEN";
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
        private TextBox txdoc_origen;
        private TextBox txunidad;
        private TextBox txproyecto;
        private TextBox txobservacion;
        private TextBox txaquien;
        private TextBox txcliente;
        private TextBox txtrabajo;
        private TextBox txalmacen;
        private TextBox txconcept;
        private TextBox txfecha;
        private TextBox txfolio;
        private Label label7;
        private Label label6;
        private Label label5;
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
    }
}