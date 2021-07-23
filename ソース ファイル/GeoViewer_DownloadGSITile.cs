//
// GeoViewer_DownloadGSITiles.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Geometry;
using DSF_NET_Map;

using static DSF_NET_TacticalDrawing.ReadXML;

using static DSF_NET_Geography.Convert_Tude_WorldPixel;
using static DSF_NET_Geography.XMapTile;

using static DSF_CS_Geography.GSITileDownloader;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO
;
using System.Windows.Forms;
using System.Xml;
//---------------------------------------------------------------------------
namespace PlaneViewer_sample
{
//---------------------------------------------------------------------------
public partial class PlaneViewerMainForm : Form
{
	private void DownloadGSITiles
	(in CTude s_tude,
	 in CTude e_tude,
	 in Int32 img_zoom_level,
	 in string save_fld)
	{ 
		//--------------------------------------------------
		// 画像タイルを取得する。
		{ 
			var tile_sx = GetTileX(ToWorldPixelX(img_zoom_level, s_tude.Longitude)).Value();
			var tile_ex = GetTileX(ToWorldPixelX(img_zoom_level, e_tude.Longitude)).Value();
			var tile_sy = GetTileY(ToWorldPixelY(img_zoom_level, e_tude.Latitude )).Value();
			var tile_ey = GetTileY(ToWorldPixelY(img_zoom_level, s_tude.Latitude )).Value();

//			Console.WriteLine("Image Tiles");
//			Console.WriteLine("number of tiles to download：" + ((tile_ex - tile_sx + 1) * (tile_ey - tile_sy + 1)).ToString());

			DownloadGSIImageTiles(tile_sx, tile_sy, tile_ex, tile_ey, img_zoom_level, save_fld);
		}

		//--------------------------------------------------
		// 標高タイルを取得する。
		{ 
			// ◆取り敢えずレベル14に固定する。標高タイルはレベル15が無い所が多い。
			Int32 ev_zoom_level = 14;

			var tile_sx = GetTileX(ToWorldPixelX(ev_zoom_level, s_tude.Longitude)).Value();
			var tile_ex = GetTileX(ToWorldPixelX(ev_zoom_level, e_tude.Longitude)).Value();
			var tile_sy = GetTileY(ToWorldPixelY(ev_zoom_level, e_tude.Latitude )).Value();
			var tile_ey = GetTileY(ToWorldPixelY(ev_zoom_level, s_tude.Latitude )).Value();

//			Console.WriteLine("DEM Tiles");
//			Console.WriteLine("number of tiles to download : " + ((tile_ex - tile_sx + 1) * (tile_ey - tile_sy + 1)).ToString());

			DownloadGSIElevationTiles(tile_sx, tile_sy, tile_ex, tile_ey, ev_zoom_level, save_fld);
		}
	}

	private void DownloadGSIImageTiles
	(in Int32 tile_sx,
	 in Int32 tile_sy,
	 in Int32 tile_ex,
	 in Int32 tile_ey,
	 in Int32 img_zoom_level,
	 in string save_fld)
	{ 
		//--------------------------------------------------
		// 地図画像タイルを取得する。

		string img_save_fld = save_fld + $"/gsi/std/{img_zoom_level}";

		if(!(Directory.Exists(img_save_fld)))
			Directory.CreateDirectory(img_save_fld);

		for(var tile_y = tile_sy; tile_y <= tile_ey; tile_y++)
			for(var tile_x = tile_sx; tile_x <= tile_ex; tile_x++)
				{
					// ◆国土地理院タイル地図のサイトの仕様が変更された場合は設定ファイルでは対応困難なのでハードコードする。
					string img_url  = $"https://cyberjapandata.gsi.go.jp/xyz/std/{img_zoom_level}/{tile_x}/{tile_y}.png";
					string img_path = img_save_fld + $"/{img_zoom_level}_{tile_x}_{tile_y}.png";

					DownloadGSITile(img_url, img_path);
				}

		//--------------------------------------------------
		// 衛星画像タイルを取得する。

		img_save_fld = save_fld + $"/gsi/seamlessphoto/{img_zoom_level}";

		if(!(Directory.Exists(img_save_fld)))
			Directory.CreateDirectory(img_save_fld);

		for(var tile_y = tile_sy; tile_y <= tile_ey; tile_y++)
			for(var tile_x = tile_sx; tile_x <= tile_ex; tile_x++)
				{
					string img_url  = $"https://cyberjapandata.gsi.go.jp/xyz/seamlessphoto/{img_zoom_level}/{tile_x}/{tile_y}.jpg";
					string img_path = img_save_fld + $"/{img_zoom_level}_{tile_x}_{tile_y}.jpg";

					DownloadGSITile(img_url, img_path);
				}
	}
		
	private void DownloadGSIElevationTiles
	(in Int32 tile_sx,
	 in Int32 tile_sy,
	 in Int32 tile_ex,
	 in Int32 tile_ey,
	 in Int32 ev_zoom_level,
	 in string save_fld)
	{ 
		//--------------------------------------------------
		// 標高タイルを取得する。

		string ev_save_fld = save_fld + $"/gsi/dem_png/{ev_zoom_level}";

		if(!(Directory.Exists(ev_save_fld)))
			Directory.CreateDirectory(ev_save_fld);

		for(var tile_y = tile_sy; tile_y <= tile_ey; tile_y++)
			for(var tile_x = tile_sx; tile_x <= tile_ex; tile_x++)
			{
				string ev_url  = $"https://cyberjapandata.gsi.go.jp/xyz/dem_png/{ev_zoom_level}/{tile_x}/{tile_y}.png";
				string ev_path = ev_save_fld + $"/{ev_zoom_level}_{tile_x}_{tile_y}.png";
			
				DownloadGSITile(ev_url, ev_path);
			}
	}

	private void DownloadGSITile(string url, string save_path)
	{
		switch(DownloadTile(url, save_path))
		{
			case DDownloadResult.Downloaded:
//				Console.WriteLine(save_path + " : Downloaded");
				break;

			case DDownloadResult.AlreadyExistInLocal:
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