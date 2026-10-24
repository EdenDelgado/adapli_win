namespace Sisvta
{
    partial class Fmprod
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fmprod));
            panel3 = new Panel();
            btarticulo = new Button();
            button2 = new Button();
            btlinea = new Button();
            btGrupos = new Button();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            btregresar = new Button();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.ControlLight;
            panel3.Controls.Add(btregresar);
            panel3.Controls.Add(btarticulo);
            panel3.Controls.Add(button2);
            panel3.Controls.Add(btlinea);
            panel3.Controls.Add(btGrupos);
            panel3.Controls.Add(label1);
            panel3.Controls.Add(pictureBox1);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(894, 82);
            panel3.TabIndex = 20;
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
            // button2
            // 
            button2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button2.BackColor = Color.MediumAquamarine;
            button2.Image = (Image)resources.GetObject("button2.Image");
            button2.Location = new Point(735, 6);
            button2.Name = "button2";
            button2.Size = new Size(70, 70);
            button2.TabIndex = 4;
            button2.Text = "Nuevo";
            button2.TextImageRelation = TextImageRelation.ImageAboveText;
            button2.UseVisualStyleBackColor = false;
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
            // btregresar
            // 
            btregresar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btregresar.BackColor = Color.MediumAquamarine;
            btregresar.Image = (Image)resources.GetObject("btregresar.Image");
            btregresar.Location = new Point(811, 3);
            btregresar.Name = "btregresar";
            btregresar.Size = new Size(80, 80);
            btregresar.TabIndex = 11;
            btregresar.Text = "Regresar";
            btregresar.TextImageRelation = TextImageRelation.ImageAboveText;
            btregresar.UseVisualStyleBackColor = false;
            btregresar.Click += btregresar_Click;
            // 
            // Fmprod
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(894, 450);
            Controls.Add(panel3);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "Fmprod";
            Text = "Fmprod";
            WindowState = FormWindowState.Maximized;
            Load += Fmprod_Load;
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel3;
        private Button btarticulo;
        private Button button2;
        private Button btlinea;
        private Button btGrupos;
        private Label label1;
        private PictureBox pictureBox1;
        private Button btregresar;
    }
}