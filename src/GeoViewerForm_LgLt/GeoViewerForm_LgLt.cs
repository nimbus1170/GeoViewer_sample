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

			// ���_�E���L���X�g�͂������Ȃ��̂ŁACGeoViewer.Center�����z�֐��ō��B
			ShowInfoImpl(((CGeoViewer_LgLt)Viewer).Center, ((CGeoViewer_LgLt)Viewer).Observer);
		}
	}
}
