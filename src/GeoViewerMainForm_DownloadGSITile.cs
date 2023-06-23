//
// GeoViewerMainForm_DownloadGSITiles.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;

using static DSF_NET_TacticalDrawing.XMLReader;

using static DSF_NET_Geography.Convert_LgLt_WP;
using static DSF_NET_Geography.GSITileDownloader;
using static DSF_NET_Geography.XMapTile;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO
;
using System.Windows.Forms;
using System.Xml;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	private void DownloadGSITiles
	(in CLgLt s_lglt,
	 in CLgLt e_lglt,
	 in Int32 img_zoom_level,
	 in string save_fld)
	{ 
		//--------------------------------------------------
		// 画像タイルを取得する。
		{ 
			var s_tile = new CTile
				(GetTileX(ToWPX(img_zoom_level, s_lglt.Lg)),
				 GetTileY(ToWPY(img_zoom_level, e_lglt.Lt))); // ◆緯度方向を逆転させる。

			var e_tile = new CTile
				(GetTileX(ToWPX(img_zoom_level, e_lglt.Lg)),
				 GetTileY(ToWPY(img_zoom_level, s_lglt.Lt)));

//			Console.WriteLine("Image Tiles");
//			Console.WriteLine("number of tiles to download：" + ((tile_ex - tile_sx + 1) * (tile_ey - tile_sy + 1)).ToString());

			DownloadGSIImageTiles(s_tile, e_tile, save_fld);
		}

		//--------------------------------------------------
		// 標高タイルを取得する。
		{ 
			// ◆取り敢えずレベル14に固定する。標高タイルはレベル15が無い所が多い。
			Int32 ev_zoom_level = 14;

			var s_tile = new CTile
				(GetTileX(ToWPX(ev_zoom_level, s_lglt.Lg)),
				 GetTileY(ToWPY(ev_zoom_level, e_lglt.Lt))); // ◆緯度方向を逆転させる。

			var e_tile = new CTile
				(GetTileX(ToWPX(ev_zoom_level, e_lglt.Lg)),
				 GetTileY(ToWPY(ev_zoom_level, s_lglt.Lt)));

//			Console.WriteLine("DEM Tiles");
//			Console.WriteLine("number of tiles to download : " + ((tile_ex - tile_sx + 1) * (tile_ey - tile_sy + 1)).ToString());

			DownloadGSIElevationTiles(s_tile, e_tile, save_fld);
		}
	}

	// 画像タイルを取得する。
	// ◆GeoViewer_Tileがタイル単位で取得するのでこのメソッドは省かない。
	private void DownloadGSIImageTiles
	(in CTile s_tile,
	 in CTile e_tile,
	 in string save_fld)
	{ 
		var tile_sx = s_tile.X.Value;
		var tile_sy = s_tile.Y.Value;
		var tile_ex = e_tile.X.Value;
		var tile_ey = e_tile.Y.Value;

		var zl = s_tile.ZoomLevel;

		// 地図画像タイルを取得する。
		for(var tile_y = tile_sy; tile_y <= tile_ey; tile_y++)
			for(var tile_x = tile_sx; tile_x <= tile_ex; tile_x++)
				DownloadGSITile(DTileType.IMAGE, zl, tile_x, tile_y, save_fld);

		// 衛星画像タイルを取得する。
		for(var tile_y = tile_sy; tile_y <= tile_ey; tile_y++)
			for(var tile_x = tile_sx; tile_x <= tile_ex; tile_x++)
				DownloadGSITile(DTileType.PHOTO, zl, tile_x, tile_y, save_fld);
	}
		
	// 標高タイルを取得する。
	// ◆GeoViewer_Tileがタイル単位で取得するのでこのメソッドは省かない。
	private void DownloadGSIElevationTiles
	(in CTile s_tile,
	 in CTile e_tile,
	 in string save_fld)
	{ 
		var tile_sx = s_tile.X.Value;
		var tile_sy = s_tile.Y.Value;
		var tile_ex = e_tile.X.Value;
		var tile_ey = e_tile.Y.Value;

		var zl = s_tile.ZoomLevel;

		for(var tile_y = tile_sy; tile_y <= tile_ey; tile_y++)
			for(var tile_x = tile_sx; tile_x <= tile_ex; tile_x++)
				DownloadGSITile(DTileType.DEM_PNG, zl, tile_x, tile_y, save_fld);
	}

	private void DownloadGSITile
		(in DTileType tile_type,
		 in int zl,
		 in int tile_x,
		 in int tile_y,
		 in string save_fld)
	{
		switch(DownloadTile(tile_type, zl, tile_x, tile_y, save_fld))
		{
			case DDownloadResult.Downloaded:
//				Console.WriteLine(save_path + " : Downloaded");
				break;

			case DDownloadResult.ExistInLocal:
//				Console.WriteLine(save_path + " : Already exists in local");
				break;

			case DDownloadResult.NotExistOnWeb:
				// 標高が存在しない部分(海)はタイルが存在しないので飛ばす。
//				Console.WriteLine(save_path + " : Not Exists on Web");
				break;

			default:
				break;
		}
	}
}
//---------------------------------------------------------------------------
}
