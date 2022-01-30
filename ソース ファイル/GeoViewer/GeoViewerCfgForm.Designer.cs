namespace PlaneViewer_sample
{
	partial class GeoViewerCfgForm
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
			if(disposing && (components != null))
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
			this.label4 = new System.Windows.Forms.Label();
			this.ElevationMagnifyTrackBar = new System.Windows.Forms.TrackBar();
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
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.WireRadioButton = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.ShininessTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.AmbientTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.SpecularTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ElevationMagnifyTrackBar)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.VisibilityNumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// ShininessTrackBar
			// 
			this.ShininessTrackBar.LargeChange = 10;
			this.ShininessTrackBar.Location = new System.Drawing.Point(102, 15);
			this.ShininessTrackBar.Maximum = 128;
			this.ShininessTrackBar.Name = "ShininessTrackBar";
			this.ShininessTrackBar.Size = new System.Drawing.Size(353, 69);
			this.ShininessTrackBar.TabIndex = 0;
			this.ShininessTrackBar.TickFrequency = 5;
			this.ShininessTrackBar.Value = 128;
			this.ShininessTrackBar.Scroll += new System.EventHandler(this.ShininessTrackBar_Scroll);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(30, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(74, 18);
			this.label1.TabIndex = 1;
			this.label1.Text = "ハイライト";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(43, 96);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(62, 18);
			this.label2.TabIndex = 3;
			this.label2.Text = "環境光";
			// 
			// AmbientTrackBar
			// 
			this.AmbientTrackBar.Location = new System.Drawing.Point(102, 88);
			this.AmbientTrackBar.Maximum = 100;
			this.AmbientTrackBar.Name = "AmbientTrackBar";
			this.AmbientTrackBar.Size = new System.Drawing.Size(353, 69);
			this.AmbientTrackBar.TabIndex = 2;
			this.AmbientTrackBar.TickFrequency = 5;
			this.AmbientTrackBar.Value = 80;
			this.AmbientTrackBar.Scroll += new System.EventHandler(this.AmbientTrackBar_Scroll);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(43, 170);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(62, 18);
			this.label3.TabIndex = 5;
			this.label3.Text = "鏡面光";
			// 
			// SpecularTrackBar
			// 
			this.SpecularTrackBar.LargeChange = 10;
			this.SpecularTrackBar.Location = new System.Drawing.Point(102, 164);
			this.SpecularTrackBar.Maximum = 100;
			this.SpecularTrackBar.Name = "SpecularTrackBar";
			this.SpecularTrackBar.Size = new System.Drawing.Size(353, 69);
			this.SpecularTrackBar.TabIndex = 4;
			this.SpecularTrackBar.TickFrequency = 5;
			this.SpecularTrackBar.Value = 100;
			this.SpecularTrackBar.Scroll += new System.EventHandler(this.SpecularTrackBar_Scroll);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(25, 243);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(80, 18);
			this.label4.TabIndex = 7;
			this.label4.Text = "標高倍率";
			// 
			// ElevationMagnifyTrackBar
			// 
			this.ElevationMagnifyTrackBar.Location = new System.Drawing.Point(102, 237);
			this.ElevationMagnifyTrackBar.Maximum = 5;
			this.ElevationMagnifyTrackBar.Name = "ElevationMagnifyTrackBar";
			this.ElevationMagnifyTrackBar.Size = new System.Drawing.Size(353, 69);
			this.ElevationMagnifyTrackBar.TabIndex = 6;
			this.ElevationMagnifyTrackBar.Value = 1;
			this.ElevationMagnifyTrackBar.Scroll += new System.EventHandler(this.ElevationMagnifyTrackBar_Scroll);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.WireRadioButton);
			this.groupBox1.Controls.Add(this.MappingRadioButton);
			this.groupBox1.Controls.Add(this.SmoothRadioButton);
			this.groupBox1.Controls.Add(this.FlatRadioButton);
			this.groupBox1.Location = new System.Drawing.Point(462, 15);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(165, 157);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "ｼｪｰﾃﾞｨﾝｸﾞﾓｰﾄﾞ";
			// 
			// MappingRadioButton
			// 
			this.MappingRadioButton.AutoSize = true;
			this.MappingRadioButton.Checked = true;
			this.MappingRadioButton.Location = new System.Drawing.Point(17, 117);
			this.MappingRadioButton.Name = "MappingRadioButton";
			this.MappingRadioButton.Size = new System.Drawing.Size(97, 22);
			this.MappingRadioButton.TabIndex = 2;
			this.MappingRadioButton.TabStop = true;
			this.MappingRadioButton.Text = "マッピング";
			this.MappingRadioButton.UseVisualStyleBackColor = true;
			this.MappingRadioButton.CheckedChanged += new System.EventHandler(this.MappingRadioButton_CheckedChanged);
			// 
			// SmoothRadioButton
			// 
			this.SmoothRadioButton.AutoSize = true;
			this.SmoothRadioButton.Location = new System.Drawing.Point(17, 87);
			this.SmoothRadioButton.Name = "SmoothRadioButton";
			this.SmoothRadioButton.Size = new System.Drawing.Size(92, 22);
			this.SmoothRadioButton.TabIndex = 1;
			this.SmoothRadioButton.Text = "スムーズ";
			this.SmoothRadioButton.UseVisualStyleBackColor = true;
			this.SmoothRadioButton.CheckedChanged += new System.EventHandler(this.SmoothRadioButton_CheckedChanged);
			// 
			// FlatRadioButton
			// 
			this.FlatRadioButton.AutoSize = true;
			this.FlatRadioButton.Location = new System.Drawing.Point(17, 57);
			this.FlatRadioButton.Name = "FlatRadioButton";
			this.FlatRadioButton.Size = new System.Drawing.Size(80, 22);
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
			this.groupBox2.Location = new System.Drawing.Point(640, 15);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(140, 182);
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
			this.VisibilityNumericUpDown.Location = new System.Drawing.Point(18, 132);
			this.VisibilityNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
			this.VisibilityNumericUpDown.Name = "VisibilityNumericUpDown";
			this.VisibilityNumericUpDown.Size = new System.Drawing.Size(100, 25);
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
			this.FogDarkRadioButton.Location = new System.Drawing.Point(17, 87);
			this.FogDarkRadioButton.Name = "FogDarkRadioButton";
			this.FogDarkRadioButton.Size = new System.Drawing.Size(69, 22);
			this.FogDarkRadioButton.TabIndex = 2;
			this.FogDarkRadioButton.TabStop = true;
			this.FogDarkRadioButton.Text = "夜暗";
			this.FogDarkRadioButton.UseVisualStyleBackColor = true;
			this.FogDarkRadioButton.CheckedChanged += new System.EventHandler(this.FogDarkRadioButton_CheckedChanged);
			// 
			// FogFogRadioButton
			// 
			this.FogFogRadioButton.AutoSize = true;
			this.FogFogRadioButton.Location = new System.Drawing.Point(17, 57);
			this.FogFogRadioButton.Name = "FogFogRadioButton";
			this.FogFogRadioButton.Size = new System.Drawing.Size(51, 22);
			this.FogFogRadioButton.TabIndex = 1;
			this.FogFogRadioButton.Text = "霧";
			this.FogFogRadioButton.UseVisualStyleBackColor = true;
			this.FogFogRadioButton.CheckedChanged += new System.EventHandler(this.FogFogRadioButton_CheckedChanged);
			// 
			// FogNoRadioButton
			// 
			this.FogNoRadioButton.AutoSize = true;
			this.FogNoRadioButton.Location = new System.Drawing.Point(17, 27);
			this.FogNoRadioButton.Name = "FogNoRadioButton";
			this.FogNoRadioButton.Size = new System.Drawing.Size(87, 22);
			this.FogNoRadioButton.TabIndex = 0;
			this.FogNoRadioButton.Text = "無限遠";
			this.FogNoRadioButton.UseVisualStyleBackColor = true;
			this.FogNoRadioButton.CheckedChanged += new System.EventHandler(this.FogNoRadioButton_CheckedChanged);
			// 
			// LocalViewCheckBox
			// 
			this.LocalViewCheckBox.AutoSize = true;
			this.LocalViewCheckBox.Location = new System.Drawing.Point(461, 183);
			this.LocalViewCheckBox.Name = "LocalViewCheckBox";
			this.LocalViewCheckBox.Size = new System.Drawing.Size(160, 22);
			this.LocalViewCheckBox.TabIndex = 10;
			this.LocalViewCheckBox.Text = "鏡面光視点有効";
			this.LocalViewCheckBox.UseVisualStyleBackColor = true;
			this.LocalViewCheckBox.CheckedChanged += new System.EventHandler(this.LocalViewCheckBox_CheckedChanged);
			// 
			// MarkerCheckBox
			// 
			this.MarkerCheckBox.AutoSize = true;
			this.MarkerCheckBox.Location = new System.Drawing.Point(461, 211);
			this.MarkerCheckBox.Name = "MarkerCheckBox";
			this.MarkerCheckBox.Size = new System.Drawing.Size(91, 22);
			this.MarkerCheckBox.TabIndex = 11;
			this.MarkerCheckBox.Text = "マーカー";
			this.MarkerCheckBox.UseVisualStyleBackColor = true;
			this.MarkerCheckBox.CheckedChanged += new System.EventHandler(this.MarkerCheckBox_CheckedChanged);
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label11.Location = new System.Drawing.Point(423, 282);
			this.label11.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(17, 18);
			this.label11.TabIndex = 24;
			this.label11.Text = "5";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label10.Location = new System.Drawing.Point(362, 282);
			this.label10.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(17, 18);
			this.label10.TabIndex = 23;
			this.label10.Text = "4";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label9.Location = new System.Drawing.Point(300, 282);
			this.label9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(17, 18);
			this.label9.TabIndex = 22;
			this.label9.Text = "3";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label8.Location = new System.Drawing.Point(238, 282);
			this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(17, 18);
			this.label8.TabIndex = 21;
			this.label8.Text = "2";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label7.Location = new System.Drawing.Point(177, 282);
			this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(17, 18);
			this.label7.TabIndex = 20;
			this.label7.Text = "1";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label6.Location = new System.Drawing.Point(115, 282);
			this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(17, 18);
			this.label6.TabIndex = 19;
			this.label6.Text = "0";
			// 
			// WireRadioButton
			// 
			this.WireRadioButton.AutoSize = true;
			this.WireRadioButton.Location = new System.Drawing.Point(17, 27);
			this.WireRadioButton.Name = "WireRadioButton";
			this.WireRadioButton.Size = new System.Drawing.Size(75, 22);
			this.WireRadioButton.TabIndex = 3;
			this.WireRadioButton.Text = "ワイヤ";
			this.WireRadioButton.UseVisualStyleBackColor = true;
			this.WireRadioButton.CheckedChanged += new System.EventHandler(this.WireRadioButton_CheckedChanged);
			// 
			// GeoViewerCfgForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 364);
			this.ControlBox = false;
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.MarkerCheckBox);
			this.Controls.Add(this.LocalViewCheckBox);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.ElevationMagnifyTrackBar);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.SpecularTrackBar);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.AmbientTrackBar);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.ShininessTrackBar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "GeoViewerCfgForm";
			this.Text = "シーン設定";
			((System.ComponentModel.ISupportInitialize)(this.ShininessTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.AmbientTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.SpecularTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ElevationMagnifyTrackBar)).EndInit();
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
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TrackBar ElevationMagnifyTrackBar;
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
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.RadioButton WireRadioButton;
	}
}