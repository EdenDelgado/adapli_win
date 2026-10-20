namespace Sisvta
{
    partial class Fmain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1556, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // Fmain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1556, 717);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MdiChildrenMinimizedAnchorBottom = false;
            Name = "Fmain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fmain Ventas en sqlserver - Sistema Integrado";
            Load += Fmain_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
    }
}