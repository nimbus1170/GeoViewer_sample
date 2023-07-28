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
			ShininessTrackBar = new TrackBar();
			label1 = new Label();
			label2 = new Label();
			AmbientTrackBar = new TrackBar();
			label3 = new Label();
			SpecularTrackBar = new TrackBar();
			label4 = new Label();
			ElevationMagnifyTrackBar = new TrackBar();
			FogModeComboBox = new ComboBox();
			VisibilityNumericUpDown = new NumericUpDown();
			LocalViewCheckBox = new CheckBox();
			MarkerCheckBox = new CheckBox();
			label11 = new Label();
			label10 = new Label();
			label9 = new Label();
			label8 = new Label();
			label7 = new Label();
			label6 = new Label();
			ShadingModeComboBox = new ComboBox();
			label5 = new Label();
			label12 = new Label();
			MeshModeComboBox = new ComboBox();
			label13 = new Label();
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
			ShininessTrackBar.Location = new Point(102, 21);
			ShininessTrackBar.Margin = new Padding(3, 4, 3, 4);
			ShininessTrackBar.Maximum = 128;
			ShininessTrackBar.Name = "ShininessTrackBar";
			ShininessTrackBar.Size = new Size(353, 69);
			ShininessTrackBar.TabIndex = 0;
			ShininessTrackBar.TickFrequency = 5;
			ShininessTrackBar.Value = 128;
			ShininessTrackBar.Scroll += ShininessTrackBar_Scroll;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(30, 23);
			label1.Name = "label1";
			label1.Size = new Size(76, 25);
			label1.TabIndex = 1;
			label1.Text = "ハイライト";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(40, 100);
			label2.Name = "label2";
			label2.Size = new Size(66, 25);
			label2.TabIndex = 3;
			label2.Text = "環境光";
			// 
			// AmbientTrackBar
			// 
			AmbientTrackBar.Location = new Point(102, 98);
			AmbientTrackBar.Margin = new Padding(3, 4, 3, 4);
			AmbientTrackBar.Maximum = 100;
			AmbientTrackBar.Name = "AmbientTrackBar";
			AmbientTrackBar.Size = new Size(353, 69);
			AmbientTrackBar.TabIndex = 2;
			AmbientTrackBar.TickFrequency = 5;
			AmbientTrackBar.Value = 80;
			AmbientTrackBar.Scroll += AmbientTrackBar_Scroll;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(43, 177);
			label3.Name = "label3";
			label3.Size = new Size(66, 25);
			label3.TabIndex = 5;
			label3.Text = "鏡面光";
			// 
			// SpecularTrackBar
			// 
			SpecularTrackBar.LargeChange = 10;
			SpecularTrackBar.Location = new Point(102, 175);
			SpecularTrackBar.Margin = new Padding(3, 4, 3, 4);
			SpecularTrackBar.Maximum = 100;
			SpecularTrackBar.Name = "SpecularTrackBar";
			SpecularTrackBar.Size = new Size(353, 69);
			SpecularTrackBar.TabIndex = 4;
			SpecularTrackBar.TickFrequency = 5;
			SpecularTrackBar.Value = 100;
			SpecularTrackBar.Scroll += SpecularTrackBar_Scroll;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new Point(25, 255);
			label4.Name = "label4";
			label4.Size = new Size(84, 25);
			label4.TabIndex = 7;
			label4.Text = "標高倍率";
			// 
			// ElevationMagnifyTrackBar
			// 
			ElevationMagnifyTrackBar.Location = new Point(102, 252);
			ElevationMagnifyTrackBar.Margin = new Padding(3, 4, 3, 4);
			ElevationMagnifyTrackBar.Maximum = 5;
			ElevationMagnifyTrackBar.Name = "ElevationMagnifyTrackBar";
			ElevationMagnifyTrackBar.Size = new Size(353, 69);
			ElevationMagnifyTrackBar.TabIndex = 6;
			ElevationMagnifyTrackBar.Value = 1;
			ElevationMagnifyTrackBar.Scroll += ElevationMagnifyTrackBar_Scroll;
			// 
			// FogModeComboBox
			// 
			FogModeComboBox.FormattingEnabled = true;
			FogModeComboBox.Items.AddRange(new object[] { "無限遠", "霧", "夜暗" });
			FogModeComboBox.Location = new Point(487, 193);
			FogModeComboBox.Name = "FogModeComboBox";
			FogModeComboBox.Size = new Size(104, 33);
			FogModeComboBox.TabIndex = 27;
			FogModeComboBox.SelectedIndexChanged += VisibilityComboBox_SelectedIndexChanged;
			// 
			// VisibilityNumericUpDown
			// 
			VisibilityNumericUpDown.Increment = new decimal(new int[] { 10, 0, 0, 0 });
			VisibilityNumericUpDown.Location = new Point(597, 195);
			VisibilityNumericUpDown.Margin = new Padding(3, 4, 3, 4);
			VisibilityNumericUpDown.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
			VisibilityNumericUpDown.Name = "VisibilityNumericUpDown";
			VisibilityNumericUpDown.Size = new Size(88, 31);
			VisibilityNumericUpDown.TabIndex = 3;
			VisibilityNumericUpDown.ThousandsSeparator = true;
			VisibilityNumericUpDown.Value = new decimal(new int[] { 3000, 0, 0, 0 });
			VisibilityNumericUpDown.ValueChanged += VisibilityNumericUpDown_ValueChanged;
			// 
			// LocalViewCheckBox
			// 
			LocalViewCheckBox.AutoSize = true;
			LocalViewCheckBox.Location = new Point(487, 247);
			LocalViewCheckBox.Margin = new Padding(3, 4, 3, 4);
			LocalViewCheckBox.Name = "LocalViewCheckBox";
			LocalViewCheckBox.Size = new Size(164, 29);
			LocalViewCheckBox.TabIndex = 10;
			LocalViewCheckBox.Text = "鏡面光視点有効";
			LocalViewCheckBox.UseVisualStyleBackColor = true;
			LocalViewCheckBox.CheckedChanged += LocalViewCheckBox_CheckedChanged;
			// 
			// MarkerCheckBox
			// 
			MarkerCheckBox.AutoSize = true;
			MarkerCheckBox.Location = new Point(487, 280);
			MarkerCheckBox.Margin = new Padding(3, 4, 3, 4);
			MarkerCheckBox.Name = "MarkerCheckBox";
			MarkerCheckBox.Size = new Size(89, 29);
			MarkerCheckBox.TabIndex = 11;
			MarkerCheckBox.Text = "マーカー";
			MarkerCheckBox.UseVisualStyleBackColor = true;
			MarkerCheckBox.CheckedChanged += MarkerCheckBox_CheckedChanged;
			// 
			// label11
			// 
			label11.AutoSize = true;
			label11.Font = new Font("MS UI Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point);
			label11.Location = new Point(428, 290);
			label11.Margin = new Padding(5, 0, 5, 0);
			label11.Name = "label11";
			label11.Size = new Size(17, 18);
			label11.TabIndex = 24;
			label11.Text = "5";
			// 
			// label10
			// 
			label10.AutoSize = true;
			label10.Font = new Font("MS UI Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point);
			label10.Location = new Point(364, 290);
			label10.Margin = new Padding(5, 0, 5, 0);
			label10.Name = "label10";
			label10.Size = new Size(17, 18);
			label10.TabIndex = 23;
			label10.Text = "4";
			// 
			// label9
			// 
			label9.AutoSize = true;
			label9.Font = new Font("MS UI Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point);
			label9.Location = new Point(302, 290);
			label9.Margin = new Padding(5, 0, 5, 0);
			label9.Name = "label9";
			label9.Size = new Size(17, 18);
			label9.TabIndex = 22;
			label9.Text = "3";
			// 
			// label8
			// 
			label8.AutoSize = true;
			label8.Font = new Font("MS UI Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point);
			label8.Location = new Point(241, 290);
			label8.Margin = new Padding(5, 0, 5, 0);
			label8.Name = "label8";
			label8.Size = new Size(17, 18);
			label8.TabIndex = 21;
			label8.Text = "2";
			// 
			// label7
			// 
			label7.AutoSize = true;
			label7.Font = new Font("MS UI Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point);
			label7.Location = new Point(177, 290);
			label7.Margin = new Padding(5, 0, 5, 0);
			label7.Name = "label7";
			label7.Size = new Size(17, 18);
			label7.TabIndex = 20;
			label7.Text = "1";
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Font = new Font("MS UI Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point);
			label6.Location = new Point(113, 290);
			label6.Margin = new Padding(5, 0, 5, 0);
			label6.Name = "label6";
			label6.Size = new Size(17, 18);
			label6.TabIndex = 19;
			label6.Text = "0";
			// 
			// ShadingModeComboBox
			// 
			ShadingModeComboBox.FormattingEnabled = true;
			ShadingModeComboBox.Items.AddRange(new object[] { "テクスチャ（地図）", "テクスチャ（写真）", "スムーズ", "フラット", "ワイヤ" });
			ShadingModeComboBox.Location = new Point(487, 51);
			ShadingModeComboBox.Name = "ShadingModeComboBox";
			ShadingModeComboBox.Size = new Size(198, 33);
			ShadingModeComboBox.TabIndex = 25;
			ShadingModeComboBox.SelectedIndexChanged += ShadingModeComboBox_SelectedIndexChanged;
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new Point(483, 23);
			label5.Name = "label5";
			label5.Size = new Size(138, 25);
			label5.TabIndex = 26;
			label5.Text = "シェーディングモード";
			// 
			// label12
			// 
			label12.AutoSize = true;
			label12.Location = new Point(483, 165);
			label12.Name = "label12";
			label12.Size = new Size(48, 25);
			label12.TabIndex = 27;
			label12.Text = "視程";
			// 
			// MeshModeComboBox
			// 
			MeshModeComboBox.FormattingEnabled = true;
			MeshModeComboBox.Items.AddRange(new object[] { "正方形＋中心点", "正方形", "三角形" });
			MeshModeComboBox.Location = new Point(487, 122);
			MeshModeComboBox.Name = "MeshModeComboBox";
			MeshModeComboBox.Size = new Size(198, 33);
			MeshModeComboBox.TabIndex = 28;
			MeshModeComboBox.SelectedIndexChanged += MeshModeComboBox_SelectedIndexChanged;
			// 
			// label13
			// 
			label13.AutoSize = true;
			label13.Location = new Point(483, 94);
			label13.Name = "label13";
			label13.Size = new Size(101, 25);
			label13.TabIndex = 29;
			label13.Text = "メッシュモード";
			// 
			// GeoViewerCfgForm
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(713, 351);
			ControlBox = false;
			Controls.Add(label13);
			Controls.Add(MeshModeComboBox);
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
			FormBorderStyle = FormBorderStyle.FixedSingle;
			Margin = new Padding(3, 4, 3, 4);
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

		private TrackBar ShininessTrackBar;
		private Label label1;
		private Label label2;
		private TrackBar AmbientTrackBar;
		private Label label3;
		private TrackBar SpecularTrackBar;
		private Label label4;
		private TrackBar ElevationMagnifyTrackBar;
		private NumericUpDown VisibilityNumericUpDown;
		private CheckBox LocalViewCheckBox;
		private CheckBox MarkerCheckBox;
		private Label label11;
		private Label label10;
		private Label label9;
		private Label label8;
		private Label label7;
		private Label label6;
		private ComboBox ShadingModeComboBox;
		private Label label5;
		private ComboBox FogModeComboBox;
		private Label label12;
		private ComboBox MeshModeComboBox;
		private Label label13;
	}
}