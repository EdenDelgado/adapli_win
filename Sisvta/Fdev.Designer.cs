namespace Sisvta
{
    partial class Fdev
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fdev));
            splitContainer1 = new SplitContainer();
            label10 = new Label();
            txnotacredito = new TextBox();
            txalmacen = new ComboBox();
            label9 = new Label();
            label6 = new Label();
            txobservacion = new TextBox();
            conceptotxt = new ComboBox();
            txusuario = new ComboBox();
            txfecha = new DateTimePicker();
            panel3 = new Panel();
            regresabt = new Button();
            button2 = new Button();
            btGuardar = new Button();
            label8 = new Label();
            pictureBox1 = new PictureBox();
            label7 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            txdoc_origen = new TextBox();
            txentrada = new TextBox();
            txfolio = new TextBox();
            dataGridView1 = new DataGridView();
            btborrar = new Button();
            btagregar = new Button();
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
            splitContainer1.Panel1.Controls.Add(label10);
            splitContainer1.Panel1.Controls.Add(txnotacredito);
            splitContainer1.Panel1.Controls.Add(txalmacen);
            splitContainer1.Panel1.Controls.Add(label9);
            splitContainer1.Panel1.Controls.Add(label6);
            splitContainer1.Panel1.Controls.Add(txobservacion);
            splitContainer1.Panel1.Controls.Add(conceptotxt);
            splitContainer1.Panel1.Controls.Add(txusuario);
            splitContainer1.Panel1.Controls.Add(txfecha);
            splitContainer1.Panel1.Controls.Add(panel3);
            splitContainer1.Panel1.Controls.Add(label7);
            splitContainer1.Panel1.Controls.Add(label5);
            splitContainer1.Panel1.Controls.Add(label4);
            splitContainer1.Panel1.Controls.Add(label3);
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(txdoc_origen);
            splitContainer1.Panel1.Controls.Add(txentrada);
            splitContainer1.Panel1.Controls.Add(txfolio);
            splitContainer1.Panel1.Controls.Add(dataGridView1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(btborrar);
            splitContainer1.Panel2.Controls.Add(btagregar);
            splitContainer1.Panel2.Controls.Add(dataGridView2);
            splitContainer1.Size = new Size(1034, 522);
            splitContainer1.SplitterDistance = 177;
            splitContainer1.TabIndex = 0;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(484, 83);
            label10.Name = "label10";
            label10.Size = new Size(75, 15);
            label10.TabIndex = 31;
            label10.Text = "Nota Credito";
            // 
            // txnotacredito
            // 
            txnotacredito.Location = new Point(475, 103);
            txnotacredito.Name = "txnotacredito";
            txnotacredito.Size = new Size(102, 23);
            txnotacredito.TabIndex = 30;
            // 
            // txalmacen
            // 
            txalmacen.FormattingEnabled = true;
            txalmacen.Location = new Point(892, 103);
            txalmacen.Name = "txalmacen";
            txalmacen.Size = new Size(121, 23);
            txalmacen.TabIndex = 29;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(646, 128);
            label9.Name = "label9";
            label9.Size = new Size(84, 15);
            label9.TabIndex = 28;
            label9.Text = "Observaciones";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(357, 130);
            label6.Name = "label6";
            label6.Size = new Size(59, 15);
            label6.TabIndex = 27;
            label6.Text = "Concepto";
            // 
            // txobservacion
            // 
            txobservacion.Location = new Point(587, 147);
            txobservacion.Name = "txobservacion";
            txobservacion.Size = new Size(351, 23);
            txobservacion.TabIndex = 26;
            // 
            // conceptotxt
            // 
            conceptotxt.FormattingEnabled = true;
            conceptotxt.Location = new Point(327, 148);
            conceptotxt.Name = "conceptotxt";
            conceptotxt.Size = new Size(203, 23);
            conceptotxt.TabIndex = 25;
            // 
            // txusuario
            // 
            txusuario.FormattingEnabled = true;
            txusuario.Location = new Point(583, 103);
            txusuario.Name = "txusuario";
            txusuario.Size = new Size(303, 23);
            txusuario.TabIndex = 24;
            // 
            // txfecha
            // 
            txfecha.Format = DateTimePickerFormat.Short;
            txfecha.Location = new Point(247, 104);
            txfecha.Name = "txfecha";
            txfecha.Size = new Size(101, 23);
            txfecha.TabIndex = 23;
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
            btGuardar.Click += btGuardar_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(123, 12);
            label8.Name = "label8";
            label8.Size = new Size(346, 32);
            label8.TabIndex = 1;
            label8.Text = "DEVOLUCIONES DE ARTICULOS";
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
            label7.Size = new Size(47, 15);
            label7.TabIndex = 18;
            label7.Text = "Entrada";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(587, 87);
            label5.Name = "label5";
            label5.Size = new Size(47, 15);
            label5.TabIndex = 16;
            label5.Text = "Usuario";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(863, 86);
            label4.Name = "label4";
            label4.Size = new Size(54, 15);
            label4.TabIndex = 15;
            label4.Text = "Almacen";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(382, 86);
            label3.Name = "label3";
            label3.Size = new Size(67, 15);
            label3.TabIndex = 14;
            label3.Text = "Doc_origen";
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
            txdoc_origen.Location = new Point(367, 103);
            txdoc_origen.Name = "txdoc_origen";
            txdoc_origen.Size = new Size(102, 23);
            txdoc_origen.TabIndex = 11;
            // 
            // txentrada
            // 
            txentrada.Location = new Point(191, 148);
            txentrada.Name = "txentrada";
            txentrada.Size = new Size(102, 23);
            txentrada.TabIndex = 7;
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
            dataGridView1.Size = new Size(133, 80);
            dataGridView1.TabIndex = 0;
            // 
            // btborrar
            // 
            btborrar.BackColor = Color.Aquamarine;
            btborrar.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btborrar.Location = new Point(863, 92);
            btborrar.Name = "btborrar";
            btborrar.Size = new Size(118, 36);
            btborrar.TabIndex = 30;
            btborrar.Text = "- Borrar Detalle";
            btborrar.UseVisualStyleBackColor = false;
            btborrar.Click += btborrar1_Click;
            // 
            // btagregar
            // 
            btagregar.BackColor = Color.Aquamarine;
            btagregar.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btagregar.Location = new Point(863, 26);
            btagregar.Name = "btagregar";
            btagregar.Size = new Size(118, 36);
            btagregar.TabIndex = 29;
            btagregar.Text = "+ Agregar Detalle";
            btagregar.UseVisualStyleBackColor = false;
            btagregar.Click += btagregar_Click;
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
            // Fdev
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1034, 522);
            Controls.Add(splitContainer1);
            Name = "Fdev";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fdev  DEVOLUCIONES ALMACEN";
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
        private TextBox txentrada;
        private TextBox txfolio;
        private Label label7;
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
        private DateTimePicker txfecha;
        private ComboBox txusuario;
        private Button btborrar;
        private Button btagregar;
        private ComboBox conceptotxt;
        private Label label9;
        private Label label6;
        private TextBox txobservacion;
        private ComboBox txalmacen;
        private Label label10;
        private TextBox txnotacredito;
    }
}