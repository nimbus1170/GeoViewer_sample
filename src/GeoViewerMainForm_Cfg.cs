//
// GeoViewerMainForm_Cfg.cs
// 地形ビューア - 設定ファイル
//
// ◆本来は別にする必要はないかもしれないが、現状はLgLtとWPとTileに分かれており、同じものを書いているので纏める。
// ◆XMLやハードコード等のバリエーションがあるので依存しないように別にする。
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using System.Xml;

using static DSF_NET_TacticalDrawing.XMLReader;

using static System.Convert;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	bool ToShowDebugInfo;

	string PlaneMode;

	string Title;

	int PolygonZoomLevel;
	int ImageZoomLevel;

	int PolygonSize;

	double NearPlane;

	CLgLt StartLgLt_0;
	CLgLt EndLgLt_0;

	string MapDataFolder;

	// ◆国土地理院データ固定
	int	   DefaultOrigin;
	string TXTTitleLine;
	string TXTFormat;
	int	   PointSize;
	double LASMargin;
	bool   ToCheckLASDataOnly = false;

	// ◆国土地理院データ固定

	string GSIImageTileFolder;
	string GSIImageTileExt;
	
	string GSIPhotoTileFolder;
	string GSIPhotoTileExt;
	
	string GSIElevationTileFolder;

	string GSIGeoidModelFile;

	int GridFontSize;

	XmlNode GridOverlayCfg = null;
	
	string DrawingFileName;

	void ReadCfgFromFile(in string cfg_fname)
	{
		// ◆個別の設定ファイルを読み込んで必要な設定を上書きするためにも使用するため、定義がなくてもエラーにはしない。
		// →◆設定漏れをチェックする必要あり。

		var cfg_doc = new XmlDocument();

		cfg_doc.Load(cfg_fname);

		//--------------------------------------------------

		var geoviewer_cfg = cfg_doc.SelectSingleNode("GeoViewerCfg")?? throw new Exception("tag GeoViewerCfg not found (" + cfg_fname + ")");

		//--------------------------------------------------

		ToShowDebugInfo = (geoviewer_cfg.SelectSingleNode("ToShowDebugInfo") != null)? true: false;;

		//--------------------------------------------------

		var plane_mode_cfg = geoviewer_cfg.SelectSingleNode("PlaneMode");

		if(plane_mode_cfg != null)
			PlaneMode = plane_mode_cfg.Attributes["Mode"].InnerText;

		//--------------------------------------------------
		// 地域設定

		var to_select_map_cfg = geoviewer_cfg.SelectSingleNode("ToSelectMapCfgFile");

		if(to_select_map_cfg != null)
		{
			// 地域設定を別ファイルから読む。

			OpenFileDialog of_dialog = new ()
				{ Title  = "地域設定ファイルを開く",
				  Filter = "地域設定ファイル(*.mapcfg.xml)|*.mapcfg.xml" };

			if(of_dialog.ShowDialog() == DialogResult.Cancel)
//				Application.Exit(); // ◆終了しない。
				Close();

			// ◆再起呼び出しだが大丈夫か？
			ReadCfgFromFile(of_dialog.FileName);

			of_dialog.Dispose();
		}
		else
		{
			// 地域設定をこのファイルから読む。

			var map_cfg = geoviewer_cfg.SelectSingleNode("MapCfg"); // ?? throw new Exception("tag MapCfg not found (" + cfg_fname + ")");

			if(map_cfg != null)
			{
				Title =	map_cfg.Attributes["Title"].InnerText;

				PolygonSize = ToInt32(map_cfg.Attributes["PolygonSize"].InnerText);

				PolygonZoomLevel = ToInt32(map_cfg.Attributes["PolygonZoomLevel"].InnerText);
				ImageZoomLevel	 = ToInt32(map_cfg.Attributes["ImageZoomLevel"  ].InnerText);

				NearPlane = ToDouble(map_cfg.Attributes["NearPlane"].InnerText);

				// クランプ前の開始・終了経緯度座標
				// ◆プレーンをWP単位にクランプするので、クランプ前のものを使用しないようにする。
				StartLgLt_0	= ReadLgLt(map_cfg.SelectSingleNode("Start"));
				EndLgLt_0	= ReadLgLt(map_cfg.SelectSingleNode("End"  ));

				DrawingFileName = map_cfg.SelectSingleNode("Drawings")?.Attributes["File"]?.InnerText;
			}
		}

		//--------------------------------------------------
		// LAS設定を読む。
		// ◆LASファイルを後から読み込む場合にも必要。

		var las_cfg = geoviewer_cfg.SelectSingleNode("LASCfg");

		if(las_cfg != null)
		{
			DefaultOrigin = ToInt32(las_cfg.Attributes["DefaultOrigin"].InnerText);
			PointSize	  = ToInt32(las_cfg.Attributes["PointSize"	  ].InnerText);

			// ◆必須ではない。
			if(las_cfg.Attributes["TXTTitleLine" ] != null) TXTTitleLine = las_cfg.Attributes["TXTTitleLine" ].InnerText;
			if(las_cfg.Attributes["TXTFormat"	 ] != null)	TXTFormat	 = las_cfg.Attributes["TXTFormat"	 ].InnerText;
		}

		//--------------------------------------------------
		// LASファイルを読む。
		// ◆ダブって設定されたらどうする？

		var lasview_cfg = geoviewer_cfg.SelectSingleNode("ToSelectLASFile");
		
		if(lasview_cfg != null)
		{
			// ファイル選択ダイアログを表示してLASファイルを選択する。

			LASMargin = ToDouble(lasview_cfg.Attributes["Margin"].InnerText);

			ToCheckLASDataOnly = (lasview_cfg.Attributes["ToCheckLASDataOnly"]?.InnerText == "true")? true: false;

			PolygonZoomLevel = ToInt32(lasview_cfg.Attributes["PolygonZoomLevel"].InnerText);
			ImageZoomLevel	 = ToInt32(lasview_cfg.Attributes["ImageZoomLevel"  ].InnerText);

			NearPlane = ToDouble(lasview_cfg.Attributes["NearPlane"].InnerText);

			OpenFileDialog of_dialog = new ()
				{ Title  = "LASファイルを開く",
				  Filter = "LASファイル(*.las;*.csv;*.txt)|*.las;*.csv;*.txt" };

			if(of_dialog.ShowDialog() == DialogResult.Cancel)
//				Application.Exit(); // ◆終了しない。
				Close();

			var las_fname = of_dialog.FileName;

			of_dialog.Dispose();

			Title = las_fname; 

StopWatch.Lap("before ReadLASFromFiles");
MemWatch .Lap("before ReadLASFromFiles");
			(LASzipData, ReadLASMsg) = ReadLASFromFile(las_fname);
StopWatch.Lap("after  ReadLASFromFile");
MemWatch .Lap("after  ReadLASFromFile");
		}
		
		//--------------------------------------------------
		// 地図データ設定
		// ◆XML読み込みの成否は、個々にNULL判定するのではなく、取り敢えず例外を出させる。

		var map_data_cfg = geoviewer_cfg.SelectSingleNode("MapDataCfg");

		if(map_data_cfg != null)
		{
			MapDataFolder = map_data_cfg.SelectSingleNode("MapData").Attributes["Folder"].InnerText;

			GSIImageTileFolder = map_data_cfg.SelectSingleNode("GSIImageTiles").Attributes["Folder"].InnerText;
			GSIImageTileExt	   = map_data_cfg.SelectSingleNode("GSIImageTiles").Attributes["Ext"   ].InnerText;

			GSIPhotoTileFolder = map_data_cfg.SelectSingleNode("GSIPhotoTiles").Attributes["Folder"].InnerText;
			GSIPhotoTileExt	   = map_data_cfg.SelectSingleNode("GSIPhotoTiles").Attributes["Ext"   ].InnerText;

			GSIElevationTileFolder = map_data_cfg.SelectSingleNode("GSIElevationTiles").Attributes["Folder"].InnerText;

			GSIGeoidModelFile = map_data_cfg.SelectSingleNode("GSIGeoidModel").Attributes["File"].InnerText;
		}

		//--------------------------------------------------
		// グリッド設定

		var grid_cfg = geoviewer_cfg.SelectSingleNode("Grid");

		if(grid_cfg != null)
		{
			GridFontSize = ToInt32(grid_cfg.Attributes["FontSize"].InnerText);
			GridOverlayCfg = grid_cfg.SelectSingleNode("GridOverlay");
		}
	}

	void ReadCfgFromParam(in string param)
	{
		// ◆paramsは予約語	
		string[] prms = param.Split(" ");

		// ◆とりあえず。
		Title = "DENKOKU-ROBO2"; 

		StartLgLt_0.Set(new CLg(ToDouble(prms[0])), new CLt(ToDouble(prms[1])));
		EndLgLt_0  .Set(new CLg(ToDouble(prms[2])), new CLt(ToDouble(prms[3])));

		PolygonZoomLevel = ToInt32(prms[4]);
		ImageZoomLevel	 = ToInt32(prms[5]);
	}
}
//---------------------------------------------------------------------------
}
