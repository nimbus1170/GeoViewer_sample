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
	//--------------------------------------------------
	// 地図データフォルダ等設定

	CMapDataCfg MapDataCfg;

	//--------------------------------------------------
	// 個別設定

	double LgLtMargin;
	bool   ToCheckDataOnly = false;
	
	bool ToShowDebugInfo;

	string PlaneMode;

	string Title;

	int MeshZoomLevel;
	int ImageZoomLevel;

	int MeshSize;

	double NearPlane;

	int PointSize;

	CLgLt StartLgLt_0;
	CLgLt EndLgLt_0;

	bool ToDrawGrid = false;

	int GridFontSize;

	XmlNode GridOverlayCfg = null;
	
	string DrawingFileName;

	//---------------------------------------------------------------------------

	void ReadCfgFromFile(in string cfg_fname)
	{
		// ◆個別の設定ファイルを読み込んで必要な設定を上書きするためにも使用するため、定義がなくてもエラーにはしない。
		// →◆設定漏れをチェックする必要あり。

		var cfg_doc = new XmlDocument();

		cfg_doc.Load(cfg_fname);

		//--------------------------------------------------

		var geoviewer_cfg = cfg_doc.SelectSingleNode("GeoViewerCfg")?? throw new Exception("tag GeoViewerCfg not found (" + cfg_fname + ")");

		//--------------------------------------------------
		// 共通設定

		MapDataCfg = new CMapDataCfg(cfg_fname);

		//--------------------------------------------------
		// 図形設定

		var shape_cfg = geoviewer_cfg.SelectSingleNode("ShapeCfg");

		if(shape_cfg != null)
		{
			LgLtMargin = ToDouble(shape_cfg.Attributes["Margin"].InnerText);

			MeshZoomLevel  = ToInt32(shape_cfg.Attributes["MeshZoomLevel" ].InnerText);
			ImageZoomLevel = ToInt32(shape_cfg.Attributes["ImageZoomLevel"].InnerText);

			NearPlane = ToDouble(shape_cfg.Attributes["NearPlane"].InnerText);

			PointSize = ToInt32(shape_cfg.Attributes["PointSize"].InnerText);

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

	//---------------------------------------------------------------------------

	void ReadCfgFromParam(in string param)
	{
		// ◆paramsは予約語	
		string[] prms = param.Split(" ");

		// ◆とりあえず。
		Title = "DENKOKU-ROBO2"; 

		StartLgLt_0.Set(new CLg(ToDouble(prms[0])), new CLt(ToDouble(prms[1])));
		EndLgLt_0  .Set(new CLg(ToDouble(prms[2])), new CLt(ToDouble(prms[3])));

		MeshZoomLevel  = ToInt32(prms[4]);
		ImageZoomLevel = ToInt32(prms[5]);
	}

	//---------------------------------------------------------------------------
}
//---------------------------------------------------------------------------
}
