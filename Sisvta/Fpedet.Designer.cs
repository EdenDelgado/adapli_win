namespace Sisvta
{
    partial class Fpedet
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            txcodigo = new TextBox();
            txitem = new Label();
            txcantidad = new TextBox();
            txnombre = new TextBox();
            txprecio = new TextBox();
            Ok = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(38, 20);
            label1.Name = "label1";
            label1.Size = new Size(31, 15);
            label1.TabIndex = 0;
            label1.Text = "Item";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(38, 49);
            label2.Name = "label2";
            label2.Size = new Size(46, 15);
            label2.TabIndex = 1;
            label2.Text = "Codigo";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(38, 101);
            label3.Name = "label3";
            label3.Size = new Size(55, 15);
            label3.TabIndex = 2;
            label3.Text = "Cantidad";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(38, 143);
            label4.Name = "label4";
            label4.Size = new Size(51, 15);
            label4.TabIndex = 3;
            label4.Text = "Nombre";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(38, 174);
            label5.Name = "label5";
            label5.Size = new Size(40, 15);
            label5.TabIndex = 4;
            label5.Text = "Precio";
            // 
            // txcodigo
            // 
            txcodigo.Location = new Point(199, 48);
            txcodigo.Name = "txcodigo";
            txcodigo.Size = new Size(100, 23);
            txcodigo.TabIndex = 5;
            txcodigo.DoubleClick += txcodigo_DoubleClick;
            // 
            // txitem
            // 
            txitem.AutoSize = true;
            txitem.Location = new Point(199, 20);
            txitem.Name = "txitem";
            txitem.Size = new Size(38, 15);
            txitem.TabIndex = 6;
            txitem.Text = "label6";
            // 
            // txcantidad
            // 
            txcantidad.Location = new Point(199, 93);
            txcantidad.Name = "txcantidad";
            txcantidad.Size = new Size(100, 23);
            txcantidad.TabIndex = 7;
            // 
            // txnombre
            // 
            txnombre.Location = new Point(199, 135);
            txnombre.Name = "txnombre";
            txnombre.Size = new Size(156, 23);
            txnombre.TabIndex = 8;
            // 
            // txprecio
            // 
            txprecio.Location = new Point(199, 171);
            txprecio.Name = "txprecio";
            txprecio.Size = new Size(100, 23);
            txprecio.TabIndex = 9;
            // 
            // Ok
            // 
            Ok.Location = new Point(151, 218);
            Ok.Name = "Ok";
            Ok.Size = new Size(75, 23);
            Ok.TabIndex = 10;
            Ok.Text = "Ok";
            Ok.UseVisualStyleBackColor = true;
            Ok.Click += Ok_Click;
            // 
            // Fpedet
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(366, 253);
            Controls.Add(Ok);
            Controls.Add(txprecio);
            Controls.Add(txnombre);
            Controls.Add(txcantidad);
            Controls.Add(txitem);
            Controls.Add(txcodigo);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Fpedet";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fpedet Captura detalle";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox txcodigo;
        private Label txitem;
        private TextBox txcantidad;
        private TextBox txnombre;
        private TextBox txprecio;
        private Button Ok;
    }
}