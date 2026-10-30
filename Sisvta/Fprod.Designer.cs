namespace Sisvta
{
    partial class Fprod
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fprod));
            splitContainer1 = new SplitContainer();
            btfiltro = new Button();
            statustxt = new TextBox();
            grupotxt = new TextBox();
            lineatxt = new TextBox();
            nomclatxt = new TextBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            panel3 = new Panel();
            btregresar = new Button();
            nuevotx = new Button();
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
            splitContainer1.Panel1.Controls.Add(btfiltro);
            splitContainer1.Panel1.Controls.Add(statustxt);
            splitContainer1.Panel1.Controls.Add(grupotxt);
            splitContainer1.Panel1.Controls.Add(lineatxt);
            splitContainer1.Panel1.Controls.Add(nomclatxt);
            splitContainer1.Panel1.Controls.Add(label5);
            splitContainer1.Panel1.Controls.Add(label4);
            splitContainer1.Panel1.Controls.Add(label3);
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(panel3);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dataGridView1);
            splitContainer1.Size = new Size(1148, 587);
            splitContainer1.SplitterDistance = 160;
            splitContainer1.TabIndex = 14;
            // 
            // btfiltro
            // 
            btfiltro.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btfiltro.BackColor = Color.MediumAquamarine;
            btfiltro.Image = (Image)resources.GetObject("btfiltro.Image");
            btfiltro.Location = new Point(974, 101);
            btfiltro.Name = "btfiltro";
            btfiltro.Size = new Size(67, 56);
            btfiltro.TabIndex = 39;
            btfiltro.TextImageRelation = TextImageRelation.ImageAboveText;
            btfiltro.UseVisualStyleBackColor = false;
            btfiltro.Click += btfiltro_Click;
            // 
            // statustxt
            // 
            statustxt.Location = new Point(850, 103);
            statustxt.Name = "statustxt";
            statustxt.Size = new Size(100, 23);
            statustxt.TabIndex = 38;
            // 
            // grupotxt
            // 
            grupotxt.Location = new Point(612, 103);
            grupotxt.Name = "grupotxt";
            grupotxt.Size = new Size(100, 23);
            grupotxt.TabIndex = 37;
            // 
            // lineatxt
            // 
            lineatxt.Location = new Point(365, 103);
            lineatxt.Name = "lineatxt";
            lineatxt.Size = new Size(100, 23);
            lineatxt.TabIndex = 36;
            // 
            // nomclatxt
            // 
            nomclatxt.Location = new Point(171, 105);
            nomclatxt.Name = "nomclatxt";
            nomclatxt.Size = new Size(100, 23);
            nomclatxt.TabIndex = 35;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(769, 101);
            label5.Name = "label5";
            label5.Size = new Size(75, 25);
            label5.TabIndex = 34;
            label5.Text = "STATUS";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(482, 101);
            label4.Name = "label4";
            label4.Size = new Size(74, 25);
            label4.TabIndex = 33;
            label4.Text = "GRUPO";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(297, 101);
            label3.Name = "label3";
            label3.Size = new Size(62, 25);
            label3.TabIndex = 32;
            label3.Text = "LINEA";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(14, 101);
            label2.Name = "label2";
            label2.Size = new Size(151, 25);
            label2.TabIndex = 31;
            label2.Text = "NOMBRE/CLAVE";
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.ControlLight;
            panel3.Controls.Add(btregresar);
            panel3.Controls.Add(nuevotx);
            panel3.Controls.Add(btlinea);
            panel3.Controls.Add(btGrupos);
            panel3.Controls.Add(label1);
            panel3.Controls.Add(pictureBox1);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(1148, 82);
            panel3.TabIndex = 20;
            // 
            // btregresar
            // 
            btregresar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btregresar.BackColor = Color.MediumAquamarine;
            btregresar.Image = (Image)resources.GetObject("btregresar.Image");
            btregresar.Location = new Point(834, 1);
            btregresar.Name = "btregresar";
            btregresar.Size = new Size(80, 80);
            btregresar.TabIndex = 11;
            btregresar.Text = "Regresar";
            btregresar.TextImageRelation = TextImageRelation.ImageAboveText;
            btregresar.UseVisualStyleBackColor = false;
            btregresar.Click += btregresar_Click;
            // 
            // nuevotx
            // 
            nuevotx.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            nuevotx.BackColor = Color.MediumAquamarine;
            nuevotx.Image = (Image)resources.GetObject("nuevotx.Image");
            nuevotx.Location = new Point(735, 6);
            nuevotx.Name = "nuevotx";
            nuevotx.Size = new Size(70, 70);
            nuevotx.TabIndex = 4;
            nuevotx.Text = "Nuevo";
            nuevotx.TextImageRelation = TextImageRelation.ImageAboveText;
            nuevotx.UseVisualStyleBackColor = false;
            nuevotx.Click += btnNuevo_Click;
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
            btGrupos.Click += btGrupos_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(123, 12);
            label1.Name = "label1";
            label1.Size = new Size(273, 32);
            label1.TabIndex = 1;
            label1.Text = "CATALOGO PRODUCTOS";
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
            dataGridView1.Size = new Size(1148, 423);
            dataGridView1.TabIndex = 13;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            dataGridView1.CellMouseClick += dataGridView1_CellMouseDoubleClick;
            // 
            // Fprod
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1148, 587);
            Controls.Add(splitContainer1);
            MdiChildrenMinimizedAnchorBottom = false;
            Name = "Fprod";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fprod Catalogo de Productos";
            WindowState = FormWindowState.Maximized;
            Load += Fprod_Load;
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
        private Button btfiltro;
        private TextBox statustxt;
        private TextBox grupotxt;
        private TextBox lineatxt;
        private TextBox nomclatxt;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Panel panel3;
        private Button nuevotx;
        private Button btlinea;
        private Button btGrupos;
        private Label label1;
        private PictureBox pictureBox1;
        private DataGridView dataGridView1;
        private Button btregresar;
    }
}
