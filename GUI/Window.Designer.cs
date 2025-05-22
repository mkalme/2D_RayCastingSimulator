namespace GUI {
    partial class Window {
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
            if (disposing && (components != null)) {
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
            this.ImageDisplayBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ImageDisplayBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ImageDisplayBox
            // 
            this.ImageDisplayBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageDisplayBox.Location = new System.Drawing.Point(0, 0);
            this.ImageDisplayBox.Name = "ImageDisplayBox";
            this.ImageDisplayBox.Size = new System.Drawing.Size(1055, 600);
            this.ImageDisplayBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ImageDisplayBox.TabIndex = 0;
            this.ImageDisplayBox.TabStop = false;
            this.ImageDisplayBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImageDisplayBox_MouseMove);
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.ClientSize = new System.Drawing.Size(1055, 600);
            this.Controls.Add(this.ImageDisplayBox);
            this.Name = "Window";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "2D Ray Casting";
            ((System.ComponentModel.ISupportInitialize)(this.ImageDisplayBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox ImageDisplayBox;
    }
}

