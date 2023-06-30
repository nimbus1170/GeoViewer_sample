//
// PlanViewerMainForm.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Geometry;
using DSF_NET_Scene;
using DSF_NET_Profiler;
using DSF_NET_Utility;

using System.Runtime.Versioning;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	// ◆ローカルで良いものも多いが、現状でLgLt、WP及びTileにわかれているので、共通部分を別にまとめておく。

	// ◆関係フォームの依存関係(作成順)のためコンストラクタで指定できないのでreadonlyやprivateにできない。
	public CGeoViewer Viewer = null;

//	readonly string CfgFileName = "";

	// ◆GeoViewerMainFormに開始・終了座標等のGeoViewerの特性を持たせるべきか？それぞれのGeoViewerに持たせるべきでは？

	// クランプ等された開始・終了経緯度
	// ◆オーバレイでも使用している。
	CLgLt StartLgLt	= null;
	CLgLt EndLgLt	= null;

	// クランプされた開始・終了WP座標
	// ◆オーバレイでも使用している。
	// ◆GeoViewerMainFormではなくGeoViewer_WPが持つべきでは？
	CWPInt StartWP = null;
	CWPInt EndWP   = null;

	// ◆オーバレイでも使用している。
	// 　現状ではオーバレイのピクセルの基準を決めるために地表面プレーンの縦横比のみを参照しているが、今後、画像を使用する可能性もあるため、保持しておく。
	// ◆CImageMapDataで保持したいが、WPとLgLtで別なので、GeoViewerForm=WPとかで保持するか。
	// 　→WPはLgLtを保持(継承)しても良いのでは？
	Bitmap MapImage = null;
	Bitmap MapPhoto = null;
	
	readonly CLog		Log		  = new ();
	readonly CStopWatch	StopWatch = new ();
	readonly CMemWatch	MemWatch  = new ();

	readonly List<string> cmd_history = new ();

	int curr_cmd_history_pos;

	public GeoViewerMainForm(string[] args)
	{
		InitializeComponent();

		try
		{
			//--------------------------------------------------
			// ◆テスト

/*			var ellipsoid = Convert_LgLt_XY.Ellipsoid;

			Convert_LgLt_XY.Origin = Convert_LgLt_XY.XYOrigin_XII;

			var origin = Convert_LgLt_XY.Origin;

			// ◆ここで例外が出る。
			var lglt = Convert_LgLt_XY.ToLgLt(new CCoord(1000.0, 2000.0));

			var lg_dms = new CDMS(lglt.Lg.DecimalDeg);
			var lt_dms = new CDMS(lglt.Lt.DecimalDeg);

			Convert_LgLt_XY.Origin = Convert_LgLt_XY.XYOrigin_IX;

			var xy = Convert_LgLt_XY.ToXY(new CLgLt(new CLg(140.08), new CLt(36.10)));
*/
			//--------------------------------------------------

			// argsがここで渡されるので、これらの処理はForm_Loadではなくここで実施する。

			switch(args.Length)
			{ 
				case 0:
					ReadCfgFromFile("GeoViewerCfg.xml");
					break;

				case 1:
					// ◆ファイル名とみなす。
					var fname = args[0];

					switch(Path.GetExtension(fname))
					{
						case ".xml":
							ReadCfgFromFile(fname);
							break;

/*						case ".las":
							// ◆他フォルダのlasファイルをドラッグ・ドロップするとカレントディレクトリがそこに移ってしまうので、GeoViewerCfg.xmlやgsiフォルダが読めない。バッチファイルとかでもできない。
							ReadCfgFromFile("GeoViewerCfg.xml");
							ReadLASFromFile(fname);
							break;
*/
						default:
							throw new Exception("unknown file ext (" + fname +")");
					}

					break;

				case 2:

					switch(args[0])
					{
						case "-cfgparam":
							// ◆フォルダ等の設定があるので一旦読み込んで必要個所を書き換える。
							ReadCfgFromFile("GeoViewerCfg.xml");
							ReadCfgFromParam(args[1]);
							break;

						default:
							throw new Exception("unknown parameter");
					}

					break;

				default:
					throw new Exception("unknown parameter");
			}
		}
		catch(System.Xml.XPath.XPathException ex)
		{
			DialogTextBox.AppendText("XPath Error : " + ex.Message + "\r\n");
		}
		catch(NullReferenceException ex)
		{
			DialogTextBox.AppendText("Error : " + ex.Message + "\r\n");
		}
		catch(Exception ex)
		{
		//	DialogTextBox.AppendText("Error : " + ex.StackTrace + "\r\n");
			DialogTextBox.AppendText("Error : " + ex.Message + "\r\n");
		}
	}

	[SupportedOSPlatform("windows")]
	private void Form1_Load(object sender, EventArgs e)
	{
		// ◆Form_Loadだと、このフォームは最後まで表示されない
		// 　非同期にできないか？すべきか？

		try
		{
			//--------------------------------------------------
			// 1 設定

			// ◆設定をハードコードする場合
			{/*
				var title = "糸島半島";
				var s_lglt = new CLgLt(new CLg(130.1), new CLt(33.5));
				var e_lglt = new CLgLt(new CLg(130.3), new CLt(33.7));
				var polygon_size = 400; // ◆1000にすると玄界島の右下に穴が開く。
			*/}

			// 設定を設定ファイルで与える場合
			// 本フォームのコンストラクタで実施

			//--------------------------------------------------
			// 2 タイトルを設定する。← 1

			Text = Title;

			//--------------------------------------------------
			// 3.1 コントローラフォームを作成する。← 1

			// ◆controller_form.Viewerはnullであり、後で設定する。
			// ◆nullを渡しているのは無意味
			var controller_form = new GeoViewerControllerForm(null){ Text = Title, Visible = true };

			//--------------------------------------------------
			// 3.2 コントローラパーツを作成する。← 3.1

			var controller_parts = new CControllerParts()
				{
					ObjXTextBox   = controller_form.ObjXTextBox,
					ObjXScrollBar = controller_form.ObjXScrollBar,
					MaxObjXLabel  = controller_form.MaxObjXLabel,
					MinObjXLabel  = controller_form.MinObjXLabel,

					ObjYTextBox   = controller_form.ObjYTextBox,
					ObjYScrollBar = controller_form.ObjYScrollBar,
					MaxObjYLabel  = controller_form.MaxObjYLabel,
					MinObjYLabel  = controller_form.MinObjYLabel,

					DirScrollBar = controller_form.DirScrollBar,
					DirTextBox   = controller_form.DirTextBox,

					DistanceScrollBar = controller_form.DistanceScrollBar,
					DistanceTextBox   = controller_form.DistanceTextBox,

					AngleScrollBar = controller_form.AngleScrollBar,
					AngleTextBox   = controller_form.AngleTextBox,

					ObserverXScrollBar = controller_form.ObserverXScrollBar,
					ObserverXTextBox   = controller_form.ObserverXTextBox,
					MaxObserverXLabel  = controller_form.MaxObserverXLabel,
					MinObserverXLabel  = controller_form.MinObserverXLabel,

					ObserverYScrollBar = controller_form.ObserverYScrollBar,
					ObserverYTextBox   = controller_form.ObserverYTextBox,
					MaxObserverYLabel  = controller_form.MaxObserverYLabel,
					MinObserverYLabel  = controller_form.MinObserverYLabel,

					ObserverAltitudeScrollBar = controller_form.ObserverAltitudeScrollBar,
					ObserverAltitudeTextBox   = controller_form.ObserverAltitudeTextBox
				};

			//--------------------------------------------------
			// 4 ジオイドデータを作成する。← 1

			// ◆例外ではなくジオイドを無視するようにしろ。
		//	if(!(File.Exists(gsi_geoid_model_file))) throw new Exception("geoid model file not found");

			var geoid_map_data = new CGSIGeoidMapData(GSIGeoidModelFile);
//			var geoid_map_data = new CGeoidMapData_Dummy(30);

			// 高度クラスにジオイドデータを設定することにより、座標オブジェクトにジオイド高が自動設定される。
			CAltitude.SetGeoidMapData(geoid_map_data);

			//--------------------------------------------------
			// 5 表示設定を作成する。

			var scene_cfg = new CSceneCfg 
				(0.6f, // 環境光反射係数 [0,1]
				 0.5f, // 鏡面反射係数   [0,1]
				 64,   // ハイライト     [0,128]
				 DShadingMode.TEXTURE_IMAGE,
				 DPolygonMode.QUAD4,
				 DFogMode.CLEAR,
				 3000f); // 視程(m)

			//--------------------------------------------------
			// 6 ビューアフォームを作成する。

			GeoViewerForm viewer_form = 
				(PlaneMode == "WP"  )? new GeoViewerForm_WP  (){ Text = Title, Visible = true }:
				(PlaneMode == "Tile")? new GeoViewerForm_Tile(){ Text = Title, Visible = true }:
				(PlaneMode == "LgLt")? new GeoViewerForm_LgLt(){ Text = Title, Visible = true }:
									   throw new Exception("unknown plane mode");

			// ●https://maps.gsi.go.jp/development/ichiran.html
			viewer_form.MapSrcLabel.Text = 
				"国土地理院 " +
					((ImageZoomLevel <=  4)? "-": // 地図画像
					 (ImageZoomLevel <=  8)? "小縮尺地図(500万分1)":
					 (ImageZoomLevel <= 11)? "小縮尺地図(100万分1)":
					 (ImageZoomLevel <= 18)? "電子国土基本図":
											 "-") + "及び" +
					((ImageZoomLevel <=  1)? "-": // 衛星画像
					 (ImageZoomLevel <=  8)? "世界衛星モザイク画像":
					 (ImageZoomLevel <= 13)? "全国ランドサットモザイク画像":
					 (ImageZoomLevel <= 18)? "全国最新写真":
											 "-") + $"(ZL{ImageZoomLevel})・" +
				"標高タイル(DEM10B-PNG形式)(ZL14)・" + // ◆標高タイルはZL14であり、ポリゴンサイズではない。
				"日本のジオイド2011(Ver.2.1)";

			//--------------------------------------------------
			// 7 ビューアを作成する。

MemWatch.Lap("before CreateGeoViewer");

			CGeoViewer viewer = 
				(PlaneMode == "WP"  )? CreateGeoViewer_WP  (viewer_form.PictureBox, geoid_map_data, scene_cfg, controller_parts):
				(PlaneMode == "Tile")? CreateGeoViewer_Tile(viewer_form.PictureBox, geoid_map_data, scene_cfg, controller_parts):
				(PlaneMode == "LgLt")? CreateGeoViewer_LgLt(viewer_form.PictureBox, geoid_map_data, scene_cfg, controller_parts):
									   throw new Exception("unknown plane mode");

MemWatch.Lap("after  CreateGeoViewer");

			//--------------------------------------------------
			// 8 ビューアフォーム、コントローラフォーム及びメインフォームにビューアを設定する。

			viewer_form	   .Viewer =
			controller_form.Viewer =
							Viewer = viewer;

			//--------------------------------------------------
			// 9 表示設定フォームを作成する。

		/*	var cfg_form =*/ new GeoViewerCfgForm(Viewer){ Text = Title, Visible = true };

			//--------------------------------------------------
			// 10.1 図形を描画する。

			DrawShapes();

			//--------------------------------------------------
			// 10.2 図形をXMLファイルから読み込み表示する。
			// ◆StickerLineはWP単位なのでGeoViewer_LgLtでは正しく表示されない。

			// XMLから読めない場合はString型でも""ではなくnullが入る。
			if(DrawingFileName != null) DrawShapesXML();

			//--------------------------------------------------
			// 11 オーバレイを描画する。

			// ◆ダウンキャストはしたくない。viewerを別々にできないので、仮想関数で作れ。
			// 　→そもそもWPとLgLtで別になるものか？
			switch(PlaneMode)
			{
				case "WP"  :
				case "Tile": DrawOverlayOnGeoViewer_WP  ((CGeoViewer_WP  )viewer); break;
				case "LgLt": DrawOverlayOnGeoViewer_LgLt((CGeoViewer_LgLt)viewer); break;
			}

			//--------------------------------------------------
			// 12 点群(LASデータ)を表示する。

			if(LASzipData != null)
			{
MemWatch.Lap("before DrawLAS");
				DrawLAS("las" + (++LAS_n), LASzipData);
MemWatch.Lap("after  DrawLASr");
			}

			//--------------------------------------------------
			// 13 シーンを(改めて)表示する。
			// ◆ビューアを前面に表示したいが、できない。

			ShowLog();

			viewer_form.Activate();
//			viewer_form.TopMost = true;
//			viewer_form.TopMost = false;
		}
		catch(System.Xml.XPath.XPathException ex)
		{
			DialogTextBox.AppendText("XPath Error : " + ex.Message + "\r\n");
		}
		catch(NullReferenceException ex)
		{
			DialogTextBox.AppendText("Error : " + ex.Message + "\r\n");
		}
		catch(Exception ex)
		{
		//	DialogTextBox.AppendText("Error : " + ex.StackTrace + "\r\n");
			DialogTextBox.AppendText("Error : " + ex.Message + "\r\n");
		}
	}

	private void InputTextBox_KeyPress(Object sender, KeyPressEventArgs e)
	{
		// プロンプト(>)の後で改行しないよう、Enterを打ち消してプロンプトに置き換える。
		// これができるのはKeyPressイベントだけ。
		if(e.KeyChar == '\r') // Enter
		{
			var tb = sender as TextBox;

			tb.AppendText("\r\n");

			var cmd_line = tb.Lines[^2][1..];

			if(cmd_line != "")
			{
				if((cmd_history.Count == 0) || (cmd_history[^1] != cmd_line))
					cmd_history.Add(cmd_line);

				curr_cmd_history_pos = cmd_history.Count - 1;
						
				var ret = ParseCommand(cmd_line);

				// ◆複数行の文字列が返ることもあるので改行も含めておく。
				if(ret != "") tb.AppendText(ret);
			}

			// ◆プロンプト(>)の後で改行しないようEnterを打ち消してプロンプトに置き換える。
			e.KeyChar = '>';
		}
	}

	private void InputTextBox_KeyDown(Object sender, KeyEventArgs e)
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

	private void InputTextBox_MouseDown(Object sender, MouseEventArgs e)
	{
		// マウスクリックでカーソルがテキストの末尾以外のところに行かないようにする。
		((TextBox)sender).AppendText("\0");
	}

	private void InputTextBox_MouseMove(Object sender, MouseEventArgs e)
	{
		// マウスによる範囲選択でカーソルがテキストの末尾以外のところに行かないようにする。範囲選択も解除される。
		if(e.Button == MouseButtons.Left) ((TextBox)sender).AppendText("\0");
	}
}
//---------------------------------------------------------------------------
}
