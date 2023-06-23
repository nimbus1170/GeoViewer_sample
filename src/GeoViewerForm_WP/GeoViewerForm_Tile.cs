//
// GeoViewerForm_Tile.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Scene;
using System;
using static DSF_NET_Geography.Convert_LgLt_WP;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerForm_Tile : GeoViewerForm
{
	public override void ShowObjInfo()
	{
		if(Viewer == null) return;

		// ◆ダウンキャストはしたくないので、CGeoViewer.Centerを仮想関数で作れ。
		ShowObjInfoImpl(ToLgLt(((CGeoViewer_WP)Viewer).Center));
	}
}
//---------------------------------------------------------------------------
}
