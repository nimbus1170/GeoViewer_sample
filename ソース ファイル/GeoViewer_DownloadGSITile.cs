//
// GeoViewer_DownloadGSITiles.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using DSF_NET_Geometry;
using DSF_NET_Map;

using static DSF_NET_TacticalDrawing.ReadXML;

using static DSF_NET_Geography.Convert_Tude_WorldPixel;

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
		// �摜�^�C�����擾����B
		{ 
			UInt32 tile_sx = (UInt32)(ToWorldPixelX(img_zoom_level, s_tude.Longitude).Value) / 256;
			UInt32 tile_ex = (UInt32)(ToWorldPixelX(img_zoom_level, e_tude.Longitude).Value) / 256;
			UInt32 tile_sy = (UInt32)(ToWorldPixelY(img_zoom_level, e_tude.Latitude ).Value) / 256;
			UInt32 tile_ey = (UInt32)(ToWorldPixelY(img_zoom_level, s_tude.Latitude ).Value) / 256;

//			Console.WriteLine("Image Tiles");
//			Console.WriteLine("number of tiles to download�F" + ((tile_ex - tile_sx + 1) * (tile_ey - tile_sy + 1)).ToString());

			//--------------------------------------------------
			// �n�}�摜�^�C�����擾����B

			string img_save_fld = save_fld + $"/gsi/std/{img_zoom_level}";

			if(!(Directory.Exists(img_save_fld)))
				Directory.CreateDirectory(img_save_fld);

			for(UInt32 tile_y = tile_sy; tile_y <= tile_ey; tile_y++)
				for(UInt32 tile_x = tile_sx; tile_x <= tile_ex; tile_x++)
					{
						// �����y�n���@�^�C���n�}�̃T�C�g�̎d�l���ύX���ꂽ�ꍇ�͐ݒ�t�@�C���ł͑Ή�����Ȃ̂Ńn�[�h�R�[�h����B
						string img_url  = $"https://cyberjapandata.gsi.go.jp/xyz/std/{img_zoom_level}/{tile_x}/{tile_y}.png";
						string img_path = img_save_fld + $"/{img_zoom_level}_{tile_x}_{tile_y}.png";

						DownloadGSITile(img_url, img_path);
					}

			//--------------------------------------------------
			// �q���摜�^�C�����擾����B

			img_save_fld = save_fld + $"/gsi/seamlessphoto/{img_zoom_level}";

			if(!(Directory.Exists(img_save_fld)))
				Directory.CreateDirectory(img_save_fld);

			for(UInt32 tile_y = tile_sy; tile_y <= tile_ey; tile_y++)
				for(UInt32 tile_x = tile_sx; tile_x <= tile_ex; tile_x++)
					{
						string img_url  = $"https://cyberjapandata.gsi.go.jp/xyz/seamlessphoto/{img_zoom_level}/{tile_x}/{tile_y}.jpg";
						string img_path = img_save_fld + $"/{img_zoom_level}_{tile_x}_{tile_y}.jpg";

						DownloadGSITile(img_url, img_path);
					}
		}
		
//		Console.WriteLine("");

		//--------------------------------------------------
		// �W���^�C�����擾����B
		{ 
			// ����芸�������x��14�ɌŒ肷��B�W���^�C���̓��x��15���������������B
			Int32 ev_zoom_level = 14;

			UInt32 tile_sx = (UInt32)(ToWorldPixelX(ev_zoom_level, s_tude.Longitude).Value) / 256;
			UInt32 tile_ex = (UInt32)(ToWorldPixelX(ev_zoom_level, e_tude.Longitude).Value) / 256;
			UInt32 tile_sy = (UInt32)(ToWorldPixelY(ev_zoom_level, e_tude.Latitude ).Value) / 256;
			UInt32 tile_ey = (UInt32)(ToWorldPixelY(ev_zoom_level, s_tude.Latitude ).Value) / 256;

//			Console.WriteLine("DEM Tiles");
//			Console.WriteLine("number of tiles to download : " + ((tile_ex - tile_sx + 1) * (tile_ey - tile_sy + 1)).ToString());

			string ev_save_fld = save_fld + $"/gsi/dem_png/{ev_zoom_level}";

			if(!(Directory.Exists(ev_save_fld)))
				Directory.CreateDirectory(ev_save_fld);

			for(UInt32 tile_y = tile_sy; tile_y <= tile_ey; tile_y++)
				for(UInt32 tile_x = tile_sx; tile_x <= tile_ex; tile_x++)
				{
					string ev_url  = $"https://cyberjapandata.gsi.go.jp/xyz/dem_png/{ev_zoom_level}/{tile_x}/{tile_y}.png";
					string ev_path = ev_save_fld + $"/{ev_zoom_level}_{tile_x}_{tile_y}.png";
			
					DownloadGSITile(ev_url, ev_path);
				}
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