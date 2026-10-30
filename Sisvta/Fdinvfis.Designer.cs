namespace Sisvta
{
    partial class Fdinvfis
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
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            cantidadtxt = new TextBox();
            nombretxt = new TextBox();
            cantidadrealtxt = new TextBox();
            clavetxt = new ComboBox();
            btaceptar = new Button();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 154);
            label2.Name = "label2";
            label2.Size = new Size(79, 15);
            label2.TabIndex = 1;
            label2.Text = "Cantidad_real";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(9, 14);
            label3.Name = "label3";
            label3.Size = new Size(85, 15);
            label3.TabIndex = 2;
            label3.Text = "ClaveProducto";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 60);
            label4.Name = "label4";
            label4.Size = new Size(51, 15);
            label4.TabIndex = 3;
            label4.Text = "Nombre";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 108);
            label5.Name = "label5";
            label5.Size = new Size(55, 15);
            label5.TabIndex = 4;
            label5.Text = "Cantidad";
            // 
            // cantidadtxt
            // 
            cantidadtxt.Location = new Point(121, 100);
            cantidadtxt.Name = "cantidadtxt";
            cantidadtxt.Size = new Size(100, 23);
            cantidadtxt.TabIndex = 7;
            // 
            // nombretxt
            // 
            nombretxt.Location = new Point(121, 57);
            nombretxt.Name = "nombretxt";
            nombretxt.Size = new Size(354, 23);
            nombretxt.TabIndex = 8;
            // 
            // cantidadrealtxt
            // 
            cantidadrealtxt.Location = new Point(121, 151);
            cantidadrealtxt.Name = "cantidadrealtxt";
            cantidadrealtxt.Size = new Size(100, 23);
            cantidadrealtxt.TabIndex = 9;
            cantidadrealtxt.Leave += preciotxt_Leave;
            // 
            // clavetxt
            // 
            clavetxt.FormattingEnabled = true;
            clavetxt.Location = new Point(121, 11);
            clavetxt.Name = "clavetxt";
            clavetxt.Size = new Size(121, 23);
            clavetxt.TabIndex = 11;
            clavetxt.SelectedIndexChanged += clavetxt_SelectedIndexChanged;
            // 
            // btaceptar
            // 
            btaceptar.Location = new Point(228, 191);
            btaceptar.Name = "btaceptar";
            btaceptar.Size = new Size(100, 34);
            btaceptar.TabIndex = 12;
            btaceptar.Text = "Aceptar";
            btaceptar.UseVisualStyleBackColor = true;
            btaceptar.Click += btaceptar_Click;
            // 
            // Fdinvfis
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(518, 239);
            Controls.Add(btaceptar);
            Controls.Add(clavetxt);
            Controls.Add(cantidadrealtxt);
            Controls.Add(nombretxt);
            Controls.Add(cantidadtxt);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Name = "Fdinvfis";
            Text = "Fdent  Captura detalle Entrada";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox cantidadtxt;
        private TextBox nombretxt;
        private TextBox cantidadrealtxt;
        private TextBox importetxt;
        private ComboBox clavetxt;
        private Button btaceptar;
    }
}