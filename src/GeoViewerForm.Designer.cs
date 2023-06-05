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
			components = new System.ComponentModel.Container();
			PictureBox = new System.Windows.Forms.PictureBox();
			ObjInfoLabel = new System.Windows.Forms.Label();
			contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
			okToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			MapSrcLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)PictureBox).BeginInit();
			contextMenuStrip1.SuspendLayout();
			SuspendLayout();
			// 
			// PictureBox
			// 
			PictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			PictureBox.Location = new System.Drawing.Point(0, 0);
			PictureBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			PictureBox.Name = "PictureBox";
			PictureBox.Size = new System.Drawing.Size(1002, 712);
			PictureBox.TabIndex = 0;
			PictureBox.TabStop = false;
			PictureBox.Paint += PictureBox_Paint;
			PictureBox.MouseDown += PictureBox_MouseDown;
			PictureBox.MouseMove += PictureBox_MouseMove;
			PictureBox.MouseUp += PictureBox_MouseUp;
			PictureBox.Resize += PictureBox_Resize;
			// 
			// ObjInfoLabel
			// 
			ObjInfoLabel.AutoSize = true;
			ObjInfoLabel.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			ObjInfoLabel.Location = new System.Drawing.Point(20, 19);
			ObjInfoLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			ObjInfoLabel.Name = "ObjInfoLabel";
			ObjInfoLabel.Size = new System.Drawing.Size(116, 18);
			ObjInfoLabel.TabIndex = 1;
			ObjInfoLabel.Text = "ObjInfoLabel";
			// 
			// contextMenuStrip1
			// 
			contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { okToolStripMenuItem });
			contextMenuStrip1.Name = "contextMenuStrip1";
			contextMenuStrip1.Size = new System.Drawing.Size(136, 36);
			// 
			// okToolStripMenuItem
			// 
			okToolStripMenuItem.Name = "okToolStripMenuItem";
			okToolStripMenuItem.Size = new System.Drawing.Size(135, 32);
			okToolStripMenuItem.Text = "マーカー";
			okToolStripMenuItem.Click += MarkerToolStripMenuItem_Click;
			// 
			// MapSrcLabel
			// 
			MapSrcLabel.AutoSize = true;
			MapSrcLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
			MapSrcLabel.Location = new System.Drawing.Point(0, 687);
			MapSrcLabel.Name = "MapSrcLabel";
			MapSrcLabel.Size = new System.Drawing.Size(113, 25);
			MapSrcLabel.TabIndex = 2;
			MapSrcLabel.Text = "MapSrcLabel";
			// 
			// GeoViewerForm
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(1002, 712);
			ControlBox = false;
			Controls.Add(MapSrcLabel);
			Controls.Add(ObjInfoLabel);
			Controls.Add(PictureBox);
			Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			Name = "GeoViewerForm";
			Text = "PlaneViewerForm";
			KeyPress += PlaneViewerForm_LgLt_KeyPress;
			PreviewKeyDown += PlaneViewerForm_LgLt_PreviewKeyDown;
			((System.ComponentModel.ISupportInitialize)PictureBox).EndInit();
			contextMenuStrip1.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		public System.Windows.Forms.PictureBox PictureBox;
		protected System.Windows.Forms.Label ObjInfoLabel;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem okToolStripMenuItem;
		public System.Windows.Forms.Label MapSrcLabel;
	}
}