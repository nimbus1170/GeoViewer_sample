//
// GeoViewer_WP_DrawLAS.cs
// �n�`�r���[�A(���[���h�s�N�Z��) - LAS�f�[�^�\��
//
//---------------------------------------------------------------------------
using DSF_NET_Color;
using DSF_NET_Geography;
using DSF_NET_Scene;

using static DSF_NET_Geography.DAltitudeBase;

using System.Runtime.Versioning;
//---------------------------------------------------------------------------
namespace GeoViewer_sample
{
//---------------------------------------------------------------------------
public partial class GeoViewerMainForm : Form
{
	[SupportedOSPlatform("windows")]
	void DrawLAS()
	{
		if(LASzipData == null) return;

		//--------------------------------------------------

		// ���T���v���Ȃ̂łƂ肠�����Œ�
		var laszip_data = LASzipData;//new CLASzip("M60412559.las");

		var laszip_header = laszip_data.Header;

		var x_scale_factor = laszip_header.x_scale_factor;
		var y_scale_factor = laszip_header.y_scale_factor;
		var z_scale_factor = laszip_header.z_scale_factor;

		var x_offset = laszip_header.x_offset;
		var y_offset = laszip_header.y_offset;
		var z_offset = laszip_header.z_offset;

		var laszip_points = laszip_data.Points;

		double x, y, z;

		CLgLt ct_1 = new ();
		CLgLt ct_2 = new ();

		//--------------------------------------------------
		// ���x�ŐF�������邽�߁A���x�͈̔͂����߂�B
		// ��LAS�f�[�^�ł͋��x�̊�͌��܂��Ă��Ȃ��̂ŁA�ő�l�E�ŏ��l���炻�͈̔͂����߂�B

		ushort min_inensity = ushort.MaxValue;
		ushort max_inensity = ushort.MinValue;

		foreach(var pt in laszip_points)
		{
			if(pt.intensity < min_inensity) min_inensity = pt.intensity;
			if(pt.intensity > max_inensity) max_inensity = pt.intensity;
		}

		float intensity_range = max_inensity - min_inensity;  

		//--------------------------------------------------

		var color = new CColorF{ A = 1.0f }; // A:�����x(�A���t�@�l)

		foreach(var pt in laszip_points)
		{
			x = x_offset + pt.X * x_scale_factor;
			y = y_offset + pt.Y * y_scale_factor;
			z = z_offset + pt.Z * z_scale_factor;

			ct_1.SetLg(x).SetLt(y).SetAltitude(z	  , AMSL);
			ct_2.SetLg(x).SetLt(y).SetAltitude(z + 1.0, AMSL);

			color.R = 
			color.G = 
			color.B = (pt.intensity - min_inensity) / intensity_range;

			Viewer.AddShape
				("laspoints",
//				 new CGeoCircle(8, ct, 0.5) // ���_���A���S(�o�ܓx)�A���a(m)
				 new CGeoLine(ct_1, ct_2)
					.SetColor(color)
					.SetLineWidth(10));
		}

		//--------------------------------------------------

		Viewer.DrawScene();
	}
}
//---------------------------------------------------------------------------
}
