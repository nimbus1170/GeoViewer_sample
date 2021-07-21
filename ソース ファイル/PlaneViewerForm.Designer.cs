namespace PlaneViewer_sample
{
	partial class PlaneViewerForm
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
			this.PictureBox = new System.Windows.Forms.PictureBox();
			this.ObjInfoLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// PictureBox
			// 
			this.PictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PictureBox.Location = new System.Drawing.Point(0, 0);
			this.PictureBox.Margin = new System.Windows.Forms.Padding(2);
			this.PictureBox.Name = "PictureBox";
			this.PictureBox.Size = new System.Drawing.Size(624, 441);
			this.PictureBox.TabIndex = 0;
			this.PictureBox.TabStop = false;
			this.PictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox_Paint);
			this.PictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseDown);
			this.PictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseMove);
			this.PictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseUp);
			this.PictureBox.Resize += new System.EventHandler(this.PictureBox_Resize);
			// 
			// ObjInfoLabel
			// 
			this.ObjInfoLabel.AutoSize = true;
			this.ObjInfoLabel.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.ObjInfoLabel.Location = new System.Drawing.Point(12, 9);
			this.ObjInfoLabel.Name = "ObjInfoLabel";
			this.ObjInfoLabel.Size = new System.Drawing.Size(77, 12);
			this.ObjInfoLabel.TabIndex = 1;
			this.ObjInfoLabel.Text = "ObjInfoLabel";
			// 
			// PlaneViewerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 441);
			this.ControlBox = false;
			this.Controls.Add(this.ObjInfoLabel);
			this.Controls.Add(this.PictureBox);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "PlaneViewerForm";
			this.Text = "PlaneViewerForm";
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PlaneViewerForm_Tude_KeyPress);
			this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.PlaneViewerForm_Tude_PreviewKeyDown);
			((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.PictureBox PictureBox;
        protected System.Windows.Forms.Label ObjInfoLabel;
    }
}