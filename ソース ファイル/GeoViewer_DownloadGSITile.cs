//
// GeoViewer_DownloadGSITiles.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Geometry;
using DSF_NET_Map;

using static DSF_NET_TacticalDrawing.ReadXML;

using static DSF_NET_Geography.Convert_Tude_WorldPixel;
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
		// �摜�^�C�����擾����B
		{ 
			var s_tile = new CTile
				(GetTileX(ToWorldPixelX(img_zoom_level, s_tude.Longitude)),
				 GetTileY(ToWorldPixelY(img_zoom_level, e_tude.Latitude ))); // ���ܓx�������t�]������B

			var e_tile = new CTile
				(GetTileX(ToWorldPixelX(img_zoom_level, e_tude.Longitude)),
				 GetTileY(ToWorldPixelY(img_zoom_level, s_tude.Latitude )));

//			Console.WriteLine("Image Tiles");
//			Console.WriteLine("number of tiles to download�F" + ((tile_ex - tile_sx + 1) * (tile_ey - tile_sy + 1)).ToString());

			DownloadGSIImageTiles(s_tile, e_tile, save_fld);
		}

		//--------------------------------------------------
		// �W���^�C�����擾����B
		{ 
			// ����芸�������x��14�ɌŒ肷��B�W���^�C���̓��x��15���������������B
			Int32 ev_zoom_level = 14;

			var s_tile = new CTile
				(GetTileX(ToWorldPixelX(ev_zoom_level, s_tude.Longitude)),
				 GetTileY(ToWorldPixelY(ev_zoom_level, e_tude.Latitude ))); // ���ܓx�������t�]������B

			var e_tile = new CTile
				(GetTileX(ToWorldPixelX(ev_zoom_level, e_tude.Longitude)),
				 GetTileY(ToWorldPixelY(ev_zoom_level, s_tude.Latitude )));

//			Console.WriteLine("DEM Tiles");
//			Console.WriteLine("number of tiles to download : " + ((tile_ex - tile_sx + 1) * (tile_ey - tile_sy + 1)).ToString());

			DownloadGSIElevationTiles(s_tile, e_tile, save_fld);
		}
	}

	private void DownloadGSIImageTiles
	(in CTile s_tile,
	 in CTile e_tile,
	 in string save_fld)
	{ 
		var tile_sx_val = s_tile.X().Value();
		var tile_sy_val = s_tile.Y().Value();
		var tile_ex_val = e_tile.X().Value();
		var tile_ey_val = e_tile.Y().Value();

		var img_zoom_level = s_tile.ZoomLevel();

		//--------------------------------------------------
		// �n�}�摜�^�C�����擾����B

		string img_save_fld = save_fld + $"/gsi/std/{img_zoom_level}";

		if(!(Directory.Exists(img_save_fld)))
			Directory.CreateDirectory(img_save_fld);

		for(var tile_y_val = tile_sy_val; tile_y_val <= tile_ey_val; tile_y_val++)
			for(var tile_x_val = tile_sx_val; tile_x_val <= tile_ex_val; tile_x_val++)
				{
					// �����y�n���@�^�C���n�}�̃T�C�g�̎d�l���ύX���ꂽ�ꍇ�͐ݒ�t�@�C���ł͑Ή�����Ȃ̂Ńn�[�h�R�[�h����B
					string img_url  = $"https://cyberjapandata.gsi.go.jp/xyz/std/{img_zoom_level}/{tile_x_val}/{tile_y_val}.png";
					string img_path = img_save_fld + $"/{img_zoom_level}_{tile_x_val}_{tile_y_val}.png";

					DownloadGSITile(img_url, img_path);
				}

		//--------------------------------------------------
		// �q���摜�^�C�����擾����B

		img_save_fld = save_fld + $"/gsi/seamlessphoto/{img_zoom_level}";

		if(!(Directory.Exists(img_save_fld)))
			Directory.CreateDirectory(img_save_fld);

		for(var tile_y_val = tile_sy_val; tile_y_val <= tile_ey_val; tile_y_val++)
			for(var tile_x_val = tile_sx_val; tile_x_val <= tile_ex_val; tile_x_val++)
				{
					string img_url  = $"https://cyberjapandata.gsi.go.jp/xyz/seamlessphoto/{img_zoom_level}/{tile_x_val}/{tile_y_val}.jpg";
					string img_path = img_save_fld + $"/{img_zoom_level}_{tile_x_val}_{tile_y_val}.jpg";

					DownloadGSITile(img_url, img_path);
				}
	}
		
	private void DownloadGSIElevationTiles
	(in CTile s_tile,
	 in CTile e_tile,
	 in string save_fld)
	{ 
		var tile_sx_val = s_tile.X().Value();
		var tile_sy_val = s_tile.Y().Value();
		var tile_ex_val = e_tile.X().Value();
		var tile_ey_val = e_tile.Y().Value();

		var ev_zoom_level = s_tile.ZoomLevel();

		//--------------------------------------------------
		// �W���^�C�����擾����B

		string ev_save_fld = save_fld + $"/gsi/dem_png/{ev_zoom_level}";

		if(!(Directory.Exists(ev_save_fld)))
			Directory.CreateDirectory(ev_save_fld);

		for(var tile_y_val = tile_sy_val; tile_y_val <= tile_ey_val; tile_y_val++)
			for(var tile_x_val = tile_sx_val; tile_x_val <= tile_ex_val; tile_x_val++)
			{
				string ev_url  = $"https://cyberjapandata.gsi.go.jp/xyz/dem_png/{ev_zoom_level}/{tile_x_val}/{tile_y_val}.png";
				string ev_path = ev_save_fld + $"/{ev_zoom_level}_{tile_x_val}_{tile_y_val}.png";
			
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
				// �W�������݂��Ȃ�����(�C)�̓^�C�������݂��Ȃ��̂Ŕ�΂��B
//				Console.WriteLine(save_path + " : Not Exists on Web");
				break;

			default:
				break;
		}
	}
}
//---------------------------------------------------------------------------
}