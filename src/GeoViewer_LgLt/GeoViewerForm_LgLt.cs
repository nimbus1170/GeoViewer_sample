//
// GeoViewerForm_LgLt.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Scene;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerForm_LgLt : GeoViewerForm
{
	public override void ShowObjInfo()
	{
		if(Viewer == null) return;

		ShowObjInfoImpl(((CGeoViewer_LgLt)Viewer).Center);
	}
}
//---------------------------------------------------------------------------
}
