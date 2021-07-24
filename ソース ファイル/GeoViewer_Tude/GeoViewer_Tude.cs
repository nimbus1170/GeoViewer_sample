//
// GeoViewer_Tude.cs
// 地形ビューア(経緯度単位)
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Geometry;
using DSF_NET_Map;
using DSF_NET_SceneGraph;

using static DSF_NET_TacticalDrawing.ReadXML;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using System.Xml;

using static System.Drawing.Drawing2D.DashStyle;
//---------------------------------------------------------------------------
namespace PlaneViewer_sample
{
//---------------------------------------------------------------------------
public partial class PlaneViewerMainForm : Form
{
	void Run_GeoViewer_Tude()
	{
		//--------------------------------------------------
		// ビューアに表示する地図の開始(南西隅)・終了(北東隅)座標を指す経緯度座標オブジェクトを作成する。
		// ここでは画像データを基準にする。

		//--------------------------------------------------
		// 設定をハードコードする場合
		{ 
/*		var title = "糸島半島";
		var s_tude = new CTude(new CLongitude(130.1), new CLatitude(33.5));
		var e_tude = new CTude(new CLongitude(130.3), new CLatitude(33.7));
		var polygon_size = 400; // ◆1000にすると玄界島の右下に穴が開く。
*/
/*		var title = "糸島半島～佐賀平野";
		var s_tude = new CTude(new CLongitude(130.1), new CLatitude(33.1));
		var e_tude = new CTude(new CLongitude(130.3), new CLatitude(33.7));
		var polygon_size = 800; // ◆1000にすると玄界島の右下に穴が開く。
*/
/*		var title = "可也山";
		var tude_s = new CTude(new CLongitude(130.15), new CLatitude(33.56));
		var tude_e = new CTude(new CLongitude(130.18), new CLatitude(33.58));
		var polygon_size = 10;
*/
/*		var title = "志賀島";
		var tude_s = new CTude(new CLongitude(130.281029), new CLatitude(33.657748));
		var tude_e = new CTude(new CLongitude(130.317340), new CLatitude(33.690176));
		var polygon_size = 20;
*/
/*		var title = "八王子";
		var s_tude = new CTude(new CLongitude(139.23), new CLatitude(35.61));
		var e_tude = new CTude(new CLongitude(139.40), new CLatitude(35.70));
		var polygon_size = 200;
*/
/*		var title = "自宅周辺";
		var tude_s = new CTude(new CLongitude(new CDMS(139, 16, 10.0).ToDecimalDeg()), new CLatitude(new CDMS(35, 39, 30.0).ToDecimalDeg()));
		var tude_e = new CTude(new CLongitude(new CDMS(139, 17, 10.0).ToDecimalDeg()), new CLatitude(new CDMS(35, 40,  0.0).ToDecimalDeg()));
		var polygon_size = 10;
*/
/*		var title = "天竜川(経度帯境界付近)";
		var s_tude = new CTude(new CLongitude(137.9), new CLatitude(34.9));
		var e_tude = new CTude(new CLongitude(138.1), new CLatitude(35.1));
		var polygon_size = 1000;
*/
/*		var title = "熊本北部";
		var s_tude = new CTude(new CLongitude(131.0), new CLatitude(33.0));
		var e_tude = new CTude(new CLongitude(131.3), new CLatitude(33.3));
		var polygon_size = 400;
*/
/*		var title = "富士山";
		var s_tude = new CTude(new CLongitude(138.65), new CLatitude(35.3));
		var e_tude = new CTude(new CLongitude(138.80), new CLatitude(35.4));
		var polygon_size = 50;
*/		}

		//--------------------------------------------------
		// 設定を設定ファイルで与える場合
		// ◆設定未定義については例外が出る。
		// →▼設定誤りに対しては詳細なメッセージを表示するようにしたい。

		var cfg_xml = new XmlDocument();

		cfg_xml.Load(ConfigFileName);

		var map_cfg_xml = cfg_xml.SelectSingleNode("PlaneViewerConfig/MapConfig");

		var title = map_cfg_xml.Attributes["Title"].InnerText;

		var polygon_size = Convert.ToInt32(map_cfg_xml.Attributes["PolygonSize"	].InnerText);

		var img_zoom_level = Convert.ToInt32(map_cfg_xml.Attributes["ImageZoomLevel"].InnerText);

		var s_tude = ReadTude(map_cfg_xml.SelectSingleNode("Start"));
		var e_tude = ReadTude(map_cfg_xml.SelectSingleNode("End"  ));
		
		var map_data_xml = cfg_xml.SelectSingleNode("PlaneViewerConfig/MapData");

		var map_data_fld = map_data_xml.SelectSingleNode("MapDataFolder").Attributes["Folder"].InnerText;

		var gsi_img_tile_fld = map_data_xml.SelectSingleNode("GSIImageTiles").Attributes["Folder"].InnerText;
		var gsi_img_tile_ext = map_data_xml.SelectSingleNode("GSIImageTiles").Attributes["Ext"	 ].InnerText;

		var gsi_ev_tile_fld = map_data_xml.SelectSingleNode("GSIElevationTiles").Attributes["Folder"].InnerText;

		var gsi_geoid_model_file = map_data_xml.SelectSingleNode("GSIGeoidModel").Attributes["File"].InnerText;

		//--------------------------------------------------

		Text = title;

		//--------------------------------------------------
		// 0 タイルをダウンロードする。

		Stopwatch.Lap("download tiles");

		DownloadGSITiles(s_tude, e_tude, img_zoom_level, map_data_fld);

		Stopwatch.Lap("- tiles downloaded");

		//--------------------------------------------------
		// 1 標高地図データを作成する。

		Stopwatch.Lap("build elevation map data");

		// 国土地理院標高タイルから標高地図データを作成する。
		// ◆ズームレベルは取り敢えず14とする。15は抜けが多い。
		var ev_map_data = new CElevationMapData_GSI_DEM_PNG(gsi_ev_tile_fld, 14, s_tude, e_tude);

		Stopwatch.Lap("- elevation map data built");

		// 高度クラスに標高地図データを設定する。
		// これにより、座標オブジェクトに標高が自動設定される。
		CAltitude.SetElevationMapData(ev_map_data);

		//--------------------------------------------------
		// 2 ジオイドデータを作成する。

		Stopwatch.Lap("build geoid map data");

		// ◆例外ではなくジオイドを無視するようにしろ。
		if(!(File.Exists(gsi_geoid_model_file))) throw new Exception("geoid model file not found");

		// 国土地理院ジオイドデータ
		var geoid_map_data = new CGSIGeoidMapData(gsi_geoid_model_file);

		Stopwatch.Lap("- geoid map data built");

		// 高度クラスにジオイドデータを設定する。
		// これにより、座標オブジェクトにジオイド高が自動設定される。
		CAltitude.SetGeoidMapData(geoid_map_data);

		//--------------------------------------------------
		// 3 地図画像データを作成する。

		//--------------------------------------------------
		// 3.1 地図画像を作成する。

		Stopwatch.Lap("build map image");

		var map_img = GSIImageTile.MakeMapImageFromGSITiles(gsi_img_tile_fld, gsi_img_tile_ext, img_zoom_level, s_tude, e_tude);

		Stopwatch.Lap("- map image built");

		//--------------------------------------------------
		// 3.2 経緯度グリッドを描画する。
		{
			Stopwatch.Lap("draw tude grid");

			// グリッドを描画する。
			// ◆キーはグリッド間隔(分)
			// ◆XMLで設定すべきか。
			var tude_grid_elements = new Dictionary<Int32, CMapGridElement>()
				{
					{ 5, new CMapGridElement(new Pen(Color.Black, 2.0f)				      , new Font("ＭＳ ゴシック", 24.0f, GraphicsUnit.Pixel), Brushes.Black)},
					{ 1, new CMapGridElement(new Pen(Color.Black, 2.0f){ DashStyle = Dot }, null												, null		   )}
				};

			var tude_grid_text_elements_in_min = new HashSet<CMapGridTextElement>(new CMapGridTextComparer());

			CTudeGrid.Draw(map_img, s_tude, e_tude, tude_grid_elements, tude_grid_text_elements_in_min);

			var g = Graphics.FromImage(map_img);

			// グリッド文字列を描画する。
			foreach(var grid_text_element in tude_grid_text_elements_in_min)
				g.DrawImage(grid_text_element.gridTextBitmap, (int)(grid_text_element.gridTextCoord.X), (int)(grid_text_element.gridTextCoord.Y));

			g.Dispose();

			Stopwatch.Lap("- tude grid drawn");
		}

		//--------------------------------------------------
		// 3.3 UTMグリッドを描画する。
		{
			Stopwatch.Lap("draw UTM grid");

			// グリッドを描画する。
			// ◆キーはグリッド間隔(km)
			// ◆XMLで設定すべきか。
			var utm_grid_elements = new Dictionary<Int32, CMapGridElement>()
			{
				{ 1, new CMapGridElement(new Pen(Color.Blue, 2.0f), new Font("ＭＳ ゴシック", 24.0f, GraphicsUnit.Pixel), Brushes.Blue)}
			};

			var utm_grid_text_elements_in_min = new HashSet<CMapGridTextElement>(new CMapGridTextComparer());

			CUTMGrid.Draw(map_img, s_tude, e_tude, utm_grid_elements, utm_grid_text_elements_in_min);

			var g = Graphics.FromImage(map_img);

			// グリッド文字列を描画する。
			foreach(var grid_text_element in utm_grid_text_elements_in_min)
				g.DrawImage(grid_text_element.gridTextBitmap, (int)(grid_text_element.gridTextCoord.X), (int)(grid_text_element.gridTextCoord.Y));

			g.Dispose();

			Stopwatch.Lap("- UTM grid drawn");
		}

		//--------------------------------------------------
		// 3.4 地図画像データを作成する。

		Stopwatch.Lap("build image map data");

		var img_map_data = new CImageMapData_Tude(map_img, s_tude, e_tude);

		Stopwatch.Lap("- image map data built");

		//--------------------------------------------------
		// 4 ビューアフォームを作成する。

		Stopwatch.Lap("build viewer form");

		// ◆viewer_form.Viewerはnullであり、後で設定する。
		var viewer_form = new GeoViewerForm_Tude();

		Stopwatch.Lap("- viewer form built");

		viewer_form.Text = title;

		// シーンの作成状況等を表示するため先に表示しておく。
		// ◆シーン作成状況の表示は整理されていないのでシーンの作成状況は実質的に表示できない。
		//  (途中で作成状況の表示(進行)が止まる。)
		viewer_form.Show();

		//--------------------------------------------------
		// 5 ビューアパラメータを作成する。← 1,2,3,4

		var viewer_params = new CGeoViewerParameters_Tude();

		{
			viewer_params.viewer_control = viewer_form.PictureBox;
			viewer_params.s_tude = s_tude;
			viewer_params.e_tude = e_tude;
			viewer_params.polygon_size = polygon_size; // 経緯度でおおよそm単位
			viewer_params.ev_map_data = ev_map_data;
			viewer_params.geoid_map_data = geoid_map_data;
			viewer_params.img_map_data = img_map_data;
			viewer_params.options = "view_tri_polygons"; //"display_progress";
		}

		//--------------------------------------------------
		// 6 コントローラフォームを作成する。

		Stopwatch.Lap("build controller form");

		// ◆controller_form.Viewerはnullであり、後で設定する。
		var controller_form = new GeoViewerControllerForm_Tude(GeoViewer_Tude);

		Stopwatch.Lap("- controller form built");

		controller_form.Text = title;

		controller_form.Show();

		//--------------------------------------------------
		// 7 コントローラパラメータを作成する。← 6

		var controller_parts = new CControllerParts();

		{
			controller_parts.ObjXTextBox   = controller_form.ObjXTextBox;
			controller_parts.ObjXScrollBar = controller_form.ObjXScrollBar;
			controller_parts.MaxObjXLabel  = controller_form.MaxObjXLabel;
			controller_parts.MinObjXLabel  = controller_form.MinObjXLabel;

			controller_parts.ObjYTextBox   = controller_form.ObjYTextBox;
			controller_parts.ObjYScrollBar = controller_form.ObjYScrollBar;
			controller_parts.MaxObjYLabel  = controller_form.MaxObjYLabel;
			controller_parts.MinObjYLabel  = controller_form.MinObjYLabel;

			controller_parts.DirScrollBar = controller_form.DirScrollBar;
			controller_parts.DirTextBox   = controller_form.DirTextBox;

			controller_parts.DistanceScrollBar = controller_form.DistanceScrollBar;
			controller_parts.DistanceTextBox   = controller_form.DistanceTextBox;

			controller_parts.AngleScrollBar = controller_form.AngleScrollBar;
			controller_parts.AngleTextBox	= controller_form.AngleTextBox;

			controller_parts.ObserverXScrollBar = controller_form.ObserverXScrollBar;
			controller_parts.ObserverXTextBox	= controller_form.ObserverXTextBox;
			controller_parts.MaxObserverXLabel  = controller_form.MaxObserverXLabel;
			controller_parts.MinObserverXLabel  = controller_form.MinObserverXLabel;

			controller_parts.ObserverYScrollBar = controller_form.ObserverYScrollBar;
			controller_parts.ObserverYTextBox	= controller_form.ObserverYTextBox;
			controller_parts.MaxObserverYLabel  = controller_form.MaxObserverYLabel;
			controller_parts.MinObserverYLabel  = controller_form.MinObserverYLabel;

			controller_parts.ObserverAltitudeScrollBar = controller_form.ObserverAltitudeScrollBar;
			controller_parts.ObserverAltitudeTextBox   = controller_form.ObserverAltitudeTextBox;
		}

		//--------------------------------------------------
		// 8 表示設定を作成する。
		// ◆表示設定フォームにはビューアを設定するためビューア作成後に作成する。

		var scene_config = new CSceneConfig
			(0.6f, // 環境光反射係数 [0,1]
			 0.5f, // 鏡面反射係数   [0,1]
			 64,   // ハイライト     [0,128]
			 DShadingMode.SHADING_MAPPING,
			 DFogMode.FOG_NO,
			 3000f); // 視程(m)

		//--------------------------------------------------
		// 9 ビューアを作成する。← 5,7,8,9
		// ▼ここの設定順序には依存関係があるので、整理してライブラリに収めろ。ユーザプログラミングに晒すな。
		// →◆ユーザプログラミングでフォーム(コントロール)が作成されるので、その部分は別になるが。

		Stopwatch.Lap("build viewer");

		GeoViewer_Tude = new CGeoViewer_Tude(viewer_params, scene_config, controller_parts, Info, Stopwatch);
			
		Stopwatch.Lap("- viewer built");

		//--------------------------------------------------
		// 10 ビューアフォームとコントローラフォームにビューアを設定する。← 9

		viewer_form	   .Viewer = GeoViewer_Tude;
		controller_form.Viewer = GeoViewer_Tude;

		//--------------------------------------------------
		// 11 表示設定フォームを作成する。← 9
		// ◆コンストラクタで対象ビューアを設定しているためここで作成する必要がある。
		// →◆コントローラフォームとビューアは双方向なのでビューアにコントロールを設定するためにビューアより先に
		// 　　作成する必要があり、そのため、コントローラフォームには後でビューアを設定しなければならない。
		// 　　他方、設定フォームはビューア作成時には不要なので、ビューア作成後に設定フォームを作成し、その際に
		// 　　ビューアを設定すれば良いのでここだけにある。
		// 　　→◆設定フォームの初期値はビューアから取得しており、ビューアへは表示設定パラメータで与えている。
		// 　　　　ビューアへの表示設定は、ビューアを経由して設定フォームに設定されているが、設定フォームからの
		// 　　　　設定に統一すべきではないか？

		var config_form = new GeoViewerConfigForm(GeoViewer_Tude);

		config_form.Text = title;

		config_form.Show();

		//--------------------------------------------------
		// 12 シーンを描画する。← 9

		Stopwatch.Lap("create scene");

		GeoViewer_Tude.CreateScene();

		Stopwatch.Lap("- scene created");

		//--------------------------------------------------
		// 13 図形を描画する。← 9

		Stopwatch.Lap("draw shapes");

		GeoViewerDrawShapes_Tude(GeoViewer_Tude);

		Stopwatch.Lap("- shapes drawn");

		//--------------------------------------------------

		Viewer = GeoViewer_Tude;
	}
}
//---------------------------------------------------------------------------
}
