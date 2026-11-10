namespace Sisvta
{
    partial class Flin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Flin));
            panel3 = new Panel();
            btregresar = new Button();
            btGuardar = new Button();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            nombretxt = new TextBox();
            label3 = new Label();
            comisiontxt = new TextBox();
            label4 = new Label();
            grupotxt = new ComboBox();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.ControlLight;
            panel3.Controls.Add(btregresar);
            panel3.Controls.Add(btGuardar);
            panel3.Controls.Add(label1);
            panel3.Controls.Add(pictureBox1);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(800, 90);
            panel3.TabIndex = 21;
            // 
            // btregresar
            // 
            btregresar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btregresar.BackColor = Color.MediumAquamarine;
            btregresar.Image = (Image)resources.GetObject("btregresar.Image");
            btregresar.Location = new Point(681, 7);
            btregresar.Name = "btregresar";
            btregresar.Size = new Size(80, 80);
            btregresar.TabIndex = 5;
            btregresar.Text = "Regresar";
            btregresar.TextImageRelation = TextImageRelation.ImageAboveText;
            btregresar.UseVisualStyleBackColor = false;
            btregresar.Click += btregresar_Click;
            // 
            // btGuardar
            // 
            btGuardar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btGuardar.BackColor = Color.MediumAquamarine;
            btGuardar.Image = (Image)resources.GetObject("btGuardar.Image");
            btGuardar.Location = new Point(541, 6);
            btGuardar.Name = "btGuardar";
            btGuardar.Size = new Size(80, 80);
            btGuardar.TabIndex = 2;
            btGuardar.Text = "Guardar";
            btGuardar.TextImageRelation = TextImageRelation.ImageAboveText;
            btGuardar.UseVisualStyleBackColor = false;
            btGuardar.Click += btGuardar_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(123, 12);
            label1.Name = "label1";
            label1.Size = new Size(251, 32);
            label1.TabIndex = 1;
            label1.Text = "LINEAS DE ARTICULOS";
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
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(71, 135);
            label2.Name = "label2";
            label2.Size = new Size(51, 15);
            label2.TabIndex = 22;
            label2.Text = "Nombre";
            // 
            // nombretxt
            // 
            nombretxt.Location = new Point(71, 158);
            nombretxt.Name = "nombretxt";
            nombretxt.Size = new Size(416, 23);
            nombretxt.TabIndex = 23;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(71, 208);
            label3.Name = "label3";
            label3.Size = new Size(87, 15);
            label3.TabIndex = 24;
            label3.Text = "Grupo de Linea";
            // 
            // comisiontxt
            // 
            comisiontxt.Location = new Point(71, 320);
            comisiontxt.Name = "comisiontxt";
            comisiontxt.Size = new Size(100, 23);
            comisiontxt.TabIndex = 27;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(71, 285);
            label4.Name = "label4";
            label4.Size = new Size(133, 15);
            label4.TabIndex = 26;
            label4.Text = "Porcentaje de Comision";
            // 
            // grupotxt
            // 
            grupotxt.FormattingEnabled = true;
            grupotxt.Location = new Point(71, 240);
            grupotxt.Name = "grupotxt";
            grupotxt.Size = new Size(303, 23);
            grupotxt.TabIndex = 29;
            // 
            // Flin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 373);
            Controls.Add(grupotxt);
            Controls.Add(comisiontxt);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(nombretxt);
            Controls.Add(label2);
            Controls.Add(panel3);
            Name = "Flin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Flin";
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel3;
        private Button btregresar;
        private Button btGuardar;
        private Label label1;
        private PictureBox pictureBox1;
        private Label label2;
        private TextBox nombretxt;
        private Label label3;
        private TextBox comisiontxt;
        private Label label4;
        private ComboBox grupotxt;
    }
}