namespace Sisvta
{
    partial class Flispre
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Flispre));
            splitContainer1 = new SplitContainer();
            btregresar = new Button();
            btGuardar = new Button();
            label11 = new Label();
            pictureBox1 = new PictureBox();
            porcentajetx = new TextBox();
            condiciontx = new TextBox();
            listapreciotx = new TextBox();
            empresatx = new TextBox();
            pdvtx = new TextBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
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
            splitContainer1.Panel1.Controls.Add(btregresar);
            splitContainer1.Panel1.Controls.Add(btGuardar);
            splitContainer1.Panel1.Controls.Add(label11);
            splitContainer1.Panel1.Controls.Add(pictureBox1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(porcentajetx);
            splitContainer1.Panel2.Controls.Add(condiciontx);
            splitContainer1.Panel2.Controls.Add(listapreciotx);
            splitContainer1.Panel2.Controls.Add(empresatx);
            splitContainer1.Panel2.Controls.Add(pdvtx);
            splitContainer1.Panel2.Controls.Add(label5);
            splitContainer1.Panel2.Controls.Add(label4);
            splitContainer1.Panel2.Controls.Add(label3);
            splitContainer1.Panel2.Controls.Add(label2);
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Size = new Size(651, 321);
            splitContainer1.SplitterDistance = 91;
            splitContainer1.TabIndex = 0;
            // 
            // btregresar
            // 
            btregresar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btregresar.BackColor = Color.MediumAquamarine;
            btregresar.Image = (Image)resources.GetObject("btregresar.Image");
            btregresar.Location = new Point(542, 12);
            btregresar.Name = "btregresar";
            btregresar.Size = new Size(80, 80);
            btregresar.TabIndex = 10;
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
            btGuardar.Location = new Point(446, 12);
            btGuardar.Name = "btGuardar";
            btGuardar.Size = new Size(80, 80);
            btGuardar.TabIndex = 8;
            btGuardar.Text = "Guardar";
            btGuardar.TextImageRelation = TextImageRelation.ImageAboveText;
            btGuardar.UseVisualStyleBackColor = false;
            btGuardar.Click += btGuardar_Click;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label11.Location = new Point(134, 16);
            label11.Name = "label11";
            label11.Size = new Size(168, 32);
            label11.TabIndex = 7;
            label11.Text = "LISTA PRECIOS";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(23, 16);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(50, 50);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // porcentajetx
            // 
            porcentajetx.Location = new Point(113, 78);
            porcentajetx.Name = "porcentajetx";
            porcentajetx.Size = new Size(115, 23);
            porcentajetx.TabIndex = 22;
            // 
            // condiciontx
            // 
            condiciontx.Location = new Point(113, 49);
            condiciontx.Name = "condiciontx";
            condiciontx.Size = new Size(115, 23);
            condiciontx.TabIndex = 21;
            // 
            // listapreciotx
            // 
            listapreciotx.Location = new Point(113, 15);
            listapreciotx.Name = "listapreciotx";
            listapreciotx.Size = new Size(158, 23);
            listapreciotx.TabIndex = 20;
            // 
            // empresatx
            // 
            empresatx.Location = new Point(113, 142);
            empresatx.Name = "empresatx";
            empresatx.Size = new Size(259, 23);
            empresatx.TabIndex = 19;
            // 
            // pdvtx
            // 
            pdvtx.Location = new Point(113, 108);
            pdvtx.Name = "pdvtx";
            pdvtx.Size = new Size(115, 23);
            pdvtx.TabIndex = 18;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 145);
            label5.Name = "label5";
            label5.Size = new Size(52, 15);
            label5.TabIndex = 17;
            label5.Text = "Empresa";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 116);
            label4.Name = "label4";
            label4.Size = new Size(29, 15);
            label4.TabIndex = 16;
            label4.Text = "PDV";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 86);
            label3.Name = "label3";
            label3.Size = new Size(63, 15);
            label3.TabIndex = 15;
            label3.Text = "Porcentaje";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 57);
            label2.Name = "label2";
            label2.Size = new Size(62, 15);
            label2.TabIndex = 14;
            label2.Text = "Condicion";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 23);
            label1.Name = "label1";
            label1.Size = new Size(31, 15);
            label1.TabIndex = 13;
            label1.Text = "Lista";
            // 
            // Flispre
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoValidate = AutoValidate.EnablePreventFocusChange;
            ClientSize = new Size(651, 321);
            Controls.Add(splitContainer1);
            Name = "Flispre";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Flispre  Lista Precios";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Label label10;
        private Label label9;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button btregresar;
        private Button btGuardar;
        private Label label11;
        private PictureBox pictureBox1;
        public TextBox nombretx;
        public TextBox almdefaultx;
        public TextBox leyendatx;
        public TextBox clavetx;
        public TextBox telefonotx;
        public TextBox cptx;
        public TextBox ciudadtx;
        public TextBox calletx;
        public ComboBox estadotx;
        public TextBox empresatx;
        public TextBox pdvtx;
        public TextBox porcentajetx;
        public TextBox condiciontx;
        public TextBox listapreciotx;
    }
}