namespace GeoViewer_sample
{
	partial class GeoViewerForm
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
			this.components = new System.ComponentModel.Container();
			this.PictureBox = new System.Windows.Forms.PictureBox();
			this.ObjInfoLabel = new System.Windows.Forms.Label();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.okToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// PictureBox
			// 
			this.PictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PictureBox.Location = new System.Drawing.Point(0, 0);
			this.PictureBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.PictureBox.Name = "PictureBox";
			this.PictureBox.Size = new System.Drawing.Size(1002, 712);
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
			this.ObjInfoLabel.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.ObjInfoLabel.Location = new System.Drawing.Point(20, 19);
			this.ObjInfoLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.ObjInfoLabel.Name = "ObjInfoLabel";
			this.ObjInfoLabel.Size = new System.Drawing.Size(116, 18);
			this.ObjInfoLabel.TabIndex = 1;
			this.ObjInfoLabel.Text = "ObjInfoLabel";
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.okToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(136, 36);
			// 
			// okToolStripMenuItem
			// 
			this.okToolStripMenuItem.Name = "okToolStripMenuItem";
			this.okToolStripMenuItem.Size = new System.Drawing.Size(135, 32);
			this.okToolStripMenuItem.Text = "マーカー";
			this.okToolStripMenuItem.Click += new System.EventHandler(this.MarkerToolStripMenuItem_Click);
			// 
			// GeoViewerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1002, 712);
			this.ControlBox = false;
			this.Controls.Add(this.ObjInfoLabel);
			this.Controls.Add(this.PictureBox);
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "GeoViewerForm";
			this.Text = "PlaneViewerForm";
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PlaneViewerForm_LgLt_KeyPress);
			this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.PlaneViewerForm_LgLt_PreviewKeyDown);
			((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.PictureBox PictureBox;
        protected System.Windows.Forms.Label ObjInfoLabel;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem okToolStripMenuItem;
	}
}