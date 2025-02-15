namespace MDIPaint
{
    partial class PaintForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaintForm));
            this.SuspendLayout();
            // 
            // PaintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1170, 632);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "PaintForm";
            this.Text = "PaintForm";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PaintForm_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PaintForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PaintForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PaintForm_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}