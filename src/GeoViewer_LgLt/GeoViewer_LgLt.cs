//
// GeoViewer_LgLt.cs
// 地形ビューア(経緯度)
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Geometry;
using DSF_NET_Map;
using DSF_NET_Scene;

using static DSF_NET_Geography.Convert_LgLt_GeoCentricCoord;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;

using static System.Drawing.Drawing2D.DashStyle;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	[SupportedOSPlatform("windows")]
	CGeoViewer_LgLt CreateGeoViewer_LgLt
		(in PictureBox picture_box,
		 in CGeoidMapData geoid_map_data,
		 in CSceneCfg scene_cfg,
		 in CControllerParts controller_parts)
	{
		StopWatch.Lap("Create GeoViewer_LgLt");

		//--------------------------------------------------
		// 1 開始・終了座標をクランプする。

		// ◆ここでクランプする。
		// 　ライブラリ内でクランプすると丁寧な気がするが、地図画像その他を各所でクランプするより、最初に与える座標範囲をクランプした方が良いと判断する。
		(StartLgLt, EndLgLt) = ClampToPolygonSize(StartLgLt_0, EndLgLt_0, PolygonSize);

		//--------------------------------------------------
		// 2 タイルをダウンロードする。

		DownloadGSITiles(StartLgLt, EndLgLt, ImageZoomLevel, MapDataFolder);

		//--------------------------------------------------
		// 3 標高地図データを作成する。

		// ◆ズームレベルは取り敢えず14とする。15は抜けが多い。
		var ev_map_data = new CElevationMapData_GSI_DEM_PNG
			(GSIElevationTileFolder,
			 14,
			 StartLgLt,
			 EndLgLt);

		// 高度クラスに標高地図データを設定することにより、座標オブジェクトに標高が自動設定される。
		CAltitude.SetElevationMapData(ev_map_data);

		//--------------------------------------------------
		// 4 地図画像データを作成する。

		//--------------------------------------------------
		// 4.1 地図画像を作成する。

		MapImage = GSIImageTile.MakeMapImageFromGSITiles(GSIImageTileFolder, GSIImageTileExt, ImageZoomLevel, StartLgLt, EndLgLt);
		MapPhoto = GSIImageTile.MakeMapImageFromGSITiles(GSIPhotoTileFolder, GSIPhotoTileExt, ImageZoomLevel, StartLgLt, EndLgLt);

		//--------------------------------------------------
		// 4.2 グリッドを描画する。

		// グリッドオーバレイを作成するようになっていなければ地図に描画する。
		if(GridOverlayCfg == null)
		{
			DrawLgLtGrid(MapImage, StartLgLt, EndLgLt, GridFontSize);
			DrawUTMGrid (MapImage, StartLgLt, EndLgLt, GridFontSize);
		}

		//--------------------------------------------------
		// 5 ビューアを作成する。
		// ▼ここの設定順序には依存関係があるので、整理してライブラリに収めろ。ユーザプログラミングに晒すな。
		// →◆ユーザプログラミングでフォーム(コントロール)が作成されるので、その部分は別になるが。

		var viewer = new CGeoViewer_LgLt
			(picture_box,
			 StartLgLt,
			 EndLgLt,
			 PolygonSize, // 経緯度でおおよそm単位
			 ev_map_data,
			 geoid_map_data,
			 "view_tri_polygons", //"display_progress";
			 scene_cfg,
			 controller_parts);
			
		//--------------------------------------------------
		// 6 シーンを描画する。

		var image_map_data = new CImageMapData_LgLt(MapImage, StartLgLt, EndLgLt);
		var photo_map_data = new CImageMapData_LgLt(MapPhoto, StartLgLt, EndLgLt);

		viewer.CreateScene
			(image_map_data,
			 photo_map_data);

		//--------------------------------------------------

		StopWatch.Lap("GeoViewer_LgLt Created");

		return viewer;
	}

	static Tuple<CLgLt, CLgLt> ClampToPolygonSize(in CLgLt src_s_lglt, in CLgLt src_e_lglt, in int polygon_size)
	{
		double dst_s_lg = (double)((int)(src_s_lglt.Lg.DecimalDeg * 100000.0 + polygon_size) / polygon_size * polygon_size) / 100000.0;
		double dst_s_lt = (double)((int)(src_s_lglt.Lt.DecimalDeg * 100000.0 + polygon_size) / polygon_size * polygon_size) / 100000.0;
		double dst_e_lg = (double)((int)(src_e_lglt.Lg.DecimalDeg * 100000.0			   ) / polygon_size * polygon_size) / 100000.0;
		double dst_e_lt = (double)((int)(src_e_lglt.Lt.DecimalDeg * 100000.0			   ) / polygon_size * polygon_size) / 100000.0;

		return new Tuple<CLgLt, CLgLt>(new CLgLt(new CLg(dst_s_lg), new CLt(dst_s_lt)), new CLgLt(new CLg(dst_e_lg), new CLt(dst_e_lt)));
	}

	static Tuple<CLgLt, CLgLt> ExtendToPolygonSize(in CLgLt src_s_lglt, in CLgLt src_e_lglt, in int polygon_size)
	{
		double dst_s_lg = (double)((int)(src_s_lglt.Lg.DecimalDeg * 100000.0			   ) / polygon_size * polygon_size) / 100000.0;
		double dst_s_lt = (double)((int)(src_s_lglt.Lt.DecimalDeg * 100000.0			   ) / polygon_size * polygon_size) / 100000.0;
		double dst_e_lg = (double)((int)(src_e_lglt.Lg.DecimalDeg * 100000.0 + polygon_size) / polygon_size * polygon_size) / 100000.0;
		double dst_e_lt = (double)((int)(src_e_lglt.Lt.DecimalDeg * 100000.0 + polygon_size) / polygon_size * polygon_size) / 100000.0;

		return new Tuple<CLgLt, CLgLt>(new CLgLt(new CLg(dst_s_lg), new CLt(dst_s_lt)), new CLgLt(new CLg(dst_e_lg), new CLt(dst_e_lt)));
	}

	[SupportedOSPlatform("windows")] // Windows固有API(Graphics)が使用されていることを宣言する。
	static void DrawLgLtGrid(in Bitmap map_img, in CLgLt s_lglt, in CLgLt e_lglt, in int font_size_m)
	{
		//--------------------------------------------------
		// フォントサイズ(ピクセル)
		// ◆X方向の計算のみで良いか？
		var font_size_pix = font_size_m * PixelPerKmX(s_lglt, e_lglt, map_img.Width) / 1000;

		//--------------------------------------------------

		// ◆キーはグリッド間隔(分)
		// ◆XMLで設定すべきか。地図画像の縮尺(ズームレベル)にもよる。MapViewerはそうなっているいるのでは？
		var lglt_grid_elements = new Dictionary<Int32, CMapGridElement>()
			{
				{ 5, new CMapGridElement(new Pen(Color.Black, 2.0f)				      , new Font("ＭＳ ゴシック", font_size_pix, GraphicsUnit.Pixel), Brushes.Black)},
				{ 1, new CMapGridElement(new Pen(Color.Black, 2.0f){ DashStyle = Dot }, null														, null		   )}
			};

		// グリッドを描画する。
		CLgLtGrid.DrawLgLtGrid(map_img, s_lglt, e_lglt, lglt_grid_elements);
	}

	[SupportedOSPlatform("windows")] // Windows固有API(Graphics)が使用されていることを宣言する。
	static void DrawUTMGrid(in Bitmap map_img, in CLgLt s_lglt, in CLgLt e_lglt, in int font_size_m)
	{
		//--------------------------------------------------
		// フォントサイズ(ピクセル)
		// ◆X方向の計算のみで良いか？
		var font_size_pix = (float)font_size_m * PixelPerKmX(s_lglt, e_lglt, map_img.Width) / 1000;

		//--------------------------------------------------

		// ◆キーはグリッド間隔(km)
		// ◆XMLで設定すべきか。地図画像の縮尺(ズームレベル)にもよる。MapViewerはそうなっているいるのでは？
		var utm_grid_elements = new Dictionary<Int32, CMapGridElement>()
		{
			{ 1, new CMapGridElement(new Pen(Color.Maroon, 2.0f), new Font("ＭＳ ゴシック", font_size_pix, GraphicsUnit.Pixel), Brushes.Maroon)}
		};

		// グリッドを描画する。
		CUTMGrid.DrawUTMGrid(map_img, s_lglt, e_lglt, utm_grid_elements);
	}

	/// <summary>2座標間の東西方向のピクセル数から1Kmのピクセル数を返す。</summary>
	static Int32 PixelPerKmX(in CLgLt s_lglt, in CLgLt e_lglt, in int pix_w)
	{
		var s_coord = ToGeoCentricCoord(new CLgLt(s_lglt.Lg, s_lglt.Lt));
		var e_coord = ToGeoCentricCoord(new CLgLt(e_lglt.Lg, s_lglt.Lt)); // ◆経度方向の長さなので緯度は同じ。

		var d = CCoord.Distance3D(s_coord, e_coord);

		return (Int32)(pix_w * 1000 / d);
	}
}
//---------------------------------------------------------------------------
}
