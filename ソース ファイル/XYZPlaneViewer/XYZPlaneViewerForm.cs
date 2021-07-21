//
//
//
//---------------------------------------------------------------------------
using DSF_NET_Geometry;
using DSF_NET_SceneGraph;

using System;
//---------------------------------------------------------------------------
namespace PlaneViewer_sample
{
//---------------------------------------------------------------------------
public partial class XYZPlaneViewerForm : PlaneViewerForm
{
	public override void DispObjInfo()
	{
		CCoord ct = ((CXYZPlaneViewer)Viewer).Center;

		ObjInfoLabel.Text = $"X : {ct.X:0000}\n" + 
							$"Y : {ct.Y:0000}";
	}
}
//---------------------------------------------------------------------------
}
