//
// GeoViewerMainForm.cs
// メインフォーム(ダイアログ)
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Scene;
using DSF_NET_Profiler;
using DSF_NET_Utility;

using static DSF_NET_Geography.Convert_LgLt_GeoCentricCoord;
using static DSF_NET_Geography.DAltitudeBase;

using static DSF_NET_Geography.Convert_LgLt_WP;
using static DSF_NET_Geography.Convert_LgLt_WPInt;
using static DSF_NET_Geography.Convert_LgLt_XY;


using static DSF_NET_Scene.CGLObject;

using static System.Convert;
using static System.Math;
using DSF_NET_Geometry;
using System.Xml.Linq;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
	// ◆デザイナからイベントが追加されると自動フォーマットされるようなので、規定どおり名前空間でインデントしておく。
	public partial class GeoViewerMainForm : Form
	{
		// ◆ローカルで良いものも多いが、現状でLgLt、WP及びTileに分かれているので、共通部分を別にまとめておく。

		// ◆関係フォームの依存関係(作成順)のためコンストラクタで指定できないのでreadonlyやprivateにできない。
		CGeoViewer Viewer = null;

		//	readonly string CfgFileName = "";

		// ◆GeoViewerMainFormに開始・終了座標等のGeoViewerの特性を持たせるべきか？それぞれのGeoViewerに持たせるべきでは？

		// クランプ等された開始・終了経緯度
		// ◆オーバレイでも使用している。
		CLgLt StartLgLt = null;
		CLgLt EndLgLt = null;

		// クランプされた開始・終了WP座標
		// ◆オーバレイでも使用している。
		// ◆GeoViewerMainFormではなくGeoViewer_WPが持つべきでは？
		CWPInt StartWP = null;
		CWPInt EndWP = null;

		// ◆オーバレイでも使用している。
		// 　現状ではオーバレイのピクセルの基準を決めるために地表面プレーンの縦横比のみを参照しているが、今後、画像を使用する可能性もあるため、保持しておく。
		// ◆CImageMapDataで保持したいが、WPとLgLtで別なので、GeoViewerForm=WPとかで保持するか。
		// 　→WPはLgLtを保持(継承)しても良いのでは？
		Bitmap MapImage = null;
		Bitmap MapPhoto = null;

		GeoViewerViewForm ViewerForm = null;

		readonly CLog Log = new();
		readonly CStopWatch StopWatch = new();
		readonly CMemWatch MemWatch = new();

		readonly List<string> cmd_history = [];

		int curr_cmd_history_pos;

		//---------------------------------------------------------------------------
		// ◆過渡的・実験的

		readonly bool ToUseShader = true;

		readonly bool IsLASDataRaw = true;

		//---------------------------------------------------------------------------

		public GeoViewerMainForm(string[] args)
		{
			InitializeComponent();

			try
			{
				//--------------------------------------------------
				// ◆設定読み込み時にToXY/ToLgLtするので、その前に設定する必要がある。
				// →◆分かりにくいので整理しろ。
				// ◆固定ではないはず。
				Convert_LgLt_GeoCentricCoord.Ellipsoid =
				Convert_LgLt_UTM			.Ellipsoid =
				Convert_LgLt_XY				.Ellipsoid = CEllipsoid.Ellipsoid_GRS80;

				//--------------------------------------------------
				// ◆テスト
				{
					var lglt1 = new CLgLt(new CLg(140.0001), new CLt(36.0));

//					Convert_LgLt_GeoCentricCoord.Ellipsoid = CEllipsoid.Ellipsoids[0];

					var xyz = Convert_LgLt_GeoCentricCoord.ToGeoCentricCoord(lglt1);

//					Convert_LgLt_GeoCentricCoord.Ellipsoid = CEllipsoid.Ellipsoids[2];

					var lglt2 = Convert_LgLt_GeoCentricCoord.ToLgLt(xyz);
					// ◆おかしい。lt1とlt2が同じ

					/*	var ellipsoid = Convert_LgLt_XY.Ellipsoid;

						Convert_LgLt_XY.Origin = Convert_LgLt_XY.XYOrigin_XII;

						var origin = Convert_LgLt_XY.Origin;

						// ◆ここで例外が出る。
						var lglt = Convert_LgLt_XY.ToLgLt(new CCoord(1000.0, 2000.0));

						var lg_dms = new CDMS(lglt.Lg.DecimalDeg);
						var lt_dms = new CDMS(lglt.Lt.DecimalDeg);

						Convert_LgLt_XY.Origin = Convert_LgLt_XY.XYOrigin_IX;

						var xy = Convert_LgLt_XY.ToXY(new CLgLt(new CLg(140.08), new CLt(36.10)));
					*/
				}

				//--------------------------------------------------
				// コマンドラインパラメータに応じた処理
				// argsがここで渡されるので、Form_Loadではなくここで処理する。

				// ◆フォルダ等の設定があるのでまず読んで、その後で個別の設定ファイルで必要個所を書き換える。
				// ◆argsにはアプリ名は含まれない。
				var app_name = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]);

				ReadCfgFromFile($"{app_name}.cfg.xml");

				switch(args.Length)
				{
					case 0:
						break;

					case 1:
						// 個別のファイルを読む。
						// それぞれの設定ファイルでデフォルト設定を上書きする。
						// ◆分かりにくいので整理しろ。

						// ◆ファイル名とみなす。
						var fname = args[0];

						switch(Path.GetExtension(fname))
						{
							case ".las":
							case ".laz":
							case ".csv":
							case ".txt":

								SetLgLtAreaFromLASFile(fname);

								var las = new CLAS(Cfg).ReadLASFile(fname, (float)Cfg.PointSize, IsLASDataRaw, (Cfg.AltitudeBase == "AGL")? AGL: AMSL);

								LASs.Add(las);

								Title = las.Cfg.LASFile;

								// Default.cfg.xmlを読んで更新されている。
								MeshZoomLevel  = las.Cfg.MeshZoomLevel;
								ImageZoomLevel = las.Cfg.ImageZoomLevel;
								DEMZoomLevel   = las.Cfg.DEMZoomLevel;

								break;

							case ".las.cfg.xml": // LASファイル名も設定ファイルで指定する。

								var lasCfg = ReadLASFromCfgFile(fname);

								LASs.Add(lasCfg);

								Title = lasCfg.Cfg.LASFile;

								break;

							case ".cfg.xml":
								ReadCfgFromFile(fname);
								break;

							default:
								throw new Exception("unknown file ext (" + fname + ")");
						}

						break;

					case 2: // ◆DKROBOに渡す用

						switch(args[0])
						{
							case "-cfgparam":
								ReadCfgFromParam(args[1]);
								break;

							default:
								throw new Exception("unknown parameter");
						}

						break;

					default:
						throw new Exception("unknown parameter");
				}

				// ◆この段階で設定されていなければエラーだが、どこまでで判断するのか分かりにくい。
				if((StartLgLt_0 == null) || (EndLgLt_0 == null)) throw new Exception("地図範囲が設定されていません。");
			}
			catch(System.Xml.XPath.XPathException ex)
			{
				ShowLog("XPath Error : " + ex.Message + "\r\n");
			}
			catch(NullReferenceException ex)
			{
				ShowLog("Error : " + ex.Message + "\r\n");
			}
			catch(Exception ex)
			{
				//	ShowLog("Error : " + ex.StackTrace + "\r\n");
				ShowLog("Error : " + ex.Message + "\r\n");
			}
		}

		private void ShowLog(in string log)
		{
			DialogTextBox.AppendText(log);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			// ◆Form_Loadだと、このフォームは最後まで表示されない
			// 　非同期にできないか？すべきか？

			try
			{
				//--------------------------------------------------
				// 1 設定

				/*	// ◆設定をハードコードする場合

					var title = "糸島半島";
					var s_lglt = new CLgLt(new CLg(130.1), new CLt(33.5));
					var e_lglt = new CLgLt(new CLg(130.3), new CLt(33.7));
					var mesh_size = 400; // ◆1000にすると玄界島の右下に穴が開く。
				*/

				// 設定を設定ファイルで与える場合
				// 本フォームのコンストラクタで実施

				//--------------------------------------------------
				// 2 タイトルを設定する。← 1

				Text = Title;

				//--------------------------------------------------
				// 3 コントローラパーツを作成する。

				var controller_parts = new CControllerParts()
				{
					ObjXTextBox	  = this.ObjXTextBox,
					ObjXScrollBar = this.ObjXScrollBar,
					MaxObjXLabel  = this.MaxObjXLabel,
					MinObjXLabel  = this.MinObjXLabel,

					ObjYTextBox   = this.ObjYTextBox,
					ObjYScrollBar = this.ObjYScrollBar,
					MaxObjYLabel  = this.MaxObjYLabel,
					MinObjYLabel  = this.MinObjYLabel,

					ObjAltScrollBar = this.ObjAltScrollBar,
					ObjAltTextBox   = this.ObjAltTextBox,

					DirScrollBar = this.DirScrollBar,
					DirTextBox   = this.DirTextBox,

					DistScrollBar = this.DistScrollBar,
					DistTextBox   = this.DistTextBox,

					AngleScrollBar = this.AngleScrollBar,
					AngleTextBox   = this.AngleTextBox,

					CamXScrollBar = this.CamXScrollBar,
					CamXTextBox   = this.CamXTextBox,
					MaxCamXLabel  = this.MaxCamXLabel,
					MinCamXLabel  = this.MinCamXLabel,

					CamYScrollBar = this.CamYScrollBar,
					CamYTextBox   = this.CamYTextBox,
					MaxCamYLabel  = this.MaxCamYLabel,
					MinCamYLabel  = this.MinCamYLabel,

					CamAltScrollBar = this.CamAltScrollBar,
					CamAltTextBox   = this.CamAltTextBox,
				};

				//--------------------------------------------------
				// 4 ジオイドデータを作成する。← 1

				// ◆例外ではなくジオイドを無視するようにしろ。
				//	if(!(File.Exists(gsi_geoid_model_file))) throw new Exception("geoid model file not found");

				var geoid_map_data = new CGSIGeoidMapData(Cfg.GeoidModelFile);
				//	var geoid_map_data = new CGeoidMapData_Dummy(30);

				// 高度クラスにジオイドデータを設定することにより、座標オブジェクトにジオイド高が自動設定される。
				CAltitude.SetGeoidMapData(geoid_map_data);

				//--------------------------------------------------
				// 5 表示設定を作成する。

				var scene_cfg = new CSceneCfg
					(0.6f, // 環境光反射係数 [0,1]
					 0.5f, // 鏡面反射係数   [0,1]
					 64,   // ハイライト     [0,128]
					 DShadingMode.TEXTURE_IMAGE,
					 DMeshMode.QUAD4,
					 DFogMode.CLEAR,
					 3000f); // 視程(m)

				//--------------------------------------------------
				// 6 ビューアフォームを作成する。

				ViewerForm =
					(Cfg.PlaneMode == "WP"	) ? new GeoViewerForm_WP  () { Text = Title, Visible = true }:
					(Cfg.PlaneMode == "Tile") ? new GeoViewerForm_Tile() { Text = Title, Visible = true }:
					(Cfg.PlaneMode == "LgLt") ? new GeoViewerForm_LgLt() { Text = Title, Visible = true }:
												throw new Exception("unknown plane mode");

				ViewerForm.MainForm = this;

				// ●https://maps.gsi.go.jp/development/ichiran.html
				// ◆地図データの名前等は地図データ内に保持するべき。
				ViewerForm.MapSrcLabel.Text =
					"国土地理院 " +
						((ImageZoomLevel <=  4) ? "-" : // 地図画像
						 (ImageZoomLevel <=  8) ? "小縮尺地図(500万分1)":
						 (ImageZoomLevel <= 11) ? "小縮尺地図(100万分1)":
						 (ImageZoomLevel <= 18) ? "電子国土基本図" :
												  "-") + "及び" +
						((ImageZoomLevel <=  1) ? "-" : // 衛星画像
						 (ImageZoomLevel <=  8) ? "世界衛星モザイク画像":
						 (ImageZoomLevel <= 13) ? "全国ランドサットモザイク画像":
						 (ImageZoomLevel <= 18) ? "全国最新写真":
												  "-") + $"(ZL{ImageZoomLevel})・" +
					"標高タイル(" +
						(Cfg.DEMTileUrl.Contains("dem_png"  )? "DEM10B-PNG":
						 Cfg.DEMTileUrl.Contains("dem5a_png")? "DEM5A-PNG":
																	 "-") + ")" + $"(ZL{DEMZoomLevel})・" +
					geoid_map_data.Name + "(" + geoid_map_data.Version + ")";

				//--------------------------------------------------
				// 7 ビューアを作成する。

MemWatch.Lap("before CreateGeoViewer");

				Viewer =
					(Cfg.PlaneMode == "WP"	)? CreateGeoViewer_WP  (ViewerForm.PictureBox, geoid_map_data, scene_cfg, controller_parts):
					(Cfg.PlaneMode == "Tile")? CreateGeoViewer_Tile(ViewerForm.PictureBox, geoid_map_data, scene_cfg, controller_parts):
					(Cfg.PlaneMode == "LgLt")? CreateGeoViewer_LgLt(ViewerForm.PictureBox, geoid_map_data, scene_cfg, controller_parts):
											   throw new Exception("unknown plane mode");

MemWatch.Lap("after  CreateGeoViewer");

				//--------------------------------------------------
				// 8 ビューアフォームにビューアを設定する。

				ViewerForm.Viewer = Viewer;

				//--------------------------------------------------
				// 9 表示設定等コントロールを設定する。
				{
					ShinTrackBar.Value = ToInt32(Viewer.Shin());
					AmbTrackBar.Value = ToInt32(Viewer.Amb() * 100);
					SpecTrackBar.Value = ToInt32(Viewer.Spec() * 100);

					ShadingModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

					switch(Viewer.ShadingMode())
					{
						case DShadingMode.TEXTURE_IMAGE: ShadingModeComboBox.SelectedIndex = 0; break;
						case DShadingMode.TEXTURE_PHOTO: ShadingModeComboBox.SelectedIndex = 1; break;
						case DShadingMode.SMOOTH: ShadingModeComboBox.SelectedIndex = 2; break;
						case DShadingMode.FLAT: ShadingModeComboBox.SelectedIndex = 3; break;
						case DShadingMode.WIRE: ShadingModeComboBox.SelectedIndex = 4; break;
					}

					MeshModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

					switch(Viewer.MeshMode())
					{
						case DMeshMode.QUAD5: MeshModeComboBox.SelectedIndex = 0; break;
						case DMeshMode.QUAD4: MeshModeComboBox.SelectedIndex = 1; break;
						case DMeshMode.TRIANGLE: MeshModeComboBox.SelectedIndex = 2; break;
					}

					FogModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

					switch(Viewer.FogMode())
					{
						case DFogMode.CLEAR: FogModeComboBox.SelectedIndex = 0; break;
						case DFogMode.FOG: FogModeComboBox.SelectedIndex = 1; break;
						case DFogMode.DARK: FogModeComboBox.SelectedIndex = 2; break;
					}

					VisibilityNumericUpDown.Value = (decimal)(Viewer.Visibility());

					if(Viewer.ToUseShader) LocalViewCheckBox.Enabled = false;

					MarkerTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
					MarkerTypeComboBox.SelectedIndex = 0;

					FovyLabel.Text = FovyTrackBar.Value.ToString() + "°";

					ZNearTrackBar.Value = (int)(Cfg.NearPlane);
					NearPlaneLabel.Text = ZNearTrackBar.Value.ToString();

					ToShowInfo[0] = true ; // 経緯度（度分秒）
					ToShowInfo[1] = false; // 経緯度（10進）
					ToShowInfo[2] = true ; // 平面直角座標
					ToShowInfo[3] = false; // UTM
					ToShowInfo[4] = false; // MGRS
					ToShowInfo[5] = true ; // 高度
					ToShowInfo[6] = false; // 地心直交座標
					ToShowInfo[7] = true ; // カメラ→注視点

					for(int i = 0; i < ObjInfoListBox.Items.Count; ++i)
						ObjInfoListBox.SetItemChecked(i, ToShowInfo[i]);

					OriginComboBox.SelectedIndex = Cfg.DefaultOrigin - 1;

					Convert_LgLt_XY.Origin = Convert_LgLt_XY.Origins[Cfg.DefaultOrigin];
				}

				//--------------------------------------------------
				// 10 結果を表示する。
				// ◆この後の処理は非同期で処理するものがあるため、表示できるものはここで表示する。
				{
					// これ以降は非同期で表示することを想定して、プロファイリングは別にする。
					StopWatch.Stop();
					MemWatch.Stop();

					//--------------------------------------------------
					// プレーンサイズを計算する。

					var lglt_00 = StartLgLt;
					var lglt_10 = new CLgLt(EndLgLt.Lg, StartLgLt.Lt);
					var lglt_01 = new CLgLt(StartLgLt.Lg, EndLgLt.Lt);

					var coord_00 = ToGeoCentricCoord(lglt_00);
					var coord_10 = ToGeoCentricCoord(lglt_10);
					var coord_01 = ToGeoCentricCoord(lglt_01);

					var dx_00_10 = coord_10.X - coord_00.X;
					var dy_00_10 = coord_10.Y - coord_00.Y;
					var dz_00_10 = coord_10.Z - coord_00.Z;

					// ◆C#にはHypotがない。
					var plane_size_EW = (int)(Sqrt(dx_00_10 * dx_00_10 + dy_00_10 * dy_00_10 + dz_00_10 * dz_00_10));

					var dx_00_01 = coord_01.X - coord_00.X;
					var dy_00_01 = coord_01.Y - coord_00.Y;
					var dz_00_01 = coord_01.Z - coord_00.Z;

					var plane_size_NS = (int)(Sqrt(dx_00_01 * dx_00_01 + dy_00_01 * dy_00_01 + dz_00_01 * dz_00_01));

					//--------------------------------------------------

					var log = Log.ToDictionary();

					var vert_nx = log["VertexNX"];
					var vert_ny = log["VertexNY"];

					var pt_size_min_max = Viewer.GLPointSizeMinMax();

					ShowLog($"                  頂点数 : {vert_nx:#,0} x {vert_ny:#,0}\r\n");
					ShowLog($"メッシュ(三角ポリゴン)数 : {(vert_nx - 1) * (vert_ny - 1) * 2:#,0}\r\n");
					ShowLog($"        テクスチャサイズ : {log["TexW"]}px x {log["TexH"]}px\r\n");
					ShowLog($"          テクスチャ枚数 : {log["TexN"]}\r\n");
					ShowLog($"          表示地域サイズ : {plane_size_EW:#,0}m x {plane_size_NS:#,0}m\r\n");
					ShowLog($"          メッシュサイズ : {plane_size_EW / (vert_nx - 1)}m x {plane_size_NS / (vert_ny - 1)}m\r\n");
					ShowLog($"        画像ズームレベル : {ImageZoomLevel}\r\n");
					ShowLog($"    メッシュズームレベル : {MeshZoomLevel}\r\n");
					ShowLog($"              OpenGL精度 : {GetGLPrecision()}\r\n");
					ShowLog($"              シェーダー : {(Viewer.ToUseShader ? "カスタム" : "組み込み")}\r\n");
					ShowLog($"  システムポイントサイズ : {pt_size_min_max.Item1}px ～ {pt_size_min_max.Item2}px\r\n");
					ShowLog($"\r\n");

					if(Cfg.ToShowDebugInfo)
					{
						ShowLog($"mesh count from planes\r\n");
						ShowLog($"gnd-sea mesh : {log["gnd_sea_mesh_count"]:#,0}\r\n");
						ShowLog($"texture mesh : {log["texture_mesh_count"]:#,0}\r\n");
						ShowLog($"\r\n");
						ShowLog($"mesh count from GLObjects\r\n");

						var gl_objs_count = Viewer.GLObjectCount();

						foreach(var gl_objs_count_i in gl_objs_count)
							ShowLog($"{gl_objs_count_i.Key,-12} : {gl_objs_count_i.Value:#,0}\r\n");

						ShowLog("\r\n");

						ShowLog(StopWatch.ResultLog + "\r\n");
						ShowLog(MemWatch.ResultLog + "\r\n");
					}
				}

				//--------------------------------------------------
				// 11 残りの処理を非同期処理するため、その前にフォームとコントロールの状態を設定する。

				// BeginInvokeで処理しないとスクロールとかしないのでここでまとめる。
				// 　Form_Loadが終了するまでは、DialogBox等のコントロールへの操作は完了していないため、Focusメソッドが効かない。
				// 　BeginInvokeでコントロールへの処理がキューイングされて描画完了後に処理される。
				BeginInvoke((MethodInvoker)delegate
				{
					// プロンプトまでスクロールする。
					DialogTextBox.Focus();
					DialogTextBox.SelectionStart = DialogTextBox.Text.Length;
					DialogTextBox.ScrollToCaret();

					// ビューアを前面に表示する。
					ViewerForm.Activate();

					Viewer.Draw();
				});

				//--------------------------------------------------
				// 12.1 図形を描画する。

				DrawShapes();

				//--------------------------------------------------
				// 12.2 図形をXMLファイルから読み込み表示する。
				// ◆StickerLineはWP単位なのでGeoViewer_LgLtでは正しく表示されない。

				// XMLから読めない場合はString型でも""ではなくnullが入る。
				if(Cfg.DrawingFileName is not null) DrawShapesXML();

				//--------------------------------------------------
				// 13 オーバレイを描画する。

				// ◆ダウンキャストはしたくない。viewerを別々にできないので、仮想関数で作れ。
				// 　→そもそもWPとLgLtで別になるものか？
				switch(Cfg.PlaneMode)
				{
					case "WP":
					case "Tile": DrawOverlayOnGeoViewer_WP((CGeoViewer_WP)Viewer); break;
					case "LgLt": DrawOverlayOnGeoViewer_LgLt((CGeoViewer_LgLt)Viewer); break;
				}

				//--------------------------------------------------
				// 14 点群を表示する。

			//	if(LASs.Count > 0)
				if(LASFnames.Count > 0)
				{
StopWatch.Lap("before DrawLAS");
MemWatch .Lap("before DrawLAS");

					ViewerForm.AdditionalInfo["LAS"] = "LAS読み込み中";

// ▼こちらを非同期にする。
			//		foreach(var las in LASs)
					foreach(var las_fname in LASFnames)
					{
						var las = new CLAS(Cfg).ReadLASFile(las_fname, (float)Cfg.PointSize, IsLASDataRaw, (Cfg.AltitudeBase == "AGL")? AGL: AMSL);

						//--------------------------------------------------

						DrawLAS("las" + (++ShapesN), las);

						//--------------------------------------------------

						// ◆ファイルを後から指定した場合はCfg.LASFileが空になる。
						// →◆ちょっと入ってこない。何度も設定XMLを読んでいるからか？
					//	ShowLog(Cfg.LASFile + "\r\n");
						ShowLog(las.Log); // ◆LAS.Logには改行が含まれる。
						ShowLog("\r\n");
					}

					StopWatch.Lap("after  DrawLAS");

StopWatch.Lap("after  DrawLAS");
MemWatch .Lap("after  DrawLAS");
				}

				//--------------------------------------------------
				// 15 図形を表示する。

				if(Shapefile is not null)
				{
					var shp_name = "shp" + (++ShapesN);

StopWatch.Lap("before DrawShapefile");
					DrawShapefile(shp_name, Shapefile);
StopWatch.Lap("after  DrawShapefile");

					ShowShapefileLog("shp" + ShapesN, Shapefile, ReadShapefileMsg);
				}

				//--------------------------------------------------
				// プロンプトを表示する。				
				// ◆プロンプトをこのように忘れず表示しなくてはならないのか？
				ShowLog(">");
			}
			catch(System.Xml.XPath.XPathException ex)
			{
				ShowLog("XPath Error : " + ex.Message + "\r\n");
			}
			catch(NullReferenceException ex)
			{
				ShowLog("Error : " + ex.Message + "\r\n");
			}
			catch(Exception ex)
			{
				//	ShowLog("Error : " + ex.StackTrace + "\r\n");
				ShowLog("Error : " + ex.Message + "\r\n");
			}
		}

		private void DialogTextBox_KeyPress(Object sender, KeyPressEventArgs e)
		{
			// プロンプト(>)の後で改行しないよう、Enterを打ち消してプロンプトに置き換える。
			// これができるのはKeyPressイベントだけ。
			if(e.KeyChar == '\r') // Enter
			{
				var tb = sender as TextBox;

				tb.AppendText("\r\n");

				var cmd_line = tb.Lines[^2][1..];

				if(!string.IsNullOrEmpty(cmd_line))
				{
					if((cmd_history.Count == 0) || (cmd_history[^1] != cmd_line))
						cmd_history.Add(cmd_line);

					curr_cmd_history_pos = cmd_history.Count - 1;

					var ret = ParseCommand(cmd_line);

					// ◆複数行の文字列が返ることもあるので改行も含めておく。
					if(!string.IsNullOrEmpty(ret)) tb.AppendText(ret);
				}

				// ◆プロンプト(>)の後で改行しないようEnterを打ち消してプロンプトに置き換える。
				e.KeyChar = '>';
			}
		}

		private void DialogTextBox_KeyDown(Object sender, KeyEventArgs e)
		{
			//--------------------------------------------------

			if(e.KeyCode == Keys.Escape) Application.Exit();

			var tb = sender as TextBox;

			//--------------------------------------------------
			// 矢印キー等でカーソルが上やコマンドプロンプトより左に行かないようにする。
			// ◆KeyPressイベントは矢印キー等では発生しない。

			if(e.KeyCode == Keys.Up) e.SuppressKeyPress = true;

			if(((e.KeyCode == Keys.Left) || (e.KeyCode == Keys.Back)) && (tb.Text[tb.SelectionStart - 1] == '>')) e.SuppressKeyPress = true;

			if(e.KeyCode == Keys.Home)
			{
				e.SuppressKeyPress = true;

				tb.SelectionStart = tb.Text.LastIndexOf('>') + 1;
			}

			//-------------------------------------------------
			// コマンド履歴を入力する。

			if(((e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Down)) && (cmd_history.Count > 0))
			{
				tb.Text = tb.Text.Remove(tb.Text.LastIndexOf('>'));

				tb.AppendText(">" + cmd_history[curr_cmd_history_pos]);

				switch(e.KeyCode)
				{
					case Keys.Up:
						if(curr_cmd_history_pos > 0) curr_cmd_history_pos--;
						break;

					case Keys.Down:
						if(curr_cmd_history_pos < (cmd_history.Count - 1)) curr_cmd_history_pos++;
						break;
				}
			}
		}

		private void DialogTextBox_MouseDown(Object sender, MouseEventArgs e)
		{
			// マウスクリックでカーソルがテキストの末尾以外のところに行かないようにする。
			((TextBox)sender).AppendText("\0");
		}

		private void DialogTextBox_MouseMove(Object sender, MouseEventArgs e)
		{
			// マウスによる範囲選択でカーソルがテキストの末尾以外のところに行かないようにする。範囲選択も解除される。
			if(e.Button == MouseButtons.Left) ((TextBox)sender).AppendText("\0");
		}

		private void ObjXScrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			Viewer?.ObjXScrollBar_Scroll().Draw();
			ViewerForm?.ShowInfo();
		}

		private void ObjYScrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			Viewer?.ObjYScrollBar_Scroll().Draw();
			ViewerForm?.ShowInfo();
		}

		private void DirScrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			Viewer?.DirScrollBar_Scroll().Draw();
			ViewerForm?.ShowInfo();
		}

		private void AngleScrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			Viewer?.AngleScrollBar_Scroll().Draw();
			ViewerForm?.ShowInfo();
		}

		private void DistScrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			Viewer?.DistScrollBar_Scroll().Draw();
			ViewerForm?.ShowInfo();
		}

		private void ObjAltScrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			Viewer?.ObjAltScrollBar_Scroll().Draw();
			ViewerForm?.ShowInfo();
		}

		private void ObjAlt0Button_Click(object sender, EventArgs e)
		{
			Viewer?.ObjAltScrollBar_Scroll(0).Draw();
			ViewerForm?.ShowInfo();
		}

		private void CamXScrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			Viewer?.CamXScrollBar_Scroll().Draw();
			ViewerForm?.ShowInfo();
		}

		private void CamYScrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			Viewer?.CamYScrollBar_Scroll().Draw();
			ViewerForm?.ShowInfo();
		}

		private void CamAltScrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			Viewer?.CamAltScrollBar_Scroll().Draw();
			ViewerForm?.ShowInfo();
		}

		private void CamAlt0Button_Click(object sender, EventArgs e)
		{
			Viewer?.CamAltScrollBar_Scroll(0).Draw();
			ViewerForm?.ShowInfo();
		}

		private void ShinTrackBar_Scroll(object sender, EventArgs e)
		{
			Viewer?.SetShin(ShinTrackBar.Value).Draw();
		}

		private void AmbTrackBar_Scroll(object sender, EventArgs e)
		{
			// [0,1]
			Viewer?.SetAmb(ToSingle(AmbTrackBar.Value) / 100.0f).Draw();
		}

		private void SpecTrackBar_Scroll(object sender, EventArgs e)
		{
			// [0,1]
			Viewer?.SetSpec(ToSingle(SpecTrackBar.Value) / 100.0f).Draw();
		}

		private void EvScaleTrackBar_Scroll(object sender, EventArgs e)
		{
			Viewer?.ScaleEv(EvScaleTrackBar.Value).Draw();
		}

		private void ShadingModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch(ShadingModeComboBox.SelectedIndex)
			{
				case 0:
					Viewer?.SetShadingMode(DShadingMode.TEXTURE_IMAGE);
					MeshModeComboBox.SelectedIndex = 1; // ◆テクスチャは四角メッシュのみ対応
					MeshModeComboBox.Enabled = false;
					break;

				case 1:
					Viewer?.SetShadingMode(DShadingMode.TEXTURE_PHOTO);
					MeshModeComboBox.SelectedIndex = 1; // ◆テクスチャは四角メッシュのみ対応
					MeshModeComboBox.Enabled = false;
					break;

				case 2:
					Viewer?.SetShadingMode(DShadingMode.SMOOTH);
					MeshModeComboBox.Enabled = true;
					break;

				case 3:
					Viewer?.SetShadingMode(DShadingMode.FLAT);

					if(Viewer.ToUseShader)
					{
						MeshModeComboBox.SelectedIndex = 2; // ◆シェーダのフラットモードは三角メッシュのみ対応
						MeshModeComboBox.Enabled = false;
					}
					else
						MeshModeComboBox.Enabled = true;

					break;

				case 4:
					Viewer?.SetShadingMode(DShadingMode.WIRE);
					MeshModeComboBox.Enabled = true;
					break;
			}

			Viewer?.Draw();
		}

		private void MeshModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch(MeshModeComboBox.SelectedIndex)
			{
				case 0: Viewer?.SetMeshMode(DMeshMode.QUAD5); break;
				case 1: Viewer?.SetMeshMode(DMeshMode.QUAD4); break;
				case 2: Viewer?.SetMeshMode(DMeshMode.TRIANGLE); break;
			}

			Viewer?.Draw();
		}

		private void FogModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch(FogModeComboBox.SelectedIndex)
			{
				case 0: Viewer?.SetFogMode(DFogMode.CLEAR); VisibilityNumericUpDown.Enabled = false; break;
				case 1: Viewer?.SetFogMode(DFogMode.FOG); VisibilityNumericUpDown.Enabled = true; break;
				case 2: Viewer?.SetFogMode(DFogMode.DARK); VisibilityNumericUpDown.Enabled = true; break;
			}

			Viewer?.Draw();
		}

		private void VisibilityNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			Viewer?.SetVisibility(ToInt32(VisibilityNumericUpDown.Value)).Draw();
		}

		private void LocalViewCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Viewer?.SetLocalView(LocalViewCheckBox.Checked).Draw();
		}

		internal void MarkerCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			MarkerTypeComboBox.Enabled = MarkerCheckBox.Checked;

			// ビューアフォームに反映する。処理はこちらでやる。	
			ViewerForm.MarkerToolStripMenuItem.Checked = MarkerCheckBox.Checked;

			Viewer?.SetMarkerOn(MarkerCheckBox.Checked).Draw();
		}

		private void MarkerTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			Viewer?.SetMarkerType((DMarkerType)MarkerTypeComboBox.SelectedIndex).Draw();
		}

		private void OriginComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			Convert_LgLt_XY.Origin = Convert_LgLt_XY.Origins[OriginComboBox.SelectedIndex + 1];

			ViewerForm?.ShowInfo();
		}

		private void FovyTrackBar_Scroll(object sender, EventArgs e)
		{
			Viewer?.SetFovy(FovyTrackBar.Value).Draw();

			FovyLabel.Text = FovyTrackBar.Value.ToString() + "°";
		}

		private void ZNearTrackBar_Scroll(object sender, EventArgs e)
		{
			Viewer?.SetZNear(ZNearTrackBar.Value).Draw();

			NearPlaneLabel.Text = ZNearTrackBar.Value.ToString();
		}

		internal readonly bool[] ToShowInfo = new bool[8];

		private void ObjInfoListBox_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			ToShowInfo[e.Index] = (e.NewValue == CheckState.Checked);

			ViewerForm?.ShowInfo();
		}
	}
}
