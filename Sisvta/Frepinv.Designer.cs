namespace Sisvta
{
    partial class Frepinv
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frepinv));
            splitContainer1 = new SplitContainer();
            comboBox3 = new ComboBox();
            comboBox2 = new ComboBox();
            txnomalmacen = new ComboBox();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            Filtrarbt = new Button();
            label2 = new Label();
            desdefechatx = new DateTimePicker();
            panel3 = new Panel();
            regresabt = new Button();
            btarticulo = new Button();
            nuevobt = new Button();
            btlinea = new Button();
            btGrupos = new Button();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            dataGridView1 = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
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
            splitContainer1.Panel1.Controls.Add(comboBox3);
            splitContainer1.Panel1.Controls.Add(comboBox2);
            splitContainer1.Panel1.Controls.Add(txnomalmacen);
            splitContainer1.Panel1.Controls.Add(label6);
            splitContainer1.Panel1.Controls.Add(label5);
            splitContainer1.Panel1.Controls.Add(label4);
            splitContainer1.Panel1.Controls.Add(Filtrarbt);
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(desdefechatx);
            splitContainer1.Panel1.Controls.Add(panel3);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dataGridView1);
            splitContainer1.Size = new Size(953, 522);
            splitContainer1.SplitterDistance = 164;
            splitContainer1.TabIndex = 0;
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(687, 113);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(121, 23);
            comboBox3.TabIndex = 34;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(453, 113);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(121, 23);
            comboBox2.TabIndex = 33;
            // 
            // txnomalmacen
            // 
            txnomalmacen.FormattingEnabled = true;
            txnomalmacen.Items.AddRange(new object[] { "ALMACEN GENERAL", "AUTOMOTRIZ" });
            txnomalmacen.Location = new Point(259, 113);
            txnomalmacen.Name = "txnomalmacen";
            txnomalmacen.Size = new Size(121, 23);
            txnomalmacen.TabIndex = 32;
            txnomalmacen.Text = "JUAREZ";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(595, 116);
            label6.Name = "label6";
            label6.Size = new Size(59, 15);
            label6.TabIndex = 31;
            label6.Text = "Existencia";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(403, 116);
            label5.Name = "label5";
            label5.Size = new Size(44, 15);
            label5.TabIndex = 30;
            label5.Text = "Costeo";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(199, 116);
            label4.Name = "label4";
            label4.Size = new Size(54, 15);
            label4.TabIndex = 29;
            label4.Text = "Almacen";
            // 
            // Filtrarbt
            // 
            Filtrarbt.Location = new Point(838, 116);
            Filtrarbt.Name = "Filtrarbt";
            Filtrarbt.Size = new Size(89, 22);
            Filtrarbt.TabIndex = 28;
            Filtrarbt.Text = "Filtrar";
            Filtrarbt.UseVisualStyleBackColor = true;
            Filtrarbt.Click += Filtrarbt_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(26, 110);
            label2.Name = "label2";
            label2.Size = new Size(38, 15);
            label2.TabIndex = 26;
            label2.Text = "Fecha";
            // 
            // desdefechatx
            // 
            desdefechatx.Format = DateTimePickerFormat.Short;
            desdefechatx.Location = new Point(70, 110);
            desdefechatx.Name = "desdefechatx";
            desdefechatx.Size = new Size(113, 23);
            desdefechatx.TabIndex = 24;
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.ControlLight;
            panel3.Controls.Add(regresabt);
            panel3.Controls.Add(btarticulo);
            panel3.Controls.Add(nuevobt);
            panel3.Controls.Add(btlinea);
            panel3.Controls.Add(btGrupos);
            panel3.Controls.Add(label1);
            panel3.Controls.Add(pictureBox1);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(953, 82);
            panel3.TabIndex = 23;
            // 
            // regresabt
            // 
            regresabt.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            regresabt.BackColor = Color.MediumAquamarine;
            regresabt.Image = (Image)resources.GetObject("regresabt.Image");
            regresabt.Location = new Point(787, 3);
            regresabt.Name = "regresabt";
            regresabt.Size = new Size(80, 80);
            regresabt.TabIndex = 7;
            regresabt.Text = "Regresar";
            regresabt.TextImageRelation = TextImageRelation.ImageAboveText;
            regresabt.UseVisualStyleBackColor = false;
            regresabt.Click += regresabt_Click;
            // 
            // btarticulo
            // 
            btarticulo.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btarticulo.BackColor = Color.MediumAquamarine;
            btarticulo.Image = (Image)resources.GetObject("btarticulo.Image");
            btarticulo.Location = new Point(465, 6);
            btarticulo.Name = "btarticulo";
            btarticulo.Size = new Size(70, 69);
            btarticulo.TabIndex = 5;
            btarticulo.Text = "Articulos";
            btarticulo.TextImageRelation = TextImageRelation.ImageAboveText;
            btarticulo.UseVisualStyleBackColor = false;
            // 
            // nuevobt
            // 
            nuevobt.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            nuevobt.BackColor = Color.MediumAquamarine;
            nuevobt.Image = (Image)resources.GetObject("nuevobt.Image");
            nuevobt.Location = new Point(706, 5);
            nuevobt.Name = "nuevobt";
            nuevobt.Size = new Size(70, 70);
            nuevobt.TabIndex = 4;
            nuevobt.Text = "Nuevo";
            nuevobt.TextImageRelation = TextImageRelation.ImageAboveText;
            nuevobt.UseVisualStyleBackColor = false;
            // 
            // btlinea
            // 
            btlinea.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btlinea.BackColor = Color.MediumAquamarine;
            btlinea.Image = (Image)resources.GetObject("btlinea.Image");
            btlinea.Location = new Point(630, 6);
            btlinea.Name = "btlinea";
            btlinea.Size = new Size(70, 69);
            btlinea.TabIndex = 3;
            btlinea.Text = "Lineas";
            btlinea.TextImageRelation = TextImageRelation.ImageAboveText;
            btlinea.UseVisualStyleBackColor = false;
            // 
            // btGrupos
            // 
            btGrupos.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btGrupos.BackColor = Color.MediumAquamarine;
            btGrupos.Image = (Image)resources.GetObject("btGrupos.Image");
            btGrupos.Location = new Point(541, 6);
            btGrupos.Name = "btGrupos";
            btGrupos.Size = new Size(70, 69);
            btGrupos.TabIndex = 2;
            btGrupos.Text = "Grupos";
            btGrupos.TextImageRelation = TextImageRelation.ImageAboveText;
            btGrupos.UseVisualStyleBackColor = false;
            btGrupos.Visible = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(94, 12);
            label1.Name = "label1";
            label1.Size = new Size(249, 32);
            label1.TabIndex = 1;
            label1.Text = "REPORTE INVENTARIO";
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
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(953, 354);
            dataGridView1.TabIndex = 1;
            // 
            // Frepinv
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(953, 522);
            Controls.Add(splitContainer1);
            Name = "Frepinv";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Frepinv  REPORTE INVENTARIO";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Panel panel3;
        private Button regresabt;
        private Button btarticulo;
        private Button nuevobt;
        private Button btlinea;
        private Button btGrupos;
        private Label label1;
        private PictureBox pictureBox1;
        private DataGridView dataGridView1;
        private Label label2;
        private DateTimePicker desdefechatx;
        private Button Filtrarbt;
        private Label label6;
        private Label label5;
        private Label label4;
        private ComboBox comboBox3;
        private ComboBox comboBox2;
        private ComboBox txnomalmacen;
    }
}