namespace GeoViewer_sample
{
	partial class GeoViewerMainForm
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
			this.DialogTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// DialogTextBox
			// 
			this.DialogTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DialogTextBox.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.DialogTextBox.Location = new System.Drawing.Point(0, 0);
			this.DialogTextBox.Multiline = true;
			this.DialogTextBox.Name = "DialogTextBox";
			this.DialogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.DialogTextBox.Size = new System.Drawing.Size(858, 466);
			this.DialogTextBox.TabIndex = 4;
			this.DialogTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputTextBox_KeyDown);
			this.DialogTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.InputTextBox_KeyPress);
			this.DialogTextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.InputTextBox_MouseDown);
			this.DialogTextBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.InputTextBox_MouseMove);
			// 
			// GeoViewerMainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(858, 466);
			this.Controls.Add(this.DialogTextBox);
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "GeoViewerMainForm";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TextBox DialogTextBox;
	}
}

