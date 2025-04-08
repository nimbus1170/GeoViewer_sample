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
			if (disposing && (components is not null))
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeoViewerMainForm));
			DialogTextBox = new TextBox();
			tabControl1 = new TabControl();
			tabPage1 = new TabPage();
			tabPage2 = new TabPage();
			groupBox2 = new GroupBox();
			ObserverYLabel = new Label();
			CamAlt0Button = new Button();
			CamAltTextBox = new TextBox();
			MinCamYLabel = new Label();
			MaxCamYLabel = new Label();
			MaxCamXLabel = new Label();
			MinCamXLabel = new Label();
			CamYScrollBarLabel = new Label();
			CamYScrollBar = new VScrollBar();
			CamXScrollBarLabel = new Label();
			CamXScrollBar = new HScrollBar();
			CamAltScrollBar = new VScrollBar();
			label14 = new Label();
			ObserverXLabel = new Label();
			CamYTextBox = new TextBox();
			CamXTextBox = new TextBox();
			groupBox1 = new GroupBox();
			ObjAlt0Button = new Button();
			ObjAltTextBox = new TextBox();
			ObjAltScrollBar = new VScrollBar();
			label1 = new Label();
			ObjYLabel = new Label();
			AngleTextBox = new TextBox();
			MinObjYLabel = new Label();
			MaxObjYLabel = new Label();
			MaxObjXLabel = new Label();
			MinObjXLabel = new Label();
			ObjYScrollBarLabel = new Label();
			ObjYScrollBar = new VScrollBar();
			ObjXScrollBarLabel = new Label();
			ObjXScrollBar = new HScrollBar();
			AngleScrollBar = new VScrollBar();
			label4 = new Label();
			DirScrollBar = new HScrollBar();
			label3 = new Label();
			DirTextBox = new TextBox();
			DistScrollBar = new HScrollBar();
			label2 = new Label();
			DistTextBox = new TextBox();
			ObjXLabel = new Label();
			ObjYTextBox = new TextBox();
			ObjXTextBox = new TextBox();
			tabPage3 = new TabPage();
			ObjInfoListBox = new CheckedListBox();
			label21 = new Label();
			NearPlaneLabel = new Label();
			FovyLabel = new Label();
			label22 = new Label();
			label20 = new Label();
			ZNearTrackBar = new TrackBar();
			FovyTrackBar = new TrackBar();
			MarkerCheckBox = new CheckBox();
			MarkerTypeComboBox = new ComboBox();
			label19 = new Label();
			OriginComboBox = new ComboBox();
			label13 = new Label();
			MeshModeComboBox = new ComboBox();
			VisibilityNumericUpDown = new NumericUpDown();
			FogModeComboBox = new ComboBox();
			label12 = new Label();
			label5 = new Label();
			ShadingModeComboBox = new ComboBox();
			label11 = new Label();
			label10 = new Label();
			label9 = new Label();
			label8 = new Label();
			label7 = new Label();
			label6 = new Label();
			LocalViewCheckBox = new CheckBox();
			label15 = new Label();
			EvScaleTrackBar = new TrackBar();
			label16 = new Label();
			SpecTrackBar = new TrackBar();
			label17 = new Label();
			AmbTrackBar = new TrackBar();
			label18 = new Label();
			ShinTrackBar = new TrackBar();
			tabControl1.SuspendLayout();
			tabPage1.SuspendLayout();
			tabPage2.SuspendLayout();
			groupBox2.SuspendLayout();
			groupBox1.SuspendLayout();
			tabPage3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)ZNearTrackBar).BeginInit();
			((System.ComponentModel.ISupportInitialize)FovyTrackBar).BeginInit();
			((System.ComponentModel.ISupportInitialize)VisibilityNumericUpDown).BeginInit();
			((System.ComponentModel.ISupportInitialize)EvScaleTrackBar).BeginInit();
			((System.ComponentModel.ISupportInitialize)SpecTrackBar).BeginInit();
			((System.ComponentModel.ISupportInitialize)AmbTrackBar).BeginInit();
			((System.ComponentModel.ISupportInitialize)ShinTrackBar).BeginInit();
			SuspendLayout();
			// 
			// DialogTextBox
			// 
			DialogTextBox.Dock = DockStyle.Fill;
			DialogTextBox.Font = new Font("ＭＳ ゴシック", 9F, FontStyle.Regular, GraphicsUnit.Point, 128);
			DialogTextBox.Location = new Point(3, 3);
			DialogTextBox.Margin = new Padding(3, 4, 3, 4);
			DialogTextBox.Multiline = true;
			DialogTextBox.Name = "DialogTextBox";
			DialogTextBox.ScrollBars = ScrollBars.Both;
			DialogTextBox.Size = new Size(922, 486);
			DialogTextBox.TabIndex = 4;
			DialogTextBox.KeyDown += DialogTextBox_KeyDown;
			DialogTextBox.KeyPress += DialogTextBox_KeyPress;
			DialogTextBox.MouseDown += DialogTextBox_MouseDown;
			DialogTextBox.MouseMove += DialogTextBox_MouseMove;
			// 
			// tabControl1
			// 
			tabControl1.Controls.Add(tabPage1);
			tabControl1.Controls.Add(tabPage2);
			tabControl1.Controls.Add(tabPage3);
			tabControl1.Dock = DockStyle.Fill;
			tabControl1.Location = new Point(0, 0);
			tabControl1.Name = "tabControl1";
			tabControl1.SelectedIndex = 0;
			tabControl1.Size = new Size(936, 530);
			tabControl1.TabIndex = 5;
			// 
			// tabPage1
			// 
			tabPage1.Controls.Add(DialogTextBox);
			tabPage1.Location = new Point(4, 34);
			tabPage1.Name = "tabPage1";
			tabPage1.Padding = new Padding(3);
			tabPage1.Size = new Size(928, 492);
			tabPage1.TabIndex = 0;
			tabPage1.Text = "メイン";
			tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			tabPage2.BackColor = SystemColors.Control;
			tabPage2.Controls.Add(groupBox2);
			tabPage2.Controls.Add(groupBox1);
			tabPage2.Location = new Point(4, 34);
			tabPage2.Name = "tabPage2";
			tabPage2.Padding = new Padding(3);
			tabPage2.Size = new Size(928, 492);
			tabPage2.TabIndex = 1;
			tabPage2.Text = "コントローラー";
			// 
			// groupBox2
			// 
			groupBox2.Controls.Add(ObserverYLabel);
			groupBox2.Controls.Add(CamAlt0Button);
			groupBox2.Controls.Add(CamAltTextBox);
			groupBox2.Controls.Add(MinCamYLabel);
			groupBox2.Controls.Add(MaxCamYLabel);
			groupBox2.Controls.Add(MaxCamXLabel);
			groupBox2.Controls.Add(MinCamXLabel);
			groupBox2.Controls.Add(CamYScrollBarLabel);
			groupBox2.Controls.Add(CamYScrollBar);
			groupBox2.Controls.Add(CamXScrollBarLabel);
			groupBox2.Controls.Add(CamXScrollBar);
			groupBox2.Controls.Add(CamAltScrollBar);
			groupBox2.Controls.Add(label14);
			groupBox2.Controls.Add(ObserverXLabel);
			groupBox2.Controls.Add(CamYTextBox);
			groupBox2.Controls.Add(CamXTextBox);
			groupBox2.Location = new Point(502, 11);
			groupBox2.Margin = new Padding(3, 4, 3, 4);
			groupBox2.Name = "groupBox2";
			groupBox2.Padding = new Padding(3, 4, 3, 4);
			groupBox2.Size = new Size(410, 471);
			groupBox2.TabIndex = 3;
			groupBox2.TabStop = false;
			groupBox2.Text = "カメラ";
			// 
			// ObserverYLabel
			// 
			ObserverYLabel.Location = new Point(28, 83);
			ObserverYLabel.Name = "ObserverYLabel";
			ObserverYLabel.Size = new Size(48, 25);
			ObserverYLabel.TabIndex = 33;
			ObserverYLabel.Text = "緯度";
			ObserverYLabel.TextAlign = ContentAlignment.MiddleRight;
			// 
			// CamAlt0Button
			// 
			CamAlt0Button.Location = new Point(240, 308);
			CamAlt0Button.Margin = new Padding(3, 4, 3, 4);
			CamAlt0Button.Name = "CamAlt0Button";
			CamAlt0Button.Size = new Size(55, 38);
			CamAlt0Button.TabIndex = 32;
			CamAlt0Button.Text = "0m";
			CamAlt0Button.UseVisualStyleBackColor = true;
			CamAlt0Button.Click += CamAlt0Button_Click;
			// 
			// CamAltTextBox
			// 
			CamAltTextBox.Location = new Point(167, 169);
			CamAltTextBox.Margin = new Padding(3, 4, 3, 4);
			CamAltTextBox.Name = "CamAltTextBox";
			CamAltTextBox.Size = new Size(82, 31);
			CamAltTextBox.TabIndex = 31;
			CamAltTextBox.Text = "80000.00";
			CamAltTextBox.TextAlign = HorizontalAlignment.Right;
			// 
			// MinCamYLabel
			// 
			MinCamYLabel.AutoSize = true;
			MinCamYLabel.Location = new Point(318, 368);
			MinCamYLabel.Name = "MinCamYLabel";
			MinCamYLabel.Size = new Size(86, 25);
			MinCamYLabel.TabIndex = 30;
			MinCamYLabel.Text = "33.00000";
			// 
			// MaxCamYLabel
			// 
			MaxCamYLabel.AutoSize = true;
			MaxCamYLabel.Location = new Point(310, 101);
			MaxCamYLabel.Name = "MaxCamYLabel";
			MaxCamYLabel.Size = new Size(86, 25);
			MaxCamYLabel.TabIndex = 29;
			MaxCamYLabel.Text = "33.00000";
			// 
			// MaxCamXLabel
			// 
			MaxCamXLabel.AutoSize = true;
			MaxCamXLabel.Location = new Point(215, 428);
			MaxCamXLabel.Name = "MaxCamXLabel";
			MaxCamXLabel.Size = new Size(96, 25);
			MaxCamXLabel.TabIndex = 28;
			MaxCamXLabel.Text = "130.00000";
			// 
			// MinCamXLabel
			// 
			MinCamXLabel.AutoSize = true;
			MinCamXLabel.Location = new Point(75, 428);
			MinCamXLabel.Name = "MinCamXLabel";
			MinCamXLabel.Size = new Size(96, 25);
			MinCamXLabel.TabIndex = 27;
			MinCamXLabel.Text = "130.00000";
			// 
			// CamYScrollBarLabel
			// 
			CamYScrollBarLabel.Font = new Font("MS UI Gothic", 9F);
			CamYScrollBarLabel.Location = new Point(308, 60);
			CamYScrollBarLabel.Name = "CamYScrollBarLabel";
			CamYScrollBarLabel.Size = new Size(48, 25);
			CamYScrollBarLabel.TabIndex = 26;
			CamYScrollBarLabel.Text = "南北";
			CamYScrollBarLabel.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// CamYScrollBar
			// 
			CamYScrollBar.Location = new Point(313, 138);
			CamYScrollBar.Maximum = 80000;
			CamYScrollBar.Name = "CamYScrollBar";
			CamYScrollBar.Size = new Size(20, 208);
			CamYScrollBar.TabIndex = 25;
			CamYScrollBar.Scroll += CamYScrollBar_Scroll;
			// 
			// CamXScrollBarLabel
			// 
			CamXScrollBarLabel.Location = new Point(28, 373);
			CamXScrollBarLabel.Name = "CamXScrollBarLabel";
			CamXScrollBarLabel.Size = new Size(48, 25);
			CamXScrollBarLabel.TabIndex = 24;
			CamXScrollBarLabel.Text = "東西";
			CamXScrollBarLabel.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// CamXScrollBar
			// 
			CamXScrollBar.Location = new Point(78, 375);
			CamXScrollBar.Maximum = 80000;
			CamXScrollBar.Name = "CamXScrollBar";
			CamXScrollBar.Size = new Size(217, 20);
			CamXScrollBar.TabIndex = 23;
			CamXScrollBar.Value = 80000;
			CamXScrollBar.Scroll += CamXScrollBar_Scroll;
			// 
			// CamAltScrollBar
			// 
			CamAltScrollBar.Location = new Point(258, 96);
			CamAltScrollBar.Maximum = 90;
			CamAltScrollBar.Name = "CamAltScrollBar";
			CamAltScrollBar.Size = new Size(20, 208);
			CamAltScrollBar.TabIndex = 22;
			CamAltScrollBar.Scroll += CamAltScrollBar_Scroll;
			// 
			// label14
			// 
			label14.AutoSize = true;
			label14.Font = new Font("MS UI Gothic", 9F);
			label14.Location = new Point(187, 139);
			label14.Name = "label14";
			label14.Size = new Size(62, 18);
			label14.TabIndex = 21;
			label14.Text = "地上高";
			label14.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// ObserverXLabel
			// 
			ObserverXLabel.Location = new Point(28, 35);
			ObserverXLabel.Name = "ObserverXLabel";
			ObserverXLabel.Size = new Size(48, 25);
			ObserverXLabel.TabIndex = 5;
			ObserverXLabel.Text = "経度";
			ObserverXLabel.TextAlign = ContentAlignment.MiddleRight;
			// 
			// CamYTextBox
			// 
			CamYTextBox.Location = new Point(83, 77);
			CamYTextBox.Margin = new Padding(3, 4, 3, 4);
			CamYTextBox.Name = "CamYTextBox";
			CamYTextBox.Size = new Size(112, 31);
			CamYTextBox.TabIndex = 4;
			CamYTextBox.Text = "33.0000000";
			CamYTextBox.TextAlign = HorizontalAlignment.Right;
			// 
			// CamXTextBox
			// 
			CamXTextBox.Location = new Point(83, 29);
			CamXTextBox.Margin = new Padding(3, 4, 3, 4);
			CamXTextBox.Name = "CamXTextBox";
			CamXTextBox.Size = new Size(112, 31);
			CamXTextBox.TabIndex = 3;
			CamXTextBox.Text = "130.0000000";
			CamXTextBox.TextAlign = HorizontalAlignment.Right;
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(ObjAlt0Button);
			groupBox1.Controls.Add(ObjAltTextBox);
			groupBox1.Controls.Add(ObjAltScrollBar);
			groupBox1.Controls.Add(label1);
			groupBox1.Controls.Add(ObjYLabel);
			groupBox1.Controls.Add(AngleTextBox);
			groupBox1.Controls.Add(MinObjYLabel);
			groupBox1.Controls.Add(MaxObjYLabel);
			groupBox1.Controls.Add(MaxObjXLabel);
			groupBox1.Controls.Add(MinObjXLabel);
			groupBox1.Controls.Add(ObjYScrollBarLabel);
			groupBox1.Controls.Add(ObjYScrollBar);
			groupBox1.Controls.Add(ObjXScrollBarLabel);
			groupBox1.Controls.Add(ObjXScrollBar);
			groupBox1.Controls.Add(AngleScrollBar);
			groupBox1.Controls.Add(label4);
			groupBox1.Controls.Add(DirScrollBar);
			groupBox1.Controls.Add(label3);
			groupBox1.Controls.Add(DirTextBox);
			groupBox1.Controls.Add(DistScrollBar);
			groupBox1.Controls.Add(label2);
			groupBox1.Controls.Add(DistTextBox);
			groupBox1.Controls.Add(ObjXLabel);
			groupBox1.Controls.Add(ObjYTextBox);
			groupBox1.Controls.Add(ObjXTextBox);
			groupBox1.Location = new Point(12, 11);
			groupBox1.Margin = new Padding(3, 4, 3, 4);
			groupBox1.Name = "groupBox1";
			groupBox1.Padding = new Padding(3, 4, 3, 4);
			groupBox1.Size = new Size(476, 471);
			groupBox1.TabIndex = 2;
			groupBox1.TabStop = false;
			groupBox1.Text = "注視点";
			// 
			// ObjAlt0Button
			// 
			ObjAlt0Button.Location = new Point(229, 325);
			ObjAlt0Button.Margin = new Padding(3, 4, 3, 4);
			ObjAlt0Button.Name = "ObjAlt0Button";
			ObjAlt0Button.Size = new Size(55, 38);
			ObjAlt0Button.TabIndex = 35;
			ObjAlt0Button.Text = "0m";
			ObjAlt0Button.UseVisualStyleBackColor = true;
			ObjAlt0Button.Click += ObjAlt0Button_Click;
			// 
			// ObjAltTextBox
			// 
			ObjAltTextBox.Location = new Point(223, 240);
			ObjAltTextBox.Margin = new Padding(3, 4, 3, 4);
			ObjAltTextBox.Name = "ObjAltTextBox";
			ObjAltTextBox.Size = new Size(61, 31);
			ObjAltTextBox.TabIndex = 34;
			ObjAltTextBox.Text = "100.00";
			ObjAltTextBox.TextAlign = HorizontalAlignment.Right;
			// 
			// ObjAltScrollBar
			// 
			ObjAltScrollBar.Location = new Point(291, 159);
			ObjAltScrollBar.Maximum = 900;
			ObjAltScrollBar.Minimum = -900;
			ObjAltScrollBar.Name = "ObjAltScrollBar";
			ObjAltScrollBar.Size = new Size(20, 204);
			ObjAltScrollBar.TabIndex = 33;
			ObjAltScrollBar.Scroll += ObjAltScrollBar_Scroll;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new Font("MS UI Gothic", 9F);
			label1.Location = new Point(222, 210);
			label1.Name = "label1";
			label1.Size = new Size(62, 18);
			label1.TabIndex = 32;
			label1.Text = "地上高";
			label1.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// ObjYLabel
			// 
			ObjYLabel.Location = new Point(27, 83);
			ObjYLabel.Name = "ObjYLabel";
			ObjYLabel.Size = new Size(48, 25);
			ObjYLabel.TabIndex = 21;
			ObjYLabel.Text = "緯度";
			ObjYLabel.TextAlign = ContentAlignment.MiddleRight;
			// 
			// AngleTextBox
			// 
			AngleTextBox.Location = new Point(269, 110);
			AngleTextBox.Margin = new Padding(3, 4, 3, 4);
			AngleTextBox.Name = "AngleTextBox";
			AngleTextBox.Size = new Size(61, 31);
			AngleTextBox.TabIndex = 20;
			AngleTextBox.Text = "-000";
			// 
			// MinObjYLabel
			// 
			MinObjYLabel.AutoSize = true;
			MinObjYLabel.Location = new Point(379, 368);
			MinObjYLabel.Name = "MinObjYLabel";
			MinObjYLabel.Size = new Size(86, 25);
			MinObjYLabel.TabIndex = 19;
			MinObjYLabel.Text = "33.00000";
			// 
			// MaxObjYLabel
			// 
			MaxObjYLabel.AutoSize = true;
			MaxObjYLabel.Location = new Point(379, 101);
			MaxObjYLabel.Name = "MaxObjYLabel";
			MaxObjYLabel.Size = new Size(86, 25);
			MaxObjYLabel.TabIndex = 18;
			MaxObjYLabel.Text = "33.00000";
			// 
			// MaxObjXLabel
			// 
			MaxObjXLabel.AutoSize = true;
			MaxObjXLabel.Location = new Point(277, 424);
			MaxObjXLabel.Name = "MaxObjXLabel";
			MaxObjXLabel.Size = new Size(96, 25);
			MaxObjXLabel.TabIndex = 17;
			MaxObjXLabel.Text = "130.00000";
			// 
			// MinObjXLabel
			// 
			MinObjXLabel.AutoSize = true;
			MinObjXLabel.Location = new Point(137, 424);
			MinObjXLabel.Name = "MinObjXLabel";
			MinObjXLabel.Size = new Size(96, 25);
			MinObjXLabel.TabIndex = 16;
			MinObjXLabel.Text = "130.00000";
			// 
			// ObjYScrollBarLabel
			// 
			ObjYScrollBarLabel.Font = new Font("MS UI Gothic", 9F);
			ObjYScrollBarLabel.Location = new Point(379, 56);
			ObjYScrollBarLabel.Name = "ObjYScrollBarLabel";
			ObjYScrollBarLabel.Size = new Size(48, 25);
			ObjYScrollBarLabel.TabIndex = 14;
			ObjYScrollBarLabel.Text = "南北";
			ObjYScrollBarLabel.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// ObjYScrollBar
			// 
			ObjYScrollBar.Location = new Point(383, 138);
			ObjYScrollBar.Maximum = 80000;
			ObjYScrollBar.Name = "ObjYScrollBar";
			ObjYScrollBar.Size = new Size(20, 208);
			ObjYScrollBar.TabIndex = 13;
			ObjYScrollBar.Scroll += ObjYScrollBar_Scroll;
			// 
			// ObjXScrollBarLabel
			// 
			ObjXScrollBarLabel.Location = new Point(87, 373);
			ObjXScrollBarLabel.Name = "ObjXScrollBarLabel";
			ObjXScrollBarLabel.Size = new Size(48, 25);
			ObjXScrollBarLabel.TabIndex = 12;
			ObjXScrollBarLabel.Text = "東西";
			ObjXScrollBarLabel.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// ObjXScrollBar
			// 
			ObjXScrollBar.Location = new Point(140, 375);
			ObjXScrollBar.Maximum = 80000;
			ObjXScrollBar.Name = "ObjXScrollBar";
			ObjXScrollBar.Size = new Size(217, 20);
			ObjXScrollBar.TabIndex = 11;
			ObjXScrollBar.Value = 80000;
			ObjXScrollBar.Scroll += ObjXScrollBar_Scroll;
			// 
			// AngleScrollBar
			// 
			AngleScrollBar.Location = new Point(336, 79);
			AngleScrollBar.Maximum = 90;
			AngleScrollBar.Name = "AngleScrollBar";
			AngleScrollBar.Size = new Size(20, 208);
			AngleScrollBar.TabIndex = 10;
			AngleScrollBar.Scroll += AngleScrollBar_Scroll;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Font = new Font("MS UI Gothic", 9F);
			label4.Location = new Point(286, 80);
			label4.Name = "label4";
			label4.Size = new Size(44, 18);
			label4.TabIndex = 9;
			label4.Text = "俯角";
			label4.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// DirScrollBar
			// 
			DirScrollBar.Location = new Point(30, 296);
			DirScrollBar.Maximum = 180;
			DirScrollBar.Minimum = -180;
			DirScrollBar.Name = "DirScrollBar";
			DirScrollBar.Size = new Size(217, 20);
			DirScrollBar.TabIndex = 8;
			DirScrollBar.Value = 180;
			DirScrollBar.Scroll += DirScrollBar_Scroll;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(9, 256);
			label3.Name = "label3";
			label3.Size = new Size(66, 25);
			label3.TabIndex = 7;
			label3.Text = "方位角";
			// 
			// DirTextBox
			// 
			DirTextBox.Location = new Point(80, 252);
			DirTextBox.Margin = new Padding(3, 4, 3, 4);
			DirTextBox.Name = "DirTextBox";
			DirTextBox.Size = new Size(61, 31);
			DirTextBox.TabIndex = 6;
			DirTextBox.Text = "-000";
			// 
			// DistScrollBar
			// 
			DistScrollBar.Location = new Point(30, 183);
			DistScrollBar.Maximum = 80000;
			DistScrollBar.Name = "DistScrollBar";
			DistScrollBar.Size = new Size(217, 20);
			DistScrollBar.TabIndex = 5;
			DistScrollBar.Value = 80000;
			DistScrollBar.Scroll += DistScrollBar_Scroll;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(27, 149);
			label2.Name = "label2";
			label2.Size = new Size(48, 25);
			label2.TabIndex = 4;
			label2.Text = "距離";
			// 
			// DistTextBox
			// 
			DistTextBox.Location = new Point(80, 142);
			DistTextBox.Margin = new Padding(3, 4, 3, 4);
			DistTextBox.Name = "DistTextBox";
			DistTextBox.Size = new Size(61, 31);
			DistTextBox.TabIndex = 3;
			DistTextBox.Text = "00000";
			// 
			// ObjXLabel
			// 
			ObjXLabel.Location = new Point(27, 35);
			ObjXLabel.Name = "ObjXLabel";
			ObjXLabel.Size = new Size(48, 25);
			ObjXLabel.TabIndex = 2;
			ObjXLabel.Text = "経度";
			ObjXLabel.TextAlign = ContentAlignment.MiddleRight;
			// 
			// ObjYTextBox
			// 
			ObjYTextBox.Location = new Point(80, 77);
			ObjYTextBox.Margin = new Padding(3, 4, 3, 4);
			ObjYTextBox.Name = "ObjYTextBox";
			ObjYTextBox.Size = new Size(112, 31);
			ObjYTextBox.TabIndex = 1;
			ObjYTextBox.Text = "33.0000000";
			ObjYTextBox.TextAlign = HorizontalAlignment.Right;
			// 
			// ObjXTextBox
			// 
			ObjXTextBox.Location = new Point(80, 29);
			ObjXTextBox.Margin = new Padding(3, 4, 3, 4);
			ObjXTextBox.Name = "ObjXTextBox";
			ObjXTextBox.Size = new Size(112, 31);
			ObjXTextBox.TabIndex = 0;
			ObjXTextBox.Text = "130.0000000";
			ObjXTextBox.TextAlign = HorizontalAlignment.Right;
			// 
			// tabPage3
			// 
			tabPage3.BackColor = SystemColors.Control;
			tabPage3.Controls.Add(ObjInfoListBox);
			tabPage3.Controls.Add(label21);
			tabPage3.Controls.Add(NearPlaneLabel);
			tabPage3.Controls.Add(FovyLabel);
			tabPage3.Controls.Add(label22);
			tabPage3.Controls.Add(label20);
			tabPage3.Controls.Add(ZNearTrackBar);
			tabPage3.Controls.Add(FovyTrackBar);
			tabPage3.Controls.Add(MarkerCheckBox);
			tabPage3.Controls.Add(MarkerTypeComboBox);
			tabPage3.Controls.Add(label19);
			tabPage3.Controls.Add(OriginComboBox);
			tabPage3.Controls.Add(label13);
			tabPage3.Controls.Add(MeshModeComboBox);
			tabPage3.Controls.Add(VisibilityNumericUpDown);
			tabPage3.Controls.Add(FogModeComboBox);
			tabPage3.Controls.Add(label12);
			tabPage3.Controls.Add(label5);
			tabPage3.Controls.Add(ShadingModeComboBox);
			tabPage3.Controls.Add(label11);
			tabPage3.Controls.Add(label10);
			tabPage3.Controls.Add(label9);
			tabPage3.Controls.Add(label8);
			tabPage3.Controls.Add(label7);
			tabPage3.Controls.Add(label6);
			tabPage3.Controls.Add(LocalViewCheckBox);
			tabPage3.Controls.Add(label15);
			tabPage3.Controls.Add(EvScaleTrackBar);
			tabPage3.Controls.Add(label16);
			tabPage3.Controls.Add(SpecTrackBar);
			tabPage3.Controls.Add(label17);
			tabPage3.Controls.Add(AmbTrackBar);
			tabPage3.Controls.Add(label18);
			tabPage3.Controls.Add(ShinTrackBar);
			tabPage3.Location = new Point(4, 34);
			tabPage3.Name = "tabPage3";
			tabPage3.Size = new Size(928, 492);
			tabPage3.TabIndex = 2;
			tabPage3.Text = "設定";
			// 
			// ObjInfoListBox
			// 
			ObjInfoListBox.BackColor = SystemColors.Control;
			ObjInfoListBox.BorderStyle = BorderStyle.None;
			ObjInfoListBox.CheckOnClick = true;
			ObjInfoListBox.FormattingEnabled = true;
			ObjInfoListBox.Items.AddRange(new object[] { "経緯度（度分秒）", "経緯度（十進）", "平面直角座標", "UTM", "MGRS", "高度", "地心直交座標", "カメラ→注視点" });
			ObjInfoListBox.Location = new Point(724, 51);
			ObjInfoListBox.Name = "ObjInfoListBox";
			ObjInfoListBox.Size = new Size(180, 224);
			ObjInfoListBox.TabIndex = 65;
			ObjInfoListBox.ItemCheck += ObjInfoListBox_ItemCheck;
			// 
			// label21
			// 
			label21.AutoSize = true;
			label21.Location = new Point(724, 23);
			label21.Name = "label21";
			label21.Size = new Size(84, 25);
			label21.TabIndex = 49;
			label21.Text = "座標表示";
			// 
			// NearPlaneLabel
			// 
			NearPlaneLabel.ImageAlign = ContentAlignment.MiddleRight;
			NearPlaneLabel.Location = new Point(45, 431);
			NearPlaneLabel.Name = "NearPlaneLabel";
			NearPlaneLabel.Size = new Size(53, 25);
			NearPlaneLabel.TabIndex = 63;
			NearPlaneLabel.Text = "10";
			NearPlaneLabel.TextAlign = ContentAlignment.MiddleRight;
			// 
			// FovyLabel
			// 
			FovyLabel.ImageAlign = ContentAlignment.MiddleRight;
			FovyLabel.Location = new Point(45, 354);
			FovyLabel.Name = "FovyLabel";
			FovyLabel.Size = new Size(53, 25);
			FovyLabel.TabIndex = 60;
			FovyLabel.Text = "45°";
			FovyLabel.TextAlign = ContentAlignment.MiddleRight;
			// 
			// label22
			// 
			label22.AutoSize = true;
			label22.Location = new Point(32, 406);
			label22.Name = "label22";
			label22.Size = new Size(66, 25);
			label22.TabIndex = 62;
			label22.Text = "近平面";
			// 
			// label20
			// 
			label20.AutoSize = true;
			label20.Location = new Point(4, 329);
			label20.Name = "label20";
			label20.Size = new Size(94, 25);
			label20.TabIndex = 59;
			label20.Text = "視野角(縦)";
			// 
			// ZNearTrackBar
			// 
			ZNearTrackBar.LargeChange = 10;
			ZNearTrackBar.Location = new Point(104, 406);
			ZNearTrackBar.Margin = new Padding(3, 4, 3, 4);
			ZNearTrackBar.Maximum = 1000;
			ZNearTrackBar.Minimum = 1;
			ZNearTrackBar.Name = "ZNearTrackBar";
			ZNearTrackBar.Size = new Size(353, 69);
			ZNearTrackBar.TabIndex = 61;
			ZNearTrackBar.TickFrequency = 100;
			ZNearTrackBar.Value = 45;
			ZNearTrackBar.Scroll += ZNearTrackBar_Scroll;
			// 
			// FovyTrackBar
			// 
			FovyTrackBar.LargeChange = 10;
			FovyTrackBar.Location = new Point(104, 329);
			FovyTrackBar.Margin = new Padding(3, 4, 3, 4);
			FovyTrackBar.Maximum = 179;
			FovyTrackBar.Minimum = 1;
			FovyTrackBar.Name = "FovyTrackBar";
			FovyTrackBar.Size = new Size(353, 69);
			FovyTrackBar.TabIndex = 58;
			FovyTrackBar.TickFrequency = 10;
			FovyTrackBar.Value = 45;
			FovyTrackBar.Scroll += FovyTrackBar_Scroll;
			// 
			// MarkerCheckBox
			// 
			MarkerCheckBox.AutoSize = true;
			MarkerCheckBox.Location = new Point(489, 292);
			MarkerCheckBox.Margin = new Padding(3, 4, 3, 4);
			MarkerCheckBox.Name = "MarkerCheckBox";
			MarkerCheckBox.Size = new Size(89, 29);
			MarkerCheckBox.TabIndex = 57;
			MarkerCheckBox.Text = "マーカー";
			MarkerCheckBox.UseVisualStyleBackColor = true;
			MarkerCheckBox.CheckedChanged += MarkerCheckBox_CheckedChanged;
			// 
			// MarkerTypeComboBox
			// 
			MarkerTypeComboBox.Enabled = false;
			MarkerTypeComboBox.FormattingEnabled = true;
			MarkerTypeComboBox.Items.AddRange(new object[] { "ピラミッド", "レクティクル", "断面図", "垂直線" });
			MarkerTypeComboBox.Location = new Point(489, 321);
			MarkerTypeComboBox.Name = "MarkerTypeComboBox";
			MarkerTypeComboBox.Size = new Size(198, 33);
			MarkerTypeComboBox.TabIndex = 55;
			MarkerTypeComboBox.SelectedIndexChanged += MarkerTypeComboBox_SelectedIndexChanged;
			// 
			// label19
			// 
			label19.AutoSize = true;
			label19.Location = new Point(489, 376);
			label19.Name = "label19";
			label19.Size = new Size(120, 25);
			label19.TabIndex = 54;
			label19.Text = "平面直角座標";
			// 
			// OriginComboBox
			// 
			OriginComboBox.FormattingEnabled = true;
			OriginComboBox.Items.AddRange(new object[] { "1系", "2系", "3系", "4系", "5系", "6系", "7系", "8系", "9系", "10系", "11系", "12系", "13系", "14系", "15系", "16系", "17系", "18系", "19系" });
			OriginComboBox.Location = new Point(489, 404);
			OriginComboBox.Name = "OriginComboBox";
			OriginComboBox.Size = new Size(198, 33);
			OriginComboBox.TabIndex = 53;
			OriginComboBox.SelectedIndexChanged += OriginComboBox_SelectedIndexChanged;
			// 
			// label13
			// 
			label13.AutoSize = true;
			label13.Location = new Point(485, 94);
			label13.Name = "label13";
			label13.Size = new Size(101, 25);
			label13.TabIndex = 52;
			label13.Text = "メッシュモード";
			// 
			// MeshModeComboBox
			// 
			MeshModeComboBox.FormattingEnabled = true;
			MeshModeComboBox.Items.AddRange(new object[] { "正方形＋中心点", "正方形", "三角形" });
			MeshModeComboBox.Location = new Point(489, 122);
			MeshModeComboBox.Name = "MeshModeComboBox";
			MeshModeComboBox.Size = new Size(198, 33);
			MeshModeComboBox.TabIndex = 51;
			MeshModeComboBox.SelectedIndexChanged += MeshModeComboBox_SelectedIndexChanged;
			// 
			// VisibilityNumericUpDown
			// 
			VisibilityNumericUpDown.Increment = new decimal(new int[] { 10, 0, 0, 0 });
			VisibilityNumericUpDown.Location = new Point(599, 195);
			VisibilityNumericUpDown.Margin = new Padding(3, 4, 3, 4);
			VisibilityNumericUpDown.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
			VisibilityNumericUpDown.Name = "VisibilityNumericUpDown";
			VisibilityNumericUpDown.Size = new Size(88, 31);
			VisibilityNumericUpDown.TabIndex = 33;
			VisibilityNumericUpDown.ThousandsSeparator = true;
			VisibilityNumericUpDown.Value = new decimal(new int[] { 3000, 0, 0, 0 });
			VisibilityNumericUpDown.ValueChanged += VisibilityNumericUpDown_ValueChanged;
			// 
			// FogModeComboBox
			// 
			FogModeComboBox.FormattingEnabled = true;
			FogModeComboBox.Items.AddRange(new object[] { "無限遠", "霧", "夜暗" });
			FogModeComboBox.Location = new Point(489, 193);
			FogModeComboBox.Name = "FogModeComboBox";
			FogModeComboBox.Size = new Size(104, 33);
			FogModeComboBox.TabIndex = 50;
			FogModeComboBox.SelectedIndexChanged += FogModeComboBox_SelectedIndexChanged;
			// 
			// label12
			// 
			label12.AutoSize = true;
			label12.Location = new Point(485, 165);
			label12.Name = "label12";
			label12.Size = new Size(48, 25);
			label12.TabIndex = 49;
			label12.Text = "視程";
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new Point(485, 23);
			label5.Name = "label5";
			label5.Size = new Size(138, 25);
			label5.TabIndex = 48;
			label5.Text = "シェーディングモード";
			// 
			// ShadingModeComboBox
			// 
			ShadingModeComboBox.FormattingEnabled = true;
			ShadingModeComboBox.Items.AddRange(new object[] { "テクスチャ（地図）", "テクスチャ（写真）", "スムーズ", "フラット", "ワイヤ" });
			ShadingModeComboBox.Location = new Point(489, 51);
			ShadingModeComboBox.Name = "ShadingModeComboBox";
			ShadingModeComboBox.Size = new Size(198, 33);
			ShadingModeComboBox.TabIndex = 47;
			ShadingModeComboBox.SelectedIndexChanged += ShadingModeComboBox_SelectedIndexChanged;
			// 
			// label11
			// 
			label11.AutoSize = true;
			label11.Font = new Font("MS UI Gothic", 9F);
			label11.Location = new Point(430, 290);
			label11.Margin = new Padding(5, 0, 5, 0);
			label11.Name = "label11";
			label11.Size = new Size(17, 18);
			label11.TabIndex = 46;
			label11.Text = "5";
			// 
			// label10
			// 
			label10.AutoSize = true;
			label10.Font = new Font("MS UI Gothic", 9F);
			label10.Location = new Point(366, 290);
			label10.Margin = new Padding(5, 0, 5, 0);
			label10.Name = "label10";
			label10.Size = new Size(17, 18);
			label10.TabIndex = 45;
			label10.Text = "4";
			// 
			// label9
			// 
			label9.AutoSize = true;
			label9.Font = new Font("MS UI Gothic", 9F);
			label9.Location = new Point(304, 290);
			label9.Margin = new Padding(5, 0, 5, 0);
			label9.Name = "label9";
			label9.Size = new Size(17, 18);
			label9.TabIndex = 44;
			label9.Text = "3";
			// 
			// label8
			// 
			label8.AutoSize = true;
			label8.Font = new Font("MS UI Gothic", 9F);
			label8.Location = new Point(243, 290);
			label8.Margin = new Padding(5, 0, 5, 0);
			label8.Name = "label8";
			label8.Size = new Size(17, 18);
			label8.TabIndex = 43;
			label8.Text = "2";
			// 
			// label7
			// 
			label7.AutoSize = true;
			label7.Font = new Font("MS UI Gothic", 9F);
			label7.Location = new Point(179, 290);
			label7.Margin = new Padding(5, 0, 5, 0);
			label7.Name = "label7";
			label7.Size = new Size(17, 18);
			label7.TabIndex = 42;
			label7.Text = "1";
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Font = new Font("MS UI Gothic", 9F);
			label6.Location = new Point(115, 290);
			label6.Margin = new Padding(5, 0, 5, 0);
			label6.Name = "label6";
			label6.Size = new Size(17, 18);
			label6.TabIndex = 41;
			label6.Text = "0";
			// 
			// LocalViewCheckBox
			// 
			LocalViewCheckBox.AutoSize = true;
			LocalViewCheckBox.Checked = true;
			LocalViewCheckBox.CheckState = CheckState.Checked;
			LocalViewCheckBox.Location = new Point(489, 247);
			LocalViewCheckBox.Margin = new Padding(3, 4, 3, 4);
			LocalViewCheckBox.Name = "LocalViewCheckBox";
			LocalViewCheckBox.Size = new Size(164, 29);
			LocalViewCheckBox.TabIndex = 39;
			LocalViewCheckBox.Text = "鏡面光視点有効";
			LocalViewCheckBox.UseVisualStyleBackColor = true;
			LocalViewCheckBox.CheckedChanged += LocalViewCheckBox_CheckedChanged;
			// 
			// label15
			// 
			label15.AutoSize = true;
			label15.Location = new Point(14, 252);
			label15.Name = "label15";
			label15.Size = new Size(84, 25);
			label15.TabIndex = 38;
			label15.Text = "標高倍率";
			// 
			// EvScaleTrackBar
			// 
			EvScaleTrackBar.Location = new Point(104, 252);
			EvScaleTrackBar.Margin = new Padding(3, 4, 3, 4);
			EvScaleTrackBar.Maximum = 5;
			EvScaleTrackBar.Name = "EvScaleTrackBar";
			EvScaleTrackBar.Size = new Size(353, 69);
			EvScaleTrackBar.TabIndex = 37;
			EvScaleTrackBar.Value = 1;
			EvScaleTrackBar.Scroll += EvScaleTrackBar_Scroll;
			// 
			// label16
			// 
			label16.AutoSize = true;
			label16.Location = new Point(32, 175);
			label16.Name = "label16";
			label16.Size = new Size(66, 25);
			label16.TabIndex = 36;
			label16.Text = "鏡面光";
			// 
			// SpecTrackBar
			// 
			SpecTrackBar.LargeChange = 10;
			SpecTrackBar.Location = new Point(104, 175);
			SpecTrackBar.Margin = new Padding(3, 4, 3, 4);
			SpecTrackBar.Maximum = 100;
			SpecTrackBar.Name = "SpecTrackBar";
			SpecTrackBar.Size = new Size(353, 69);
			SpecTrackBar.TabIndex = 35;
			SpecTrackBar.TickFrequency = 5;
			SpecTrackBar.Value = 100;
			SpecTrackBar.Scroll += SpecTrackBar_Scroll;
			// 
			// label17
			// 
			label17.AutoSize = true;
			label17.Location = new Point(32, 98);
			label17.Name = "label17";
			label17.Size = new Size(66, 25);
			label17.TabIndex = 34;
			label17.Text = "環境光";
			// 
			// AmbTrackBar
			// 
			AmbTrackBar.Location = new Point(104, 98);
			AmbTrackBar.Margin = new Padding(3, 4, 3, 4);
			AmbTrackBar.Maximum = 100;
			AmbTrackBar.Name = "AmbTrackBar";
			AmbTrackBar.Size = new Size(353, 69);
			AmbTrackBar.TabIndex = 32;
			AmbTrackBar.TickFrequency = 5;
			AmbTrackBar.Value = 80;
			AmbTrackBar.Scroll += AmbTrackBar_Scroll;
			// 
			// label18
			// 
			label18.AutoSize = true;
			label18.Location = new Point(22, 23);
			label18.Name = "label18";
			label18.Size = new Size(76, 25);
			label18.TabIndex = 31;
			label18.Text = "ハイライト";
			// 
			// ShinTrackBar
			// 
			ShinTrackBar.LargeChange = 10;
			ShinTrackBar.Location = new Point(104, 21);
			ShinTrackBar.Margin = new Padding(3, 4, 3, 4);
			ShinTrackBar.Maximum = 128;
			ShinTrackBar.Name = "ShinTrackBar";
			ShinTrackBar.Size = new Size(353, 69);
			ShinTrackBar.TabIndex = 30;
			ShinTrackBar.TickFrequency = 5;
			ShinTrackBar.Value = 128;
			ShinTrackBar.Scroll += ShinTrackBar_Scroll;
			// 
			// GeoViewerMainForm
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = SystemColors.Control;
			ClientSize = new Size(936, 530);
			Controls.Add(tabControl1);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(3, 6, 3, 6);
			Name = "GeoViewerMainForm";
			Text = "GeoViewerMainForm";
			Load += Form1_Load;
			tabControl1.ResumeLayout(false);
			tabPage1.ResumeLayout(false);
			tabPage1.PerformLayout();
			tabPage2.ResumeLayout(false);
			groupBox2.ResumeLayout(false);
			groupBox2.PerformLayout();
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			tabPage3.ResumeLayout(false);
			tabPage3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)ZNearTrackBar).EndInit();
			((System.ComponentModel.ISupportInitialize)FovyTrackBar).EndInit();
			((System.ComponentModel.ISupportInitialize)VisibilityNumericUpDown).EndInit();
			((System.ComponentModel.ISupportInitialize)EvScaleTrackBar).EndInit();
			((System.ComponentModel.ISupportInitialize)SpecTrackBar).EndInit();
			((System.ComponentModel.ISupportInitialize)AmbTrackBar).EndInit();
			((System.ComponentModel.ISupportInitialize)ShinTrackBar).EndInit();
			ResumeLayout(false);
		}

		#endregion
		private System.Windows.Forms.TextBox DialogTextBox;
		private TabControl tabControl1;
		private TabPage tabPage1;
		private TabPage tabPage2;
		private TabPage tabPage3;
		private GroupBox groupBox2;
		protected Label ObserverYLabel;
		private Button CamAlt0Button;
		internal TextBox CamAltTextBox;
		internal Label MinCamYLabel;
		internal Label MaxCamYLabel;
		internal Label MaxCamXLabel;
		internal Label MinCamXLabel;
		protected Label CamYScrollBarLabel;
		internal VScrollBar CamYScrollBar;
		protected Label CamXScrollBarLabel;
		internal HScrollBar CamXScrollBar;
		internal VScrollBar CamAltScrollBar;
		private Label label14;
		protected Label ObserverXLabel;
		internal TextBox CamYTextBox;
		internal TextBox CamXTextBox;
		private GroupBox groupBox1;
		private Button ObjAlt0Button;
		internal TextBox ObjAltTextBox;
		internal VScrollBar ObjAltScrollBar;
		private Label label1;
		protected Label ObjYLabel;
		internal TextBox AngleTextBox;
		internal Label MinObjYLabel;
		internal Label MaxObjYLabel;
		internal Label MaxObjXLabel;
		internal Label MinObjXLabel;
		protected Label ObjYScrollBarLabel;
		internal VScrollBar ObjYScrollBar;
		protected Label ObjXScrollBarLabel;
		internal HScrollBar ObjXScrollBar;
		internal VScrollBar AngleScrollBar;
		private Label label4;
		internal HScrollBar DirScrollBar;
		private Label label3;
		internal TextBox DirTextBox;
		internal HScrollBar DistScrollBar;
		private Label label2;
		internal TextBox DistTextBox;
		protected Label ObjXLabel;
		internal TextBox ObjYTextBox;
		internal TextBox ObjXTextBox;
		private Label label13;
		private ComboBox MeshModeComboBox;
		private NumericUpDown VisibilityNumericUpDown;
		private ComboBox FogModeComboBox;
		private Label label12;
		private Label label5;
		private ComboBox ShadingModeComboBox;
		private Label label11;
		private Label label10;
		private Label label9;
		private Label label8;
		private Label label7;
		private Label label6;
		private CheckBox LocalViewCheckBox;
		private Label label15;
		private TrackBar EvScaleTrackBar;
		private Label label16;
		private TrackBar SpecTrackBar;
		private Label label17;
		private TrackBar AmbTrackBar;
		private Label label18;
		private TrackBar ShinTrackBar;
		private Label label19;
		private ComboBox OriginComboBox;
		private ComboBox MarkerTypeComboBox;
		public CheckBox MarkerCheckBox;
		private Label label20;
		private TrackBar FovyTrackBar;
		public Label FovyLabel;
		private Label label22;
		private TrackBar ZNearTrackBar;
		private Label NearPlaneLabel;
		private Label label21;
		internal CheckedListBox ObjInfoListBox;
	}
}

