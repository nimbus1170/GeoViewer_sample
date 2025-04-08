//
// GeoViewerForm_LgLt.cs
//
//---------------------------------------------------------------------------
using DSF_NET_Scene;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
	public partial class GeoViewerForm_LgLt : GeoViewerViewForm
	{
		internal override void ShowInfo()
		{
			if(Viewer == null) return;

			// ◆ダウンキャストはしたくないので、CGeoViewer.Centerを仮想関数で作れ。
			ShowInfoImpl(((CGeoViewer_LgLt)Viewer).Center, ((CGeoViewer_LgLt)Viewer).Observer);
		}
	}
}
