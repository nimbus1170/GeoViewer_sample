namespace GeoViewer_sample
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
			ShininessTrackBar = new System.Windows.Forms.TrackBar();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			AmbientTrackBar = new System.Windows.Forms.TrackBar();
			label3 = new System.Windows.Forms.Label();
			SpecularTrackBar = new System.Windows.Forms.TrackBar();
			label4 = new System.Windows.Forms.Label();
			ElevationMagnifyTrackBar = new System.Windows.Forms.TrackBar();
			FogModeComboBox = new System.Windows.Forms.ComboBox();
			VisibilityNumericUpDown = new System.Windows.Forms.NumericUpDown();
			LocalViewCheckBox = new System.Windows.Forms.CheckBox();
			MarkerCheckBox = new System.Windows.Forms.CheckBox();
			label11 = new System.Windows.Forms.Label();
			label10 = new System.Windows.Forms.Label();
			label9 = new System.Windows.Forms.Label();
			label8 = new System.Windows.Forms.Label();
			label7 = new System.Windows.Forms.Label();
			label6 = new System.Windows.Forms.Label();
			ShadingModeComboBox = new System.Windows.Forms.ComboBox();
			label5 = new System.Windows.Forms.Label();
			label12 = new System.Windows.Forms.Label();
			PolygonModeComboBox = new System.Windows.Forms.ComboBox();
			label13 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)ShininessTrackBar).BeginInit();
			((System.ComponentModel.ISupportInitialize)AmbientTrackBar).BeginInit();
			((System.ComponentModel.ISupportInitialize)SpecularTrackBar).BeginInit();
			((System.ComponentModel.ISupportInitialize)ElevationMagnifyTrackBar).BeginInit();
			((System.ComponentModel.ISupportInitialize)VisibilityNumericUpDown).BeginInit();
			SuspendLayout();
			// 
			// ShininessTrackBar
			// 
			ShininessTrackBar.LargeChange = 10;
			ShininessTrackBar.Location = new System.Drawing.Point(102, 21);
			ShininessTrackBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			ShininessTrackBar.Maximum = 128;
			ShininessTrackBar.Name = "ShininessTrackBar";
			ShininessTrackBar.Size = new System.Drawing.Size(353, 69);
			ShininessTrackBar.TabIndex = 0;
			ShininessTrackBar.TickFrequency = 5;
			ShininessTrackBar.Value = 128;
			ShininessTrackBar.Scroll += ShininessTrackBar_Scroll;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(30, 23);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(76, 25);
			label1.TabIndex = 1;
			label1.Text = "ハイライト";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(40, 100);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(66, 25);
			label2.TabIndex = 3;
			label2.Text = "環境光";
			// 
			// AmbientTrackBar
			// 
			AmbientTrackBar.Location = new System.Drawing.Point(102, 98);
			AmbientTrackBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			AmbientTrackBar.Maximum = 100;
			AmbientTrackBar.Name = "AmbientTrackBar";
			AmbientTrackBar.Size = new System.Drawing.Size(353, 69);
			AmbientTrackBar.TabIndex = 2;
			AmbientTrackBar.TickFrequency = 5;
			AmbientTrackBar.Value = 80;
			AmbientTrackBar.Scroll += AmbientTrackBar_Scroll;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(43, 177);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(66, 25);
			label3.TabIndex = 5;
			label3.Text = "鏡面光";
			// 
			// SpecularTrackBar
			// 
			SpecularTrackBar.LargeChange = 10;
			SpecularTrackBar.Location = new System.Drawing.Point(102, 175);
			SpecularTrackBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			SpecularTrackBar.Maximum = 100;
			SpecularTrackBar.Name = "SpecularTrackBar";
			SpecularTrackBar.Size = new System.Drawing.Size(353, 69);
			SpecularTrackBar.TabIndex = 4;
			SpecularTrackBar.TickFrequency = 5;
			SpecularTrackBar.Value = 100;
			SpecularTrackBar.Scroll += SpecularTrackBar_Scroll;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(25, 255);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(84, 25);
			label4.TabIndex = 7;
			label4.Text = "標高倍率";
			// 
			// ElevationMagnifyTrackBar
			// 
			ElevationMagnifyTrackBar.Location = new System.Drawing.Point(102, 252);
			ElevationMagnifyTrackBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			ElevationMagnifyTrackBar.Maximum = 5;
			ElevationMagnifyTrackBar.Name = "ElevationMagnifyTrackBar";
			ElevationMagnifyTrackBar.Size = new System.Drawing.Size(353, 69);
			ElevationMagnifyTrackBar.TabIndex = 6;
			ElevationMagnifyTrackBar.Value = 1;
			ElevationMagnifyTrackBar.Scroll += ElevationMagnifyTrackBar_Scroll;
			// 
			// FogModeComboBox
			// 
			FogModeComboBox.FormattingEnabled = true;
			FogModeComboBox.Items.AddRange(new System.Object[] { "無限遠", "霧", "夜暗" });
			FogModeComboBox.Location = new System.Drawing.Point(487, 193);
			FogModeComboBox.Name = "FogModeComboBox";
			FogModeComboBox.Size = new System.Drawing.Size(104, 33);
			FogModeComboBox.TabIndex = 27;
			FogModeComboBox.SelectedIndexChanged += VisibilityComboBox_SelectedIndexChanged;
			// 
			// VisibilityNumericUpDown
			// 
			VisibilityNumericUpDown.Increment = new System.Decimal(new System.Int32[] { 10, 0, 0, 0 });
			VisibilityNumericUpDown.Location = new System.Drawing.Point(597, 195);
			VisibilityNumericUpDown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			VisibilityNumericUpDown.Maximum = new System.Decimal(new System.Int32[] { 100000, 0, 0, 0 });
			VisibilityNumericUpDown.Name = "VisibilityNumericUpDown";
			VisibilityNumericUpDown.Size = new System.Drawing.Size(88, 31);
			VisibilityNumericUpDown.TabIndex = 3;
			VisibilityNumericUpDown.ThousandsSeparator = true;
			VisibilityNumericUpDown.Value = new System.Decimal(new System.Int32[] { 3000, 0, 0, 0 });
			VisibilityNumericUpDown.ValueChanged += VisibilityNumericUpDown_ValueChanged;
			// 
			// LocalViewCheckBox
			// 
			LocalViewCheckBox.AutoSize = true;
			LocalViewCheckBox.Location = new System.Drawing.Point(487, 247);
			LocalViewCheckBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			LocalViewCheckBox.Name = "LocalViewCheckBox";
			LocalViewCheckBox.Size = new System.Drawing.Size(164, 29);
			LocalViewCheckBox.TabIndex = 10;
			LocalViewCheckBox.Text = "鏡面光視点有効";
			LocalViewCheckBox.UseVisualStyleBackColor = true;
			LocalViewCheckBox.CheckedChanged += LocalViewCheckBox_CheckedChanged;
			// 
			// MarkerCheckBox
			// 
			MarkerCheckBox.AutoSize = true;
			MarkerCheckBox.Location = new System.Drawing.Point(487, 280);
			MarkerCheckBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			MarkerCheckBox.Name = "MarkerCheckBox";
			MarkerCheckBox.Size = new System.Drawing.Size(89, 29);
			MarkerCheckBox.TabIndex = 11;
			MarkerCheckBox.Text = "マーカー";
			MarkerCheckBox.UseVisualStyleBackColor = true;
			MarkerCheckBox.CheckedChanged += MarkerCheckBox_CheckedChanged;
			// 
			// label11
			// 
			label11.AutoSize = true;
			label11.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			label11.Location = new System.Drawing.Point(428, 290);
			label11.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			label11.Name = "label11";
			label11.Size = new System.Drawing.Size(17, 18);
			label11.TabIndex = 24;
			label11.Text = "5";
			// 
			// label10
			// 
			label10.AutoSize = true;
			label10.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			label10.Location = new System.Drawing.Point(364, 290);
			label10.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			label10.Name = "label10";
			label10.Size = new System.Drawing.Size(17, 18);
			label10.TabIndex = 23;
			label10.Text = "4";
			// 
			// label9
			// 
			label9.AutoSize = true;
			label9.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			label9.Location = new System.Drawing.Point(302, 290);
			label9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			label9.Name = "label9";
			label9.Size = new System.Drawing.Size(17, 18);
			label9.TabIndex = 22;
			label9.Text = "3";
			// 
			// label8
			// 
			label8.AutoSize = true;
			label8.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			label8.Location = new System.Drawing.Point(241, 290);
			label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			label8.Name = "label8";
			label8.Size = new System.Drawing.Size(17, 18);
			label8.TabIndex = 21;
			label8.Text = "2";
			// 
			// label7
			// 
			label7.AutoSize = true;
			label7.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			label7.Location = new System.Drawing.Point(177, 290);
			label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(17, 18);
			label7.TabIndex = 20;
			label7.Text = "1";
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			label6.Location = new System.Drawing.Point(113, 290);
			label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(17, 18);
			label6.TabIndex = 19;
			label6.Text = "0";
			// 
			// ShadingModeComboBox
			// 
			ShadingModeComboBox.FormattingEnabled = true;
			ShadingModeComboBox.Items.AddRange(new System.Object[] { "テクスチャ", "スムーズ", "フラット", "ワイヤ" });
			ShadingModeComboBox.Location = new System.Drawing.Point(487, 51);
			ShadingModeComboBox.Name = "ShadingModeComboBox";
			ShadingModeComboBox.Size = new System.Drawing.Size(198, 33);
			ShadingModeComboBox.TabIndex = 25;
			ShadingModeComboBox.SelectedIndexChanged += ShadingModeComboBox_SelectedIndexChanged;
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(483, 23);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(138, 25);
			label5.TabIndex = 26;
			label5.Text = "シェーディングモード";
			// 
			// label12
			// 
			label12.AutoSize = true;
			label12.Location = new System.Drawing.Point(483, 165);
			label12.Name = "label12";
			label12.Size = new System.Drawing.Size(48, 25);
			label12.TabIndex = 27;
			label12.Text = "視程";
			// 
			// PolygonModeComboBox
			// 
			PolygonModeComboBox.FormattingEnabled = true;
			PolygonModeComboBox.Items.AddRange(new System.Object[] { "正方形＋中心点", "正方形", "三角形" });
			PolygonModeComboBox.Location = new System.Drawing.Point(487, 122);
			PolygonModeComboBox.Name = "PolygonModeComboBox";
			PolygonModeComboBox.Size = new System.Drawing.Size(198, 33);
			PolygonModeComboBox.TabIndex = 28;
			PolygonModeComboBox.SelectedIndexChanged += PolygonModeComboBox_SelectedIndexChanged;
			// 
			// label13
			// 
			label13.AutoSize = true;
			label13.Location = new System.Drawing.Point(483, 94);
			label13.Name = "label13";
			label13.Size = new System.Drawing.Size(104, 25);
			label13.TabIndex = 29;
			label13.Text = "ポリゴンモード";
			// 
			// GeoViewerCfgForm
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(713, 351);
			ControlBox = false;
			Controls.Add(label13);
			Controls.Add(PolygonModeComboBox);
			Controls.Add(VisibilityNumericUpDown);
			Controls.Add(FogModeComboBox);
			Controls.Add(label12);
			Controls.Add(label5);
			Controls.Add(ShadingModeComboBox);
			Controls.Add(label11);
			Controls.Add(label10);
			Controls.Add(label9);
			Controls.Add(label8);
			Controls.Add(label7);
			Controls.Add(label6);
			Controls.Add(MarkerCheckBox);
			Controls.Add(LocalViewCheckBox);
			Controls.Add(label4);
			Controls.Add(ElevationMagnifyTrackBar);
			Controls.Add(label3);
			Controls.Add(SpecularTrackBar);
			Controls.Add(label2);
			Controls.Add(AmbientTrackBar);
			Controls.Add(label1);
			Controls.Add(ShininessTrackBar);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			Name = "GeoViewerCfgForm";
			Text = "シーン設定";
			((System.ComponentModel.ISupportInitialize)ShininessTrackBar).EndInit();
			((System.ComponentModel.ISupportInitialize)AmbientTrackBar).EndInit();
			((System.ComponentModel.ISupportInitialize)SpecularTrackBar).EndInit();
			((System.ComponentModel.ISupportInitialize)ElevationMagnifyTrackBar).EndInit();
			((System.ComponentModel.ISupportInitialize)VisibilityNumericUpDown).EndInit();
			ResumeLayout(false);
			PerformLayout();
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
		private System.Windows.Forms.NumericUpDown VisibilityNumericUpDown;
		private System.Windows.Forms.CheckBox LocalViewCheckBox;
		private System.Windows.Forms.CheckBox MarkerCheckBox;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox ShadingModeComboBox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox FogModeComboBox;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ComboBox PolygonModeComboBox;
		private System.Windows.Forms.Label label13;
	}
}