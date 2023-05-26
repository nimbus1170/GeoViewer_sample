//
// GeoViewer_WP.cs
// 地形ビューア(ワールドピクセル)
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
	CGeoViewer_WP CreateGeoViewer_WP
		(in PictureBox picture_box,
		 in CGeoidMapData geoid_map_data,
		 in CSceneCfg scene_cfg,
		 in CControllerParts controller_parts)
	{
		//--------------------------------------------------
		// 1 タイルをダウンロードする。
		// ◆GeoViewer_Tileと比較して、正しいか？

		// ◆ここはクランプ前で良いのか？
		DownloadGSITiles(StartLgLt_0, EndLgLt_0, ImageZoomLevel, MapDataFolder);

		//--------------------------------------------------
		// 2 開始・終了座標について、経緯度座標をクランプしてWP座標を作成するとともに、WP座標にクランプされた経緯度座標を作成する。

		// ◆緯度方向を逆転させる。緯度方向の逆転を一般化するには範囲にする必要があるか。
		StartWP	= new CWPInt(ToWPIntX(PolygonZoomLevel, StartLgLt_0.Lg), ToWPIntY(PolygonZoomLevel, EndLgLt_0  .Lt));
		EndWP	= new CWPInt(ToWPIntX(PolygonZoomLevel, EndLgLt_0  .Lg), ToWPIntY(PolygonZoomLevel, StartLgLt_0.Lt));

		// ◆南北は逆転している。WPの南北逆転を一律に変換できないか？範囲クラスにするべきか。
		StartLgLt = new CLgLt(ToLg(StartWP.X), ToLt(EndWP  .Y));
		EndLgLt	  = new CLgLt(ToLg(EndWP  .X), ToLt(StartWP.Y));

		//--------------------------------------------------
		// 3 標高地図データを作成する。

		// 開始・終了座標は標高データのズームレベルの新規インスタンスにする。
		// ◆標高データのズームレベルは取り敢えず14のみ。
		var ev_map_data = new CElevationMapData_GSI_DEM_PNG
			(GSIElevationTileFolder,
			 GetTile(new CWPInt(StartWP, 14)),
			 GetTile(new CWPInt(EndWP  , 14)));

		// 高度クラスに標高地図データを設定することにより、座標オブジェクトに標高が自動設定される。
		CAltitude.SetElevationMapData(ev_map_data);

		//--------------------------------------------------
		// 4 地図画像データを作成する。

		// 地図画像データのズームレベルにするので新規インスタンスにする。
		var img_s_wp = new CWPInt(StartWP, ImageZoomLevel);
		var img_e_wp = new CWPInt(EndWP  , ImageZoomLevel);

		//--------------------------------------------------
		// 4.1 地図画像を作成する。

		MapImage = GSIImageTile.MakeMapImageFromGSITiles(GSIImageTileFolder, GSIImageTileExt, img_s_wp, img_e_wp);
		MapPhoto = GSIImageTile.MakeMapImageFromGSITiles(GSIPhotoTileFolder, GSIPhotoTileExt, img_s_wp, img_e_wp);

		//--------------------------------------------------
		// 4.2 グリッドを描画する。
		
		// グリッドオーバレイを作成するようになっていなければ地図に描画する。
		if(GridOverlayCfg == null)
		{
			DrawLgLtGrid(MapImage, StartLgLt, EndLgLt, GridFontSize);
			DrawUTMGrid (MapImage, StartLgLt, EndLgLt, GridFontSize);

			DrawLgLtGrid(MapPhoto, StartLgLt, EndLgLt, GridFontSize);
			DrawUTMGrid (MapPhoto, StartLgLt, EndLgLt, GridFontSize);
		}

		// ◆テスト
		{
/*			// ◆地図画像は作成できていると思われる。グリッドが足りないように見えるが、タイルから切り出す前だからか？そうでもない？
			var map_img_test_form = new MapImageTestForm();
			map_img_test_form.pictureBox1.Image = map_img;
			map_img_test_form.Show();
*/		}

		//--------------------------------------------------
		// 5 ビューアを作成する。
		// ▼ここの設定順序には依存関係があるので、整理してライブラリに収めろ。ユーザプログラミングに晒すな。
		// →◆ユーザプログラミングでフォーム(コントロール)が作成されるので、その部分は別になるが。

		var viewer = new CGeoViewer_WP
			(picture_box,
			 StartWP,
			 EndWP,
			 ev_map_data,
			 geoid_map_data,
			 "view_tri_polygons",
			 scene_cfg,
			 controller_parts);

		//--------------------------------------------------
		// 6 シーンを描画する。

		var image_map_data = new CImageMapData_WP(MapImage, img_s_wp, img_e_wp);
		var photo_map_data = new CImageMapData_WP(MapPhoto, img_s_wp, img_e_wp);

		viewer.CreateScene
			(image_map_data,
			 photo_map_data);

		//--------------------------------------------------

		return viewer;
	}
}
//---------------------------------------------------------------------------
}
