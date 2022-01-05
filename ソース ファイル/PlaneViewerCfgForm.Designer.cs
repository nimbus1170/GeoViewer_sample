namespace PlaneViewer_sample
{
	partial class PlaneViewerCfgForm
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
			this.ShininessTrackBar = new System.Windows.Forms.TrackBar();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.AmbientTrackBar = new System.Windows.Forms.TrackBar();
			this.label3 = new System.Windows.Forms.Label();
			this.SpecularTrackBar = new System.Windows.Forms.TrackBar();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.MappingRadioButton = new System.Windows.Forms.RadioButton();
			this.SmoothRadioButton = new System.Windows.Forms.RadioButton();
			this.FlatRadioButton = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.VisibilityNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.FogDarkRadioButton = new System.Windows.Forms.RadioButton();
			this.FogFogRadioButton = new System.Windows.Forms.RadioButton();
			this.FogNoRadioButton = new System.Windows.Forms.RadioButton();
			this.LocalViewCheckBox = new System.Windows.Forms.CheckBox();
			this.MarkerCheckBox = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.ShininessTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.AmbientTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.SpecularTrackBar)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.VisibilityNumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// ShininessTrackBar
			// 
			this.ShininessTrackBar.LargeChange = 10;
			this.ShininessTrackBar.Location = new System.Drawing.Point(61, 10);
			this.ShininessTrackBar.Margin = new System.Windows.Forms.Padding(2);
			this.ShininessTrackBar.Maximum = 128;
			this.ShininessTrackBar.Name = "ShininessTrackBar";
			this.ShininessTrackBar.Size = new System.Drawing.Size(212, 45);
			this.ShininessTrackBar.TabIndex = 0;
			this.ShininessTrackBar.TickFrequency = 5;
			this.ShininessTrackBar.Value = 128;
			this.ShininessTrackBar.Scroll += new System.EventHandler(this.ShininessTrackBar_Scroll);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(18, 14);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(49, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "ハイライト";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(26, 64);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(41, 12);
			this.label2.TabIndex = 3;
			this.label2.Text = "環境光";
			// 
			// AmbientTrackBar
			// 
			this.AmbientTrackBar.Location = new System.Drawing.Point(61, 59);
			this.AmbientTrackBar.Margin = new System.Windows.Forms.Padding(2);
			this.AmbientTrackBar.Maximum = 100;
			this.AmbientTrackBar.Name = "AmbientTrackBar";
			this.AmbientTrackBar.Size = new System.Drawing.Size(212, 45);
			this.AmbientTrackBar.TabIndex = 2;
			this.AmbientTrackBar.TickFrequency = 5;
			this.AmbientTrackBar.Value = 80;
			this.AmbientTrackBar.Scroll += new System.EventHandler(this.AmbientTrackBar_Scroll);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(26, 113);
			this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(41, 12);
			this.label3.TabIndex = 5;
			this.label3.Text = "鏡面光";
			// 
			// SpecularTrackBar
			// 
			this.SpecularTrackBar.LargeChange = 10;
			this.SpecularTrackBar.Location = new System.Drawing.Point(61, 109);
			this.SpecularTrackBar.Margin = new System.Windows.Forms.Padding(2);
			this.SpecularTrackBar.Maximum = 100;
			this.SpecularTrackBar.Name = "SpecularTrackBar";
			this.SpecularTrackBar.Size = new System.Drawing.Size(212, 45);
			this.SpecularTrackBar.TabIndex = 4;
			this.SpecularTrackBar.TickFrequency = 5;
			this.SpecularTrackBar.Value = 100;
			this.SpecularTrackBar.Scroll += new System.EventHandler(this.SpecularTrackBar_Scroll);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.MappingRadioButton);
			this.groupBox1.Controls.Add(this.SmoothRadioButton);
			this.groupBox1.Controls.Add(this.FlatRadioButton);
			this.groupBox1.Location = new System.Drawing.Point(277, 10);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox1.Size = new System.Drawing.Size(99, 82);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "ｼｪｰﾃﾞｨﾝｸﾞﾓｰﾄﾞ";
			// 
			// MappingRadioButton
			// 
			this.MappingRadioButton.AutoSize = true;
			this.MappingRadioButton.Checked = true;
			this.MappingRadioButton.Location = new System.Drawing.Point(10, 58);
			this.MappingRadioButton.Margin = new System.Windows.Forms.Padding(2);
			this.MappingRadioButton.Name = "MappingRadioButton";
			this.MappingRadioButton.Size = new System.Drawing.Size(66, 16);
			this.MappingRadioButton.TabIndex = 2;
			this.MappingRadioButton.TabStop = true;
			this.MappingRadioButton.Text = "マッピング";
			this.MappingRadioButton.UseVisualStyleBackColor = true;
			this.MappingRadioButton.CheckedChanged += new System.EventHandler(this.MappingRadioButton_CheckedChanged);
			// 
			// SmoothRadioButton
			// 
			this.SmoothRadioButton.AutoSize = true;
			this.SmoothRadioButton.Location = new System.Drawing.Point(10, 38);
			this.SmoothRadioButton.Margin = new System.Windows.Forms.Padding(2);
			this.SmoothRadioButton.Name = "SmoothRadioButton";
			this.SmoothRadioButton.Size = new System.Drawing.Size(62, 16);
			this.SmoothRadioButton.TabIndex = 1;
			this.SmoothRadioButton.Text = "スムーズ";
			this.SmoothRadioButton.UseVisualStyleBackColor = true;
			this.SmoothRadioButton.CheckedChanged += new System.EventHandler(this.SmoothRadioButton_CheckedChanged);
			// 
			// FlatRadioButton
			// 
			this.FlatRadioButton.AutoSize = true;
			this.FlatRadioButton.Location = new System.Drawing.Point(10, 18);
			this.FlatRadioButton.Margin = new System.Windows.Forms.Padding(2);
			this.FlatRadioButton.Name = "FlatRadioButton";
			this.FlatRadioButton.Size = new System.Drawing.Size(54, 16);
			this.FlatRadioButton.TabIndex = 0;
			this.FlatRadioButton.Text = "フラット";
			this.FlatRadioButton.UseVisualStyleBackColor = true;
			this.FlatRadioButton.CheckedChanged += new System.EventHandler(this.FlatRadioButton_CheckedChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.VisibilityNumericUpDown);
			this.groupBox2.Controls.Add(this.FogDarkRadioButton);
			this.groupBox2.Controls.Add(this.FogFogRadioButton);
			this.groupBox2.Controls.Add(this.FogNoRadioButton);
			this.groupBox2.Location = new System.Drawing.Point(384, 10);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox2.Size = new System.Drawing.Size(84, 121);
			this.groupBox2.TabIndex = 9;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "視程";
			// 
			// VisibilityNumericUpDown
			// 
			this.VisibilityNumericUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.VisibilityNumericUpDown.Location = new System.Drawing.Point(11, 88);
			this.VisibilityNumericUpDown.Margin = new System.Windows.Forms.Padding(2);
			this.VisibilityNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
			this.VisibilityNumericUpDown.Name = "VisibilityNumericUpDown";
			this.VisibilityNumericUpDown.Size = new System.Drawing.Size(60, 19);
			this.VisibilityNumericUpDown.TabIndex = 3;
			this.VisibilityNumericUpDown.ThousandsSeparator = true;
			this.VisibilityNumericUpDown.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
			this.VisibilityNumericUpDown.ValueChanged += new System.EventHandler(this.VisibilityNumericUpDown_ValueChanged);
			// 
			// FogDarkRadioButton
			// 
			this.FogDarkRadioButton.AutoSize = true;
			this.FogDarkRadioButton.Checked = true;
			this.FogDarkRadioButton.Location = new System.Drawing.Point(10, 58);
			this.FogDarkRadioButton.Margin = new System.Windows.Forms.Padding(2);
			this.FogDarkRadioButton.Name = "FogDarkRadioButton";
			this.FogDarkRadioButton.Size = new System.Drawing.Size(47, 16);
			this.FogDarkRadioButton.TabIndex = 2;
			this.FogDarkRadioButton.TabStop = true;
			this.FogDarkRadioButton.Text = "夜暗";
			this.FogDarkRadioButton.UseVisualStyleBackColor = true;
			this.FogDarkRadioButton.CheckedChanged += new System.EventHandler(this.FogDarkRadioButton_CheckedChanged);
			// 
			// FogFogRadioButton
			// 
			this.FogFogRadioButton.AutoSize = true;
			this.FogFogRadioButton.Location = new System.Drawing.Point(10, 38);
			this.FogFogRadioButton.Margin = new System.Windows.Forms.Padding(2);
			this.FogFogRadioButton.Name = "FogFogRadioButton";
			this.FogFogRadioButton.Size = new System.Drawing.Size(35, 16);
			this.FogFogRadioButton.TabIndex = 1;
			this.FogFogRadioButton.Text = "霧";
			this.FogFogRadioButton.UseVisualStyleBackColor = true;
			this.FogFogRadioButton.CheckedChanged += new System.EventHandler(this.FogFogRadioButton_CheckedChanged);
			// 
			// FogNoRadioButton
			// 
			this.FogNoRadioButton.AutoSize = true;
			this.FogNoRadioButton.Location = new System.Drawing.Point(10, 18);
			this.FogNoRadioButton.Margin = new System.Windows.Forms.Padding(2);
			this.FogNoRadioButton.Name = "FogNoRadioButton";
			this.FogNoRadioButton.Size = new System.Drawing.Size(59, 16);
			this.FogNoRadioButton.TabIndex = 0;
			this.FogNoRadioButton.Text = "無限遠";
			this.FogNoRadioButton.UseVisualStyleBackColor = true;
			this.FogNoRadioButton.CheckedChanged += new System.EventHandler(this.FogNoRadioButton_CheckedChanged);
			// 
			// LocalViewCheckBox
			// 
			this.LocalViewCheckBox.AutoSize = true;
			this.LocalViewCheckBox.Location = new System.Drawing.Point(277, 100);
			this.LocalViewCheckBox.Margin = new System.Windows.Forms.Padding(2);
			this.LocalViewCheckBox.Name = "LocalViewCheckBox";
			this.LocalViewCheckBox.Size = new System.Drawing.Size(108, 16);
			this.LocalViewCheckBox.TabIndex = 10;
			this.LocalViewCheckBox.Text = "鏡面光視点有効";
			this.LocalViewCheckBox.UseVisualStyleBackColor = true;
			this.LocalViewCheckBox.CheckedChanged += new System.EventHandler(this.LocalViewCheckBox_CheckedChanged);
			// 
			// MarkerCheckBox
			// 
			this.MarkerCheckBox.AutoSize = true;
			this.MarkerCheckBox.Location = new System.Drawing.Point(277, 120);
			this.MarkerCheckBox.Margin = new System.Windows.Forms.Padding(2);
			this.MarkerCheckBox.Name = "MarkerCheckBox";
			this.MarkerCheckBox.Size = new System.Drawing.Size(62, 16);
			this.MarkerCheckBox.TabIndex = 11;
			this.MarkerCheckBox.Text = "マーカー";
			this.MarkerCheckBox.UseVisualStyleBackColor = true;
			this.MarkerCheckBox.CheckedChanged += new System.EventHandler(this.MarkerCheckBox_CheckedChanged);
			// 
			// PlaneViewerConfigForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(480, 159);
			this.ControlBox = false;
			this.Controls.Add(this.MarkerCheckBox);
			this.Controls.Add(this.LocalViewCheckBox);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.SpecularTrackBar);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.AmbientTrackBar);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.ShininessTrackBar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "PlaneViewerConfigForm";
			this.Text = "シーン設定";
			((System.ComponentModel.ISupportInitialize)(this.ShininessTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.AmbientTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.SpecularTrackBar)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.VisibilityNumericUpDown)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TrackBar ShininessTrackBar;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TrackBar AmbientTrackBar;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TrackBar SpecularTrackBar;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton MappingRadioButton;
		private System.Windows.Forms.RadioButton SmoothRadioButton;
		private System.Windows.Forms.RadioButton FlatRadioButton;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.NumericUpDown VisibilityNumericUpDown;
		private System.Windows.Forms.RadioButton FogDarkRadioButton;
		private System.Windows.Forms.RadioButton FogFogRadioButton;
		private System.Windows.Forms.RadioButton FogNoRadioButton;
		private System.Windows.Forms.CheckBox LocalViewCheckBox;
		private System.Windows.Forms.CheckBox MarkerCheckBox;
	}
}