//
// GeoViewerForm_Tile.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Scene;

using static DSF_NET_Geography.Convert_LgLt_WP;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewViewerForm_Tile : GeoViewerForm
{
	public override void ShowObjInfo()
	{
		if(Viewer == null) return;

		ShowObjInfoImpl(ToLgLt(((CGeoViewer_WP)Viewer).Center));
	}
}
//---------------------------------------------------------------------------
}
