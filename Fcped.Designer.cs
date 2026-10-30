namespace Sisvta
{
    partial class Fcped
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            label1 = new Label();
            txpedido = new TextBox();
            dataGridView1 = new DataGridView();
            label2 = new Label();
            label3 = new Label();
            txcliente = new TextBox();
            txfecha = new TextBox();
            txnomcli = new TextBox();
            tximporta = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(27, 22);
            label1.Name = "label1";
            label1.Size = new Size(77, 15);
            label1.TabIndex = 0;
            label1.Text = "Num. Pedido";
            // 
            // txpedido
            // 
            txpedido.Location = new Point(110, 22);
            txpedido.Name = "txpedido";
            txpedido.Size = new Size(100, 23);
            txpedido.TabIndex = 1;
            txpedido.DoubleClick += txpedido_DoubleClick;
            txpedido.Leave += txpedido_Leave;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.Location = new Point(-1, 85);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(681, 353);
            dataGridView1.TabIndex = 10;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 59);
            label2.Name = "label2";
            label2.Size = new Size(44, 15);
            label2.TabIndex = 11;
            label2.Text = "Cliente";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(282, 30);
            label3.Name = "label3";
            label3.Size = new Size(38, 15);
            label3.TabIndex = 12;
            label3.Text = "Fecha";
            // 
            // txcliente
            // 
            txcliente.Location = new Point(110, 56);
            txcliente.Name = "txcliente";
            txcliente.Size = new Size(80, 23);
            txcliente.TabIndex = 13;
            // 
            // txfecha
            // 
            txfecha.Location = new Point(354, 22);
            txfecha.Name = "txfecha";
            txfecha.Size = new Size(100, 23);
            txfecha.TabIndex = 14;
            // 
            // txnomcli
            // 
            txnomcli.Location = new Point(248, 56);
            txnomcli.Name = "txnomcli";
            txnomcli.Size = new Size(300, 23);
            txnomcli.TabIndex = 15;
            // 
            // tximporta
            // 
            tximporta.Location = new Point(571, 21);
            tximporta.Name = "tximporta";
            tximporta.Size = new Size(109, 23);
            tximporta.TabIndex = 16;
            tximporta.Text = "importapedido";
            tximporta.UseVisualStyleBackColor = true;
            tximporta.Click += tximporta_Click;
            // 
            // Fcped
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(686, 435);
            Controls.Add(tximporta);
            Controls.Add(txnomcli);
            Controls.Add(txfecha);
            Controls.Add(txcliente);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(dataGridView1);
            Controls.Add(txpedido);
            Controls.Add(label1);
            Name = "Fcped";
            Text = "Fcped Consulta de Pedidos";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txpedido;
        private DataGridView dataGridView1;
        private Label label2;
        private Label label3;
        private TextBox txcliente;
        private TextBox txfecha;
        private TextBox txnomcli;
        private Button tximporta;
    }
}