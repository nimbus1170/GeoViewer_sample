//
// GeoViewer_Tile.cs
// 地形ビューア(タイル単位)
//
// ◆タイル単位でのダウンロードが不完全
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Geometry;
using DSF_NET_Map;
using DSF_NET_SceneGraph;

using static DSF_NET_Geography.Convert_Tude_GeoCentricCoord;
using static DSF_NET_Geography.Convert_Tude_WorldPixel;
using static DSF_NET_Geography.Convert_Tude_WorldPixelInt;
using static DSF_NET_Geography.XMapTile;

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
	void Run_GeoViewer_Tile()
	{
		//--------------------------------------------------
		// ◆設定をハードコードする場合
		{
/*		var title = "糸島半島～福岡市";
		var ct = new CTude(new CLongitude(130.2), new CLatitude(33.6));
		var view_zoom_level = 10;
*/
/*		var title = "八王子";
		var ct = new CTude(new CLongitude(139.30), new CLatitude(35.65));
		var view_zoom_level = 12;
*/
/*		var title = "富士山";
		var ct = new CTude(new CLongitude(138.7), new CLatitude(35.35));
		var view_zoom_level = 12;
*/		}

		//--------------------------------------------------
		// 設定を設定ファイルで与える場合
		// 座標範囲s_tudeとe_tudeを含むタイルを連結表示する。
		// ●１個タイルの頂点数は256x256なので、view_zoom_levelでポリゴンサイズが決まる。
		// ◆設定未定義については例外が出る。
		// →▼設定誤りに対しては詳細なメッセージを表示するようにしたい。

		var cfg_xml = new XmlDocument();

		cfg_xml.Load(ConfigFileName);

		var map_cfg_xml = cfg_xml.SelectSingleNode("PlaneViewerConfig/MapConfig");

		var title = map_cfg_xml.Attributes["Title"].InnerText;

		var polygon_zoom_level = Convert.ToInt32(map_cfg_xml.Attributes["PolygonZoomLevel"].InnerText);
		var img_zoom_level	   = Convert.ToInt32(map_cfg_xml.Attributes["ImageZoomLevel"  ].InnerText);

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

		// ◆ここで緯度方向を逆転させる。
		var wp_sx = ToWorldPixelIntX(polygon_zoom_level, s_tude.Longitude);
		var wp_sy = ToWorldPixelIntY(polygon_zoom_level, e_tude.Latitude );
		var wp_ex = ToWorldPixelIntX(polygon_zoom_level, e_tude.Longitude);
		var wp_ey = ToWorldPixelIntY(polygon_zoom_level, s_tude.Latitude );

		// 表示タイル
		// 標高データ(タイル)や画像データ(タイル)とは(ズームレベルが)関係なく、表示範囲を(タイル単位に)決定するためのもの。
		var view_tile_sx = GetTileX(wp_sx);
		var view_tile_sy = GetTileY(wp_sy);
		var view_tile_ex = GetTileX(wp_ex);
		var view_tile_ey = GetTileY(wp_ey);

		//--------------------------------------------------
		// 1 標高タイルをダウンロードして標高地図データを作成する。

		// 表示タイルに含まれる標高タイルを求める。
		// 表示タイルと標高タイルはズームレベルが異なるので、表示タイルの開始・終了ピクセルのズームレベルを標高タイルのズームレベルに変更して求める。

		var ev_wp_sx = GetStartWorldPixelIntX(view_tile_sx);
		var ev_wp_sy = GetStartWorldPixelIntY(view_tile_sy);
		var ev_wp_ex = GetEndWorldPixelIntX  (view_tile_ex);
		var ev_wp_ey = GetEndWorldPixelIntY  (view_tile_ey);

		// ◆標高データのズームレベルは取り敢えず14のみ。
		Int32 ev_zoom_level = 14;

		ev_wp_sx.ZoomLevel = 
		ev_wp_sy.ZoomLevel = 
		ev_wp_ex.ZoomLevel = 
		ev_wp_ey.ZoomLevel = ev_zoom_level;

		var ev_s_tile = new CTile(GetTileX(ev_wp_sx), GetTileY(ev_wp_sy));
		var ev_e_tile = new CTile(GetTileX(ev_wp_ex), GetTileY(ev_wp_ey));

		//--------------------------------------------------
		// 1.1 標高タイルをダウンロードする。

		Stopwatch.Lap("download elevation tiles");

		DownloadGSIElevationTiles(ev_s_tile, ev_e_tile, map_data_fld);

		Stopwatch.Lap("- elevation tiles downloaded");

		//--------------------------------------------------
		// 1.2 標高データを作成する。

		Stopwatch.Lap("build elevation map data");
		
		// 国土地理院標高タイルから標高地図データを作成する。
		var ev_map_data = new CElevationMapData_GSI_DEM_PNG(gsi_ev_tile_fld, ev_s_tile, ev_e_tile);

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
		// ◆タイル版では高度クラスに直接反映しない。
		CAltitude.SetGeoidMapData(geoid_map_data);

		//--------------------------------------------------
		// 3 地図画像タイルをダウンロードして地図画像データを作成する。

		var img_wp_sx = GetStartWorldPixelIntX(view_tile_sx);
		var img_wp_sy = GetStartWorldPixelIntY(view_tile_sy);
		var img_wp_ex = GetEndWorldPixelIntX  (view_tile_ex);
		var img_wp_ey = GetEndWorldPixelIntY  (view_tile_ey);

		img_wp_sx.ZoomLevel = 
		img_wp_sy.ZoomLevel = 
		img_wp_ex.ZoomLevel = 
		img_wp_ey.ZoomLevel = img_zoom_level;

		var img_tile_sx = GetTileX(img_wp_sx);
		var img_tile_sy = GetTileY(img_wp_sy);
		var img_tile_ex = GetTileX(img_wp_ex);
		var img_tile_ey = GetTileY(img_wp_ey);

		var img_s_tile = new CTile(img_tile_sx, img_tile_sy);
		var img_e_tile = new CTile(img_tile_ex, img_tile_ey);

		//--------------------------------------------------
		// 3.1 地図画像タイルをダウンロードする。

		Stopwatch.Lap("download map tiles");

		DownloadGSIImageTiles(img_s_tile, img_e_tile, map_data_fld);

		Stopwatch.Lap("- map tiles downloaded");

		//--------------------------------------------------
		// 3.2 地図画像を作成する。

		Stopwatch.Lap("build map image");

		var map_img = GSIImageTile.MakeMapImageFromGSITiles(gsi_img_tile_fld, gsi_img_tile_ext, img_s_tile, img_e_tile);

		Stopwatch.Lap("- map image built");

		//--------------------------------------------------
		// 3.3 グリッドを描画する。
		{ 
			// ◆南北は逆転している。WPの南北逆転を一律に変換できないか？範囲クラスにするべきか。
			var img_s_lg = ToLongitude(GetStartWorldPixelIntX(img_tile_sx));
			var img_s_lt = ToLatitude (GetEndWorldPixelIntY  (img_tile_ey));
			var img_e_lg = ToLongitude(GetEndWorldPixelIntX  (img_tile_ex));
			var img_e_lt = ToLatitude (GetStartWorldPixelIntY(img_tile_sy));

			var img_s_tude = new CTude(img_s_lg, img_s_lt);
			var img_e_tude = new CTude(img_e_lg, img_e_lt);

			//--------------------------------------------------
			// 3.3.1 経緯度グリッドを描画する。
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

				CTudeGrid.Draw(map_img, img_s_tude, img_e_tude, tude_grid_elements, tude_grid_text_elements_in_min);

				var g = Graphics.FromImage(map_img);

				// グリッド文字列を描画する。
				foreach(var grid_text_element in tude_grid_text_elements_in_min)
					g.DrawImage(grid_text_element.gridTextBitmap, (int)(grid_text_element.gridTextCoord.X), (int)(grid_text_element.gridTextCoord.Y));

				g.Dispose();

				Stopwatch.Lap("- tude grid drawn");
			}

			//--------------------------------------------------
			// 3.3.2 UTMグリッドを描画する。
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

				CUTMGrid.Draw(map_img, img_s_tude, img_e_tude, utm_grid_elements, utm_grid_text_elements_in_min);

				var g = Graphics.FromImage(map_img);

				// グリッド文字列を描画する。
				foreach(var grid_text_element in utm_grid_text_elements_in_min)
					g.DrawImage(grid_text_element.gridTextBitmap, (int)(grid_text_element.gridTextCoord.X), (int)(grid_text_element.gridTextCoord.Y));

				g.Dispose();

				Stopwatch.Lap("- UTM grid drawn");
			}
		}

		//--------------------------------------------------
		// 3.4 地図画像データを作成する。

		var img_map_data = new CImageMapData_WP(map_img, img_s_tile, img_e_tile);

		Stopwatch.Lap("build image map data");

		// ◆テスト
		// →地図画像は作成できていると思われる。グリッドが足りないように見えるが、タイルから切り出す前だからか？そうでもない？
		{
/*				MapImageTestForm map_img_test_form = new MapImageTestForm();

			map_img_test_form.pictureBox1.Image = map_img;

			map_img_test_form.Show();
*/		}

		Stopwatch.Lap("- image map data built");

		//--------------------------------------------------
		// 4 ビューアフォームを作成する。

		Stopwatch.Lap("build viewer form");

		// ◆viewer_form.Viewerはnullであり、後で設定する。
		var viewer_form = new GeoViewViewerForm_Tile();

		Stopwatch.Lap("- viewer form built");

		viewer_form.Text = title;

		// シーンの作成状況等を表示するため先に表示しておく。
		// ◆シーン作成状況の表示は整理されていないのでシーンの作成状況は実質的に表示できない。
		//  (途中で作成状況の表示(進行)が止まる。)
		viewer_form.Show();

		//--------------------------------------------------
		// 5 ビューアパラメータを作成する。← 1,2,3,4

		var viewer_params = new CViewerParameters_Tile();

		{
			viewer_params.viewer_control = viewer_form.PictureBox;
			viewer_params.s_tile = new CTile(view_tile_sx, view_tile_sy);
			viewer_params.e_tile = new CTile(view_tile_ex, view_tile_ey);
			viewer_params.ev_map_data = ev_map_data;
			viewer_params.geoid_map_data = geoid_map_data;
			viewer_params.img_map_data = img_map_data;
			viewer_params.options = "view_tri_polygons";
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

		CSceneConfig scene_config = new CSceneConfig
			(0.6f, // 環境光反射係数 [0,1]
		 	 0.5f, // 鏡面反射係数   [0,1]
			 64,   // ハイライト     [0,128]
			 DShadingMode.SHADING_MAPPING, // ◆SHADING_FLATとかだと例外が出る。
			 DFogMode.FOG_NO,
			 3000f); // 視程(m)

		//--------------------------------------------------
		// 9 ビューアを作成する。← 5,7,8
		// ▼ここの設定順序には依存関係があるので、整理してライブラリに収めろ。ユーザプログラミングに晒すな。
		// →◆ユーザプログラミングでフォーム(コントロール)が作成されるので、その部分は別になるが。

		Stopwatch.Lap("build viewer");

		GeoViewer_WP = new CGeoViewer_WP(viewer_params, scene_config, controller_parts, Info, Stopwatch);
			
		Stopwatch.Lap("- viewer built");

		//--------------------------------------------------
		// 10 ビューアフォームとコントローラフォームにビューアを設定する。←⑨

		viewer_form	   .Viewer = GeoViewer_WP;
		controller_form.Viewer = GeoViewer_WP;

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

		var config_form = new GeoViewerConfigForm(GeoViewer_WP);

		config_form.Text = title;

		config_form.Show();

		//--------------------------------------------------
		// 12 シーンを描画する。← 9

		Stopwatch.Lap("create scene");

		GeoViewer_WP.CreateScene();

		Stopwatch.Lap("- scene created");

		//--------------------------------------------------
		// 13 図形を描画する。← 9

		Stopwatch.Lap("draw shapes");

		// ◆ここはWP版を渡す。
		GeoViewerDrawShapes_Tude(GeoViewer_WP);

		Stopwatch.Lap("- shapes drawn");

		//--------------------------------------------------

		Viewer = GeoViewer_WP;

		//--------------------------------------------------
		// 表示用のポリゴンサイズを計算する。
		{ 
			var tude_00 = s_tude;
			var tude_10 = new CTude(e_tude.Longitude, s_tude.Latitude);
			var tude_01 = new CTude(s_tude.Longitude, e_tude.Latitude);

			var coord_00 = ToGeoCentricCoord(tude_00);
			var coord_10 = ToGeoCentricCoord(tude_10);
			var coord_01 = ToGeoCentricCoord(tude_01);

			var dx_00_10 = coord_10.X - coord_00.X;
			var dy_00_10 = coord_10.Y - coord_00.Y;
			var dz_00_10 = coord_10.Z - coord_00.Z;

			PlaneSizeEW = (int)(Math.Sqrt(dx_00_10 * dx_00_10 + dy_00_10 * dy_00_10 + dz_00_10 * dz_00_10));

			var dx_00_01 = coord_01.X - coord_00.X;
			var dy_00_01 = coord_01.Y - coord_00.Y;
			var dz_00_01 = coord_01.Z - coord_00.Z;

			PlaneSizeNS = (int)(Math.Sqrt(dx_00_01 * dx_00_01 + dy_00_01 * dy_00_01 + dz_00_01 * dz_00_01));
		}
	}
}
//---------------------------------------------------------------------------
}
