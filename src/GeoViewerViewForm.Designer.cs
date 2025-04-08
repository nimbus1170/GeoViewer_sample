namespace GeoViewer_sample
{
	partial class GeoViewerViewForm
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
			if(disposing && (components is not null))
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeoViewerViewForm));
			PictureBox = new PictureBox();
			InfoLabel = new Label();
			contextMenuStrip1 = new ContextMenuStrip(components);
			MarkerToolStripMenuItem = new ToolStripMenuItem();
			MapSrcLabel = new Label();
			((System.ComponentModel.ISupportInitialize)PictureBox).BeginInit();
			contextMenuStrip1.SuspendLayout();
			SuspendLayout();
			// 
			// PictureBox
			// 
			PictureBox.Dock = DockStyle.Fill;
			PictureBox.Location = new Point(0, 0);
			PictureBox.Margin = new Padding(3, 4, 3, 4);
			PictureBox.Name = "PictureBox";
			PictureBox.Size = new Size(1178, 844);
			PictureBox.TabIndex = 0;
			PictureBox.TabStop = false;
			PictureBox.Paint += PictureBox_Paint;
			PictureBox.MouseDown += PictureBox_MouseDown;
			PictureBox.MouseMove += PictureBox_MouseMove;
			PictureBox.MouseUp += PictureBox_MouseUp;
			PictureBox.Resize += PictureBox_Resize;
			// 
			// InfoLabel
			// 
			InfoLabel.AutoSize = true;
			InfoLabel.Font = new Font("ＭＳ ゴシック", 9F);
			InfoLabel.Location = new Point(20, 19);
			InfoLabel.Margin = new Padding(5, 0, 5, 0);
			InfoLabel.Name = "InfoLabel";
			InfoLabel.Size = new Size(80, 18);
			InfoLabel.TabIndex = 1;
			InfoLabel.Text = "ロード中";
			// 
			// contextMenuStrip1
			// 
			contextMenuStrip1.ImageScalingSize = new Size(24, 24);
			contextMenuStrip1.Items.AddRange(new ToolStripItem[] { MarkerToolStripMenuItem });
			contextMenuStrip1.Name = "contextMenuStrip1";
			contextMenuStrip1.Size = new Size(136, 36);
			// 
			// MarkerToolStripMenuItem
			// 
			MarkerToolStripMenuItem.Name = "MarkerToolStripMenuItem";
			MarkerToolStripMenuItem.Size = new Size(135, 32);
			MarkerToolStripMenuItem.Text = "マーカー";
			MarkerToolStripMenuItem.Click += MarkerToolStripMenuItem_Click;
			// 
			// MapSrcLabel
			// 
			MapSrcLabel.AutoSize = true;
			MapSrcLabel.Dock = DockStyle.Bottom;
			MapSrcLabel.Location = new Point(0, 819);
			MapSrcLabel.Name = "MapSrcLabel";
			MapSrcLabel.Size = new Size(113, 25);
			MapSrcLabel.TabIndex = 2;
			MapSrcLabel.Text = "MapSrcLabel";
			// 
			// GeoViewerViewForm
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1178, 844);
			ControlBox = false;
			Controls.Add(MapSrcLabel);
			Controls.Add(InfoLabel);
			Controls.Add(PictureBox);
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(3, 4, 3, 4);
			Name = "GeoViewerViewForm";
			Text = "GeoViewerViewForm";
			KeyPress += GeoViewerForm_KeyPress;
			PreviewKeyDown += GeoViewerForm_PreviewKeyDown;
			((System.ComponentModel.ISupportInitialize)PictureBox).EndInit();
			contextMenuStrip1.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		public PictureBox PictureBox;
		protected Label InfoLabel;
		private ContextMenuStrip contextMenuStrip1;
		public Label MapSrcLabel;
		public ToolStripMenuItem MarkerToolStripMenuItem;
	}
}