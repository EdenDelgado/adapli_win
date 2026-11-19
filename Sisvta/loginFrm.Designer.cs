namespace Sisvta
{
    partial class loginFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(loginFrm));
            panel1 = new Panel();
            label3 = new Label();
            label4 = new Label();
            label1 = new Label();
            label2 = new Label();
            txtuser = new TextBox();
            txtpas = new TextBox();
            InicioDeSesionLbl = new Label();
            btnlogin = new Button();
            linkLabel2 = new LinkLabel();
            btncerrar = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)btncerrar).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(0, 122, 204);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label4);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(250, 360);
            panel1.TabIndex = 0;
            panel1.MouseDown += panel1_MouseDown;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Franklin Gothic Medium", 40F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(51, 139);
            label3.Name = "label3";
            label3.Size = new Size(203, 68);
            label3.TabIndex = 10;
            label3.Text = "ADAPLI";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Franklin Gothic Medium", 40F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.White;
            label4.Location = new Point(2, 134);
            label4.Name = "label4";
            label4.Size = new Size(69, 68);
            label4.TabIndex = 11;
            label4.Text = ": :";
            // 
            // label1
            // 
            label1.BackColor = Color.Gray;
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Enabled = false;
            label1.Location = new Point(268, 130);
            label1.Name = "label1";
            label1.Size = new Size(454, 4);
            label1.TabIndex = 2;
            // 
            // label2
            // 
            label2.BackColor = Color.Gray;
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Location = new Point(268, 204);
            label2.Name = "label2";
            label2.Size = new Size(454, 4);
            label2.TabIndex = 3;
            label2.Text = "label2";
            // 
            // txtuser
            // 
            txtuser.BackColor = Color.White;
            txtuser.BorderStyle = BorderStyle.None;
            txtuser.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtuser.ForeColor = Color.DimGray;
            txtuser.Location = new Point(291, 100);
            txtuser.Name = "txtuser";
            txtuser.Size = new Size(413, 19);
            txtuser.TabIndex = 1;
            txtuser.TextChanged += txtuser_TextChanged;
            txtuser.Enter += txtuser_Enter;
            txtuser.Leave += txtuser_Leave;
            // 
            // txtpas
            // 
            txtpas.BackColor = Color.White;
            txtpas.BorderStyle = BorderStyle.None;
            txtpas.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtpas.ForeColor = Color.DimGray;
            txtpas.Location = new Point(268, 182);
            txtpas.Name = "txtpas";
            txtpas.PasswordChar = '@';
            txtpas.Size = new Size(413, 19);
            txtpas.TabIndex = 2;
            txtpas.Text = "CONTRASEÑA";
            txtpas.Enter += txtpas_Enter;
            txtpas.Leave += txtpas_Leave;
            // 
            // InicioDeSesionLbl
            // 
            InicioDeSesionLbl.AutoSize = true;
            InicioDeSesionLbl.Font = new Font("Franklin Gothic Medium", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            InicioDeSesionLbl.Location = new Point(394, 39);
            InicioDeSesionLbl.Name = "InicioDeSesionLbl";
            InicioDeSesionLbl.Size = new Size(219, 34);
            InicioDeSesionLbl.TabIndex = 6;
            InicioDeSesionLbl.Text = "INICIO DE SESIÓN";
            // 
            // btnlogin
            // 
            btnlogin.BackColor = Color.Transparent;
            btnlogin.FlatAppearance.BorderColor = Color.Black;
            btnlogin.FlatAppearance.MouseDownBackColor = Color.FromArgb(28, 28, 28);
            btnlogin.FlatAppearance.MouseOverBackColor = Color.White;
            btnlogin.FlatStyle = FlatStyle.Flat;
            btnlogin.ForeColor = Color.Black;
            btnlogin.Location = new Point(268, 262);
            btnlogin.Name = "btnlogin";
            btnlogin.Size = new Size(454, 40);
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
            // loginFrm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(780, 360);
            Controls.Add(btncerrar);
            Controls.Add(linkLabel2);
            Controls.Add(btnlogin);
            Controls.Add(InicioDeSesionLbl);
            Controls.Add(txtpas);
            Controls.Add(txtuser);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "loginFrm";
            Opacity = 0.9D;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            MouseDown += Form1_MouseDown;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)btncerrar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Label label2;
        private TextBox txtuser;
        private TextBox txtpas;
        private Label InicioDeSesionLbl;
        private Button btnlogin;
        private LinkLabel linkLabel2;
        private PictureBox btncerrar;
        private Label label3;
        private Label label4;
    }
}
