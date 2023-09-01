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
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	private void DownloadGSITiles
	(in CLgLt s_lglt,
	 in CLgLt e_lglt,
	 in Int32 img_zl,
	 in string save_fld)
	{ 
		//--------------------------------------------------
		// 画像タイルを取得する。
		{ 
			var s_tile = new CTile
				(ToTileX(ToWPX(img_zl, s_lglt.Lg)),
				 ToTileY(ToWPY(img_zl, e_lglt.Lt))); // ◆緯度方向を逆転させる。

			var e_tile = new CTile
				(ToTileX(ToWPX(img_zl, e_lglt.Lg)),
				 ToTileY(ToWPY(img_zl, s_lglt.Lt)));

			DownloadGSIImageTiles(s_tile, e_tile, save_fld);
		}

		//--------------------------------------------------
		// 標高タイルを取得する。
		{ 
			// ◆取り敢えずレベル14に固定する。標高タイルはレベル15が無い所が多い。
			Int32 ev_zl = 14;

			var s_tile = new CTile
				(ToTileX(ToWPX(ev_zl, s_lglt.Lg)),
				 ToTileY(ToWPY(ev_zl, e_lglt.Lt))); // ◆緯度方向を逆転させる。

			var e_tile = new CTile
				(ToTileX(ToWPX(ev_zl, e_lglt.Lg)),
				 ToTileY(ToWPY(ev_zl, s_lglt.Lt)));

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
		var zl = s_tile.ZoomLevel;

		DownloadTiles(DTileType.IMAGE, zl, s_tile, e_tile, save_fld);
		DownloadTiles(DTileType.PHOTO, zl, s_tile, e_tile, save_fld);
	}
		
	// 標高タイルを取得する。
	// ◆GeoViewer_Tileがタイル単位で取得するのでこのメソッドは省かない。
	private void DownloadGSIElevationTiles
	(in CTile s_tile,
	 in CTile e_tile,
	 in string save_fld)
	{ 
		var zl = s_tile.ZoomLevel;

		DownloadTiles(DTileType.DEM_PNG, zl, s_tile, e_tile, save_fld);
	}
}
//---------------------------------------------------------------------------
}
