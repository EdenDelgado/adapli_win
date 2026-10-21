namespace Sisvta
{
    partial class Fped
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
            dataGridView1 = new DataGridView();
            panel1 = new Panel();
            btelimina = new Button();
            btactualiza = new Button();
            lbndet = new Label();
            addproducto = new Button();
            panel2 = new Panel();
            btimportatx = new Button();
            dtfecha = new DateTimePicker();
            guardar = new Button();
            tximporta = new Button();
            txnomcli = new TextBox();
            txcliente = new TextBox();
            label3 = new Label();
            label2 = new Label();
            txpedido = new TextBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.Location = new Point(0, 85);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(686, 353);
            dataGridView1.TabIndex = 10;
            dataGridView1.DoubleClick += dataGridView1_DoubleClick;
            // 
            // panel1
            // 
            panel1.Controls.Add(btelimina);
            panel1.Controls.Add(btactualiza);
            panel1.Controls.Add(lbndet);
            panel1.Controls.Add(addproducto);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 396);
            panel1.Name = "panel1";
            panel1.Size = new Size(686, 39);
            panel1.TabIndex = 17;
            // 
            // btelimina
            // 
            btelimina.Location = new Point(456, 11);
            btelimina.Name = "btelimina";
            btelimina.Size = new Size(75, 23);
            btelimina.TabIndex = 3;
            btelimina.Text = "Elimina";
            btelimina.UseVisualStyleBackColor = true;
            // 
            // btactualiza
            // 
            btactualiza.Location = new Point(311, 5);
            btactualiza.Name = "btactualiza";
            btactualiza.Size = new Size(75, 23);
            btactualiza.TabIndex = 2;
            btactualiza.Text = "Actualiza";
            btactualiza.UseVisualStyleBackColor = true;
            // 
            // lbndet
            // 
            lbndet.AutoSize = true;
            lbndet.Location = new Point(226, 9);
            lbndet.Name = "lbndet";
            lbndet.Size = new Size(38, 15);
            lbndet.TabIndex = 1;
            lbndet.Text = "label4";
            // 
            // addproducto
            // 
            addproducto.Location = new Point(12, 4);
            addproducto.Name = "addproducto";
            addproducto.Size = new Size(104, 23);
            addproducto.TabIndex = 0;
            addproducto.Text = "Añadir Producto";
            addproducto.UseVisualStyleBackColor = true;
            addproducto.Click += addproducto_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(btimportatx);
            panel2.Controls.Add(dtfecha);
            panel2.Controls.Add(guardar);
            panel2.Controls.Add(tximporta);
            panel2.Controls.Add(txnomcli);
            panel2.Controls.Add(txcliente);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(txpedido);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(686, 82);
            panel2.TabIndex = 18;
            // 
            // btimportatx
            // 
            btimportatx.Location = new Point(561, 26);
            btimportatx.Name = "btimportatx";
            btimportatx.Size = new Size(109, 23);
            btimportatx.TabIndex = 27;
            btimportatx.Text = "importapedidotxt";
            btimportatx.UseVisualStyleBackColor = true;
            btimportatx.Click += btimportatx_Click;
            // 
            // dtfecha
            // 
            dtfecha.Format = DateTimePickerFormat.Short;
            dtfecha.Location = new Point(338, 12);
            dtfecha.Name = "dtfecha";
            dtfecha.Size = new Size(100, 23);
            dtfecha.TabIndex = 26;
            // 
            // guardar
            // 
            guardar.Location = new Point(561, 55);
            guardar.Name = "guardar";
            guardar.Size = new Size(94, 21);
            guardar.TabIndex = 25;
            guardar.Text = "guardar";
            guardar.UseVisualStyleBackColor = true;
            guardar.Click += guardar_Click;
            // 
            // tximporta
            // 
            tximporta.Location = new Point(561, 6);
            tximporta.Name = "tximporta";
            tximporta.Size = new Size(109, 23);
            tximporta.TabIndex = 24;
            tximporta.Text = "importapedido";
            tximporta.UseVisualStyleBackColor = true;
            tximporta.Click += tximporta_Click;
            // 
            // txnomcli
            // 
            txnomcli.Location = new Point(238, 41);
            txnomcli.Name = "txnomcli";
            txnomcli.Size = new Size(300, 23);
            txnomcli.TabIndex = 23;
            // 
            // txcliente
            // 
            txcliente.Location = new Point(100, 41);
            txcliente.Name = "txcliente";
            txcliente.Size = new Size(80, 23);
            txcliente.TabIndex = 21;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(272, 10);
            label3.Name = "label3";
            label3.Size = new Size(38, 15);
            label3.TabIndex = 20;
            label3.Text = "Fecha";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(17, 44);
            label2.Name = "label2";
            label2.Size = new Size(44, 15);
            label2.TabIndex = 19;
            label2.Text = "Cliente";
            // 
            // txpedido
            // 
            txpedido.Location = new Point(100, 7);
            txpedido.Name = "txpedido";
            txpedido.Size = new Size(100, 23);
            txpedido.TabIndex = 18;
            txpedido.Leave += txpedido_Leave;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(17, 10);
            label1.Name = "label1";
            label1.Size = new Size(79, 15);
            label1.TabIndex = 17;
            label1.Text = "Num. Factura";
            // 
            // Fped
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(686, 435);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(dataGridView1);
            Name = "Fped";
            Text = "Fped Captura de Pedidos";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private DataGridView dataGridView1;
        private Panel panel1;
        private Panel panel2;
        private Button tximporta;
        private TextBox txnomcli;
      
        private TextBox txcliente;
        private Label label3;
        private Label label2;
        private TextBox txpedido;
        private Label label1;
        private Button addproducto;
        private Button guardar;
        private DateTimePicker dtfecha;
        private Label lbndet;
        private Button btactualiza;
        private Button btelimina;
        private Button btimportatx;
    }
}