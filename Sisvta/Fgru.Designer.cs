namespace Sisvta
{
    partial class Fgru
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fgru));
            label1 = new Label();
            nombretxt = new TextBox();
            btgrabar = new Button();
            panel3 = new Panel();
            btregresar = new Button();
            button2 = new Button();
            btGuardar = new Button();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(53, 105);
            label1.Name = "label1";
            label1.Size = new Size(51, 15);
            label1.TabIndex = 0;
            label1.Text = "Nombre";
            // 
            // nombretxt
            // 
            nombretxt.Location = new Point(44, 133);
            nombretxt.Name = "nombretxt";
            nombretxt.Size = new Size(100, 23);
            nombretxt.TabIndex = 1;
            // 
            // btgrabar
            // 
            btgrabar.Location = new Point(0, 0);
            btgrabar.Name = "btgrabar";
            btgrabar.Size = new Size(75, 23);
            btgrabar.TabIndex = 23;
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.ControlLight;
            panel3.Controls.Add(btregresar);
            panel3.Controls.Add(button2);
            panel3.Controls.Add(btGuardar);
            panel3.Controls.Add(label2);
            panel3.Controls.Add(pictureBox1);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(821, 90);
            panel3.TabIndex = 22;
            // 
            // btregresar
            // 
            btregresar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btregresar.BackColor = Color.MediumAquamarine;
            btregresar.Image = (Image)resources.GetObject("btregresar.Image");
            btregresar.Location = new Point(741, 3);
            btregresar.Name = "btregresar";
            btregresar.Size = new Size(80, 80);
            btregresar.TabIndex = 5;
            btregresar.Text = "Regresar";
            btregresar.TextImageRelation = TextImageRelation.ImageAboveText;
            btregresar.UseVisualStyleBackColor = false;
            btregresar.Click += btregresar_Click;
            // 
            // button2
            // 
            button2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button2.BackColor = Color.MediumAquamarine;
            button2.Image = (Image)resources.GetObject("button2.Image");
            button2.Location = new Point(655, 6);
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
            btGuardar.Location = new Point(569, 6);
            btGuardar.Name = "btGuardar";
            btGuardar.Size = new Size(80, 80);
            btGuardar.TabIndex = 2;
            btGuardar.Text = "Guardar";
            btGuardar.TextImageRelation = TextImageRelation.ImageAboveText;
            btGuardar.UseVisualStyleBackColor = false;
            btGuardar.Click += btguardar_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(123, 12);
            label2.Name = "label2";
            label2.Size = new Size(186, 32);
            label2.TabIndex = 1;
            label2.Text = "GRUPOS LINEAS";
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
            // Fgru
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(821, 193);
            Controls.Add(panel3);
            Controls.Add(btgrabar);
            Controls.Add(nombretxt);
            Controls.Add(label1);
            Name = "Fgru";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fgru";
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox nombretxt;
        private Button btgrabar;
        private Panel panel3;
        private Button btregresar;
        private Button button2;
        private Button btGuardar;
        private Label label2;
        private PictureBox pictureBox1;
    }
}