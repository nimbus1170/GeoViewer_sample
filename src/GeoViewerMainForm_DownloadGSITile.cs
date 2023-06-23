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
		// �摜�^�C�����擾����B
		{ 
			var s_tile = new CTile
				(GetTileX(ToWPX(img_zoom_level, s_lglt.Lg)),
				 GetTileY(ToWPY(img_zoom_level, e_lglt.Lt))); // ���ܓx�������t�]������B

			var e_tile = new CTile
				(GetTileX(ToWPX(img_zoom_level, e_lglt.Lg)),
				 GetTileY(ToWPY(img_zoom_level, s_lglt.Lt)));

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
				(GetTileX(ToWPX(ev_zoom_level, s_lglt.Lg)),
				 GetTileY(ToWPY(ev_zoom_level, e_lglt.Lt))); // ���ܓx�������t�]������B

			var e_tile = new CTile
				(GetTileX(ToWPX(ev_zoom_level, e_lglt.Lg)),
				 GetTileY(ToWPY(ev_zoom_level, s_lglt.Lt)));

//			Console.WriteLine("DEM Tiles");
//			Console.WriteLine("number of tiles to download : " + ((tile_ex - tile_sx + 1) * (tile_ey - tile_sy + 1)).ToString());

			DownloadGSIElevationTiles(s_tile, e_tile, save_fld);
		}
	}

	// �摜�^�C�����擾����B
	// ��GeoViewer_Tile���^�C���P�ʂŎ擾����̂ł��̃��\�b�h�͏Ȃ��Ȃ��B
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

		// �n�}�摜�^�C�����擾����B
		for(var tile_y = tile_sy; tile_y <= tile_ey; tile_y++)
			for(var tile_x = tile_sx; tile_x <= tile_ex; tile_x++)
				DownloadGSITile(DTileType.IMAGE, zl, tile_x, tile_y, save_fld);

		// �q���摜�^�C�����擾����B
		for(var tile_y = tile_sy; tile_y <= tile_ey; tile_y++)
			for(var tile_x = tile_sx; tile_x <= tile_ex; tile_x++)
				DownloadGSITile(DTileType.PHOTO, zl, tile_x, tile_y, save_fld);
	}
		
	// �W���^�C�����擾����B
	// ��GeoViewer_Tile���^�C���P�ʂŎ擾����̂ł��̃��\�b�h�͏Ȃ��Ȃ��B
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
