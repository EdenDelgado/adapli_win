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
            SuspendLayout();
            // 
            // Fmain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1556, 717);
            MdiChildrenMinimizedAnchorBottom = false;
            Name = "Fmain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fmain Ventas en sqlserver - Sistema Integrado";
            Load += Fmain_Load;
            ResumeLayout(false);
        }

        #endregion
    }
}