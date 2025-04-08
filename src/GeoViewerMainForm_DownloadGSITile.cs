//
// GeoViewerMainForm_DownloadGSITiles.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Geography;
using static DSF_NET_Geography.Convert_LgLt_WP;
using static DSF_NET_Geography.Convert_WP_Tile;
using static DSF_NET_Geography.GSITileDownloader;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
	public partial class GeoViewerMainForm : Form
	{
		private void DownloadGSITiles
		(in CLgLt s_lglt,
		 in CLgLt e_lglt,
		 in Int32 img_zl,
		 in string save_fld)
		{ 
			//--------------------------------------------------
			// �摜�^�C�����擾����B
			{ 
				var s_tile = new CTile
					(ToTileX(ToWPX(img_zl, s_lglt.Lg)),
					 ToTileY(ToWPY(img_zl, e_lglt.Lt))); // ���ܓx�������t�]������B

				var e_tile = new CTile
					(ToTileX(ToWPX(img_zl, e_lglt.Lg)),
					 ToTileY(ToWPY(img_zl, s_lglt.Lt)));

				DownloadGSIImageTiles(s_tile, e_tile, save_fld);
			}

			//--------------------------------------------------
			// �W���^�C�����擾����B
			{ 
				var s_tile = new CTile
					(ToTileX(ToWPX(DEMZoomLevel, s_lglt.Lg)),
					 ToTileY(ToWPY(DEMZoomLevel, e_lglt.Lt))); // ���ܓx�������t�]������B

				var e_tile = new CTile
					(ToTileX(ToWPX(DEMZoomLevel, e_lglt.Lg)),
					 ToTileY(ToWPY(DEMZoomLevel, s_lglt.Lt)));

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
			var zl = s_tile.ZoomLevel;

			DownloadTiles(DTileType.IMAGE, zl, s_tile, e_tile, Cfg.ImageTileUrl, save_fld);
			DownloadTiles(DTileType.PHOTO, zl, s_tile, e_tile, Cfg.PhotoTileUrl, save_fld);
		}
		
		// �W���^�C�����擾����B
		// ��GeoViewer_Tile���^�C���P�ʂŎ擾����̂ł��̃��\�b�h�͏Ȃ��Ȃ��B
		private void DownloadGSIElevationTiles
		(in CTile s_tile,
		 in CTile e_tile,
		 in string save_fld)
		{ 
			var zl = s_tile.ZoomLevel;

			DownloadTiles(DTileType.DEM_PNG, zl, s_tile, e_tile, Cfg.DEMTileUrl, save_fld);
		}
	}
}
