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

		// ���_�E���L���X�g�͂������Ȃ��̂ŁACGeoViewer.Center�����z�֐��ō��B
		ShowObjInfoImpl(((CGeoViewer_LgLt)Viewer).Center);
	}
}
//---------------------------------------------------------------------------
}
