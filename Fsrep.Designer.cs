namespace Sisvta
{
    partial class Fsrep
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fsrep));
            btrepinv = new Button();
            btrepconin = new Button();
            btrepcosv = new Button();
            btrepexi = new Button();
            button5 = new Button();
            btrepsal = new Button();
            btsugcompra = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            dateTimePicker1 = new DateTimePicker();
            dateTimePicker2 = new DateTimePicker();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            comboBox3 = new ComboBox();
            comboBox4 = new ComboBox();
            panel3 = new Panel();
            regresabt = new Button();
            label8 = new Label();
            pictureBox1 = new PictureBox();
            label6 = new Label();
            label7 = new Label();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btrepinv
            // 
            btrepinv.Location = new Point(53, 89);
            btrepinv.Name = "btrepinv";
            btrepinv.Size = new Size(106, 30);
            btrepinv.TabIndex = 0;
            btrepinv.Text = "InventarioxFecha";
            btrepinv.UseVisualStyleBackColor = true;
            btrepinv.Click += btrepinv_Click;
            // 
            // btrepconin
            // 
            btrepconin.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btrepconin.Location = new Point(284, 89);
            btrepconin.Name = "btrepconin";
            btrepconin.Size = new Size(165, 30);
            btrepconin.TabIndex = 1;
            btrepconin.Text = "CONSUMO DE INVENTARIO";
            btrepconin.UseVisualStyleBackColor = true;
            btrepconin.Click += btrepconin_Click;
            // 
            // btrepcosv
            // 
            btrepcosv.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btrepcosv.Location = new Point(515, 89);
            btrepcosv.Name = "btrepcosv";
            btrepcosv.Size = new Size(138, 30);
            btrepcosv.TabIndex = 2;
            btrepcosv.Text = "LISTADO COSTO VENTA";
            btrepcosv.UseVisualStyleBackColor = true;
            btrepcosv.Click += btrepcosv_Click;
            // 
            // btrepexi
            // 
            btrepexi.Location = new Point(428, 166);
            btrepexi.Name = "btrepexi";
            btrepexi.Size = new Size(106, 30);
            btrepexi.TabIndex = 3;
            btrepexi.Text = "Ver";
            btrepexi.UseVisualStyleBackColor = true;
            btrepexi.Click += btrepexi_Click;
            // 
            // button5
            // 
            button5.Location = new Point(669, 166);
            button5.Name = "button5";
            button5.Size = new Size(106, 30);
            button5.TabIndex = 4;
            button5.Text = "Ver";
            button5.UseVisualStyleBackColor = true;
            // 
            // btrepsal
            // 
            btrepsal.Location = new Point(547, 338);
            btrepsal.Name = "btrepsal";
            btrepsal.Size = new Size(106, 30);
            btrepsal.TabIndex = 5;
            btrepsal.Text = "Ver";
            btrepsal.UseVisualStyleBackColor = true;
            btrepsal.Click += btrepsal_Click;
            // 
            // btsugcompra
            // 
            btsugcompra.Location = new Point(88, 177);
            btsugcompra.Name = "btsugcompra";
            btsugcompra.Size = new Size(161, 30);
            btsugcompra.TabIndex = 6;
            btsugcompra.Text = "Sugerencias de Compra";
            btsugcompra.UseVisualStyleBackColor = true;
            btsugcompra.Click += btsugcompra_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.Highlight;
            label1.Location = new Point(165, 255);
            label1.Name = "label1";
            label1.Size = new Size(300, 21);
            label1.TabIndex = 7;
            label1.Text = "INVENTARIO SALIDAS POR CONCEPTO";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(165, 286);
            label2.Name = "label2";
            label2.Size = new Size(47, 15);
            label2.TabIndex = 8;
            label2.Text = "INICIAL";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(381, 286);
            label3.Name = "label3";
            label3.Size = new Size(39, 15);
            label3.TabIndex = 9;
            label3.Text = "FINAL";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(35, 333);
            label4.Name = "label4";
            label4.Size = new Size(63, 15);
            label4.TabIndex = 10;
            label4.Text = "ALMACEN";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(305, 338);
            label5.Name = "label5";
            label5.Size = new Size(68, 15);
            label5.TabIndex = 11;
            label5.Text = "CONCEPTO";
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Format = DateTimePickerFormat.Short;
            dateTimePicker1.Location = new Point(258, 280);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(73, 23);
            dateTimePicker1.TabIndex = 12;
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.Format = DateTimePickerFormat.Short;
            dateTimePicker2.Location = new Point(442, 286);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.Size = new Size(73, 23);
            dateTimePicker2.TabIndex = 13;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "ALMACEN GENERAL", "AUTOMOTRIZ" });
            comboBox1.Location = new Point(304, 172);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 14;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "ACTIVO", "RENTADO", "VENDIDO" });
            comboBox2.Location = new Point(547, 171);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(121, 23);
            comboBox2.TabIndex = 15;
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Items.AddRange(new object[] { "ALMACEN GENERAL", "AUTOMOTRIZ" });
            comboBox3.Location = new Point(144, 330);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(121, 23);
            comboBox3.TabIndex = 16;
            // 
            // comboBox4
            // 
            comboBox4.FormattingEnabled = true;
            comboBox4.Items.AddRange(new object[] { "DEVOLUCION COMPRA", "AJUSTE DE INVENTARIOS", "VENTAS", "MERMA", "TRASPASO" });
            comboBox4.Location = new Point(394, 335);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(121, 23);
            comboBox4.TabIndex = 17;
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.ControlLight;
            panel3.Controls.Add(regresabt);
            panel3.Controls.Add(label8);
            panel3.Controls.Add(pictureBox1);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(854, 84);
            panel3.TabIndex = 23;
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
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(123, 12);
            label8.Name = "label8";
            label8.Size = new Size(320, 40);
            label8.TabIndex = 1;
            label8.Text = "REPORTES INVENTARIO";
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
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.ForeColor = SystemColors.HotTrack;
            label6.Location = new Point(304, 142);
            label6.Name = "label6";
            label6.Size = new Size(130, 17);
            label6.TabIndex = 24;
            label6.Text = "EXISTENCIA/COSTEO";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.ForeColor = SystemColors.Highlight;
            label7.Location = new Point(572, 142);
            label7.Name = "label7";
            label7.Size = new Size(81, 17);
            label7.TabIndex = 25;
            label7.Text = "INVENTARIO";
            // 
            // Fsrep
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(854, 620);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(panel3);
            Controls.Add(comboBox4);
            Controls.Add(comboBox3);
            Controls.Add(comboBox2);
            Controls.Add(comboBox1);
            Controls.Add(dateTimePicker2);
            Controls.Add(dateTimePicker1);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btsugcompra);
            Controls.Add(btrepsal);
            Controls.Add(button5);
            Controls.Add(btrepexi);
            Controls.Add(btrepcosv);
            Controls.Add(btrepconin);
            Controls.Add(btrepinv);
            Name = "Fsrep";
            Text = "Fsrep";
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btrepinv;
        private Button btrepconin;
        private Button btrepcosv;
        private Button btrepexi;
        private Button button5;
        private Button btrepsal;
        private Button btsugcompra;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private DateTimePicker dateTimePicker1;
        private DateTimePicker dateTimePicker2;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private ComboBox comboBox3;
        private ComboBox comboBox4;
        private Panel panel3;
        private Button regresabt;
        private Label label8;
        private PictureBox pictureBox1;
        private Label label6;
        private Label label7;
    }
}