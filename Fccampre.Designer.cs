
namespace Sisvta
{
    partial class Fccampre
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fccampre));
            splitContainer1 = new SplitContainer();
            txlista = new ComboBox();
            lineatxt = new ComboBox();
            label3 = new Label();
            label2 = new Label();
            Filtrarbt = new Button();
            panel3 = new Panel();
            regresabt = new Button();
            nuevobt = new Button();
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
            splitContainer1.Panel1.Controls.Add(txlista);
            splitContainer1.Panel1.Controls.Add(lineatxt);
            splitContainer1.Panel1.Controls.Add(label3);
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(Filtrarbt);
            splitContainer1.Panel1.Controls.Add(panel3);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dataGridView1);
            splitContainer1.Size = new Size(953, 522);
            splitContainer1.SplitterDistance = 164;
            splitContainer1.TabIndex = 0;
            // 
            // txlista
            // 
            txlista.FormattingEnabled = true;
            txlista.Location = new Point(469, 104);
            txlista.Name = "txlista";
            txlista.Size = new Size(121, 23);
            txlista.TabIndex = 34;
            // 
            // lineatxt
            // 
            lineatxt.FormattingEnabled = true;
            lineatxt.Location = new Point(158, 105);
            lineatxt.Name = "lineatxt";
            lineatxt.Size = new Size(121, 23);
            lineatxt.TabIndex = 33;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(384, 110);
            label3.Name = "label3";
            label3.Size = new Size(64, 15);
            label3.TabIndex = 32;
            label3.Text = "ListaPrecio";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(60, 110);
            label2.Name = "label2";
            label2.Size = new Size(80, 15);
            label2.TabIndex = 31;
            label2.Text = "Linea Articulo";
            // 
            // Filtrarbt
            // 
            Filtrarbt.Location = new Point(720, 105);
            Filtrarbt.Name = "Filtrarbt";
            Filtrarbt.Size = new Size(89, 22);
            Filtrarbt.TabIndex = 28;
            Filtrarbt.Text = "Filtrar";
            Filtrarbt.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.ControlLight;
            panel3.Controls.Add(regresabt);
            panel3.Controls.Add(nuevobt);
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
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(94, 12);
            label1.Name = "label1";
            label1.Size = new Size(238, 32);
            label1.TabIndex = 1;
            label1.Text = "CAMBIO DE PRECIOS";
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
            // Fccampre
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(953, 522);
            Controls.Add(splitContainer1);
            Name = "Fccampre";
            Text = "Fccampre CAMBIO DE PRECIOS";
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
        private Button nuevobt;
        private Label label1;
        private PictureBox pictureBox1;
        private DataGridView dataGridView1;
        private Button Filtrarbt;
        private ComboBox txlista;
        private ComboBox lineatxt;
        private Label label3;
        private Label label2;
    }
}