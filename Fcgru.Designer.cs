namespace Sisvta
{
    partial class Fcgru
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fcgru));
            dataGridView1 = new DataGridView();
            panel3 = new Panel();
            btarticulo = new Button();
            nuevobt = new Button();
            btlinea = new Button();
            btGrupos = new Button();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            regresabt = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(0, 88);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(245, 301);
            dataGridView1.TabIndex = 3;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            dataGridView1.DoubleClick += dataGridView1_DoubleClick;
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
            panel3.Size = new Size(879, 82);
            panel3.TabIndex = 22;
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
            btarticulo.Click += btarticulo_Click;
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
            nuevobt.Click += nuevobt_Click;
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
            btlinea.Click += btlinea_Click;
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
            label1.Size = new Size(283, 32);
            label1.TabIndex = 1;
            label1.Text = "CATALOGO GRUPO LINEA";
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
            // Fcgru
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(879, 434);
            Controls.Add(panel3);
            Controls.Add(dataGridView1);
            Name = "Fcgru";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fcgru Catalogo Grupo-Lineas";
            Load += Fcgru_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private Panel panel3;
        private Button btarticulo;
        private Button nuevobt;
        private Button btlinea;
        private Button btGrupos;
        private Label label1;
        private PictureBox pictureBox1;
        private Button regresabt;
    }
}