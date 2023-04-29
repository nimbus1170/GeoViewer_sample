//
// GeoViewer_Tile.cs
// 地形ビューア(タイル)
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Scene;

using static DSF_NET_Geography.Convert_LgLt_WP;
using static DSF_NET_Geography.Convert_LgLt_WPInt;
using static DSF_NET_Geography.XMapTile;

using System.Runtime.Versioning;
using System.Windows.Forms;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	[SupportedOSPlatform("windows")]
	CGeoViewer_WP CreateGeoViewer_Tile
		(in PictureBox picture_box,
		 in CGeoidMapData geoid_map_data,
		 in CSceneCfg scene_cfg,
		 in CControllerParts controller_parts)
	{
		StopWatch.Lap("Create GeoViewer_Tile");

		//--------------------------------------------------
		// 1 開始・終了座標について、経緯度座標をクランプしてWP座標を作成するとともに、WP座標にクランプされた経緯度座標を作成する。

		// タイルへのクランプ前のポリゴン単位のWP座標
		var s_wp_0 = new CWPInt(ToWPIntX(PolygonZoomLevel, StartLgLt_0.Lg), ToWPIntY(PolygonZoomLevel, EndLgLt_0  .Lt));
		var e_wp_0 = new CWPInt(ToWPIntX(PolygonZoomLevel, EndLgLt_0  .Lg), ToWPIntY(PolygonZoomLevel, StartLgLt_0.Lt));

		// タイル
		var s_tile = GetTile(s_wp_0);
		var e_tile = GetTile(e_wp_0);

		// タイルにクランプされたWP座標
		StartWP	= new CWPInt(GetStartWPIntX(s_tile.X), GetStartWPIntY(s_tile.Y));
		EndWP	= new CWPInt(GetEndWPIntX  (e_tile.X), GetEndWPIntY  (e_tile.Y));

		// タイルにクランプされた経緯度座標
		StartLgLt = new CLgLt(ToLg(StartWP.X), ToLt(EndWP  .Y));
		EndLgLt	  = new CLgLt(ToLg(EndWP  .X), ToLt(StartWP.Y));

		//--------------------------------------------------
		// 2 標高データを作成する。

		// 表示タイルに含まれる標高タイルを求める。
		// ズームレベルを変更するので、元のWP座標に影響を与えないよう(ディープ)コピーする。
		var ev_s_wp = new CWPInt(StartWP);
		var ev_e_wp = new CWPInt(EndWP  );

		// ◆標高データのズームレベルは取り敢えず14のみ。
		ev_s_wp.ZoomLevel =
		ev_e_wp.ZoomLevel = 14;

		var ev_s_tile = GetTile(ev_s_wp);
		var ev_e_tile = GetTile(ev_e_wp);

		//--------------------------------------------------
		// 2.1 標高タイルをダウンロードする。

		DownloadGSIElevationTiles(ev_s_tile, ev_e_tile, MapDataFolder);

		//--------------------------------------------------
		// 2.2 標高データを作成する。
		
		// 国土地理院標高タイルから標高地図データを作成する。
		var ev_map_data = new CElevationMapData_GSI_DEM_PNG(GSIElevationTileFolder, ev_s_tile, ev_e_tile);

		// 高度クラスに標高地図データを設定する。
		// これにより、座標オブジェクトに標高が自動設定される。
		CAltitude.SetElevationMapData(ev_map_data);

		//--------------------------------------------------
		// 3 地図画像データを作成する。

		// 地図画像データのズームレベルにするので新規インスタンスにする。
		var img_s_wp = new CWPInt(StartWP, ImageZoomLevel);
		var img_e_wp = new CWPInt(EndWP	 , ImageZoomLevel);

		var img_s_tile = GetTile(img_s_wp);
		var img_e_tile = GetTile(img_e_wp);

		//--------------------------------------------------
		// 3.1 地図画像タイルをダウンロードする。

		DownloadGSIImageTiles(img_s_tile, img_e_tile, MapDataFolder);

		//--------------------------------------------------
		// 3.2 地図画像を作成する。

	// ◆タイルで範囲を与えるとズレる。タイルはそのタイルの始点を指してないか？標高との整合もWPの方が正しいようだ。
		MapImage = GSIImageTile.MakeMapImageFromGSITiles(GSIImageTileFolder, GSIImageTileExt, img_s_tile/*tile*/, img_e_tile/*tile*/);

		//--------------------------------------------------
		// 3.3 グリッドを描画する。

		// グリッドオーバレイを作成するようになっていなければ地図に描画する。
		if(GridOverlayCfg == null)
		{ 
			DrawLgLtGrid(MapImage, StartLgLt, EndLgLt, GridFontSize);
			DrawUTMGrid (MapImage, StartLgLt, EndLgLt, GridFontSize);
		}

		//--------------------------------------------------
		// 3.4 地図画像データを作成する。

		var img_map_data = new CImageMapData_WP(MapImage, img_s_tile, img_e_tile);

		// ◆テスト
		{
/*			MapImageTestForm map_img_test_form = new MapImageTestForm();

			map_img_test_form.pictureBox1.Image = map_img;

			map_img_test_form.Show();
*/		}

		//--------------------------------------------------
		// 4 ビューアを作成する。
		// ▼ここの設定順序には依存関係があるので、整理してライブラリに収めろ。ユーザプログラミングに晒すな。
		// →◆ユーザプログラミングでフォーム(コントロール)が作成されるので、その部分は別になるが。

		var viewer = new CGeoViewer_WP
			(picture_box,
			s_tile,
			e_tile,
			ev_map_data,
			geoid_map_data,
			img_map_data,
			"view_tri_polygons",
			scene_cfg,
			controller_parts);
			
		//--------------------------------------------------
		// 5 シーンを描画する。

		viewer.CreateScene();

		//--------------------------------------------------

		StopWatch.Lap("GeoViewer_Tile Created");

		return viewer;
	}
}
//---------------------------------------------------------------------------
}
