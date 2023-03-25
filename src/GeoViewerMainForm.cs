//
// PlanViewerMainForm.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Scene;
using DSF_NET_Profiler;
using DSF_NET_Utility;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	// ◆ローカルで良いものも多いが、現状でLgLt、WP及びTileにわかれているので、共通部分を別にまとめておく。

	// ◆関係フォームの依存関係(作成順)のためコンストラクタで指定できないのでreadonlyやprivateにできない。
	public CGeoViewer Viewer = null;

	readonly string CfgFileName;

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
	
	readonly CInfoMap  Info		= new ();
	readonly CProfiler Profiler	= new ();

	readonly List<string> cmd_history = new ();

	int curr_cmd_history_pos;

	public GeoViewerMainForm(string[] args)
	{
		InitializeComponent();

		CfgFileName = args[0];
	}

	[SupportedOSPlatform("windows")] // Windows固有API(Graphics)が使用されていることを宣言する。
	private void Form1_Load(object sender, EventArgs e)
	{
		// ◆Form_Loadだと、このフォームは最後まで表示されない
		// 　非同期にできないか？すべきか？

		try
		{
			//--------------------------------------------------
			// 1 設定

			//--------------------------------------------------
			// ◆設定をハードコードする場合
			{/*
				var title = "糸島半島";
				var s_lglt = new CLgLt(new CLg(130.1), new CLt(33.5));
				var e_lglt = new CLgLt(new CLg(130.3), new CLt(33.7));
				var polygon_size = 400; // ◆1000にすると玄界島の右下に穴が開く。
			*/}

			//--------------------------------------------------
			// 設定を設定ファイルで与える場合
			// 座標範囲s_lglt～e_lgltに含まれるタイルを連結表示する。
			// ●１個タイルの頂点数は256x256なので、PolygonZoomLevelでポリゴンサイズが決まる。

			ReadCfgFromFile(CfgFileName);

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

			// 高度クラスにジオイドデータを設定することにより、座標オブジェクトにジオイド高が自動設定される。
			CAltitude.SetGeoidMapData(geoid_map_data);

			//--------------------------------------------------
			// 5 表示設定を作成する。

			var scene_cfg = new CSceneCfg 
				(0.6f, // 環境光反射係数 [0,1]
				 0.5f, // 鏡面反射係数   [0,1]
				 64,   // ハイライト     [0,128]
				 DShadingMode.SHADING_MAPPING,
				 DFogMode.FOG_NO,
				 3000f); // 視程(m)

			//--------------------------------------------------
			// 6 モードを選択する。
			// ◆開発途上のプロトタイプ的な機能

			// ◆どれか一つ選んで実行する。
			var mode =
				"WP";
				//"Tile";
				//"LgLt";

			//--------------------------------------------------
			// 7 ビューアフォームを作成する。

			// ◆viewer_form.Viewerはnullであり、後で設定する。
			GeoViewerForm viewer_form = 
				(mode == "WP"  )? new GeoViewerForm_WP  (){ Text = Title, Visible = true }:
				(mode == "Tile")? new GeoViewerForm_Tile(){ Text = Title, Visible = true }:
				(mode == "LgLt")? new GeoViewerForm_LgLt(){ Text = Title, Visible = true }: null;

			//--------------------------------------------------
			// 8 ビューアを作成する。

			// ◆3項演算子ではvarは使えない。
			CGeoViewer viewer = 
				(mode == "WP"  )? CreateGeoViewer_WP  (viewer_form.PictureBox, geoid_map_data, scene_cfg, controller_parts):
				(mode == "Tile")? CreateGeoViewer_Tile(viewer_form.PictureBox, geoid_map_data, scene_cfg, controller_parts):
				(mode == "LgLt")? CreateGeoViewer_LgLt(viewer_form.PictureBox, geoid_map_data, scene_cfg, controller_parts): null;

			//--------------------------------------------------
			// 9 ビューアフォーム、コントローラフォーム及びメインフォームにビューアを設定する。

			viewer_form	   .Viewer =
			controller_form.Viewer =
							Viewer = viewer;

			//--------------------------------------------------
			// 10 表示設定フォームを作成する。

		/*	var cfg_form =*/ new GeoViewerCfgForm(Viewer){ Text = Title, Visible = true };

			//--------------------------------------------------
			// 11.1 図形を描画する。

			DrawShapes();

			//--------------------------------------------------
			// 11.2 図形をXMLファイルから読み込み表示する。
			// ◆StickerLineはWP単位なのでGeoViewer_LgLtでは正しく表示されない。

			// XMLから読めない場合はString型でも""ではなくnullが入る。
			if(DrawingFileName != null) DrawShapesXML();

			//--------------------------------------------------
			// 12 オーバレイを描画する。

			// ◆ダウンキャストはしたくない。viewerを別々にできないので、仮想関数で作れ。
			// 　→そもそもWPとLgLtで別になるものか？
			switch(mode)
			{
				case "WP"  :
				case "Tile": DrawOverlayGeoViewer_WP  ((CGeoViewer_WP  )viewer); break;
				case "LgLt": DrawOverlayGeoViewer_LgLt((CGeoViewer_LgLt)viewer); break;
			}
		
			//--------------------------------------------------

			ShowLog();
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

			var cmd_line = tb.Lines[^2].Substring(1);

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
