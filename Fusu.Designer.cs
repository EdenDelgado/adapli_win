namespace Sisvta
{
    partial class Fusu
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fusu));
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            txtuser = new TextBox();
            txtpas = new TextBox();
            label3 = new Label();
            btnlogin = new Button();
            linkLabel2 = new LinkLabel();
            btncerrar = new PictureBox();
            btnminimizar = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btncerrar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnminimizar).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(0, 122, 204);
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(250, 360);
            panel1.TabIndex = 0;
            panel1.MouseDown += panel1_MouseDown;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 87);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(235, 108);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.BackColor = Color.Gray;
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Enabled = false;
            label1.Location = new Point(268, 87);
            label1.Name = "label1";
            label1.Size = new Size(454, 4);
            label1.TabIndex = 2;
            // 
            // label2
            // 
            label2.BackColor = Color.Gray;
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Location = new Point(268, 161);
            label2.Name = "label2";
            label2.Size = new Size(454, 4);
            label2.TabIndex = 3;
            label2.Text = "label2";
            // 
            // txtuser
            // 
            txtuser.BackColor = Color.FromArgb(15, 15, 15);
            txtuser.BorderStyle = BorderStyle.None;
            txtuser.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtuser.ForeColor = Color.DimGray;
            txtuser.Location = new Point(291, 57);
            txtuser.Name = "txtuser";
            txtuser.Size = new Size(413, 19);
            txtuser.TabIndex = 1;
            txtuser.TextChanged += txtuser_TextChanged;
            txtuser.Enter += txtuser_Enter;
            txtuser.Leave += txtuser_Leave;
            // 
            // txtpas
            // 
            txtpas.BackColor = Color.FromArgb(15, 15, 15);
            txtpas.BorderStyle = BorderStyle.None;
            txtpas.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtpas.ForeColor = Color.DimGray;
            txtpas.Location = new Point(268, 139);
            txtpas.Name = "txtpas";
            txtpas.PasswordChar = '@';
            txtpas.Size = new Size(413, 19);
            txtpas.TabIndex = 2;
            txtpas.Text = "CONTRASEÑA";
            txtpas.Enter += txtpas_Enter;
            txtpas.Leave += txtpas_Leave;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Franklin Gothic Medium", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(394, 22);
            label3.Name = "label3";
            label3.Size = new Size(87, 34);
            label3.TabIndex = 6;
            label3.Text = "LOGIN";
            // 
            // btnlogin
            // 
            btnlogin.BackColor = Color.FromArgb(40, 40, 40);
            btnlogin.FlatAppearance.BorderSize = 0;
            btnlogin.FlatAppearance.MouseDownBackColor = Color.FromArgb(28, 28, 28);
            btnlogin.FlatAppearance.MouseOverBackColor = Color.FromArgb(64, 64, 64);
            btnlogin.FlatStyle = FlatStyle.Flat;
            btnlogin.ForeColor = Color.Gainsboro;
            btnlogin.Location = new Point(305, 262);
            btnlogin.Name = "btnlogin";
            btnlogin.Size = new Size(307, 40);
            btnlogin.TabIndex = 3;
            btnlogin.Text = "ACCEDER";
            btnlogin.UseVisualStyleBackColor = false;
            btnlogin.Click += btnlogin_Click;
            btnlogin.MouseDown += btnlogin_MouseDown;
            // 
            // linkLabel2
            // 
            linkLabel2.AutoSize = true;
            linkLabel2.Font = new Font("Franklin Gothic Medium", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkLabel2.ForeColor = SystemColors.GrayText;
            linkLabel2.LinkColor = Color.Gray;
            linkLabel2.Location = new Point(294, 329);
            linkLabel2.Name = "linkLabel2";
            linkLabel2.Size = new Size(129, 16);
            linkLabel2.TabIndex = 0;
            linkLabel2.TabStop = true;
            linkLabel2.Text = "Ha olvidado contraseña?";
            // 
            // btncerrar
            // 
            btncerrar.Image = (Image)resources.GetObject("btncerrar.Image");
            btncerrar.Location = new Point(740, 12);
            btncerrar.Name = "btncerrar";
            btncerrar.Size = new Size(28, 28);
            btncerrar.TabIndex = 9;
            btncerrar.TabStop = false;
            btncerrar.Click += btncerrar_Click;
            // 
            // btnminimizar
            // 
            btnminimizar.Image = (Image)resources.GetObject("btnminimizar.Image");
            btnminimizar.Location = new Point(706, 12);
            btnminimizar.Name = "btnminimizar";
            btnminimizar.Size = new Size(28, 28);
            btnminimizar.TabIndex = 10;
            btnminimizar.TabStop = false;
            btnminimizar.Click += btnminimizar_Click;
            // 
            // Fusu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(15, 15, 15);
            ClientSize = new Size(780, 360);
            Controls.Add(btnminimizar);
            Controls.Add(btncerrar);
            Controls.Add(linkLabel2);
            Controls.Add(btnlogin);
            Controls.Add(label3);
            Controls.Add(txtpas);
            Controls.Add(txtuser);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Fusu";
            Opacity = 0.9D;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            MouseDown += Form1_MouseDown;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)btncerrar).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnminimizar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Label label2;
        private TextBox txtuser;
        private TextBox txtpas;
        private Label label3;
        private Button btnlogin;
        private LinkLabel linkLabel2;
        private PictureBox pictureBox1;
        private PictureBox btncerrar;
        private PictureBox btnminimizar;
    }
}
