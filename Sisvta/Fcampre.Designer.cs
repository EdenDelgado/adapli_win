namespace Sisvta
{
    partial class Fcampre
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
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            nombretxt = new TextBox();
            preciotxt = new TextBox();
            clavetxt = new ComboBox();
            btaceptar = new Button();
            listapreciotxt = new TextBox();
            SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(9, 16);
            label3.Name = "label3";
            label3.Size = new Size(85, 15);
            label3.TabIndex = 2;
            label3.Text = "ClaveProducto";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 62);
            label4.Name = "label4";
            label4.Size = new Size(51, 15);
            label4.TabIndex = 3;
            label4.Text = "Nombre";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 110);
            label5.Name = "label5";
            label5.Size = new Size(40, 15);
            label5.TabIndex = 4;
            label5.Text = "Precio";
            // 
            // nombretxt
            // 
            nombretxt.Enabled = false;
            nombretxt.Location = new Point(121, 59);
            nombretxt.Name = "nombretxt";
            nombretxt.Size = new Size(376, 23);
            nombretxt.TabIndex = 8;
            // 
            // preciotxt
            // 
            preciotxt.Location = new Point(121, 102);
            preciotxt.Name = "preciotxt";
            preciotxt.Size = new Size(100, 23);
            preciotxt.TabIndex = 9;
            preciotxt.Leave += preciotxt_Leave;
            // 
            // clavetxt
            // 
            clavetxt.Enabled = false;
            clavetxt.FormattingEnabled = true;
            clavetxt.Location = new Point(121, 13);
            clavetxt.Name = "clavetxt";
            clavetxt.Size = new Size(121, 23);
            clavetxt.TabIndex = 11;
            // 
            // btaceptar
            // 
            btaceptar.Location = new Point(230, 155);
            btaceptar.Name = "btaceptar";
            btaceptar.Size = new Size(100, 34);
            btaceptar.TabIndex = 12;
            btaceptar.Text = "Aceptar";
            btaceptar.UseVisualStyleBackColor = true;
            btaceptar.Click += btaceptar_Click;
            // 
            // listapreciotxt
            // 
            listapreciotxt.Enabled = false;
            listapreciotxt.Location = new Point(278, 13);
            listapreciotxt.Name = "listapreciotxt";
            listapreciotxt.Size = new Size(175, 23);
            listapreciotxt.TabIndex = 13;
            // 
            // Fcampre
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(509, 198);
            Controls.Add(listapreciotxt);
            Controls.Add(btaceptar);
            Controls.Add(clavetxt);
            Controls.Add(preciotxt);
            Controls.Add(nombretxt);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Name = "Fcampre";
            Text = "Fcampre Cambio Precios";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox nombretxt;
        private TextBox preciotxt;
        private ComboBox clavetxt;
        private Button btaceptar;
        private TextBox listapreciotxt;
    }
}