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

	int MeshZoomLevel;
	int ImageZoomLevel;

	int MeshSize;

	double NearPlane;

	CLgLt StartLgLt_0;
	CLgLt EndLgLt_0;

	string MapDataFolder;

	// 平面直角座標
	// ◆国土地理院データ固定
	int	DefaultOrigin;

	// 点群
	// ◆国土地理院データ固定
	string LASFile   = "";
	Int64  ReadStart = -1;
	Int64  ReadEnd	 = -1;
	Int64  ReadStep	 = -1;
	int	   PointSize;
	string TXTTitleLine;
	string TXTFormat;

	// 点群・図形共通
	bool   ToCheckDataOnly = false;
	double LgLtMargin;
	
	// ◆国土地理院データ固定

	string GSIImageTileFolder;
	string GSIImageTileExt;
	
	string GSIPhotoTileFolder;
	string GSIPhotoTileExt;
	
	string GSIElevationTileFolder;

	string GSIGeoidModelFile;

	bool ToDrawGrid = false;

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
		// LAS設定
		// ◆LASファイルを後から読み込む場合にも必要。

		var las_cfg = geoviewer_cfg.SelectSingleNode("LASCfg");

		if(las_cfg != null)
		{
			DefaultOrigin = ToInt32(las_cfg.Attributes["DefaultOrigin"].InnerText);
			PointSize	  = ToInt32(las_cfg.Attributes["PointSize"	  ].InnerText);

			// ◆必須ではない。
			if(las_cfg.Attributes["LASFile"	 ] != null) LASFile =			las_cfg.Attributes["LASFile"  ].InnerText ; // ◆使われていない。
			if(las_cfg.Attributes["ReadStart"] != null) ReadStart = ToInt64(las_cfg.Attributes["ReadStart"].InnerText);
			if(las_cfg.Attributes["ReadEnd"  ] != null) ReadEnd	  = ToInt64(las_cfg.Attributes["ReadEnd"  ].InnerText);
			if(las_cfg.Attributes["ReadStep" ] != null) ReadStep  = ToInt64(las_cfg.Attributes["ReadStep" ].InnerText);

			// ◆必須ではない。
			if(las_cfg.Attributes["TXTTitleLine" ] != null) TXTTitleLine = las_cfg.Attributes["TXTTitleLine" ].InnerText;
			if(las_cfg.Attributes["TXTFormat"	 ] != null)	TXTFormat	 = las_cfg.Attributes["TXTFormat"	 ].InnerText;
		}

		//--------------------------------------------------
		// 図形設定

		var shape_cfg = geoviewer_cfg.SelectSingleNode("ShapeCfg");

		if(shape_cfg != null)
		{
			LgLtMargin = ToDouble(shape_cfg.Attributes["Margin"].InnerText);

			MeshZoomLevel  = ToInt32(shape_cfg.Attributes["MeshZoomLevel" ].InnerText);
			ImageZoomLevel = ToInt32(shape_cfg.Attributes["ImageZoomLevel"].InnerText);

			NearPlane = ToDouble(shape_cfg.Attributes["NearPlane"].InnerText);

			ToCheckDataOnly	= (shape_cfg.Attributes["ToCheckDataOnly"]?.InnerText == "true")? true: false;

			ToDrawShapeAsTIN   = (shape_cfg.Attributes["ToDrawShapeAsTIN"  ]?.InnerText == "true")? true: false;
			ToDrawShapeAsLayer = (shape_cfg.Attributes["ToDrawShapeAsLayer"]?.InnerText == "true")? true: false;
		}
		
		//--------------------------------------------------
		// グリッド設定

		var grid_cfg = geoviewer_cfg.SelectSingleNode("Grid");

		if(grid_cfg != null)
		{
			ToDrawGrid = true;		
			GridFontSize = ToInt32(grid_cfg.Attributes["FontSize"].InnerText);
			GridOverlayCfg = grid_cfg.SelectSingleNode("GridOverlay");
		}
		
		//--------------------------------------------------

		ToShowDebugInfo = (geoviewer_cfg.SelectSingleNode("ToShowDebugInfo") != null)? true: false;;

		//--------------------------------------------------

		var plane_mode_cfg = geoviewer_cfg.SelectSingleNode("PlaneMode");

		if(plane_mode_cfg != null)
			PlaneMode = plane_mode_cfg.Attributes["Mode"].InnerText;

		//--------------------------------------------------
		// 地図データ設定
		// ◆XML読み込みの成否は、個々にnull判定するのではなく、取り敢えず例外を出させる。

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
		// ファイル選択モード
		// ◆地図データ設定等は、この前に完了しておく必要がある。

		var to_select_map_cfg = geoviewer_cfg.SelectSingleNode("ToSelectMapCfgFile");
		var to_select_las	  = geoviewer_cfg.SelectSingleNode("ToSelectLASFile");
		var to_select_shape	  = geoviewer_cfg.SelectSingleNode("ToSelectShapeFile");

		if((to_select_map_cfg != null) ||
		   (to_select_las	  != null) ||
		   (to_select_shape	  != null))
		{
			// ファイル選択モード

			OpenFileDialog of_dialog = new ()
				{ Title  = "ファイルを開く",
				  Filter = "地域設定ファイル(*.mapcfg.xml)|*.mapcfg.xml|" + 
						   "点群ファイル(*.las;*.csv;*.txt)|*.las;*.csv;*.txt|" +
						   "図形ファイル(*.shp)|*.shp",
				  FilterIndex =
					(to_select_map_cfg != null)? 1:
					(to_select_las	   != null)? 2:
					(to_select_shape   != null)? 3: 1 }; // ◆無かったら1か？

			// ◆ファイル選択モードでファイルを選択しなかったら終了する。
			if(of_dialog.ShowDialog() == DialogResult.Cancel)
//				Application.Exit(); // ◆終了しない。
				Close();

			var fname = of_dialog.FileName;

			switch(of_dialog.FilterIndex)
			{
				case 1: // 地域設定
				{
					// ◆再起呼び出しだが大丈夫か？
					ReadCfgFromFile(fname);
					break;
				}

				case 2: // 点群ファイル
				{
					Title = fname; 

StopWatch.Lap("before ReadLASFromFiles");
MemWatch .Lap("before ReadLASFromFiles");
					(LASzipData, ReadLASMsg) = ReadLASFromFile(fname);
StopWatch.Lap("after  ReadLASFromFile");
MemWatch .Lap("after  ReadLASFromFile");

					break;
				}

				case 3: // 図形ファイル
				{
					Title = fname;

StopWatch.Lap("before ReadShapefileFromFiles");
MemWatch .Lap("before ReadShapefileFromFiles");
					(ShapeFile, ReadShapefileMsg) = ReadShapefileFromFile(fname);
StopWatch.Lap("after  ReadShapefileFromFile");
MemWatch .Lap("after  ReadShapefileFromFile");

					break;
				}
			}

			of_dialog.Dispose();
		}
		else
		{
			// 地域設定をこのファイルから読む。

			var map_cfg = geoviewer_cfg.SelectSingleNode("MapCfg"); // ?? throw new Exception("tag MapCfg not found (" + cfg_fname + ")");

			// ◆ファイル選択モードもあるので、定義されていなくても間違いではない？
			if(map_cfg != null)
			{
				Title =	map_cfg.Attributes["Title"].InnerText;

				MeshSize = ToInt32(map_cfg.Attributes["MeshSize"].InnerText);

				MeshZoomLevel  = ToInt32(map_cfg.Attributes["MeshZoomLevel" ].InnerText);
				ImageZoomLevel = ToInt32(map_cfg.Attributes["ImageZoomLevel"].InnerText);

				if(PlaneMode == "Tile") ImageZoomLevel = MeshZoomLevel;

				NearPlane = ToDouble(map_cfg.Attributes["NearPlane"].InnerText);

				// クランプ前の開始・終了経緯度座標
				// ◆プレーンをWP単位にクランプするので、クランプ前のものを使用しないようにする。
				StartLgLt_0	= ReadLgLt(map_cfg.SelectSingleNode("Start"));
				EndLgLt_0	= ReadLgLt(map_cfg.SelectSingleNode("End"  ));

				DrawingFileName = map_cfg.SelectSingleNode("Drawings")?.Attributes["File"]?.InnerText;
			}
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

		MeshZoomLevel = ToInt32(prms[4]);
		ImageZoomLevel	 = ToInt32(prms[5]);
	}
}
//---------------------------------------------------------------------------
}
