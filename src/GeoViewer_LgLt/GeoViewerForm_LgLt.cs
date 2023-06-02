//
// GeoViewerForm_LgLt.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Scene;
using System;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerForm_LgLt : GeoViewerForm
{
	public override void ShowObjInfo()
	{
		if(Viewer == null) return;

		// ◆ダウンキャストはしたくないので、CGeoViewer.Centerを仮想関数で作れ。
		ShowObjInfoImpl(((CGeoViewer_LgLt)Viewer).Center);
	}
}
//---------------------------------------------------------------------------
}
