﻿//
// GeoViewer_Tile.cs
// 地形ビューア(タイル)
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using static DSF_NET_Geography.Convert_LgLt_WP;
using static DSF_NET_Geography.Convert_LgLt_WPInt;
using static DSF_NET_Geography.Convert_WP_Tile;
using static DSF_NET_Geography.DAltitudeBase;

using DSF_NET_Scene;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
	public partial class GeoViewerMainForm : Form
	{
		CGeoViewer_WP CreateGeoViewer_Tile
			(in PictureBox picture_box,
			 in CGeoidMapData geoid_map_data,
			 in CSceneCfg scene_cfg,
			 in CControllerParts controller_parts)
		{
			StopWatch.Lap("Create GeoViewer_Tile");

			//--------------------------------------------------
			// 1 開始・終了座標について、経緯度座標をクランプしてWP座標を作成するとともに、WP座標にクランプされた経緯度座標を作成する。

			// タイルへのクランプ前のメッシュ単位のWP座標
			var s_wp_0 = new CWPInt(ToWPIntX(MeshZoomLevel, StartLgLt_0.Lg), ToWPIntY(MeshZoomLevel, EndLgLt_0  .Lt));
	//		var e_wp_0 = new CWPInt(ToWPIntX(MeshZoomLevel, EndLgLt_0  .Lg), ToWPIntY(MeshZoomLevel, StartLgLt_0.Lt));

			// タイル
			var s_tile = ToTile(s_wp_0);
	//		var e_tile = ToTile(e_wp_0);

			// タイルにクランプされたWP座標
			StartWP	= new CWPInt(GetStartWPIntX(s_tile.X), GetStartWPIntY(s_tile.Y));
	//		EndWP	= new CWPInt(GetEndWPIntX  (e_tile.X), GetEndWPIntY  (e_tile.Y));
			EndWP	= new CWPInt(GetEndWPIntX  (s_tile.X), GetEndWPIntY  (s_tile.Y));

			// タイルにクランプされた経緯度座標
			StartLgLt = new CLgLt(ToLg(StartWP.X), ToLt(EndWP  .Y));
			EndLgLt	  = new CLgLt(ToLg(EndWP  .X), ToLt(StartWP.Y));

			//--------------------------------------------------
			// 2 標高データを作成する。

			// 表示タイルに含まれる標高タイルを求める。
			// ズームレベルを変更するので、元のWP座標に影響を与えないよう(ディープ)コピーする。
			var ev_s_wp = new CWPInt(StartWP);
			var ev_e_wp = new CWPInt(EndWP  );

			ev_s_wp.ZoomLevel =
			ev_e_wp.ZoomLevel = DEMZoomLevel;

			var ev_s_tile = ToTile(ev_s_wp);
			var ev_e_tile = ToTile(ev_e_wp);

			//--------------------------------------------------
			// 2.1 標高タイルをダウンロードする。

			DownloadGSIElevationTiles(ev_s_tile, ev_e_tile, Cfg.MapDataFolder);

			//--------------------------------------------------
			// 2.2 標高データを作成する。
		
			// 国土地理院標高タイルから標高地図データを作成する。
			var ev_map_data = new CElevationMapData_GSI_DEM_PNG(Cfg.DEMTileFolder, ev_s_tile, ev_e_tile);

			// 高度クラスに標高地図データを設定する。
			// これにより、座標オブジェクトに標高が自動設定される。
			CAltitude.SetElevationMapData(ev_map_data);

			//--------------------------------------------------
			// 3 地図画像データを作成する。

			// 地図画像データのズームレベルにするので新規インスタンスにする。
			var img_s_wp = new CWPInt(StartWP, ImageZoomLevel);
			var img_e_wp = new CWPInt(EndWP	 , ImageZoomLevel);

			var img_s_tile = ToTile(img_s_wp);
			var img_e_tile = ToTile(img_e_wp);

			//--------------------------------------------------
			// 3.1 地図画像タイルをダウンロードする。

			DownloadGSIImageTiles(img_s_tile, img_e_tile, Cfg.MapDataFolder);

			//--------------------------------------------------
			// 3.2 地図画像を作成する。

		// ◆タイルで範囲を与えるとズレる。タイルはそのタイルの始点を指してないか？標高との整合もWPの方が正しいようだ。
			MapImage = GSIImageTile.MakeMapImageFromGSITiles(Cfg.ImageTileFolder, Cfg.ImageTileExt, img_s_tile/*tile*/, img_e_tile/*tile*/);
			MapPhoto = GSIImageTile.MakeMapImageFromGSITiles(Cfg.PhotoTileFolder, Cfg.PhotoTileExt, img_s_tile/*tile*/, img_e_tile/*tile*/);

			//--------------------------------------------------
			// 3.3 グリッドを描画する。

			// グリッドオーバレイを作成するようになっていなければ地図に描画する。
			if(Cfg.ToDrawGrid && (Cfg.GridOverlayCfg == null))
			{ 
				DrawLgLtGrid(MapImage, StartLgLt, EndLgLt, Cfg.GridFontSize);
				DrawUTMGrid (MapImage, StartLgLt, EndLgLt, Cfg.GridFontSize);

				DrawLgLtGrid(MapPhoto, StartLgLt, EndLgLt, Cfg.GridFontSize);
				DrawUTMGrid (MapPhoto, StartLgLt, EndLgLt, Cfg.GridFontSize);
			}

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
				 ToUseShader,
				 s_tile,
	//			 e_tile,
				  s_tile,
				 Cfg.NearPlane,
				 ev_map_data,
				 geoid_map_data,
				 "view_tri_mesh",
				 scene_cfg,
				 controller_parts);
			
			//--------------------------------------------------
			// 5 シーンを描画する。

			var image_map_data = new CImageMapData_WP(MapImage, img_s_tile, img_e_tile);
			var photo_map_data = new CImageMapData_WP(MapPhoto, img_s_tile, img_e_tile);

			viewer.CreateScene
				(image_map_data,
				 photo_map_data);

			//--------------------------------------------------

			StopWatch.Lap("GeoViewer_Tile Created");

			return viewer;
		}
	}
}
