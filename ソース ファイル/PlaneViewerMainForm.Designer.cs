namespace PlaneViewer_sample
{
	partial class PlaneViewerMainForm
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.MessageListBox = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// MessageListBox
			// 
			this.MessageListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MessageListBox.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.MessageListBox.FormattingEnabled = true;
			this.MessageListBox.HorizontalScrollbar = true;
			this.MessageListBox.ItemHeight = 12;
			this.MessageListBox.Location = new System.Drawing.Point(0, 0);
			this.MessageListBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.MessageListBox.Name = "MessageListBox";
			this.MessageListBox.Size = new System.Drawing.Size(515, 311);
			this.MessageListBox.TabIndex = 0;
			// 
			// PlaneViewerMainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(515, 311);
			this.Controls.Add(this.MessageListBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PlaneViewerMainForm";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox MessageListBox;
	}
}

