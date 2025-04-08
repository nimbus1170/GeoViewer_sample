//
// GeoViewerForm_Tile.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Scene;
using static DSF_NET_Geography.Convert_LgLt_WP;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
	public partial class GeoViewerForm_Tile : GeoViewerViewForm
	{
		internal override void ShowInfo()
		{
			if(Viewer == null) return;

			// ◆ダウンキャストはしたくないので、CGeoViewer.Centerを仮想関数で作れ。
			ShowInfoImpl(ToLgLt(((CGeoViewer_WP)Viewer).Center), ToLgLt(((CGeoViewer_WP)Viewer).Observer));
		}
	}
}
