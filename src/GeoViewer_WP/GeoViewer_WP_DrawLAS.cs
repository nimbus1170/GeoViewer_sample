//
// GeoViewer_WP.cs
// 地形ビューア(ワールドピクセル) - オーバレイ描画
//
//---------------------------------------------------------------------------
using DSF_NET_Color;
using DSF_NET_Geography;
using DSF_NET_Scene;

using static DSF_NET_Geography.CEllipsoid;

using static DSF_NET_Geography.Convert_LgLt_GeoCentricCoord;
using static DSF_NET_Geography.Convert_LgLt_WP;
using static DSF_NET_Geography.Convert_LgLt_WPInt;
using static DSF_NET_Geography.GeoObserver;
using static DSF_NET_Geometry.CCoord;
using static DSF_NET_Geometry.CDMS;
using static DSF_NET_TacticalDrawing.GeoObserver;

using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;

using static System.Convert;
using System;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	[SupportedOSPlatform("windows")]
	void DrawLAS()
	{
		//--------------------------------------------------

		var las_data = new CLASzip("M60412559.las");

		var las_points = las_data.Points;

		double x, y, z;

		CLgLt pt_1 = new ();
		CLgLt pt_2 = new ();

		foreach(var pt in las_points)
		{
			x = pt.X;
			y = pt.Y;
			z = pt.Z;

			pt_1.Set(new CLg(x), new CLt(y));
			pt_2.Set(new CLg(x), new CLt(y));

			pt_1.SetAltitude(z		, DAltitudeBase.AMSL);
			pt_2.SetAltitude(z + 1.0, DAltitudeBase.AMSL);
			 
			Viewer.AddShape
				("line_las",
				 new CGeoLine(pt_1, pt_2)
					.SetColor(new CColorF(1.0f, 0.0f, 0.0f, 0.5f))
					.SetLineWidth(10.0f));
		}

		//--------------------------------------------------

		Viewer.DrawScene();
	}
}
//---------------------------------------------------------------------------
}
